using Hr.Application.Common.Enums;
using Hr.Application.DTOs;
using Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Services.Interfaces
{
    public interface IWeekendService
    {
        IEnumerable<Weekend> GetAllWeekends();
        void Delete(Weekend weekend);
        void Create(Weekend weekend);
        Weekend GetWeekendById(int id);
         Weekend GetWeekendByName(string Name);
        IEnumerable<Weekend> GetById(int id);
        public bool Update(WeekendDTO updatedWeekends, int generalSettingId);
        public List<string> Days();
        public bool CheckPublicHolidaysExists(Weekend weekend);




    }
}
