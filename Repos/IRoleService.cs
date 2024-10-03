using BOs;
using BOs.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAllRoles();
        Role GetRoleById(Guid id);
        Role CreateRole(RoleRequest roleRequest);
        Role UpdateRole(Guid id, RoleRequest roleRequest);
        bool DeleteRole(Guid id);
    }
}
