namespace EmployeeManagementSystem.API.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfJoining { get; set; } = DateTime.UtcNow;
    }
}
