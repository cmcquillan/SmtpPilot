using NUnit.Framework;
using System.Buffers;

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
