using Hr.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs
{
    public class AttendanceEmployeDto
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string Date { get; set; }
        [Required(ErrorMessage = "Arrival Time is required.")]

        [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Arrival Time must be in the format 'hh:mm'.")]
        public string ArrivalTime { get; set; }
        [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Arrival Time must be in the format 'hh:mm'.")]
        public string? LeaveTime { get; set; }
 
        public int SelectedEmployee { get; set; }
        public string? EmployeeName { get; set; }
        public string? DepartmentName { get; set; }

        //public IEnumerable<SelectListItem>? EmployeeList { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}
