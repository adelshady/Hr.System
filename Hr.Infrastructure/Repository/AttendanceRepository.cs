using Hr.Application.Interfaces;
using Hr.Domain.Entities;
using Hr.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Infrastructure.Repository
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        private readonly ApplicationDbContext context;

        public AttendanceRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void update(Attendance attendance)
        {
            context.Attendances.Update(attendance);
        }
    }
}
