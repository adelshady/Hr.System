using Hr.Application.DTOs;
using Hr.Application.DTOs.Employee;
using Hr.Application.Interfaces;
using Hr.Application.Services.implementation;
using Hr.Application.Services.Interfaces;
using Hr.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hr.System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices employeeServices;

        public EmployeeController(IEmployeeServices employeeServices)
        {

            this.employeeServices = employeeServices;
        }

        [HttpGet]
        public IActionResult GetAllEmploye()
        {
            try
            {
                var EmployeDto = employeeServices.GetAllEmployee();

                return Ok(EmployeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var EmployeDto = employeeServices.GetEmployeeId(id);

                return Ok(EmployeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create(GetAllEmployeeDto EmployeeDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (EmployeeDto == null ||
                        !TimeSpan.TryParse(EmployeeDto.ArrivalTime, out TimeSpan arrivalTime) ||
                        !TimeSpan.TryParse(EmployeeDto.LeaveTime, out TimeSpan leaveTime) ||
                         arrivalTime >= leaveTime)
                    {
                        ModelState.AddModelError("LeaveTime", "Leave time cannot be before or equal to arrival time.");
                        return BadRequest(ModelState);
                    }
                    if (employeeServices.CheckEmployeeExists(EmployeeDto))
                    {
                        ModelState.AddModelError("FirstName", "First Name and Last Name is founded ");
                        return BadRequest(ModelState);
                    }
                    DateTime BirthDate = DateTime.Parse(EmployeeDto.BirthDate);
                    DateTime HireDate = DateTime.Parse(EmployeeDto.HireDate);
                    var age = BirthDate - HireDate;
                    int years = (int)(age.TotalDays / 365.25);
                    if (HireDate <= BirthDate || years >18)
                    {
                        ModelState.AddModelError("HireDate", "HireDate is less than BirthDate ");
                        return BadRequest(new
                        {
                            Message = "HireDate is less than BirthDate",
                           Errors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                         });
                    }
                    employeeServices.CreateEmployee(EmployeeDto);

                    return Ok(new { Message = "Employe record created successfully." });
                }
                else
                {
                    return BadRequest(EmployeeDto);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(GetAllEmployeeDto employeeDto, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (employeeDto == null ||
                      !TimeSpan.TryParse(employeeDto.ArrivalTime, out TimeSpan arrivalTime) ||
                      !TimeSpan.TryParse(employeeDto.LeaveTime, out TimeSpan leaveTime) ||
                       arrivalTime >= leaveTime)
                    {
                        ModelState.AddModelError("LeaveTime", "Leave time cannot be before or equal to arrival time.");
                        return BadRequest(ModelState);
                    }
                    if (employeeServices.GetAllEmployee().Any(
                        x => x.FirstName.ToLower() == employeeDto.FirstName.ToLower()
                        && x.LastName.ToLower() == employeeDto.LastName.ToLower() &&
                        x.DepartmentId == employeeDto.DepartmentId &&
                        x.NationalId ==employeeDto.NationalId &&
                        x.ID != employeeDto.ID))
                    {
                        ModelState.AddModelError("FirstName", "the name is founded plz enter another name");
                        return BadRequest(ModelState);
                    }
                    employeeServices.UpdateEmployee(employeeDto, id);
                    return Ok(employeeDto);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            try
            {
                employeeServices.Remove(id);
                return Ok(new {message = "Employee deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
