using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs
{
    public class WeekendDTO
    {
        public int Id { get; set; }
        public int OvertimeHour { get; set; }
        public int DiscountHour { get; set; }
        public int? empid { get; set; }
        public List<WeekendCheckDTO>? Weekends { get; set; }
        public IEnumerable<SelectListItem>? EmployeeList { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
