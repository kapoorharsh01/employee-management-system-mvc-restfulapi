using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeApiService _apiService;
        public EmployeesController(EmployeeApiService apiService)
        {
            _apiService = apiService;
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Index(
            int pageNumber = 1, 
            int pageSize = 10, 
            string sortColumn = "EmployeeId",
            string sortDirection = "asc"
            )
        {
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.SortColumn = sortColumn;
            ViewBag.SortDirection = sortDirection;

            var employees = await _apiService.GetAllEmployeesAsync(pageNumber, pageSize, sortColumn, sortDirection);
            return View(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee obj)
        {
            await _apiService.CreateEmployeeAsync(obj);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _apiService.GetEmployeeByIdAsync(id);
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Employee obj)
        {
            await _apiService.UpdateEmployeeAsync(id, obj);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteEmployeeAsync(id);
            return Ok();
        }
    }
}
