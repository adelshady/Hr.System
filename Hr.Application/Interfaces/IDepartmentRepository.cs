using Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        public void Update(Department department);
    }
}
