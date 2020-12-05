using System;

namespace Otus.Teaching.Pcf.GivingToCustomer.Core.Domain
{
    public class PromoCodeCustomer : BaseEntity
    {
        public Guid PromoCodeId { get; set; }

        public Guid CustomerId { get; set; }
    }
}
