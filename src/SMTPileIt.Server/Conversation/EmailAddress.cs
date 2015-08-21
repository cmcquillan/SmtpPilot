using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.Conversation
{
    public struct EmailAddress : IAddress, IEquatable<EmailAddress>
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
            for (ampPos = emailAddress.Length - 1; ampPos > 0; ampPos--)
            {
                if (emailAddress[ampPos] == '@')
                    break;
            }

            if (ampPos == 0)
                throw new ArgumentException("Must provide a valid email address.", nameof(emailAddress));

            _address = emailAddress;

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
                _host = emailAddress.Substring(ampPos + 1, emailAddress.Length - ampPos - (emailAddress.Length - rightBracketPos + 1));
                _displayName = emailAddress.Substring(0, leftBracketPos - 1).Trim();
                _user = emailAddress.Substring(leftBracketPos + 1, ampPos - leftBracketPos - 1);
            }
            else
            {
                _host = emailAddress.Substring(ampPos + 1, emailAddress.Length - ampPos - 1);
                _displayName = String.Empty;
                _user = emailAddress.Substring(0, ampPos);
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

            if (obj is EmailAddress)
            {
                return Equals((EmailAddress)obj);
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
                return String.Format("{0}@{2}", User, Host);
        }
    }
}
