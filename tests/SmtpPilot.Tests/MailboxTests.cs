using NUnit.Framework;
using SmtpPilot.Server.Conversation;
using System;
using System.Collections.Generic;

namespace SmtpPilot.Tests
{
    [TestFixture]
    public class MailboxTests
    {
        [Test]
        [TestCase("<foo@bar.com>", "<foo@BAR.COM>")]
        [TestCase("<foo@bar.com>", "<foo@BaR.cOm>")]
        public void MailboxEqualityTests(string address1, string address2)
        {
            var box1 = Mailbox.Parse(address1);
            var box2 = Mailbox.Parse(address2);

            Assert.AreEqual(box1, box2);
        }

        [Test]
        [TestCase("<FOO@bar.com>", "<foo@bar.com>")]
        [TestCase("<example-abc@abc-domain.com>", "<example-ABC@ABC-domain.com>")]
        public void MailboxInequalityTests(string address1, string address2)
        {
            var box1 = Mailbox.Parse(address1);
            var box2 = Mailbox.Parse(address2);

            Assert.AreNotEqual(box1, box2);
        }

        [Test]
        public void MailboxHashsetRecognizesLocalPartCasing()
        {
            var box1 = Mailbox.Parse("<foo@bar.com>");
            var box2 = Mailbox.Parse("<FOO@bar.com>");

            var set = new HashSet<Mailbox>
            {
                box1,
                box2
            };

            Assert.AreEqual(2, set.Count);
        }


        [Test]
        public void MailboxHashsetIgnoresDomainCasing()
        {
            var box1 = Mailbox.Parse("<foo@bar.com>");
            var box2 = Mailbox.Parse("<foo@BAR.com>");

            var set = new HashSet<Mailbox>
            {
                box1,
                box2
            };

            Assert.AreEqual(1, set.Count);
        }

        [Test]
        [TestCase("<foo@bar.com>", "foo")]
        [TestCase("<\"foo baz\"@bar.com>", "\"foo baz\"")]
        [TestCase("<test@domain.com>", "test")]
        [TestCase("<lastname@domain.com>", "lastname")]
        [TestCase("<test.email.with+symbol@domain.com>", "test.email.with+symbol")]
        [TestCase("<id-with-dash@domain.com>", "id-with-dash")]
        [TestCase("<a@domain.com>", "a")]
        [TestCase("<\"abc.test email\"@domain.com>", "\"abc.test email\"")]
        [TestCase("<\"xyz.test.@.test.com\"@domain.com>", "\"xyz.test.@.test.com\"")]
        [TestCase("<\"abc.(),:;<>[]\\\".EMAIL.\\\"email@\\ \\\"email\\\".test\"@strange.domain.com>", "\"abc.(),:;<>[]\\\".EMAIL.\\\"email@\\ \\\"email\\\".test\"")]
        [TestCase("<example-abc@abc-domain.com>", "example-abc")]
        [TestCase("<admin@mailserver1>", "admin")]
        [TestCase("<#!$%&'*+-/=?^_{}|~@domain.org>", "#!$%&'*+-/=?^_{}|~")]
        [TestCase("<\"() <>[]:,;@\\\"!#$%&’-/=?^_`{}| ~.a\"@domain.org > ", "\"() <>[]:,;@\\\"!#$%&’-/=?^_`{}| ~.a\"")]
        [TestCase("<\" \"@domain.org>", "\" \"")]
        [TestCase("<example@localhost>", "example")]
        [TestCase("<example@s.solutions>", "example")]
        [TestCase("<test@com>", "test")]
        [TestCase("<test@localserver>", "test")]
        [TestCase("<test@[IPv6:2018:db8::1]>", "test")]
        public void MailboxSpanConstructorObtainsCorrectUserName(string address, string localPart)
        {
            var addr = Mailbox.Parse(address.AsSpan());

            Assert.AreEqual(localPart, addr.LocalPart);
        }

        [Test]
        [TestCase("<foo@bar.com>", "bar.com")]
        [TestCase("<\"foo baz\"@bar.com>", "bar.com")]
        [TestCase("<test@domain.com>", "domain.com")]
        [TestCase("<lastname@domain.com>", "domain.com")]
        [TestCase("<admin@mailserver1>", "mailserver1")]
        [TestCase("<#!$%&'*+-/=?^_{}|~@domain.org>", "domain.org")]
        [TestCase("<example@localhost>", "localhost")]
        [TestCase("<example@s.solutions>", "s.solutions")]
        [TestCase("<test@com>", "com")]
        [TestCase("<test@[IPv6:2018:db8::1]>", "IPv6:2018:db8::1")]
        [TestCase("<test@[192.168.2.120]>", "192.168.2.120")]
        public void MailboxSpanConstructorObtainsCorrectDomain(string address, string domain)
        {
            var addr = Mailbox.Parse(address.AsSpan());

            Assert.AreEqual(domain, addr.Domain);
        }
    }
}
