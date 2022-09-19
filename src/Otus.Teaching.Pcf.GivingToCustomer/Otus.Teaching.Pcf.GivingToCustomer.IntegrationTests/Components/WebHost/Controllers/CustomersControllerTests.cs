using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.IntegrationTests.Data;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Domain;
using Otus.Teaching.Pcf.GivingToCustomer.DataAccess.Repositories;
using Otus.Teaching.Pcf.GivingToCustomer.WebHost.Controllers;
using Otus.Teaching.Pcf.GivingToCustomer.WebHost.Models;
using Xunit;

namespace Otus.Teaching.Pcf.GivingToCustomer.IntegrationTests.Components.WebHost.Controllers
{
    [Collection(EfDatabaseCollection.DbCollection)]
    public class CustomersControllerTests: IClassFixture<EfDatabaseFixture>
    {
        private readonly CustomersController _customersController;
        private IMongoCollection<Customer> _customersCollection;
        private IMongoCollection<Preference> _preferencesCollection;
        private readonly GivingToCustomerMongoService<Customer> _customerRepository;
        private readonly GivingToCustomerMongoService<Preference> _preferenceRepository;


        public CustomersControllerTests(EfDatabaseFixture efDatabaseFixture)
        {
            var settingsTest = new GivingToCustomerMongoDatabaseSettingsTest();

            _customersCollection = efDatabaseFixture.Db.GetCollection<Customer>("Customer");
            _preferencesCollection = efDatabaseFixture.Db.GetCollection<Preference>("Preference");

            _customerRepository = new GivingToCustomerMongoService<Customer>(settingsTest);
            _preferenceRepository = new GivingToCustomerMongoService<Preference>(settingsTest);

            _customersController = new CustomersController(
                _customerRepository, 
                _preferenceRepository);
        }
        
        [Fact]
        public async Task CreateCustomerAsync_CanCreateCustomer_ShouldCreateExpectedCustomer()
        {
            //Arrange 
            var preferenceId = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c");
            var request = new CreateOrEditCustomerRequest()
            {
                Email = "some@mail.ru",
                FirstName = "Иван",
                LastName = "Петров",
                PreferenceIds = new List<Guid>()
                {
                    preferenceId
                }
            };

            //Act
            var result = await _customersController.CreateCustomerAsync(request);
            var actionResult = result.Result as CreatedAtActionResult;
            var id = (Guid)actionResult.Value;
            
            //Assert
            var actual = await _customerRepository.GetByIdAsync(id);
            
            actual.Email.Should().Be(request.Email);
            actual.FirstName.Should().Be(request.FirstName);
            actual.LastName.Should().Be(request.LastName);
            actual.Preferences.Should()
                .ContainSingle()
                .And
                .Contain(x => x.PreferenceId == preferenceId);
        }
    }
}