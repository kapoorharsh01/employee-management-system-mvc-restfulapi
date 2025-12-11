using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.API.DTOs;
using EmployeeManagementSystem.API.Repositories;
using EmployeeManagementSystem.API.Helpers;
using EmployeeManagementSystem.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("AllowNg")]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;
        private readonly ILogger<EmployeesApiController> _logger;
        private readonly IMapper _mapper;

        public EmployeesApiController(IRepository<Employee> repository, ILogger<EmployeesApiController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<EmployeeDto>>> GetEmployees(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string sortColumn,
            [FromQuery] string sortDirection)

        //https://localhost:7033/api/employeesapi?pageNumber=2&pageSize=5&sortColumn=Name&sortDirection=desc
        //When someone visits this URL, grab these values from the ? part.
        {
            try
            {
                var result = await _repository.GetAllAsync(pageNumber, pageSize, sortColumn, sortDirection);
                _logger.LogInformation("All Employees have been sent as requested!");

                var employeesDto = _mapper.Map<List<EmployeeDto>>(result.Employees);
                //Console.WriteLine($"Response: {employeesDto}");
                return Ok(new PagedResult<EmployeeDto>
                {
                    Employees = employeesDto,
                    TotalItems = result.TotalItems,
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while sending all employees.");
                throw;
            }
        }



        // getAll without query params & it's the part of ng project
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _repository.ListAllAsync();
            return _mapper.Map<List<EmployeeDto>>(employees);
        }


        [HttpPost("check-email")]
        public async Task<ActionResult> IsEmailExists([FromBody] CheckEmailDto dto)
        {
            var exists = await _repository.EmailExistsAsync(dto.EmployeeId, dto.Email);
            return Ok(new { exists });
        }






        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var result = await _repository.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new { Message = "Employeee not found" });
            }

            _logger.LogInformation("Employee fetched successfully: {Id}", id);

            var employee = _mapper.Map<EmployeeDto>(result);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult> AddEmployee(EmployeeDto dto)
        {
            try
            {
                // POST: use excludeId = 0 (new employee, no real ID yet)
                //if (await _repository.EmailExistsAsync(0, dto.Email)) // refer at last for detailed explaination
                //{
                //    _logger.LogWarning("Creation of employee failed because this {Email} already exists", dto.Email);

                //    return BadRequest(new ApiResponse { Success = false, Message = "Email already exists" });
                //}

                var employee = _mapper.Map<Employee>(dto);

                await _repository.AddAsync(employee);
                _logger.LogInformation("Employee created!");

                return Ok(new
                {
                    Status = 200,
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
        public async Task<ActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto dto)
        {
            try
            {
                var existingEmployee = await _repository.GetByIdAsync(id);

                if (existingEmployee == null)
                {
                    _logger.LogWarning("Employee not found with : {Id}", id);

                    return NotFound(new { Message = "Employeee not found" });
                }
                //if (await _repository.EmailExistsAsync(id, dto.Email))
                //{
                //    _logger.LogWarning("Updation of employee failed because this {Email} already exists", dto.Email);

                //    return BadRequest(new ApiResponse { Success = false, Message = "Email already exists" });
                //}

                _mapper.Map(dto, existingEmployee); // refer at last for detailed explaination

                await _repository.UpdateAsync(existingEmployee);
                _logger.LogInformation("Employee updated!");

                return Ok(new
                {
                    Status = 200,
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
                return NotFound(new { Message = "Employee not found" });
            }
            return NoContent();
        }
    }
}

/* ---------------------------------------------------------
   EMAIL CHECK LOGIC – QUICK SUMMARY
   ---------------------------------------------------------
   1. POST (Create):
      - New employees do NOT have a real ID yet.
      - EF Core does not assign the ID until SaveChanges().
      - So we pass excludeId = 0 when checking email.
      - This checks if ANY existing employee has the same email.

      Example:
      await EmailExistsAsync(0, dto.Email)

   2. PUT (Update):
      - Existing employees already have a real ID.
      - We must exclude the current user from the email check.
      - So we pass the same ID we are updating.
      - This ensures: "No other employee has this email."

      Example:
      await EmailExistsAsync(id, dto.Email)

   3. Mapping Notes:
      - POST: _mapper.Map<Employee>(dto) creates a NEW entity.
      - PUT:  _mapper.Map(dto, existingEmployee) updates in-place
              WITHOUT creating a new DB record.
----------------------------------------------------------- */
