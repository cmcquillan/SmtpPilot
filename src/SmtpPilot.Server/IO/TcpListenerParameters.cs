using System;
using System.Collections.Generic;
using System.Net;

namespace SmtpPilot.Server.IO
{
    public class TcpListenerParameters : IEquatable<TcpListenerParameters>
    {
        public TcpListenerParameters(IPAddress address, ushort port)
        {
            Address = address;
            Port = port;
        }

        public IPAddress Address { get; }

        public ushort Port { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as TcpListenerParameters);
        }

        public bool Equals(TcpListenerParameters other)
        {
            return other != null &&
                   EqualityComparer<IPAddress>.Default.Equals(Address, other.Address) &&
                   Port == other.Port;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Address, Port);
        }

        public static bool operator ==(TcpListenerParameters left, TcpListenerParameters right)
        {
            return EqualityComparer<TcpListenerParameters>.Default.Equals(left, right);
        }

        public static bool operator !=(TcpListenerParameters left, TcpListenerParameters right)
        {
            return !(left == right);
        }
    }
}
