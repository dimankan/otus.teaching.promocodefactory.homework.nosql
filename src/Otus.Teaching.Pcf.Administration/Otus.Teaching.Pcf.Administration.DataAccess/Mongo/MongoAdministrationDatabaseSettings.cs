namespace Otus.Teaching.Pcf.Administration.DataAccess.MongoAdmin
{
    public class MongoAdministrationDatabaseSettings : IMongoAdministrationDatabaseSettings
    {
        public string EmployeesCollectionName { get; set; }
        public string RolesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoAdministrationDatabaseSettings
    {
        string EmployeesCollectionName { get; set; }
        string RolesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
