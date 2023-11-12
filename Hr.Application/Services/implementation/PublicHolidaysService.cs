using Hr.Application.DTOs;
using Hr.Application.Interfaces;
using Hr.Application.Services.Interfaces;
using Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Services.implementation
{
    public class PublicHolidaysService :IPublicHolidaysService
    {
        private readonly IUnitOfWork unitOfWork;

        public PublicHolidaysService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<PublicHolidays> GetAllPublicHolidays()
        {
           return unitOfWork.PublicHolidaysRepository.GetAll();
        }
        public PublicHolidays GetPublicHolidayId(int id)
        {
            return unitOfWork.PublicHolidaysRepository.Get(x=>x.Id==id);
        }
        public void Create(PublicHolidays publicHolidays)
        {
            unitOfWork.PublicHolidaysRepository.Add(publicHolidays);
            unitOfWork.Save();
        }
        public void Update(PublicHolidays publicHoliday)
        {
            unitOfWork.PublicHolidaysRepository.Update(publicHoliday);
            unitOfWork.Save();
        }
        public void Remove(PublicHolidays publicHoliday)
        {
            unitOfWork.PublicHolidaysRepository.Remove(publicHoliday);
            unitOfWork.Save();
        }
      public bool CheckPublicHolidaysExists(PublicHolidaysDTO publicHolidayDTO)
        {
            return unitOfWork.PublicHolidaysRepository.Any(x => x.Name.ToLower() == publicHolidayDTO.Name.ToLower());
        }










    }
}
