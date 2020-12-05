using System;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Domain;
using Otus.Teaching.Pcf.GivingToCustomer.WebHost.Models;

 namespace Otus.Teaching.Pcf.GivingToCustomer.WebHost.Mappers
{
    public class PromoCodeMapper
    {
        public static PromoCode MapFromModel(GivePromoCodeRequest request) {

            var promocode = new PromoCode();
            promocode.Id = request.PromoCodeId;
            
            promocode.PartnerId = request.PartnerId;
            promocode.Code = request.PromoCode;
            promocode.ServiceInfo = request.ServiceInfo;
           
            promocode.BeginDate = DateTime.Parse(request.BeginDate);
            promocode.EndDate = DateTime.Parse(request.EndDate);

            promocode.PreferenceId = request.PreferenceId;

            return promocode;
        }
    }
}
