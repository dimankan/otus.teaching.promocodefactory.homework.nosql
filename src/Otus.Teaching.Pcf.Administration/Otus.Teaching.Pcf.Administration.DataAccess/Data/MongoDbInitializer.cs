using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.Pcf.Administration.DataAccess.Data
{
    public class MongoDbInitializer:IDbInitializer
    {
        private readonly IMongoCollection<Employee> _employeeCollection;
        private readonly IMongoCollection<Role> _roleCollection;
        public MongoDbInitializer(IMongoCollection<Employee> employeeCollection, IMongoCollection<Role> roleCollection)
        {
            _employeeCollection = employeeCollection;
            _roleCollection = roleCollection;
        }
        public void InitializeDb()
        {
            _employeeCollection.InsertManyAsync(FakeDataFactory.Employees);
            _roleCollection.InsertManyAsync(FakeDataFactory.Roles);
        }
    }
}
