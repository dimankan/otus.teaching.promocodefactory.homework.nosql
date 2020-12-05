using MongoDB.Driver;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Domain;
using Otus.Teaching.Pcf.GivingToCustomer.DataAccess.Mongo;
using System;

namespace Otus.Teaching.Pcf.GivingToCustomer.DataAccess.Data
{
    public class MongoDbInitializer : IDbInitializer
    {
        private IMongoCollection<Customer> _customers;
        private IMongoCollection<Preference> _preferences;
        private IMongoCollection<CustomerPreference> _customerPreferences;

        public MongoDbInitializer(IMongoGivingToCustomerDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _customers = database.GetCollection<Customer>(settings.CustomersCollectionName);
            _preferences = database.GetCollection<Preference>(settings.PreferencesCollectionName);
            _customerPreferences = database.GetCollection<CustomerPreference>(settings.CustomerPreferenceCollectionName);
        }

        public void InitializeDb()
        {
            try
            {
                _customerPreferences.InsertMany(FakeDataFactory.CustomerPreference);
                _customers.InsertMany(FakeDataFactory.Customers);
                _preferences.InsertMany(FakeDataFactory.Preferences);
            }
            catch(Exception)
            {

            }
        }
    }
}
