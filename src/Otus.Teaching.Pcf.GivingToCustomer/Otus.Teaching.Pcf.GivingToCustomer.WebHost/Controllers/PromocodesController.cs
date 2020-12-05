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
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private IMongoDbContext mongoDbContext;

        public PromocodesController(IMongoDbContext mongoDbContext)
        {
            this.mongoDbContext = mongoDbContext;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promocodes = await mongoDbContext.PromoCodes.FindAsync(x => true);

            var response = promocodes.ToList().Select(x => new PromoCodeShortResponse()
            {
                Id = x.Id,
                Code = x.Code,
                BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                PartnerId = x.PartnerId,
                ServiceInfo = x.ServiceInfo
            }).ToList();

            return Ok(response);
        }
        
        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //  Получаем клиентов с этим предпочтением:
            var customerPreferences = await mongoDbContext.CustomerPreferences.FindAsync(x => x.PreferenceId == request.PreferenceId);
            var customerIds = customerPreferences.ToList().Select(x => x.CustomerId);
            var customers = await mongoDbContext.Customers.FindAsync(x => customerIds.Contains(x.Id));

            PromoCode promoCode = PromoCodeMapper.MapFromModel(request);
            var promoCodeCustomers = customers.ToList().Select(customer => new PromoCodeCustomer() { CustomerId = customer.Id, PromoCodeId = promoCode.Id });

            await mongoDbContext.PromoCodeCustomers.InsertManyAsync(promoCodeCustomers);
            await mongoDbContext.PromoCodes.InsertOneAsync(promoCode);

            return CreatedAtAction(nameof(GetPromocodesAsync), new { }, null);
        }
    }
}