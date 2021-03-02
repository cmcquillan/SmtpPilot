using System;
using System.Collections.Generic;

namespace SmtpPilot.Server.Conversation
{
    public struct EmailAddress : IEquatable<EmailAddress>
    {
        private readonly string _address;
        private readonly AddressType _addressType;
        private readonly string _displayName;
        private readonly string _host;
        private readonly string _user;

        public EmailAddress(string emailAddress, AddressType type)
        {
            _addressType = type;
            int ampPos = 0, leftBracketPos = 0, rightBracketPos = emailAddress.Length;

            /*
             * Count from the right to avoid any issues with quoted users or display names.
             */
            for (var i = emailAddress.Length - 1; i > 0; i--)
            {
                if (emailAddress[i] == '@')
                {
                    ampPos = i;
                    break;
                }
            }

            if (ampPos == 0)
                throw new ArgumentException("Must provide a valid email address.", nameof(emailAddress));

            _address = emailAddress.ToString();

            /*
             * Now look for indication of user name.
             */
            for (int i = 0; i < emailAddress.Length; i++)
            {
                if (emailAddress[i] == '<')
                    leftBracketPos = i;

                if (emailAddress[i] == '>')
                    rightBracketPos = i;
            }

            if (leftBracketPos != 0)
            {
                _host = _address.Substring(ampPos + 1, emailAddress.Length - ampPos - (emailAddress.Length - rightBracketPos + 1));
                _displayName = _address.Substring(0, leftBracketPos - 1).Trim();
                _user = _address.Substring(leftBracketPos + 1, ampPos - leftBracketPos - 1);
            }
            else
            {
                _host = _address.Substring(ampPos + 1, emailAddress.Length - ampPos - 1);
                _displayName = String.Empty;
                _user = _address.Substring(0, ampPos);
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }
        }

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        public string Host
        {
            get
            {
                return _host;
            }
        }

        public AddressType Type
        {
            get
            {
                return _addressType;
            }
        }

        public string User
        {
            get
            {
                return _user;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is EmailAddress address)
            {
                return Equals(address);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return AddressComparer.FullAddress.GetHashCode(this);
        }

        public bool Equals(EmailAddress other)
        {
            return Equals(other, AddressComparer.FullAddress);
        }

        public bool Equals(EmailAddress other, IEqualityComparer<EmailAddress> comparer)
        {
            return comparer.Equals(this, other);
        }

        public override string ToString()
        {
            if (!String.IsNullOrEmpty(DisplayName))
                return String.Format("{0} <{1}@{2}>", DisplayName, User, Host);
            else
                return String.Format("{0}@{1}", User, Host);
        }
    }
}
