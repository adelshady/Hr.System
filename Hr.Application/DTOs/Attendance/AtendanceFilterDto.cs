using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs.Attendance
{
    public class AtendanceFilterDto
    {
        public DateTime? From{ get; set; }
        public DateTime? To{ get; set; }

        public List<AttendanceEmployeDto>? attendanceEmployeDtos { get; set; } = new List<AttendanceEmployeDto>();
    }
}
