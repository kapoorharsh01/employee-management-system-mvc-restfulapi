using System.ComponentModel.DataAnnotations;
namespace EmployeeManagementSystem.API.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfJoining { get; set; } = DateTime.UtcNow;
    }
    public class PagedResult<T>
    {
        public List<T> Employees { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
