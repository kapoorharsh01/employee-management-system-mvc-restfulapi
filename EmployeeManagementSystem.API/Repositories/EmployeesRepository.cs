using EmployeeManagementSystem.API.Data;
using EmployeeManagementSystem.API.Models;
using EmployeeManagementSystem.API.Helpers;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagementSystem.API.Repositories
{
    public class EmployeesRepository : IRepository<Employee>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeesRepository> _logger;
        public EmployeesRepository(ApplicationDbContext context, ILogger<EmployeesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(Employee entity)
        {
            //if (await _context.AllEmployees.AnyAsync(e => e.Email == entity.Email))
            //{
            //    throw new InvalidOperationException($"Email '{entity.Email}' already exists for another employee.");
            //}

            _logger.LogInformation("DB: Creating employee = {Id}", entity.EmployeeId);
            await _context.AllEmployees.AddAsync(entity);

            await _context.SaveChangesAsync();
            _logger.LogInformation("DB: Employee saved with Id = {Id}", entity.EmployeeId);

        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var employee = await _context.AllEmployees.FindAsync(id);
                if (employee == null)
                {
                    _logger.LogWarning("DB: Attempted to delete non-existing employee with Id = {Id}", id);
                    return false;
                }
                _context.AllEmployees.Remove(employee);
                await _context.SaveChangesAsync();

                _logger.LogInformation("DB: Employee deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DB: Error while deleting employee");
                throw;
            }
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

            var totalEmployees = await query.CountAsync();

            //Paging

            _logger.LogInformation("DB: Sorting Started");
            _logger.LogInformation("DB: Paging Started");

            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

//            SELECT[e].[EmployeeId], [e].[Name], [e].[Email], [e].[Department], [e].[DateOfJoining]
//            FROM[AllEmployees] AS[e]
//            WHERE[e].[Department] = 'IT'--(your filters if any)
//                ORDER BY[e].[Name] ASC-- sorting
//                OFFSET 20 ROWS    -- Skip
//                FETCH NEXT 10 ROWS ONLY; --Take

            _logger.LogInformation("DB: Sorting done");
            _logger.LogInformation("DB: Paging done");

            _logger.LogInformation("DB: Employees list fetched.");
            return new PagedResult<Employee>
            {
                Employees = employees,
                TotalItems = totalEmployees,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        // Implementation of getAllEmployees without query params & it's the part of ng project
        public async Task<IEnumerable<Employee>> ListAllAsync()
        {
            return await _context.AllEmployees.ToListAsync();
        }


        public async Task<Employee?> GetByIdAsync(int id)
        {
            try
            {
                var employee = await _context.AllEmployees.FindAsync(id);
                if (employee == null)
                {
                    _logger.LogWarning("DB: Error occured while fetching employee's data = {Id}", id);
                    return null;
                }
                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DB: Error occurred while fetching employee = {Id}", id);
                throw;
            }

        }

        public async Task UpdateAsync(Employee entity)
        {
            //_context.AllEmployees.Update(entity);
            _logger.LogInformation("DB: Updating employee = {Id}", entity.EmployeeId);
            _context.Entry(entity).State = EntityState.Modified;
            //if (await _context.AllEmployees.AnyAsync(e => e.Email == entity.Email))
            //{
            //    throw new InvalidOperationException($"Email '{entity.Email}' already exists for another employee.");
            //}
            await _context.SaveChangesAsync();
            _logger.LogInformation("DB: Employee updated with Id = {Id}", entity.EmployeeId);
        }
        public async Task<bool> EmailExistsAsync(int excludeId, string email)
        {
            return await _context.AllEmployees.Where(e => e.EmployeeId != excludeId).AnyAsync(e => e.Email == email);
        }
    }
}
