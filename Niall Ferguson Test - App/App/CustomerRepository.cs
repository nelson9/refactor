using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
    }

    public class CustomerRepository : ICustomerRepository
    {
        public void AddCustomer(Customer customer)
        {
            CustomerDataAccess.AddCustomer(customer);
        }
    }
}
