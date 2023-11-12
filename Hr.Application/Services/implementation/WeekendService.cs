using Hr.Application.Common.Enums;
using Hr.Application.DTOs;
using Hr.Application.Interfaces;
using Hr.Application.Services.Interfaces;
using Hr.Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Services.implementation
{
    public class WeekendService : IWeekendService
    {
        IUnitOfWork unitOfWork;
        public List<string> weekends;

        public WeekendService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

        }

        public IEnumerable<Weekend> GetAllWeekends()
        {
            return unitOfWork.WeekendRepository.GetAll(includeProperties:"GeneralSettings");
        }
        public IEnumerable<Weekend> GetById(int id)
        {
            return unitOfWork.WeekendRepository.GetAll(x => x.GeneralSettingsId == id);
        }
        public void Create(Weekend weekend)
        {
            unitOfWork.WeekendRepository.Add(weekend);
            unitOfWork.Save();
        }
        public Weekend GetWeekendById(int id)
        {
            return unitOfWork.WeekendRepository.Get(x => x.GeneralSettingsId == id);
        }
        public Weekend GetWeekendByName(string Name)
        {
            return unitOfWork.WeekendRepository.Get(x => x.Name == Name);
        }
        public bool Update(WeekendDTO updatedWeekends,int generalSettingId)
        {

            if (updatedWeekends == null || updatedWeekends.Weekends == null)
            {
                return false;
            }

            foreach (var day in updatedWeekends.Weekends)
            {
                bool success = day.isSelected;
                if (success)
                {
                    var exsist = unitOfWork.WeekendRepository.Any(x => x.Name.ToLower() == day.displayValue.ToLower() && x.GeneralSettingsId ==generalSettingId);
                    if (exsist)
                    {
                        continue;
                    }
                    var weekday = new Weekend { Name = day.displayValue,GeneralSettingsId=updatedWeekends.Id };
                    unitOfWork.WeekendRepository.Add(weekday);
                }
                else
                {
                    var weekday = unitOfWork.WeekendRepository.Get(x=>x.Name==day.displayValue &&x.GeneralSettingsId==updatedWeekends.Id);
                    if (weekday != null)
                    {
                       unitOfWork.WeekendRepository.Remove(weekday);
                    }
                }
            }
            
            unitOfWork.Save();
            return true;
        }
        public void Delete(Weekend weekend)
        {
            unitOfWork.WeekendRepository.Remove(weekend);
            unitOfWork.Save();
        }
        public List<string> Days()
        {
            var enumValues = Enum.GetValues(typeof(WeekDays)).Cast<WeekDays>();
            var dayStrings = enumValues.Select(enumValue => enumValue.ToString()).ToList();
            return dayStrings;

        }

        public bool CheckPublicHolidaysExists(Weekend weekend)
        {
            return unitOfWork.WeekendRepository.Any(x => x.Name.ToLower() == weekend.Name.ToLower()&&x.GeneralSettingsId==weekend.GeneralSettingsId);
        }

      
    }
}
