using Otus.Teaching.Pcf.Administration.Core.Abstractions.Repositories;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;

namespace Otus.Teaching.Pcf.Administration.DataAccess.Data
{
    public class MongoDbInitializer
        : IDbInitializer
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public MongoDbInitializer(IRepository<Role> roleRepository, IRepository<Employee> employeeRepository)
        {
            _roleRepository = roleRepository;
            _employeeRepository = employeeRepository;
        }

        public void InitializeDb()
        {
            foreach (var role in FakeDataFactory.Roles)
            {
                _roleRepository.AddAsync(role);
            }

            foreach (var employee in FakeDataFactory.Employees)
            {
                _employeeRepository.AddAsync(employee);
            }
        }
    }
}