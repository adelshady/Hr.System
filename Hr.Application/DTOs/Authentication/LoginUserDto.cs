using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs.Authentication
{
    public class LoginUserDto
    {
         
        public string EmailOrUserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
