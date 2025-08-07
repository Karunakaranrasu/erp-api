using erp_api.Data;
using erp_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace erp_api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        async Task<Employee> IEmployeeRepository.AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        async Task IEmployeeRepository.DeleteAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp != null)
            {
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
            }
        }

        async Task<IEnumerable<Employee>> IEmployeeRepository.GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        async Task<Employee> IEmployeeRepository.GetByIdAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp != null)
            {
                return emp;
            }
            return null;
        }

        async Task IEmployeeRepository.UpdateAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
