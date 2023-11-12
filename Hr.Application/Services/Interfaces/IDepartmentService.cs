using Hr.Application.DTOs.Department;
using Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Services.Interfaces
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentDTO> GetAllDepartment();
         DepartmentDTO GetDepartmentId(int id);
        void Create(DepartmentDTO departmentDto);
        void Update(int id,DepartmentDTO department);

        bool CheckDepartmentExists(DepartmentDTO departmentDto);
        (bool IsSuccess, int employeeCount) Remove(int id);
    }
}
