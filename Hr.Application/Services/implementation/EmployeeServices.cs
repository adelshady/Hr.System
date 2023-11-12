
using Hr.Application.Common;
using Hr.Application.DTOs;
using Hr.Application.DTOs.Employee;
using Hr.Application.Interfaces;
using Hr.Application.Services.Interfaces;
using Hr.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Services.implementation
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IUnitOfWork uniteOfWork;
        private readonly IDepartmentService departmentService;
        private readonly UserManager<ApplicationUser> userManager;

        public EmployeeServices(IUnitOfWork uniteOfWork,  
            IDepartmentService departmentService 
            ,UserManager<ApplicationUser> userManager)
        {
            this.uniteOfWork = uniteOfWork;
            
            this.departmentService = departmentService;
            this.userManager = userManager;
        }

        #region Employee Attendance
        public IEnumerable<GetAllEmployeAttendanceDto> GetAllEmployeeForAttendance()
        {
            var listOfEmployee = new List<GetAllEmployeAttendanceDto>();
            var employes = uniteOfWork.EmployeeRepository.GetAll();
            foreach (var employee in employes)
            {
                var emp = new GetAllEmployeAttendanceDto()
                {
                    Id = employee.Id,
                    Name = employee.FirstName + " " + employee.LastName,
                };
                listOfEmployee.Add(emp);
            }
            return listOfEmployee;
        } 

        public GetAllEmployeAttendanceDto GetAttendanceById(int id)
        {
            var employee = uniteOfWork.EmployeeRepository.Get(x => x.Id == id);
            if(employee == null)
            {
                throw new Exception("Not found Employee");
            }
            else
            {
                var empDto = new GetAllEmployeAttendanceDto()
                {
                    Id = employee.Id,
                    Name = employee.FirstName+" "+ employee.LastName,
                };
                return empDto;
            }
        }

       
        #endregion

        #region Employe

        public bool CheckEmployeeExists(GetAllEmployeeDto EmployeeDto)
        {
          return  uniteOfWork.EmployeeRepository
                .Any(x => x.FirstName.ToLower() == EmployeeDto.FirstName.ToLower() 
                     && x.LastName.ToLower() == EmployeeDto.LastName.ToLower() 
                     && x.DepartmentId==EmployeeDto.DepartmentId
                     && x.NationalId == EmployeeDto.NationalId);
        }

        public IEnumerable<GetAllEmployeeDto> GetAllEmployee()
        {
            var EmployeeList = new List<GetAllEmployeeDto>();
            var employees = uniteOfWork.EmployeeRepository.GetAll(includeProperties: "Department");
            if (employees != null)
            {
                foreach (var emp in employees)
                {

                    var emps = new GetAllEmployeeDto()
                    {
                        ID = emp.Id,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        BirthDate = emp.BirthDate.ToString("yyyy-MM-dd"),
                        ArrivalTime = emp.ArrivalTime.ToString("hh\\:mm"), // Format TimeSpan as "hh:mm"
                        LeaveTime = emp.LeaveTime.ToString("hh\\:mm"), // Format TimeSpan as "hh:mm"
                        City = emp.City,
                        Country = emp.Country,
                        Gender = emp.Gender,
                        HireDate = emp.HireDate.ToString("yyyy-MM-dd"),
                        NationalId = emp.NationalId,
                        Nationality = emp.Nationality,
                        Salary = emp.Salary,
                        DepartmentId = emp.DepartmentId,
                        DeptName = emp.Department.DeptName,
                        UserId = emp.UserId
                        
                    };
                    EmployeeList.Add(emps);
                }
                return EmployeeList;
            }
            else
            {
                return Enumerable.Empty<GetAllEmployeeDto>();
            }
        }

        public GetAllEmployeeDto GetEmployeeByUserId(string userId)
        {
            var employees = uniteOfWork.EmployeeRepository.Get(x => x.UserId == userId, includeProperties: "Department");
            if (employees != null)
            {
                var emps = new GetAllEmployeeDto()
                {
                    ID = employees.Id,
                    UserId = employees.UserId,
                    FirstName = employees.FirstName,
                    LastName = employees.LastName,
                    BirthDate = employees.BirthDate.ToString("yyyy-MM-dd"),
                    ArrivalTime = employees.ArrivalTime.ToString("hh\\:mm"), // Format TimeSpan as "hh:mm"
                    LeaveTime = employees.LeaveTime.ToString("hh\\:mm"), // Format TimeSpan as "hh:mm"  
                    City = employees.City,
                    Country = employees.Country,
                    Gender = employees.Gender,
                    HireDate = employees.HireDate.ToString("yyyy-MM-dd"),
                    NationalId = employees.NationalId,
                    Nationality = employees.Nationality,
                    Salary = employees.Salary,
                    DepartmentId = employees.DepartmentId,
                    DeptName = employees.Department.DeptName
                };
                return emps;
            }
            else
            {
                throw new Exception("Employee Not Found");
            }
        }
        public void CreateEmployee(GetAllEmployeeDto EmployeeDto)
        {
            try
            {

                DateTime BirthDate = DateTime.Parse(EmployeeDto.BirthDate);
                DateTime HireDate = DateTime.Parse(EmployeeDto.HireDate);
     
                TimeSpan arrivalTime = TimeSpan.ParseExact(EmployeeDto.ArrivalTime, "hh\\:mm", CultureInfo.InvariantCulture);
                TimeSpan leaveTime = TimeSpan.ParseExact(EmployeeDto.LeaveTime, "hh\\:mm", CultureInfo.InvariantCulture);

                var empDto = new Employee
                {
                    FirstName = EmployeeDto.FirstName,
                    LastName = EmployeeDto.LastName,
                    ArrivalTime = arrivalTime,
                    LeaveTime = leaveTime,
                    BirthDate = BirthDate,
                    City = EmployeeDto.City,
                    Country = EmployeeDto.Country,
                    Gender = EmployeeDto.Gender,
                    HireDate = HireDate,
                    NationalId = EmployeeDto.NationalId,
                    Nationality = EmployeeDto.Nationality,
                    Salary = EmployeeDto.Salary,
                    DepartmentId = EmployeeDto.DepartmentId,
                };

                uniteOfWork.EmployeeRepository.Add(empDto);
                uniteOfWork.Save();

            }
            catch (Exception)
            {

                throw new Exception("Error happend during create ");
            }
        }

        public GetAllEmployeeDto GetEmployeeId(int id)
        {
            var employees = uniteOfWork.EmployeeRepository.Get(x => x.Id == id, includeProperties: "Department");
            if (employees != null)
            {
                var emps = new GetAllEmployeeDto()
                {
                    ID = employees.Id,
                    FirstName = employees.FirstName,
                    LastName = employees.LastName,
                    BirthDate = employees.BirthDate.ToString("yyyy-MM-dd"),
                    ArrivalTime = employees.ArrivalTime.ToString("hh\\:mm"), // Format TimeSpan as "hh:mm"
                    LeaveTime = employees.LeaveTime.ToString("hh\\:mm"), // Format TimeSpan as "hh:mm"
                    City = employees.City,
                    Country = employees.Country,
                    Gender = employees.Gender,
                    HireDate = employees.HireDate.ToString("yyyy-MM-dd"),
                    NationalId = employees.NationalId,
                    Nationality = employees.Nationality,
                    Salary = employees.Salary,
                    DepartmentId = employees.DepartmentId,
                    DeptName = employees.Department.DeptName
                };
                return emps;
            }
            else
            {
                return null;
            }
        }

        public void UpdateEmployee(GetAllEmployeeDto EmployeeDto, int id)
        {
            try
            {
                var employeFromDb = uniteOfWork.EmployeeRepository.Get(x => x.Id == id );
                if (employeFromDb != null)
                {

                    DateTime BirthDate = DateTime.Parse(EmployeeDto.BirthDate);
                    DateTime HireDate = DateTime.Parse(EmployeeDto.HireDate);
                    
                    TimeSpan arrivalTime = TimeSpan.ParseExact(EmployeeDto.ArrivalTime, "hh\\:mm", CultureInfo.InvariantCulture);
                    TimeSpan leaveTime = TimeSpan.ParseExact(EmployeeDto.LeaveTime, "hh\\:mm", CultureInfo.InvariantCulture);

                    employeFromDb.Id = EmployeeDto.ID;
                    employeFromDb.FirstName = EmployeeDto.FirstName;
                    employeFromDb.LastName = EmployeeDto.LastName;
                    employeFromDb.ArrivalTime = arrivalTime; // Use the parsed TimeSpan
                    employeFromDb.LeaveTime = leaveTime; // Use the parsed TimeSpan
                    employeFromDb.BirthDate = BirthDate;
                    employeFromDb.City = EmployeeDto.City;
                    employeFromDb.Country = EmployeeDto.Country;
                    employeFromDb.Gender = EmployeeDto.Gender;
                    employeFromDb.HireDate = HireDate;
                    employeFromDb.NationalId = EmployeeDto.NationalId;
                    employeFromDb.Nationality = EmployeeDto.Nationality;
                    employeFromDb.Salary = EmployeeDto.Salary;
                    employeFromDb.DepartmentId = EmployeeDto.DepartmentId;
                    employeFromDb.UserId = EmployeeDto.UserId;
                    uniteOfWork.EmployeeRepository.Update(employeFromDb);
                    uniteOfWork.Save();
                }
                else
                {
                    throw new Exception("Not found");
                }
            }
            catch (Exception)
            {
                throw new Exception("Error happend");
            }
        }

     
        public void Remove(int id)
        {
            var emp = uniteOfWork.EmployeeRepository.Get(x => x.Id == id);

            if (emp != null)
            {
                var userId = emp.UserId;
                uniteOfWork.EmployeeRepository.Remove(emp);

                if (!string.IsNullOrEmpty(userId))
                {
                    var user = userManager.FindByIdAsync(userId).Result;
                    if (user != null)
                    {
                        var result = userManager.DeleteAsync(user).Result;

                        if (result.Succeeded)
                        {
                            uniteOfWork.Save();
                        }
                    }
                }
                else
                {
                    uniteOfWork.Save();
                }
            }
        }
        #endregion
    }
}
