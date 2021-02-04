using NUnit.Framework;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Tests
{
    [TestFixture]
    public class SequenceReaderTests
    {
        private ReadOnlySequence<byte> SingleSegmentSequence
        {
            get
            {
                return SequenceHelper.Seq(
                    SequenceHelper.C('H', 'E', 'L', 'O', '\r', '\n')
                );
            }
        }

        [Test]
        public void SequenceIsRead()
        {

        }
    }
}
