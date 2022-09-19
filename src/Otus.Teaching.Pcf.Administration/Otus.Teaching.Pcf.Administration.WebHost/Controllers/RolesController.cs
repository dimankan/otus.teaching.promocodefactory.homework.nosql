using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.Pcf.Administration.WebHost.Models;
using Otus.Teaching.Pcf.Administration.Core.Abstractions.Repositories;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using Otus.Teaching.Pcf.Administration.DataAccess.Repositories;

namespace Otus.Teaching.Pcf.Administration.WebHost.Controllers
{
    /// <summary>
    /// Роли сотрудников
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RolesController
    {
        private readonly AdministrationMongoService<Role> _rolesRepository;

        public RolesController(AdministrationMongoService<Role> rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }
        
        /// <summary>
        /// Получить все доступные роли сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<RoleItemResponse>> GetRolesAsync()
        {
            var roles = await _rolesRepository.GetAllAsync();

            var rolesModelList = roles.Select(x => 
                new RoleItemResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();

            return rolesModelList;
        }
    }
}