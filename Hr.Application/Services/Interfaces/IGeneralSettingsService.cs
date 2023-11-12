using Hr.Application.DTOs;
using Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Services.Interfaces
{
    public interface IGeneralSettingsService
    {
        IEnumerable<GeneralSettings> GetAllGeneralSettings();
        GeneralSettings GetGeneralSettingId(int id);
        public GeneralSettings GetGeneralSettingForAll();
        void Create(GeneralSettings generalSettings);
        void Update(GeneralSettings generalSettings);
       
        void Remove(GeneralSettings generalSettings);
        IEnumerable<string> GetWeekendDaysForGeneralSettings(int generalSettingsId);
        public bool CheckGeneralSettingsExists(int? empid);
        public bool  CheckEmployeeExists(int? empid);
        public GeneralSettings GetGeneralSettingByID(int id);
        public bool CheckWeekendById(int weekId);
    }
}
