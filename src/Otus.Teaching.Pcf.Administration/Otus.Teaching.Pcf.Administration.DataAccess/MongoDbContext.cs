using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using Otus.Teaching.Pcf.Administration.DataAccess.MongoAdmin;

namespace Otus.Teaching.Pcf.Administration.DataAccess
{
    public class MongoDbContext : IMongoDbContext
    {
        public IMongoCollection<Employee> Employees { get; set; }
        public IMongoCollection<Role> Roles { get; set; }

        public MongoDbContext(IMongoAdministrationDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Employees = database.GetCollection<Employee>(settings.EmployeesCollectionName);
            Roles = database.GetCollection<Role>(settings.RolesCollectionName);
        }
    }
}
