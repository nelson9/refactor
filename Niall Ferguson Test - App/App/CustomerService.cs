using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{
    public class CustomerService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerCreditService _customerCreditService;
       

        public CustomerService(ICompanyRepository companyRepository, ICustomerRepository customerRepository, ICustomerCreditService customerCreditService)
        {
            _companyRepository = companyRepository;
            _customerRepository = customerRepository;
            _customerCreditService = customerCreditService;
        }

        public bool AddCustomer(string firname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            if (!IsValidInput(firname, surname, email, dateOfBirth)) return false;

            var company = _companyRepository.GetById(companyId);
            var customer = new Customer
            {
                Company = company,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            switch (company.Name)
            {
                case "VeryImportantClient":
                    // Skip credit check
                    customer.HasCreditLimit = false;
                    break;
                case "ImportantClient":
                    // Do credit check and double credit limit
                    customer.HasCreditLimit = true;
                    customer.CreditLimit = GetCreditLimit(customer) * 2; 
                    break;
                default:
                    // Do credit check
                    customer.HasCreditLimit = true;
                    customer.CreditLimit = GetCreditLimit(customer);
                    break;
            }

            if (customer.HasCreditLimit && customer.CreditLimit < 500)
            {
                return false;
            }

            _customerRepository.AddCustomer(customer);

            return true;
        }

        private bool IsValidInput(string firname, string surname, string email, DateTime dateOfBirth)
        {
            return IsValidName(firname, surname) && IsValidEmail(email) && IsOverage(dateOfBirth);
        }

        private bool IsValidName(string firname, string surname)
        {
            return !string.IsNullOrEmpty(firname) || !string.IsNullOrEmpty(surname);
        }

        private bool IsValidEmail(string email)
        {
            return email.Contains("@") || email.Contains(".");
        }

        private bool IsOverage(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            return age >= 21;
        }

        private int GetCreditLimit(Customer customer)
        {
            return _customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname,
                customer.DateOfBirth);
        }
    }
}
