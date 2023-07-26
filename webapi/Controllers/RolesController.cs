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

        [HttpGet("{id}")]
        public async Task<ActionResult<GetRoleDTO>> GetRole(int id)
        {
            return await _roleService.GetRole(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> PutRole(EditRoleDTO roleDto)
        {
            return await _roleService.EditRole(roleDto) ? "Data changed successfully" : "There was a problem editing data"; 
        }

        [HttpPost]
        public async Task<ActionResult<CreateRoleDTO>> CreateRole(CreateRoleDTO roleDto)
        {
            return await _roleService.CreateRole(roleDto);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteRole(int id)
        {
            return await _roleService.DeleteRole(id);
        }

        
    }
}
