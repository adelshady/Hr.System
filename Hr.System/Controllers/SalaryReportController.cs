using Hr.Application.DTOs.Salary;
using Hr.Application.Services.implementation;
using Hr.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Hr.System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryReportController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeeServices employeeServices;
        private readonly IAttendanceServices attendanceServices;

        public SalaryReportController(IConfiguration configuration,
            IEmployeeServices employeeServices,
            IAttendanceServices attendanceServices)
        {
            _configuration = configuration;
            this.employeeServices = employeeServices;
            this.attendanceServices = attendanceServices;
        }

        [HttpGet("CalculateSalaryReports")]
        public IActionResult CalculateSalaryReports()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<SalaryDto> salaries = new List<SalaryDto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var employees = employeeServices.GetAllEmployee();
                foreach (var emp in employees)
                {
                    using (SqlCommand command = new SqlCommand("sp_CalculateEmployeeSalaryReport", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@EmployeeId", emp.ID));
                        DateTime currentDate = DateTime.Now;
                        int month = currentDate.Month;
                        int year = currentDate.Year;
                        command.Parameters.Add(new SqlParameter("@Month", month));
                        command.Parameters.Add(new SqlParameter("@Year", year));
                        try
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var attend = attendanceServices.GetAllAttendance().Where(x => x.SelectedEmployee == emp.ID);
                                        bool isEmployeeInMonthYear = false;

                                        foreach (var attendance in attend)
                                        {
                                            DateTime attendanceDate = DateTime.Parse(attendance.Date);

                                            if (attendanceDate.Year == year && attendanceDate.Month == month)
                                            {
                                                isEmployeeInMonthYear = true;
                                                break; // No need to continue checking once a match is found
                                            }
                                        }
                                        DateTime empBirthDate = DateTime.Parse(emp.HireDate);

                                        if ((empBirthDate.Year > year && empBirthDate.Month > month) || isEmployeeInMonthYear)
                                        {
                                            var salaryDto = new SalaryDto
                                            {
                                                Name = reader["EmployeeName"].ToString(),
                                                Department = reader["Department"].ToString(),
                                                BaseSalary = reader["Salary"].ToString(),
                                                AttendanceDays = reader["AttendanceDays"].ToString(),
                                                AbsenceDays = reader["AbsenceDays"].ToString(),
                                                AdditionalPerHour = reader["AdditionalPerHour"].ToString(),
                                                HourlyDiscount = reader["HourlyDiscount"].ToString(),
                                                TotalDiscount = reader["TotalDiscount"].ToString(),
                                                TotalAdditional = reader["TotalAdditional"].ToString(),
                                                NetSalary = reader["NetSalary"].ToString()
                                            };
                                            salaries.Add(salaryDto);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return StatusCode(500, new { error = "An error occurred", message = ex.Message });
                        }
                    }
                }
            }
            var salaryReport = new SalayReportDto
            {
                Month = DateTime.Now.ToString("MMMM"),
                Year = DateTime.Now.Year.ToString(),
                Salaries = salaries
            };

            return Ok(salaryReport);
        }



        [HttpPost]
        public IActionResult CalculateCustomSalaryReports([FromBody] SalayReportDto request)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                List<SalaryDto> salaries = new List<SalaryDto>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var employees = employeeServices.GetAllEmployee();

                    foreach (var emp in employees)
                    {
                        using (SqlCommand command = new SqlCommand("sp_CalculateEmployeeSalaryReport", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter("@EmployeeId", emp.ID));
                            command.Parameters.Add(new SqlParameter("@Month", request.Month)); // Use the month from the request
                            command.Parameters.Add(new SqlParameter("@Year", request.Year));   // Use the year from the request

                            try
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            var attend = attendanceServices.GetAllAttendance().Where(x => x.SelectedEmployee == emp.ID);
                                            bool isEmployeeInMonthYear = false;

                                            foreach (var attendance in attend)
                                            {
                                                DateTime attendanceDate = DateTime.Parse(attendance.Date);

                                                if (attendanceDate.Year == Convert.ToInt32(request.Year) && attendanceDate.Month
                                                    == Convert.ToInt32(request.Month))
                                                {
                                                    isEmployeeInMonthYear = true;
                                                    break; // No need to continue checking once a match is found
                                                }
                                            }
                                            DateTime empBirthDate = DateTime.Parse(emp.HireDate);
                                            if ((empBirthDate.Year == Convert.ToInt32(request.Year) && empBirthDate.Month
                                                == Convert.ToInt32(request.Month)) || isEmployeeInMonthYear)
                                            {
                                                // Map the result set to SalaryDto
                                                var salaryDto = new SalaryDto
                                                {
                                                    Name = reader["EmployeeName"].ToString(),
                                                    Department = reader["Department"].ToString(),
                                                    BaseSalary = reader["Salary"].ToString(),
                                                    AttendanceDays = reader["AttendanceDays"].ToString(),
                                                    AbsenceDays = reader["AbsenceDays"].ToString(),
                                                    AdditionalPerHour = reader["AdditionalPerHour"].ToString(),
                                                    HourlyDiscount = reader["HourlyDiscount"].ToString(),
                                                    TotalDiscount = reader["TotalDiscount"].ToString(),
                                                    TotalAdditional = reader["TotalAdditional"].ToString(),
                                                    NetSalary = reader["NetSalary"].ToString()
                                                };

                                                salaries.Add(salaryDto);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
                            }
                        }
                    }
                }

                var salaryReport = new SalayReportDto
                {
                    Month = request.Month,
                    Year = request.Year,
                    Salaries = salaries
                };

                return Ok(salaryReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }
    }
}
