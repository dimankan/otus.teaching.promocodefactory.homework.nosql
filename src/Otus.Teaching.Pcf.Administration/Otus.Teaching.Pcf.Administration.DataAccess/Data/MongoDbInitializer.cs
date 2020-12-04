using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using Otus.Teaching.Pcf.Administration.DataAccess.MongoAdmin;
using System;

namespace Otus.Teaching.Pcf.Administration.DataAccess.Data
{
    public class MongoDbInitializer : IDbInitializer
    {
        private IMongoCollection<Employee> _employees;
        private IMongoCollection<Role> _roles;

        public MongoDbInitializer(IMongoAdministrationDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _employees = database.GetCollection<Employee>(settings.EmployeesCollectionName);
            _roles = database.GetCollection<Role>(settings.RolesCollectionName);
        }

        public void InitializeDb()
        {
            try
            {
                _employees.InsertMany(FakeDataFactory.Employees);
                _roles.InsertMany(FakeDataFactory.Roles);
            }
            catch (Exception)
            { }
        }
    }
}
