using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Domain;
using Otus.Teaching.Pcf.GivingToCustomer.DataAccess;
using Otus.Teaching.Pcf.GivingToCustomer.WebHost.Mappers;
using Otus.Teaching.Pcf.GivingToCustomer.WebHost.Models;

namespace Otus.Teaching.Pcf.GivingToCustomer.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IMongoDbContext mongoDbContext;

        public CustomersController(IMongoDbContext mongoDbContext)
        {
            this.mongoDbContext = mongoDbContext;
        }
        
        /// <summary>
        /// Получить список клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<CustomerShortResponse>>> GetCustomersAsync()
        {
            var customers =  await mongoDbContext.Customers.FindAsync(x => true);

            var response = customers.ToList().Select(x => new CustomerShortResponse()
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToList();

            return Ok(response);
        }
        
        /// <summary>
        /// Получить клиента по id
        /// </summary>
        /// <param name="id">Id клиента, например <example>a6c8c6b1-4349-45b0-ab31-244740aaf0f0</example></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customers =  await mongoDbContext.Customers.FindAsync(x => x.Id == id);
            var customer = customers?.SingleOrDefault();

            if (customer == null)
                return NotFound();

            var preferenceIds = (await mongoDbContext.CustomerPreferences.FindAsync(x => x.CustomerId == customer.Id)).ToEnumerable().Select(x => x.PreferenceId);
            var preferences = (await mongoDbContext.Preferences.FindAsync(x => preferenceIds.Contains(x.Id))).ToEnumerable();
            var promoCodeIds = (await mongoDbContext.PromoCodeCustomers.FindAsync(x => x.CustomerId == customer.Id)).ToEnumerable().Select(x => x.PromoCodeId);
            var promoCodes = (await mongoDbContext.PromoCodes.FindAsync(x => promoCodeIds.Contains(x.Id))).ToEnumerable();

            var response = new CustomerResponse(customer, preferences, promoCodes);

            return Ok(response);
        }
        
        /// <summary>
        /// Создать нового клиента
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CustomerResponse>> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            Customer customer = CustomerMapper.MapFromModel(request);
            
            await mongoDbContext.Customers.InsertOneAsync(customer);
            await mongoDbContext.CustomerPreferences.InsertManyAsync(request.PreferenceIds.Select(x => new CustomerPreference() { CustomerId = customer.Id, PreferenceId = x }));

            return CreatedAtAction(nameof(GetCustomerAsync), new {id = customer.Id}, customer.Id);
        }
        
        /// <summary>
        /// Обновить клиента
        /// </summary>
        /// <param name="id">Id клиента, например <example>a6c8c6b1-4349-45b0-ab31-244740aaf0f0</example></param>
        /// <param name="request">Данные запроса></param>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customers = await mongoDbContext.Customers.FindAsync(x => x.Id == id);
            var customer = customers?.SingleOrDefault();
            
            if (customer == null)
                return NotFound();            
            
            CustomerMapper.MapFromModel(request, customer);

            await mongoDbContext.Customers.ReplaceOneAsync(x => x.Id == id, customer);
            await mongoDbContext.CustomerPreferences.DeleteManyAsync(x => x.CustomerId == id);
            await mongoDbContext.CustomerPreferences.InsertManyAsync(request.PreferenceIds.Select(preferenceId => 
                new CustomerPreference() { CustomerId = id, PreferenceId = preferenceId }));

            return NoContent();
        }
        
        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="id">Id клиента, например <example>a6c8c6b1-4349-45b0-ab31-244740aaf0f0</example></param>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCustomerAsync(Guid id)
        {
            var customers = await mongoDbContext.Customers.FindOneAndDeleteAsync(x => x.Id == id);
            return NoContent();
        }
    }
}