using erp_api.Entities;
using erp_api.Models;
using erp_api.Repositories;

namespace erp_api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }
        async Task<Customer> ICustomerService.Create(CustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Age = dto.Age,
                City = dto.City,
            };

            return await _repository.AddAsync(customer);
        }

        async Task ICustomerService.Delete(int Id)
        {
            await _repository.DeleteAsync(Id);
        }

        async Task<IEnumerable<Customer>> ICustomerService.GetAll()
        {
            return await _repository.GetAllAsync();
        }

        async Task<Customer> ICustomerService.GetById(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        async Task ICustomerService.Update(int Id, CustomerDto dto)
        {
            var customer = await _repository.GetByIdAsync(Id);

            if (customer == null) return;

            customer.Name = dto.Name;
            customer.Age = dto.Age;
            customer.City = dto.City;

            await _repository.UpdateAsync(customer);
        }
    }
}
