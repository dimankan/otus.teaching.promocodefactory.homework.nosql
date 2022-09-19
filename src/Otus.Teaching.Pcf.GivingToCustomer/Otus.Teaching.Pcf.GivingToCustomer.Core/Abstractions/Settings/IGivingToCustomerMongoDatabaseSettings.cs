using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.Pcf.GivingToCustomer.Core.Abstractions.Settings
{
    public interface IGivingToCustomerMongoDatabaseSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CustomersCollectionName { get; set; }

        public string CustomerPreferencesCollectionName { get; set; }

        public string PreferencesCollectionName { get; set; }

        public string PromoCodesCollectionName { get; set; }

        public string PromoCodeCustomersCollectionName { get; set; }
    }
}
