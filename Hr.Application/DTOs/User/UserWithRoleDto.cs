using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs.User
{
    public class UserWithRoleDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
        
        public string Password { get; set; }

        public List<string> Roles { get; set; }
    }
}
