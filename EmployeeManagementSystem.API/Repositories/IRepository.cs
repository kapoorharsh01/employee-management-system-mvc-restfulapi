using EmployeeManagementSystem.API.Models;
namespace EmployeeManagementSystem.API.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize, string sortColumn, string sortDirection);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> EmailExistsAsync(int excludeId, string email);
    }
}
