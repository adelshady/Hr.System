using Hr.Application.Common;
using Hr.Application.DTOs.User;
using Hr.Application.Services.implementation;
using Hr.Application.Services.Interfaces;
using Hr.Domain.Entities;
using Hr.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using static Hr.Application.Common.SD;

namespace Hr.System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRoleService roleService;
        private readonly ApplicationDbContext context;
        private readonly IEmployeeServices employeeService;

        public UserController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IRoleService roleService,
            ApplicationDbContext context,
            IEmployeeServices employeeService
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.roleService = roleService;
            this.context = context;
            this.employeeService = employeeService;
        }


        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var users = await userManager.Users.Where(x => x.UserName != SD.AdminUserName).ToListAsync();

                if (users != null)
                {
                    var userRoleDto = new List<UserWithRoleDto>();

                    foreach (var user in users)
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        var employee = employeeService.GetEmployeeByUserId(user.Id);

                        if (employee != null)
                        {
                            var userDto = new UserWithRoleDto
                            {
                                Id = user.Id,
                                FullName = $"{employee.FirstName} {employee.LastName}",
                                UserName = user.UserName,
                                Email = user.Email,
                                Password = user.PasswordHash,
                                Roles = roles.ToList()
                            };
                            userRoleDto.Add(userDto);
                        }
                    }

                    return Ok(userRoleDto);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Handle the exception
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(string userId)
        {

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            // Retrieve the roles associated with the user
            var roles = await userManager.GetRolesAsync(user);

            var employee = employeeService.GetEmployeeByUserId(userId);

            if (employee != null)
            {
                // Map user's roles to IdentityRole objects
                var userRoles = roleManager.Roles
                    .Where(r => roles.Contains(r.Name))
                    .Select(r => new IdentityRole { Id = r.Id, Name = r.Name })
                    .ToList();

                // Get all role IDs for the user
                var roleIds = roles.Select(roleName => roleManager.Roles.SingleOrDefault(r => r.Name == roleName)?.Id).ToList();

                // Create a DTO  with user and role information
                var UserDto = new UserRoleFromDto
                {
                    EmpId = employee.ID,
                    UserId = user.Id,
                    EmployeeName = $"{employee.FirstName} {employee.LastName}",
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = user.PasswordHash,
                    userRoles = userRoles,
                    selectRolesIds = roleIds,
                    Roles = GetRolesListExceptUserRoles(user.Id),
                    Employees = employeeService.GetAllEmployee().Select(x => new SelectListItem
                    {
                        Text = $"{x.FirstName} {x.LastName}",
                        Value = x.ID.ToString()
                    }).ToList()
                };
                return Ok(UserDto);
            }
            else
            {
                return NotFound("Employee not found for the user.");
            }
        }


        [HttpGet("GetToCreate")]
        public async Task<IActionResult> GetToCreate()
        {
            try
            {
                var roles = GetRolesList();
                var employees = await GetUserSelectListWithoutRoles();
                var UserDto = new UserRoleFromDto
                {

                    EmpId = 0,
                    UserName = "",
                    Email = "",
                    Password = "",
                    Roles = roles.ToList(),
                    Employees = employees.ToList()
                };
                return Ok(UserDto);
            }
            catch (Exception ex)
            {
                // Handle the exception
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRoleFromDto model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email
                    };

                    user.PasswordHash = model.Password;

                    var result = await userManager.CreateAsync(user);


                    if (result.Succeeded)
                    {
                        var emp = employeeService.GetEmployeeId(model.EmpId);
                        emp.UserId = user.Id;
                        employeeService.UpdateEmployee(emp, emp.ID);


                        var selectedRoles = await roleManager.Roles
                            .Where(role => model.selectRolesIds.Contains(role.Id))
                            .ToListAsync();

                        var addRolesResult = await userManager.AddToRolesAsync(user, selectedRoles.Select(role => role.Name));

                        if (addRolesResult.Succeeded)
                        {
                            transaction.Commit();
                            return Ok(new { message = "User created successfully" });
                        }
                    }

                    // If any step fails, roll back the transaction.
                    transaction.Rollback();
                }

                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id ,UserRoleFromDto model)
        {

            using (var transaction = context.Database.BeginTransaction())
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.PasswordHash = model.Password;
                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {


                        var selectedRoles = await roleManager.Roles
                            .Where(role => model.selectRolesIds.Contains(role.Id))
                            .ToListAsync();

                        // Remove existing roles
                        var userRoles = await userManager.GetRolesAsync(user);
                        var removeRolesResult = await userManager.RemoveFromRolesAsync(user, userRoles);

                        if (removeRolesResult.Succeeded)
                        {
                            var addRolesResult = await userManager.AddToRolesAsync(user, selectedRoles.Select(role => role.Name));

                            if (addRolesResult.Succeeded)
                            {
                                transaction.Commit();
                                return Ok(new { message = "User roles updated successfully" });
                            }
                        }
                    }

                    // If any step fails, roll back the transaction.
                    transaction.Rollback();
                }

                return BadRequest("Failed to create user or update user roles");
            }

        }


        [HttpDelete("RemoveUser")]
        public async Task<IActionResult> RemoveUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            try
            {
                // Remove the user from ASP.NET Identity
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {

                    var emp = employeeService.GetEmployeeByUserId(userId);
                    emp.UserId = null;
                    employeeService.UpdateEmployee(emp, emp.ID);

                    return Ok(new { message = "User removed and associated Employee updated." });
                }

                return BadRequest("Failed to remove the user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }


        private IEnumerable<SelectListItem> GetRolesList()
        {
            return roleManager.Roles.Select(role => new SelectListItem
            {
                Value = role.Id,
                Text = role.Name
            });
        }

        private IEnumerable<SelectListItem> GetRolesListExceptUserRoles(string currentUserId)
        {

            var currentUserRoles = userManager.GetRolesAsync(userManager.FindByIdAsync(currentUserId).Result).Result;

            var rolesWithoutCurrentUserRoles = roleManager.Roles
                .Where(role => !currentUserRoles.Contains(role.Name))
                .Select(role => new SelectListItem
                {
                    Value = role.Id,
                    Text = role.Name
                });

            return rolesWithoutCurrentUserRoles;

        }

        private async Task<IEnumerable<SelectListItem>> GetUserSelectListWithoutRoles()
        {
            var EmpolyeeWithoutUserAccess = employeeService.GetAllEmployee().Where(x => x.UserId == null);

            var selectListItems = EmpolyeeWithoutUserAccess
                .Select(user => new SelectListItem
                {
                    Text = $"{user.FirstName} {user.LastName}",
                    Value = user.ID.ToString()
                })
                .ToList();

            return selectListItems;
        }


    }

}
