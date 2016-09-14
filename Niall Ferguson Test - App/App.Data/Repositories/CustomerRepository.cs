using App.Models;

namespace App.Data.Repositories
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
