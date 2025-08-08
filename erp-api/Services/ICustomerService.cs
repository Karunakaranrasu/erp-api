using erp_api.Entities;
using erp_api.Models;

namespace erp_api.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAll();

        Task<Customer> GetById(int id);

        Task<Customer> Create(CustomerDto dto);

        Task Update(int Id, CustomerDto dto);

        Task Delete(int Id);
    }
}
