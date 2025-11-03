using EmployeeManagementSystem.API.Data;
using EmployeeManagementSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.API.Repositories
{
    public class EmployeesRepository : IRepository<Employee>
    {
        private readonly ApplicationDbContext _context;
        public EmployeesRepository(ApplicationDbContext context)
        {
            _context = context;  
        }
        public async Task AddAsync(Employee entity)
        {
            //if (await _context.AllEmployees.AnyAsync(e => e.Email == entity.Email))
            //{
            //    throw new InvalidOperationException($"Email '{entity.Email}' already exists for another employee.");
            //}
            await _context.AllEmployees.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _context.AllEmployees.FindAsync(id);
            if(employee == null)
            {
                throw new KeyNotFoundException();
            }
            _context.AllEmployees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<Employee>> GetAllAsync(int pageNumber, int pageSize, string sortColumn, string sortDirection)
        {
            var query = _context.AllEmployees.AsQueryable();

            //Sorting
            bool isDesc = sortDirection == "desc";
            query = sortColumn switch
            {
                "name" => isDesc ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),

                "email" => isDesc ? query.OrderByDescending(e => e.Email) : query.OrderBy(e => e.Email),

                "department" => isDesc ? query.OrderByDescending(e => e.Department) : query.OrderBy(e => e.Department),

                "dateOfJoining" => isDesc ? query.OrderByDescending(e => e.DateOfJoining) : query.OrderBy(e => e.DateOfJoining),

                 _ => isDesc ? query.OrderByDescending(e => e.EmployeeId) : query.OrderBy(e => e.EmployeeId)
            };

            //Paging
            var totalEmployees = await query.CountAsync();
            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Employee>
            {
                Employees = employees,
                TotalItems = totalEmployees,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var employee = await _context.AllEmployees.FindAsync(id);
            if (employee == null)
            {
                throw new KeyNotFoundException();
            }
            return employee;
        }

        public async Task UpdateAsync(Employee entity)
        {
            //_context.AllEmployees.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
            //if (await _context.AllEmployees.AnyAsync(e => e.Email == entity.Email))
            //{
            //    throw new InvalidOperationException($"Email '{entity.Email}' already exists for another employee.");
            //}
            await _context.SaveChangesAsync();
        }
        public async Task<bool> EmailExistsAsync(int excludeId, string email)
        {
            return await _context.AllEmployees.Where(e => e.EmployeeId != excludeId).AnyAsync(e => e.Email == email);
        }
    }
}
