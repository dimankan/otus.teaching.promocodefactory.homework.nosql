using FluentAssertions;
using Moq;
using Otus.Teaching.Pcf.Administration.Core.Abstractions.Repositories;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using Otus.Teaching.Pcf.Administration.WebHost.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Otus.Teaching.Pcf.Administration.IntegrationTests.Components.WebHost.Controllers
{
    public class EmployeesControllerTests
    {
        private Mock<IRepository<Employee>> _employeesRepository;
        private EmployeesController _employeesController;

        public EmployeesControllerTests()
        {
            _employeesRepository = new Mock<IRepository<Employee>>();
            _employeesController = new EmployeesController(_employeesRepository.Object);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ExistedEmployee_ExpectedId()
        {
            //Arrange
            var expectedEmployeeId = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f");

            _employeesRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new Employee()
            {
                Id = expectedEmployeeId,
                Role = new Role()
            }));

            //Act
            var result = await _employeesController.GetEmployeeByIdAsync(expectedEmployeeId);

            //Assert
            result.Value.Id.Should().Be(expectedEmployeeId);
        }
    }
}