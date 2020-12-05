namespace Otus.Teaching.Pcf.GivingToCustomer.DataAccess.Mongo
{
    public class MongoGivingToCustomerDatabaseSettings : IMongoGivingToCustomerDatabaseSettings
    {
        public string PromoCodesCollectionName { get; set; }
        public string CustomersCollectionName { get; set; }
        public string PreferencesCollectionName { get; set; }
        public string CustomerPreferenceCollectionName { get; set; }
        public string PromoCodeCustomerCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoGivingToCustomerDatabaseSettings
    {
        public string PromoCodesCollectionName { get; set; }
        public string CustomersCollectionName { get; set; }
        public string PreferencesCollectionName { get; set; }
        public string CustomerPreferenceCollectionName { get; set; }
        public string PromoCodeCustomerCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
