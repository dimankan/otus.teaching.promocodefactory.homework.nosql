using Otus.Teaching.Pcf.GivingToCustomer.Core.Abstractions.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.Pcf.Administration.IntegrationTests.Data
{
    public class GivingToCustomerMongoDatabaseSettingsTest : IGivingToCustomerMongoDatabaseSettings
    {

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CustomersCollectionName { get; set; }

        public string CustomerPreferencesCollectionName { get; set; }

        public string PreferencesCollectionName { get; set; }

        public string PromoCodesCollectionName { get; set; }

        public string PromoCodeCustomersCollectionName { get; set; }

        public GivingToCustomerMongoDatabaseSettingsTest()
        {
            CustomersCollectionName = "Customers";
            CustomerPreferencesCollectionName = "CustomerPreferences";
            PreferencesCollectionName = "Preferences";
            PromoCodesCollectionName = "PromoCodes";
            PromoCodeCustomersCollectionName = "PromoCodeCustomers";
            ConnectionString = "mongodb://localhost:27018";
            DatabaseName = "TestDb";
        }
    }
}
