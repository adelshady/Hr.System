using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs.Salary
{
    public class SalaryDto
    {
        public string Name { get; set; }

        public string Department { get; set; }
        public string BaseSalary { get; set; }
        public string AttendanceDays { get; set; }
        public string AbsenceDays { get; set; }
        public string AdditionalPerHour { get; set; }
        public string HourlyDiscount { get; set; }
        public string TotalDiscount { get; set; }
        public string TotalAdditional { get; set; }
        public string NetSalary { get; set; }
    }
}
