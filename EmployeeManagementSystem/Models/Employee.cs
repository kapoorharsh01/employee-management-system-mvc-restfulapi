using System.ComponentModel.DataAnnotations;
namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(75)]
        public string? Department { get; set; }

        [StringLength(50)]
        [EmailAddress]
        //[RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$")]
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
