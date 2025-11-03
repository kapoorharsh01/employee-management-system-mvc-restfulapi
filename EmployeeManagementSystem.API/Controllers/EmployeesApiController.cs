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
        public EmployeesApiController(IRepository<Employee> repository)
        {
            _repository = repository;
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
            var employees = await _repository.GetAllAsync(pageNumber, pageSize, sortColumn, sortDirection);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee obj)
        {
            if(await _repository.EmailExistsAsync(obj.EmployeeId, obj.Email))
            {
                return BadRequest(new {message = "Email Exists"});
            }
            await _repository.AddAsync(obj);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(int id, [FromBody] Employee obj)
        {
            if (id != obj.EmployeeId)
            {
                return BadRequest();
            }
            if (await _repository.EmailExistsAsync(obj.EmployeeId, obj.Email))
            {
                return BadRequest(new { message = "Email Exists" });
            }
            await _repository.UpdateAsync(obj);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
