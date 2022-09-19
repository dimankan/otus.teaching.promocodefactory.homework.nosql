using System;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using Otus.Teaching.Pcf.Administration.DataAccess.Repositories;
using Otus.Teaching.Pcf.Administration.IntegrationTests.Data;
using Otus.Teaching.Pcf.Administration.WebHost.Controllers;
using Xunit;

namespace Otus.Teaching.Pcf.Administration.IntegrationTests.Components.WebHost.Controllers
{
    [Collection(EfDatabaseCollection.DbCollection)]
    public class EmployeesControllerTests: IClassFixture<EfDatabaseFixture>
    {
        private IMongoCollection<Employee> _employeesCollection;
        private EmployeesController _employeesController;

        public EmployeesControllerTests(EfDatabaseFixture efDatabaseFixture)
        {

            _employeesCollection = efDatabaseFixture.Db.GetCollection<Employee>("Employee");

            var employeesRepository = new AdministrationMongoService<Employee>(new AdministrationMongoDatabaseSettingsTest());

            _employeesController = new EmployeesController(employeesRepository);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ExistedEmployee_ExpectedId()
        {
            //Arrange
            var expectedEmployeeId = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f");

            //Act
            var result = await _employeesController.GetEmployeeByIdAsync(expectedEmployeeId);

            //Assert
            result.Value.Id.Should().Be(expectedEmployeeId);
        }
    }
}