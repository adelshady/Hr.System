using Hr.Application.Services.implementation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs.Salary
{
    public class SalayReportDto
    {

        [RegularExpression("^(0?[1-9]|1[0-2])$|^$", ErrorMessage = "Month must be a valid number between 01 and 12")]

        public string? Month { get; set; }


        [RegularExpression("^\\d{4}$", ErrorMessage = "Year must be a valid 4-digit number")]
        public string? Year { get; set; } = "1900";


        public List<SalaryDto>? Salaries { get; set; }
        
    }
}
