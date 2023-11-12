using Hr.Application.DTOs;
using Hr.Application.DTOs.Attendance;
using Hr.Application.DTOs.Employee;
using Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Services.Interfaces
{
    public interface IAttendanceServices
    {
        
        IEnumerable<GetAllEmployeAttendanceDto> GetAllEmployeeForAttendance();

        IEnumerable<AttendanceEmployeDto> GetAllAttendance();
        AttendanceEmployeDto GetAttendanceId(int id);
        void CreateAttendance(AttendanceEmployeDto attendanceDto);
        void UpdateAttendance(AttendanceEmployeDto attendanceDto, int id);
        bool DeleteAttendance(int id);
        bool CheckAttendanceExists(AttendanceEmployeDto attendanceDto);
        string GetDayOfWeekForDate(DateTime date);
        List<string> GetEmployeeWeekendDays(int employeeId);


        public IEnumerable<AttendanceEmployeDto> FilterAttendancesByDateRange(AtendanceFilterDto filter);
    }
}
