using System;
using Moq;
using NUnit.Framework;


namespace App.UnitTests
{
    [TestFixture]
    public class CustomerServiceTest
    {
        private CustomerService _customerService;
        [SetUp]
        public void Setup()
        {
            var companyRepository = new Mock<ICompanyRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            var customerCreditService = new Mock<ICustomerCreditService>();
            _customerService = new CustomerService(companyRepository.Object, customerRepository.Object, customerCreditService.Object);
        }


        [Test]
        public void AddCustomer_FirstNameNull_ReturnFalse()
        {
            //Arrrange
            string firname = null;
            string surname = "surname";
            string email = "email";
            DateTime dateOfBirth = new DateTime();
            int companyId = 1;

            //Act
            var result = _customerService.AddCustomer(firname, surname, email, dateOfBirth, companyId);
            //Assert

            Assert.IsFalse(result);
        }

        [Test]
        public void AddCustomer_FirstNameEmpty_ReturnFalse()
        {
            //Arrrange
            string firname = string.Empty;
            string surname = "surname";
            string email = "email";
            DateTime dateOfBirth = new DateTime();
            int companyId = 1;

            //Act
            var result = _customerService.AddCustomer(firname, surname, email, dateOfBirth, companyId);
            //Assert

            Assert.IsFalse(result);
        }

        [Test]
        public void AddCustomer_SurnameNull_ReturnFalse()
        {
            //Arrrange
            string firname = "firname";
            string surname = null;
            string email = "email";
            DateTime dateOfBirth = new DateTime();
            int companyId = 1;

            //Act
            var result = _customerService.AddCustomer(firname, surname, email, dateOfBirth, companyId);
            //Assert

            Assert.IsFalse(result);
        }

        [Test]
        public void AddCustomer_SurnameEmpty_ReturnFalse()
        {
            //Arrrange
            string firname = "firname";
            string surname = string.Empty;
            string email = "email";
            DateTime dateOfBirth = new DateTime();
            int companyId = 1;

            //Act
            var result = _customerService.AddCustomer(firname, surname, email, dateOfBirth, companyId);
            //Assert

            Assert.IsFalse(result);
        }

        [Test]
        public void AddCustomer_EmailNoAtSignOrDot_ReturnFalse()
        {
            //Arrrange
            string firname = "firname";
            string surname = "surname";
            string email = "email";
            DateTime dateOfBirth = new DateTime();
            int companyId = 1;

            //Act
            var result = _customerService.AddCustomer(firname, surname, email, dateOfBirth, companyId);
            //Assert

            Assert.IsFalse(result);
        }

        [Test]
        public void AddCustomer_Under21_ReturnFalse()
        {
            //Arrrange
            string firname = "firname";
            string surname = "surname";
            string email = "email@email.com";
            DateTime dateOfBirth = DateTime.Now;
         
            int companyId = 1;

            //Act
            var result = _customerService.AddCustomer(firname, surname, email, dateOfBirth.AddYears(-20), companyId);
            
            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddCustomer_IsVeryImportantClient_ReturnTrue()
        {
            //Arrrange
            var companyRepository = new Mock<ICompanyRepository>();
            var customerDataAccessRepository = new Mock<ICustomerRepository>();
            var customerCreditService = new Mock<ICustomerCreditService>();

            companyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Company
            {
                Name = "VeryImportantClient"
            });

            var customerService = new CustomerService(companyRepository.Object, customerDataAccessRepository.Object, customerCreditService.Object);
            string firname = "firname";
            string surname = "surname";
            string email = "email@email.com";
            DateTime dateOfBirth = DateTime.Now;

            int companyId = 1;

            //Act
            var result = customerService.AddCustomer(firname, surname, email, dateOfBirth.AddYears(-22), companyId);

            //Assert
            Assert.IsTrue(result);
            customerDataAccessRepository.Verify(x => x.AddCustomer(It.IsAny<Customer>()), Times.AtLeastOnce);
        }

        [Test]
        public void AddCustomer_IsImportantClientCreditMoreThan500_ReturnTrue()
        {
            //Arrrange
            var companyRepository = new Mock<ICompanyRepository>();
            var customerDataAccessRepository = new Mock<ICustomerRepository>();
            var customerCreditService = new Mock<ICustomerCreditService>();

            companyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Company
            {
                Name = "ImportantClient"
            });
            customerCreditService.Setup(
                x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(300);
            var customerService = new CustomerService(companyRepository.Object, customerDataAccessRepository.Object, customerCreditService.Object);
            string firname = "firname";
            string surname = "surname";
            string email = "email@email.com";
            DateTime dateOfBirth = DateTime.Now;

            int companyId = 1;

            //Act
            var result = customerService.AddCustomer(firname, surname, email, dateOfBirth.AddYears(-22), companyId);

            //Assert
            Assert.IsTrue(result);
            customerDataAccessRepository.Verify(x => x.AddCustomer(It.IsAny<Customer>()), Times.AtLeastOnce);
        }

        [Test]
        public void AddCustomer_IsImportantClientCreditLessThan500_ReturnFalse()
        {
            //Arrrange
            var companyRepository = new Mock<ICompanyRepository>();
            var customerDataAccessRepository = new Mock<ICustomerRepository>();
            var customerCreditService = new Mock<ICustomerCreditService>();

            companyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Company
            {
                Name = "ImportantClient"
            });
            customerCreditService.Setup(
                x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(100);
            var customerService = new CustomerService(companyRepository.Object, customerDataAccessRepository.Object, customerCreditService.Object);
            string firname = "firname";
            string surname = "surname";
            string email = "email@email.com";
            DateTime dateOfBirth = DateTime.Now;

            int companyId = 1;

            //Act
            var result = customerService.AddCustomer(firname, surname, email, dateOfBirth.AddYears(-22), companyId);

            //Assert
            Assert.IsFalse(result);
            customerDataAccessRepository.Verify(x => x.AddCustomer(It.IsAny<Customer>()), Times.Never);
        }

        [Test]
        public void AddCustomer_ClientCreditMoreThan500_ReturnTrue()
        {
            //Arrrange
            var companyRepository = new Mock<ICompanyRepository>();
            var customerDataAccessRepository = new Mock<ICustomerRepository>();
            var customerCreditService = new Mock<ICustomerCreditService>();

            companyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Company());
            customerCreditService.Setup(
                x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(550);
            var customerService = new CustomerService(companyRepository.Object, customerDataAccessRepository.Object, customerCreditService.Object);
            string firname = "firname";
            string surname = "surname";
            string email = "email@email.com";
            DateTime dateOfBirth = DateTime.Now;

            int companyId = 1;

            //Act
            var result = customerService.AddCustomer(firname, surname, email, dateOfBirth.AddYears(-22), companyId);

            //Assert
            Assert.IsTrue(result);
            customerDataAccessRepository.Verify(x => x.AddCustomer(It.IsAny<Customer>()), Times.AtLeastOnce);
        }

        [Test]
        public void AddCustomer_ClientCreditLessThan500_ReturnFalse()
        {
            //Arrrange
            var companyRepository = new Mock<ICompanyRepository>();
            var customerDataAccessRepository = new Mock<ICustomerRepository>();
            var customerCreditService = new Mock<ICustomerCreditService>();

            companyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Company());
            customerCreditService.Setup(
                x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(100);
            var customerService = new CustomerService(companyRepository.Object, customerDataAccessRepository.Object, customerCreditService.Object);
            string firname = "firname";
            string surname = "surname";
            string email = "email@email.com";
            DateTime dateOfBirth = DateTime.Now;

            int companyId = 1;

            //Act
            var result = customerService.AddCustomer(firname, surname, email, dateOfBirth.AddYears(-22), companyId);

            //Assert
            Assert.IsFalse(result);
            customerDataAccessRepository.Verify(x => x.AddCustomer(It.IsAny<Customer>()), Times.Never);
        }
    }
}
