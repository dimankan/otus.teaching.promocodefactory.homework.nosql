using MongoDB.Driver;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Domain;
using Otus.Teaching.Pcf.GivingToCustomer.DataAccess;
using Otus.Teaching.Pcf.GivingToCustomer.DataAccess.Data;

namespace Otus.Teaching.Pcf.GivingToCustomer.IntegrationTests.Data
{
    public class EfTestDbInitializer : IDbInitializer
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<Customer> _customersCollection;
        private readonly IMongoCollection<Preference> _preferenceCollection;

        public EfTestDbInitializer(IMongoDatabase db)
        {
            _db = db;
            _customersCollection = _db.GetCollection<Customer>("Customer");
            _preferenceCollection = _db.GetCollection<Preference>("Preference");
        }
        
        public void InitializeDb()
        {
            var _customersCollection = _db.GetCollection<Customer>("Customer");
            var _preferenceCollection = _db.GetCollection<Preference>("Preference");

            if (_customersCollection.EstimatedDocumentCount() == 0)
                _customersCollection.InsertManyAsync(TestDataFactory.Customers);

            if (_preferenceCollection.EstimatedDocumentCount() == 0)
                _preferenceCollection.InsertManyAsync(TestDataFactory.Preferences);
        }

        public void CleanDb()
        {
            if (_customersCollection.EstimatedDocumentCount() > 0)
                _customersCollection.DeleteMany(x => true);

            if (_preferenceCollection.EstimatedDocumentCount() > 0)
                _preferenceCollection.DeleteMany(x => true);
        }
    }
}