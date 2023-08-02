using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        
        private readonly RoleService _roleService;

        public RolesController( RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetRoleDTO>>> GetRole()
        {
            return await _roleService.GetRoles(new List<int> {});
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetRoleDTO>> GetRoleById(int id)
        {
            return await _roleService.GetRole(id);
        }
        [HttpGet("{name:alpha}")]
        public async Task<ActionResult<List<GetRoleDTO>>> GetRoles(string name)
        {
            return await _roleService.GetRoles(name);
        }
        [HttpGet("company/{id}")]
        public async Task<ActionResult<List<GetRoleDTO>>> GetRoleByCompanyId(int id)
        {
            return await _roleService.GetRolesByCompanyId(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> PutRole(EditRoleDTO roleDto)
        {
            return await _roleService.EditRole(roleDto); 
        }

        [HttpPost]
        public async Task<ActionResult<CreateRoleDTO>> CreateRole(CreateRoleDTO roleDto)
        {
            return await _roleService.CreateRole(roleDto);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteRole(int id)
        {
            return await _roleService.DeleteRole(id);
        }

        
    }
}
