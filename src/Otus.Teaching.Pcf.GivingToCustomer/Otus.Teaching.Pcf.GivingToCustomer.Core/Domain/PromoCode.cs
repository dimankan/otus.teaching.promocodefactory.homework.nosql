using System;

namespace Otus.Teaching.Pcf.GivingToCustomer.Core.Domain
{
    public class PromoCode
        : BaseEntity
    {
        public string Code { get; set; }

        public string ServiceInfo { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid PartnerId { get; set; }        

        public Guid PreferenceId { get; set; }        
    }
}