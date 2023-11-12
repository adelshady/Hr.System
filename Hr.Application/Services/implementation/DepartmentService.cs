using Hr.Application.DTOs.Department;
using Hr.Application.Interfaces;
using Hr.Application.Services.Interfaces;
using Hr.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Services.implementation
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool CheckDepartmentExists(DepartmentDTO departmentDto)
        {
            return unitOfWork.DepartmentRepository.Any(x => x.DeptName.ToLower() == departmentDto.Name.ToLower());
        }

        public void Create(DepartmentDTO departmentDto)
        {
            if(departmentDto != null)
            {
                var department = new Department
                {
                    DeptName = departmentDto.Name
                };

                unitOfWork.DepartmentRepository.Add(department);
                unitOfWork.Save();
            }
         
        }

        public IEnumerable<DepartmentDTO> GetAllDepartment()
        {
            var departmentDtos= new List<DepartmentDTO>();
            var allDepartments = unitOfWork.DepartmentRepository.GetAll();
            if (allDepartments != null && allDepartments.Any())
            {
                foreach (var department in allDepartments)
                {
                    var departmentDTO = new DepartmentDTO()
                    {
                        Id = department.Id,
                        Name = department.DeptName
                    };
                    departmentDtos.Add(departmentDTO);
                }
                return departmentDtos;
            }
            else
            {
                return null;
            }
        }

        public DepartmentDTO GetDepartmentId(int id)
        {
            
            var department = unitOfWork.DepartmentRepository.Get(x => x.Id == id);

            if (department != null)
            {
                var departmentDTO = new DepartmentDTO
                {
                    Id = department.Id,
                    Name = department.DeptName
                };

                return departmentDTO;
            }
            else
            {
                throw new Exception("Not found Department");
            }

        }

        public void Update(int id, DepartmentDTO departmentDto)
        {
            try 
            { 
                var existingDepartment = unitOfWork.DepartmentRepository.Get(x => x.Id == id);

                if (existingDepartment == null)
                {
                    throw new Exception("Not found Department");
                }
                existingDepartment.DeptName = departmentDto.Name;

                unitOfWork.DepartmentRepository.Update(existingDepartment);
                unitOfWork.Save();
            }
            catch
            {
                throw new Exception("Not found Department");
            }
             
        }

        public (bool IsSuccess, int employeeCount) Remove(int id)
        {
            var department = unitOfWork.DepartmentRepository.Get(x => x.Id == id);
            var employee = unitOfWork.EmployeeRepository.GetAll(x => x.DepartmentId == department.Id);

            int employeeCount = 0;

            if (employee != null)
            {
                foreach (var emp in employee)
                {
                    employeeCount++;
                }
            }

            if (department == null)
            {
                throw new Exception("Not found Department");
            }

            if (employeeCount > 0)
            {
                // There are students in the department, don't remove it
                return (false, employeeCount);
            }

            unitOfWork.DepartmentRepository.Remove(department);
            unitOfWork.Save();

            return (true, 0); // Operation was successful, no students left
        }
    }

}
