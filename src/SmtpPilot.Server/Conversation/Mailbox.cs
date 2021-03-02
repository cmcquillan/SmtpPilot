using System;

namespace SmtpPilot.Server.Conversation
{
    public struct Mailbox : IEquatable<Mailbox>
    {
        public static Mailbox Parse(ReadOnlySpan<char> mailbox)
        {
            // Determine address format and limits.
            int startLocal = 0, endLocal = 0;
            int startDomain = 0, endDomain = 0;
            ReadState state = ReadState.None;

            for (int i = 0; i < mailbox.Length; i++)
            {
                state = (state, mailbox[i]) switch
                {
                    (ReadState.None, ' ') => ReadState.None,
                    (ReadState.None, '<') => ReadState.BeforeLocalPart,
                    (ReadState.BeforeLocalPart, '"') => UpdateState(ref startLocal, i, ReadState.QuotedLocalPart),
                    (ReadState.BeforeLocalPart, _) => UpdateState(ref startLocal, i, ReadState.LocalPart),

                    (ReadState.QuotedLocalPart, '\\') when mailbox[i + 1] >= ' ' && mailbox[i + 1] <= '~' => UpdateState(ref i, 1, ReadState.QuotedLocalPart),
                    (ReadState.QuotedLocalPart, _) when mailbox[i] != '"' && mailbox[i] != '\\' => ReadState.QuotedLocalPart,
                    (ReadState.QuotedLocalPart, '"') => ReadState.EndQuotedLocalPart,
                    (ReadState.EndQuotedLocalPart, '@') => UpdateState(ref endLocal, i, ReadState.StartDomain),

                    (ReadState.LocalPart, _) when mailbox[i] == '@' => UpdateState(ref endLocal, i, ReadState.StartDomain),
                    (ReadState.LocalPartDotAllowed, _) when mailbox[i] == '@' => UpdateState(ref endLocal, i, ReadState.StartDomain),

                    (ReadState.LocalPart, _) when mailbox[i] != '.' => ReadState.LocalPartDotAllowed,
                    (ReadState.LocalPartDotAllowed, _) when mailbox[i] == '.' => ReadState.LocalPart,
                    (ReadState.LocalPartDotAllowed, _) when IsAtom(mailbox[i]) => ReadState.LocalPartDotAllowed,

                    (ReadState.StartDomain, '[') => UpdateState(ref startDomain, i + 1, ReadState.AddressLiteral),
                    (ReadState.AddressLiteral, '>') => ReadState.Done,
                    (ReadState.AddressLiteral, ']') => UpdateState(ref endDomain, i, ReadState.AddressLiteral),
                    (ReadState.AddressLiteral, _) => state,

                    (ReadState.StartDomain, _) => UpdateState(ref startDomain, i, ReadState.Domain),
                    (ReadState.Domain, '>') => UpdateState(ref endDomain, i, ReadState.Done),
                    (ReadState.Domain, ' ') => UpdateState(ref endDomain, i, ReadState.Done),
                    (ReadState.Domain, _) => state,

                    (ReadState.Done, _) => state,

                    (_, _) => throw new Exception($"Exit during {state}:{mailbox[i]}"),
                };
            }

            return new Mailbox
            {
                Domain = mailbox[startDomain..endDomain].ToString(),
                LocalPart = mailbox[startLocal..endLocal].ToString(),
            };

            static ReadState UpdateState(ref int one, int mod, ReadState newState)
            {
                one += mod;
                return newState;
            }
        }

        private static bool IsAtom(char v)
        {
            return v switch
            {
                '!' => true,
                '#' => true,
                '$' => true,
                '&' => true,
                '%' => true,
                '*' => true,
                '+' => true,
                '-' => true,
                '/' => true,
                '=' => true,
                '?' => true,
                '^' => true,
                '_' => true,
                '`' => true,
                '{' => true,
                '}' => true,
                '|' => true,
                '~' => true,
                '\'' => true,
                _ when Char.IsLetterOrDigit(v) => true,
                _ => false
            };
        }

        public override bool Equals(object obj)
        {
            return obj is Mailbox mailbox && Equals(mailbox);
        }

        public bool Equals(Mailbox other)
        {
            return String.Equals(LocalPart, other.LocalPart, StringComparison.Ordinal) &&
                   String.Equals(Domain, other.Domain, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(LocalPart) * 31
                ^ StringComparer.OrdinalIgnoreCase.GetHashCode(Domain) * 17;
        }

        public string LocalPart { get; private set; }

        public string Domain { get; private set; }

        public static bool operator ==(Mailbox left, Mailbox right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Mailbox left, Mailbox right)
        {
            return !(left == right);
        }
    }

    internal enum ReadState
    {
        None,
        BeforeLocalPart,
        LocalPart,
        LocalPartDotAllowed,
        QuotedLocalPart,
        EndQuotedLocalPart,
        StartDomain,
        Domain,
        AddressLiteral,
        EndAddressLiteral,
        Done,
    }
}
