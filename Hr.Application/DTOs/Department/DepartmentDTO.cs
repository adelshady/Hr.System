using Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs.Department
{
    public class DepartmentDTO
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Department name must be between 2 and 30 characters.")]
        public string Name { get; set; }


        // public List<string>? Employees { get; set; }
    }
}
