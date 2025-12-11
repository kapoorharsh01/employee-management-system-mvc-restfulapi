using EmployeeManagementSystem.API.Helpers;
namespace EmployeeManagementSystem.API.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize, string sortColumn, string sortDirection);
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> EmailExistsAsync(int excludeId, string email);

        //part of ng project
        Task<IEnumerable<T>> ListAllAsync();
    }
}
