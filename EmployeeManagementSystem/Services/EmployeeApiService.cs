using EmployeeManagementSystem.Models;
using System.Text.Json;
namespace EmployeeManagementSystem.Services
{
    public class EmployeeApiService
    {
        private readonly HttpClient _httpClient;
        public EmployeeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //GET : api/employeesapi
        public async Task<PagedResult<Employee>> GetAllEmployeesAsync(int pageNumber, int pageSize, string sortColumn, string sortDirection)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var url = $"EmployeesApi?pageNumber={pageNumber}&pageSize={pageSize}&sortColumn={sortColumn}&sortDirection={sortDirection}";
            //Console.WriteLine($"Calling API: {_httpClient.BaseAddress}{url}");
            var employees = await _httpClient.GetFromJsonAsync<PagedResult<Employee>>(url, options);
            //Console.WriteLine($"Response: {employees}");
            return employees;
        }

        //GET : api/employeesapi/id
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var employee = await _httpClient.GetFromJsonAsync<Employee>($"EmployeesApi/{id}", options);
            
            return employee;
        }

        //POST : api/employeesapi/
        public async Task CreateEmployeeAsync(Employee obj)
        {
            var response = await _httpClient.PostAsJsonAsync("EmployeesApi", obj);
            response.EnsureSuccessStatusCode();
        }

        //PUT : api/employeesapi/id
        public async Task UpdateEmployeeAsync(int id, Employee obj)
        {
            var response = await _httpClient.PutAsJsonAsync($"EmployeesApi/{id}", obj);
            //if (!response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine($"Error status code: {response.StatusCode}");
            //    string errorContent = await response.Content.ReadAsStringAsync();
            //    Console.WriteLine($"Error response body: {errorContent}");
            //    // Handle specific error codes or log the error
            //}
            response.EnsureSuccessStatusCode();
        }

        //Delete : api/employeesapi/id
        public async Task DeleteEmployeeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"EmployeesApi/{id}");
            response.EnsureSuccessStatusCode();
        }


    }
}
