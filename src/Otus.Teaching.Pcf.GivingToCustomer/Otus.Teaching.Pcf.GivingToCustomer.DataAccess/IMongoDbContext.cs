using MongoDB.Driver;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Domain;

namespace Otus.Teaching.Pcf.GivingToCustomer.DataAccess
{
    public interface IMongoDbContext
    {
        IMongoCollection<PromoCode> PromoCodes { get; set; }
        IMongoCollection<Preference> Preferences { get; set; }
        IMongoCollection<Customer> Customers { get; set; }
        IMongoCollection<CustomerPreference> CustomerPreferences { get; set; }
        IMongoCollection<PromoCodeCustomer> PromoCodeCustomers { get; set; }
    }
}
