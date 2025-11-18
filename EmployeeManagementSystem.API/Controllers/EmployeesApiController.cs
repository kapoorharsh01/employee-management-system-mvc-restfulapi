using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.API.Models;
using EmployeeManagementSystem.API.Repositories;

namespace EmployeeManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;
        private readonly ILogger<EmployeesApiController> _logger;

        public EmployeesApiController(IRepository<Employee> repository, ILogger<EmployeesApiController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Employee>>> GetEmployees(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string sortColumn,
            [FromQuery] string sortDirection)

        //https://localhost:7033/api/employeesapi?pageNumber=2&pageSize=5&sortColumn=Name&sortDirection=desc
        //When someone visits this URL, grab these values from the ? part.
        {
            try
            {
                var employees = await _repository.GetAllAsync(pageNumber, pageSize, sortColumn, sortDirection);
                _logger.LogInformation("All Employees have been sent as requested!");

                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while sending all employees.");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound(new ApiResponse { Success = false, Message = "Employeee not found" });
            }

            _logger.LogInformation("Employee fetched successfully: {Id}", id);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult> AddEmployee(Employee obj)
        {
            try
            {
                if (await _repository.EmailExistsAsync(obj.EmployeeId, obj.Email))
                {
                    _logger.LogWarning("Creation of employee failed because this {Email} already exists", obj.Email);

                    return BadRequest(new ApiResponse { Success = false, Message = "Email already exists" });
                }

                await _repository.AddAsync(obj);
                _logger.LogInformation("Employee created!");

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Employee created successfully!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while Adding Employee.");
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(int id, [FromBody] Employee obj)
        {
            try
            {
                if (id != obj.EmployeeId)
                {
                    _logger.LogWarning("Employee not found with : {Id}", id);

                    return NotFound(new ApiResponse { Success = false, Message = "Employeee not found" });
                }
                if (await _repository.EmailExistsAsync(obj.EmployeeId, obj.Email))
                {
                    _logger.LogWarning("Updation of employee failed because this {Email} already exists", obj.Email);

                    return BadRequest(new ApiResponse { Success = false, Message = "Email already exists" });
                }
                await _repository.UpdateAsync(obj);
                _logger.LogInformation("Employee updated!");

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Employee updated successfully!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while Updating Employee.");
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var isDeleted = await _repository.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Employee not found"
                });
            }
            return NoContent();
        }
    }
}
