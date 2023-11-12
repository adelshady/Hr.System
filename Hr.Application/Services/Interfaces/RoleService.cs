using Hr.Application.Interfaces;
using Hr.Application.Services.implementation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Services.Interfaces
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        public Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return _unitOfWork.RoleRepository.GetAllRolesAsync();
        }

        public Task DeleteRoleAsync(string roleId)
        {
            return _unitOfWork.RoleRepository.DeleteRoleAsync(roleId);
        }

       
    }

}
