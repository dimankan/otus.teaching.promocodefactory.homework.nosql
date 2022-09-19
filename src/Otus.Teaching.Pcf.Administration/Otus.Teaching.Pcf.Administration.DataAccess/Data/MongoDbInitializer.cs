using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.Core.Abstractions.Settings;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.Pcf.Administration.DataAccess.Data
{

    public class MongoDbInitializer : IDbInitializer
    {
        private readonly IMongoCollection<Employee> _employees;
        private readonly IMongoCollection<Role> _roles;

        public MongoDbInitializer(IAdministrationMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _employees = database.GetCollection<Employee>(settings.EmployeesCollectionName);
            _roles = database.GetCollection<Role>(settings.RolesCollectionName);
        }

        public void InitializeDb()
        {
            if (_employees.EstimatedDocumentCount() == 0)
                _employees.InsertManyAsync(FakeDataFactory.Employees);

            if (_roles.EstimatedDocumentCount() == 0)
                _roles.InsertManyAsync(FakeDataFactory.Roles);
        }
    }
}
