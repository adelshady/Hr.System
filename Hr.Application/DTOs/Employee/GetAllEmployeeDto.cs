using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs.Employee
{
    public class GetAllEmployeeDto
    {
        public int ID { get; set; }
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        [MaxLength(30, ErrorMessage = "Country cannot exceed 50 characters.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(30, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(30, ErrorMessage = "Gender must be a single character.")]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string BirthDate { get; set; }

        public string Nationality { get; set; }
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 numeric characters.")]
        public string NationalId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string HireDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive value.")]
        public double Salary { get; set; }

        [Required(ErrorMessage = "Arrival Time is required.")]
        [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Arrival Time must be in the format 'hh:mm'.")]
        public string ArrivalTime { get; set; }

        [Required(ErrorMessage = "Leave Time is required.")]
        [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Leave Time must be in the format 'hh:mm'.")]
        public string? LeaveTime { get; set; }

        public string? UserId { get; set; }
        public int DepartmentId { get; set; }
        public string? DeptName { get; set; }
       

    }
}
