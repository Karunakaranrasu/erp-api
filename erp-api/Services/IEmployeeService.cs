using erp_api.Entities;
using erp_api.Models;

namespace erp_api.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAll();

        Task<Employee> GetById(int id);

        Task<Employee> Create(EmployeeDto dto);

        Task Update(int Id, EmployeeDto dto);

        Task Delete(int Id);

    }
}
