using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs.User
{
    public class UserRoleFromDto
    {
        public IEnumerable<SelectListItem> Employees { get; set; } = new List<SelectListItem>();
        public int EmpId { get; set; }
        public string? UserId { get; set; }
        public string? EmployeeName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }   
        public List<IdentityRole>? userRoles { get; set; } = new List<IdentityRole>();
        public List<string>? selectRolesIds { get; set; } = new List<string>();
        public IEnumerable<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
        
    }
}
