using Hr.Application.Interfaces;
using Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Infrastructure.Repository
{
    public interface IGeneralSettingsRepository :IRepository<GeneralSettings>
    {
        void Update(GeneralSettings generalSettings);
    }
}