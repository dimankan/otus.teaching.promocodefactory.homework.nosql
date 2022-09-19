using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.Pcf.Administration.Core.Abstractions.Settings
{
    public interface IAdministrationMongoDatabaseSettings
    {
        public string EmployeesCollectionName { get; set; }

        public string RolesCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
