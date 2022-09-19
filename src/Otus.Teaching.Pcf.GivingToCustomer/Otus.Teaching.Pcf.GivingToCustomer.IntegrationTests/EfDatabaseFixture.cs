using System;
using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.IntegrationTests.Data;
using Otus.Teaching.Pcf.GivingToCustomer.IntegrationTests.Data;

namespace Otus.Teaching.Pcf.GivingToCustomer.IntegrationTests
{
    public class EfDatabaseFixture: IDisposable
    {
        private readonly EfTestDbInitializer _efTestDbInitializer;
        public IMongoDatabase Db { get; private set; }
        public GivingToCustomerMongoDatabaseSettingsTest _settings { get; private set; }

        public EfDatabaseFixture()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27018");
            Db = mongoClient.GetDatabase("TestDb");
            _efTestDbInitializer = new EfTestDbInitializer(Db);
            _settings = new GivingToCustomerMongoDatabaseSettingsTest();
            _efTestDbInitializer.InitializeDb();
        }

        public void Dispose()
        {
            _efTestDbInitializer.CleanDb();
        }
    }
}