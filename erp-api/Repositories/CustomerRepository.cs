using erp_api.Data;
using erp_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace erp_api.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }
        async Task<Customer> ICustomerRepository.AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        async Task ICustomerRepository.DeleteAsync(int id)
        {
            var cus = await _context.Customers.FindAsync(id);

            if (cus != null)
            {
                _context.Customers.Remove(cus);
                await _context.SaveChangesAsync();
            }
        }

        async Task<IEnumerable<Customer>> ICustomerRepository.GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        async Task<Customer> ICustomerRepository.GetByIdAsync(int id)
        {
            var cus = await _context.Customers.FindAsync(id);

            if (cus != null)
            {
                return cus;
            }
            return null;
        }

        async Task ICustomerRepository.UpdateAsync(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
