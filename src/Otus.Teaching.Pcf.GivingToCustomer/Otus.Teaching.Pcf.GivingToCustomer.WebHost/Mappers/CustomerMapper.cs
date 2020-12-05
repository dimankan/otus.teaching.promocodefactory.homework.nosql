using System;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Domain;
using Otus.Teaching.Pcf.GivingToCustomer.WebHost.Models;

namespace Otus.Teaching.Pcf.GivingToCustomer.WebHost.Mappers
{
    public class CustomerMapper
    {

        public static Customer MapFromModel(CreateOrEditCustomerRequest model, Customer customer = null)
        {
            if(customer == null)
            {
                customer = new Customer();
                customer.Id = Guid.NewGuid();
            }
            
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Email = model.Email;
            
            return customer;
        }
    }
}
