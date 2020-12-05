using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Otus.Teaching.Pcf.GivingToCustomer.DataAccess;
using Otus.Teaching.Pcf.GivingToCustomer.WebHost.Models;

namespace Otus.Teaching.Pcf.GivingToCustomer.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения клиентов
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController
        : ControllerBase
    {   
        private IMongoDbContext mongoDbContext;

        public PreferencesController(IMongoDbContext mongoDbContext)
        {
            this.mongoDbContext = mongoDbContext;
        }

        /// <summary>
        /// Получить список предпочтений
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PreferenceResponse>>> GetPreferencesAsync()
        {
            var preferences = await mongoDbContext.Preferences.FindAsync(x => true);

            var response = preferences.ToList().Select(x => new PreferenceResponse()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return Ok(response);
        }
    }
}