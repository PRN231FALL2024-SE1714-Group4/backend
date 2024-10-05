using BOs.DTOS;
using BOs;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Implements
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _unitOfWork.RoleRepository.Get();
        }

        public Role GetRoleById(Guid id)
        {
            return _unitOfWork.RoleRepository.GetByID(id);
        }

        public Role CreateRole(RoleRequest roleRequest)
        {
            var role = new Role
            {
                Name = roleRequest.Name
            };

            _unitOfWork.RoleRepository.Insert(role);
            _unitOfWork.Save();
            return _unitOfWork.RoleRepository.GetByID(role.RoleID);
        }

        public Role UpdateRole(Guid id, RoleRequest roleRequest)
        {
            var role = _unitOfWork.RoleRepository.GetByID(id);
            if (role == null) return null;

            role.Name = roleRequest.Name;

            _unitOfWork.RoleRepository.Update(id, role);
            _unitOfWork.Save();

            return _unitOfWork.RoleRepository.GetByID(role.RoleID);
        }

        public bool DeleteRole(Guid id)
        {
            var isDeleted = _unitOfWork.RoleRepository.Delete(id);
            if (isDeleted)
            {
                _unitOfWork.Save();
            }
            return isDeleted;
        }
    }
}
