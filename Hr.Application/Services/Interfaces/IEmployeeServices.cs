using Hr.Application.DTOs;
using Hr.Application.DTOs.Employee;
using Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Services.Interfaces
{
    public interface IEmployeeServices
    {
        #region Employee Attendance 
        IEnumerable<GetAllEmployeAttendanceDto> GetAllEmployeeForAttendance();
        GetAllEmployeAttendanceDto GetAttendanceById(int id);

        #endregion


        #region Employee
        IEnumerable<GetAllEmployeeDto> GetAllEmployee();
        GetAllEmployeeDto GetEmployeeId(int id);
        void CreateEmployee(GetAllEmployeeDto EmployeeDto);
        void UpdateEmployee(GetAllEmployeeDto EmployeeDto, int id);
        bool CheckEmployeeExists(GetAllEmployeeDto EmployeeDto);
        void Remove(int id);
        GetAllEmployeeDto GetEmployeeByUserId(string userId);

        #endregion
    }
}
