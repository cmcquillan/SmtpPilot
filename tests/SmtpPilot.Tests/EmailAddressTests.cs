using NUnit.Framework;
using SmtpPilot.Server.Conversation;

namespace SmtpPilot.Tests
{
    [TestFixture]
    public class EmailAddressTests
    {
        [Test]
        [TestCase("foo@bar.com", "foo")]
        [TestCase("FooBar <foo@bar.com>", "foo")]
        [TestCase("FooBar <\"foo baz\"@bar.com", "\"foo baz\"")]
        public void EmailAddressObtainsCorrectUserName(string address, string userNameShouldEqual)
        {
            var addr = new EmailAddress(address, AddressType.To);

            Assert.AreEqual(userNameShouldEqual, addr.User);
        }

        [Test]
        [TestCase("foo@bar.com", "bar.com")]
        [TestCase("FooBar <foo@bar.com>", "bar.com")]
        [TestCase("FooBar <foo@bar.com> ", "bar.com")] /* Trailing Space */
        [TestCase("FooBar <\"foo baz\"@bar.com>", "bar.com")]
        public void EmailAddressObtainsCorrecthost(string address, string hostShouldEqual)
        {
            var addr = new EmailAddress(address, AddressType.To);

            Assert.AreEqual(hostShouldEqual, addr.Host);
        }

        [Test]
        [TestCase("foo@bar.com", "")]
        [TestCase("FooBar <foo@bar.com>", "FooBar")]
        [TestCase("FooBar   <foo@bar.com>", "FooBar")] /* Handling extra spaces */
        [TestCase("Foo Bar <foo@bar.com>", "Foo Bar")]
        [TestCase("FooBar <\"foo baz\"@bar.com>", "FooBar")]
        public void EmailAddressObtainsCorrectDisplayName(string address, string displayShouldEqual)
        {
            var addr = new EmailAddress(address, AddressType.To);

            Assert.AreEqual(displayShouldEqual, addr.DisplayName);
        }
    }
}
