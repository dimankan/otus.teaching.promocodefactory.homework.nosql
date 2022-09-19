using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using Otus.Teaching.Pcf.Administration.DataAccess;
using Otus.Teaching.Pcf.Administration.DataAccess.Data;

namespace Otus.Teaching.Pcf.Administration.IntegrationTests.Data
{
    public class MongoTestDbInitializer : IDbInitializer
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<Employee> _employeeCollection;
        private readonly IMongoCollection<Role> _roleCollection;

        public MongoTestDbInitializer(IMongoDatabase db)
        {
            _db = db;
            _employeeCollection = _db.GetCollection<Employee>("Employee");
            _roleCollection = _db.GetCollection<Role>("Role");
        }

        public void InitializeDb()
        {
            var _employeeCollection = _db.GetCollection<Employee>("Employee");
            var _roleCollection = _db.GetCollection<Role>("Role");

            if (_roleCollection.EstimatedDocumentCount() == 0)
                _roleCollection.InsertManyAsync(TestDataFactory.Roles);

            if (_employeeCollection.EstimatedDocumentCount() == 0)
                _employeeCollection.InsertManyAsync(TestDataFactory.Employees);
        }

        public void CleanDb()
        {
            if (_roleCollection.EstimatedDocumentCount() > 0)
                _roleCollection.DeleteMany(x => true);

            if (_employeeCollection.EstimatedDocumentCount() > 0)
                _employeeCollection.DeleteMany(x => true);
        }
    }
}