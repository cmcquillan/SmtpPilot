using System;

namespace SmtpPilot.Server.Communication
{
    public interface IMailClient
    {
        Guid ClientId { get; }

        void Write(string message);

        void Disconnect();

        int SecondsClientHasBeenSilent { get; }

        bool ReadUntil(byte[] marker, Span<char> buffer, int readOffset, out int count);

        bool Read(int count, Span<char> buffer);

        bool PeekUntil(byte[] marker, Span<char> buffer, out int count);

        bool Peek(int count, Span<char> buffer);
    }
}
