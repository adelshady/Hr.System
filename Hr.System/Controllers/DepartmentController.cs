using Hr.Application.Common.Global;
using Hr.Application.DTOs.Department;
using Hr.Application.Services.implementation;
using Hr.Application.Services.Interfaces;
using Hr.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hr.System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        [HttpGet]
     
        public ActionResult GetAll()
        {
            try
            {
                var allDepartments = departmentService.GetAllDepartment();
                if (allDepartments != null)
                {
                    var Department = departmentService.GetAllDepartment();

                    return Ok(Department);
                }
                return NotFound();
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var department = departmentService.GetDepartmentId(id);
                if (department != null)
                {
                    return Ok(department);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult Create([FromBody] DepartmentDTO departmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (departmentService.CheckDepartmentExists(departmentDTO))
                    {
                        ModelState.AddModelError("DeptName", "Deptartment Name is founded ");
                        return BadRequest(ModelState);
                    }
                    if (ModelState.IsValid)
                    {
                        departmentService.Create(departmentDTO);
                        return Ok(departmentDTO);
                    }
                    return BadRequest("Invalid department data");
                }
                else
                {

                    return BadRequest("Invalid department data");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult Edit(int id, [FromBody] DepartmentDTO updatedDepartmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (departmentService.GetAllDepartment().Any(
                        x => x.Name.ToLower() == updatedDepartmentDTO.Name.ToLower() && x.Id != updatedDepartmentDTO.Id))
                    {
                        ModelState.AddModelError("DeptName", "Deptartment Name is founded");
                        return BadRequest(ModelState);
                    }

                    departmentService.Update(id,updatedDepartmentDTO);
                    return Ok(updatedDepartmentDTO);
                }

                return BadRequest("Invalid department data");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var (isSuccess, employeeCount) = departmentService.Remove(id);

                if (!isSuccess)
                {
                    return BadRequest(new { message = $"Department has {employeeCount} Employees  please transfre them to another department ." });
                }

                return Ok(new { message = "Department has been removed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }



    }
}
