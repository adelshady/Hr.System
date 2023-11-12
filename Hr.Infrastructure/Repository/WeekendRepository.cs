using Hr.Application.Interfaces;
using Hr.Domain.Entities;
using Hr.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Infrastructure.Repository
{
    public class WeekendRepository:Repository<Weekend>,IWeekendRepository
    {
        ApplicationDbContext context;
        public WeekendRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public void update(Weekend weekend)
        {
            context.Update(weekend);
        }

    }
}
