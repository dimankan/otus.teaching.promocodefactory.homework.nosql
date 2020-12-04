using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.Pcf.Administration.WebHost.Models;
using Otus.Teaching.Pcf.Administration.Core.Abstractions.Repositories;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using Otus.Teaching.Pcf.Administration.DataAccess;
using MongoDB.Driver;

namespace Otus.Teaching.Pcf.Administration.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController
        : ControllerBase
    {
        private readonly IMongoDbContext _mongoDbContext;

        public EmployeesController(IMongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }
        
        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _mongoDbContext.Employees.FindAsync(x => true);

            var employeesModelList = employees.ToList().Select(x => 
                new EmployeeShortResponse()
                    {
                        Id = x.Id,
                        Email = x.Email,
                        FullName = x.FullName,
                    }).ToList();

            return employeesModelList;
        }
        
        /// <summary>
        /// Получить данные сотрудника по id
        /// </summary>
        /// <param name="id">Id сотрудника, например <example>451533d5-d8d5-4a11-9c7b-eb9f14e1a32f</example></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employees = await _mongoDbContext.Employees.FindAsync(x => x.Id == id);
            var employee = employees?.SingleOrDefault();

            if (employee == null)
                return NotFound();

            var roles = await _mongoDbContext.Roles.FindAsync(x => x.Id == employee.RoleId);
            var role = roles.SingleOrDefault();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Role = new RoleItemResponse()
                {
                    Id = employee.Id,
                    Name = role.Name,
                    Description = role.Description
                },
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }
        
        /// <summary>
        /// Обновить количество выданных промокодов
        /// </summary>
        /// <param name="id">Id сотрудника, например <example>451533d5-d8d5-4a11-9c7b-eb9f14e1a32f</example></param>
        /// <returns></returns>
        [HttpPost("{id:guid}/appliedPromocodes")]
        
        public async Task<IActionResult> UpdateAppliedPromocodesAsync(Guid id)
        {
            var employees = await _mongoDbContext.Employees.FindAsync(x => x.Id == id);
            var employee = employees?.SingleOrDefault();

            if (employee == null)
                return NotFound();

            employee.AppliedPromocodesCount++;

            await _mongoDbContext.Employees.ReplaceOneAsync(x => x.Id == id, employee);

            return Ok();
        }
    }
}