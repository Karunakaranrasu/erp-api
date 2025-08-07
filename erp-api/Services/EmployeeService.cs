using erp_api.Entities;
using erp_api.Models;
using erp_api.Repositories;

namespace erp_api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        async Task<Employee> IEmployeeService.Create(EmployeeDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Department = dto.Department,
                DateJoined = DateTime.Now,
            };

            return await _repository.AddAsync(employee);
        }

        async Task IEmployeeService.Delete(int Id)
        {
            await _repository.DeleteAsync(Id);
        }

        async Task<IEnumerable<Employee>> IEmployeeService.GetAll()
        {
            return await _repository.GetAllAsync();
        }

        async Task<Employee> IEmployeeService.GetById(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        async Task IEmployeeService.Update(int Id, EmployeeDto dto)
        {
            var employee = await _repository.GetByIdAsync(Id);

            if (employee == null) return;

            employee.Name = dto.Name;
            employee.Department = dto.Department;
            employee.DateJoined = DateTime.Now;

            await _repository.UpdateAsync(employee);

        }
    }
}
