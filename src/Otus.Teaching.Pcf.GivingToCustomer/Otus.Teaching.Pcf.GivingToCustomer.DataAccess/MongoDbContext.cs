using MongoDB.Driver;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Domain;
using Otus.Teaching.Pcf.GivingToCustomer.DataAccess.Mongo;

namespace Otus.Teaching.Pcf.GivingToCustomer.DataAccess
{
    public class MongoDbContext : IMongoDbContext
    {
        public IMongoCollection<PromoCode> PromoCodes { get; set; }
        public IMongoCollection<Preference> Preferences { get; set; }
        public IMongoCollection<Customer> Customers { get; set; }
        public IMongoCollection<CustomerPreference> CustomerPreferences { get; set; }
        public IMongoCollection<PromoCodeCustomer> PromoCodeCustomers { get; set; }

        public MongoDbContext(IMongoGivingToCustomerDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            PromoCodes = database.GetCollection<PromoCode>(settings.PromoCodesCollectionName);
            Preferences = database.GetCollection<Preference>(settings.PreferencesCollectionName);
            Customers = database.GetCollection<Customer>(settings.CustomersCollectionName);
            CustomerPreferences = database.GetCollection<CustomerPreference>(settings.CustomerPreferenceCollectionName);
            PromoCodeCustomers = database.GetCollection<PromoCodeCustomer>(settings.PromoCodeCustomerCollectionName);
        }
    }
}
