using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs.Role
{
    public class PermissionFormDto
    {
        public string? RoleId { get; set; }
        [Required(ErrorMessage = "Group Name is Required")]
        public string RoleName { get; set; }
        public List<RolePermissionCheckDto> RoleClaims { get; set; }
    }
}
