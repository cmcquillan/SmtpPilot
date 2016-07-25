using SmtpPilot.Server.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Conversation
{
    public static class AddressComparer
    {
        public static FullAddressComparer FullAddress { get { return new FullAddressComparer(); } }

        public static UserDomainComparer UserDomain { get { return new UserDomainComparer(); } }
    }

    public class UserDomainComparer : IEqualityComparer<EmailAddress>
    {
        public bool Equals(EmailAddress x, EmailAddress y)
        {
            return String.Equals(x.User, y.User)
                && String.Equals(x.Host, y.Host);
        }

        public int GetHashCode(EmailAddress obj)
        {
            int hash = 31;
            hash = hash * 17 * obj.Host.GetHashCode();
            hash = hash * 17 * obj.User.GetHashCode();

            return hash;
        }
    }

    public class FullAddressComparer : IEqualityComparer<EmailAddress>
    {
        public bool Equals(EmailAddress x, EmailAddress y)
        {
            return String.Equals(x.Address, y.Address)
                && String.Equals(x.DisplayName, y.DisplayName)
                && String.Equals(x.Host, y.Host)
                && String.Equals(x.User, y.User)
                && x.Type == y.Type;
        }

        public int GetHashCode(EmailAddress obj)
        {
            int hash = 31;
            hash = hash * 17 + obj.Address.GetHashCode();
            hash = hash * 17 + obj.DisplayName.GetHashCode();
            hash = hash * 17 + obj.Host.GetHashCode();
            hash = hash * 17 + obj.User.GetHashCode();
            hash = hash * 17 + obj.Type.GetHashCode();
            return hash;
        }
    }
}
