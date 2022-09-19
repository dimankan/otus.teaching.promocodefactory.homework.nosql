using System;
using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.IntegrationTests.Data;

namespace Otus.Teaching.Pcf.Administration.IntegrationTests
{
    public class EfDatabaseFixture: IDisposable
    {
        private readonly MongoTestDbInitializer _efTestDbInitializer;
        public IMongoDatabase Db { get; private set; }
        public AdministrationMongoDatabaseSettingsTest _settings { get; private set; }

        public EfDatabaseFixture()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            Db = mongoClient.GetDatabase("TestDb");
            _efTestDbInitializer = new MongoTestDbInitializer(Db);
            _settings = new AdministrationMongoDatabaseSettingsTest();
            _efTestDbInitializer.InitializeDb();
        }

        public void Dispose()
        {
            _efTestDbInitializer.CleanDb();
        }
    }
}