using Hr.Domain.Entities;
using Hr.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Infrastructure.Repository
{
    public class GeneralSettingsRepository : Repository<GeneralSettings>, IGeneralSettingsRepository
    {
        ApplicationDbContext context;
        public GeneralSettingsRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Update(GeneralSettings generalSettings)
        {
            context.Update(generalSettings);
        }

    }
}
