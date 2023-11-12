using Hr.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Interfaces
{
    public interface IUnitOfWork
    {

        IAttendanceRepository AttendanceRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IRoleRepository RoleRepository { get; }
        IPublicHolidaysRepository PublicHolidaysRepository { get; }
        IWeekendRepository WeekendRepository { get; }
        IGeneralSettingsRepository GeneralSettingsRepository { get; }
        int Save();
    }
}
