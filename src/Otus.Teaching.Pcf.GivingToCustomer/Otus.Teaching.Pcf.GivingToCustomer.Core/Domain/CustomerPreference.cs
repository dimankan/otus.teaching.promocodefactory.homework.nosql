using System;

namespace Otus.Teaching.Pcf.GivingToCustomer.Core.Domain
{
    public class CustomerPreference : BaseEntity
    {
        public Guid CustomerId { get; set; }

        public Guid PreferenceId { get; set; }
    }
}