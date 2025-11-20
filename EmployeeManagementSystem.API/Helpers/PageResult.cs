namespace EmployeeManagementSystem.API.Helpers
{
    public class PagedResult<T>
    {
        public List<T> Employees { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
