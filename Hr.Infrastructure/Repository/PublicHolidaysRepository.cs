using Hr.Domain.Entities;
using Hr.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hr.Infrastructure.Data;

namespace Hr.Infrastructure.Repository
{
    public class PublicHolidaysRepository :Repository<PublicHolidays>,IPublicHolidaysRepository
    {

        private readonly ApplicationDbContext context;
        public PublicHolidaysRepository(ApplicationDbContext context): base(context)
        {
            this.context = context;
        }

        public void Update(PublicHolidays publicHoliday)
        {
            context.Update(publicHoliday);
        }









    }
}
