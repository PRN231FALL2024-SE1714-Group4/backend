using BOs.DTOS;
using BOs;
using Microsoft.AspNetCore.Mvc;
using Repos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/Role
        [HttpGet]
        public ActionResult<IEnumerable<Role>> GetAllRoles()
        {
            var roles = _roleService.GetAllRoles();
            return Ok(roles);
        }

        // GET: api/Role/{id}
        [HttpGet("{id}")]
        public ActionResult<Role> GetRoleById(Guid id)
        {
            var role = _roleService.GetRoleById(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        // POST: api/Role
        [HttpPost]
        public ActionResult<Role> CreateRole([FromBody] RoleRequest roleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = _roleService.CreateRole(roleRequest);
            return Ok(role);
        }

        // PUT: api/Role/{id}
        [HttpPut("{id}")]
        public ActionResult<Role> UpdateRole(Guid id, [FromBody] RoleRequest roleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = _roleService.UpdateRole(id, roleRequest);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        // DELETE: api/Role/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteRole(Guid id)
        {
            var deleted = _roleService.DeleteRole(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
