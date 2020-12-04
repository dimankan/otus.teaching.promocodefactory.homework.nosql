using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;

namespace Otus.Teaching.Pcf.Administration.DataAccess
{
    public interface IMongoDbContext
    {
        IMongoCollection<Employee> Employees { get; set; }
        IMongoCollection<Role> Roles { get; set; }
    }
}