using MongoDB.Driver;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Abstractions.Settings;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.Pcf.GivingToCustomer.DataAccess.Data
{

    public class MongoDbInitializer : IDbInitializer
    {
        private readonly IMongoCollection<Customer> _customers;
        private readonly IMongoCollection<Preference> _preferences;

        public MongoDbInitializer(IGivingToCustomerMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _customers = database.GetCollection<Customer>(settings.CustomersCollectionName);
            _preferences = database.GetCollection<Preference>(settings.PreferencesCollectionName);
        }

        public void InitializeDb()
        {
            if (_customers.EstimatedDocumentCount() == 0)
                _customers.Database.DropCollection(_customers.CollectionNamespace.CollectionName);
            _customers.InsertManyAsync(FakeDataFactory.Customers);

            if (_preferences.EstimatedDocumentCount() == 0)
                _preferences.Database.DropCollection(_preferences.CollectionNamespace.CollectionName);

            _preferences.InsertManyAsync(FakeDataFactory.Preferences);

        }
    }
}
