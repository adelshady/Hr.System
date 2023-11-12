using Hr.Application.DTOs;
using Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Services.Interfaces
{
    public interface IPublicHolidaysService
    {
        IEnumerable<PublicHolidays> GetAllPublicHolidays();
        PublicHolidays GetPublicHolidayId(int id);
        void Create(PublicHolidays publicHoliday);
        void Update(PublicHolidays publicHoliday);
        bool CheckPublicHolidaysExists(PublicHolidaysDTO publicHolidayDTO);
        void Remove(PublicHolidays publicHoliday);

    }
}
