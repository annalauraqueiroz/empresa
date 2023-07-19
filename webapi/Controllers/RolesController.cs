using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly CompanyContext _context;
        private readonly RoleService _roleService;

        public RolesController(CompanyContext context, RoleService roleService)
        {
            _context = context;
            _roleService = roleService;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<List<GetRoleDTO>>> GetRole()
        {
            return await _roleService.GetRoles();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetRoleDTO>> GetRole(int id)
        {
            return await _roleService.GetRole(id);
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<GetRoleDTO>> CreateRole(CreateEditRoleDTO roleDto)
        {
            if (roleDto == null)
            {
              return Problem("É necessário fornecer as informações do cargo");
            }

            if (string.IsNullOrEmpty(roleDto.Name) == true)
            {
              return Problem("É necessário o nome do cargo");
            }

            if (roleDto.CompanyId <= 0) 
            {
                return Problem("É necessário informar a empresa que o cargo pertence");
            }

            var alreadyExist = await _context.Role.Where(role => role.Id == roleDto.Id).AnyAsync();

            if (alreadyExist == true) 
            { 
                return Problem("O ID deste cargo já existe. Por favor, utilize outro ID.");
            }

            var role = new Role();
            role.Id = roleDto.Id;
            role.Name = roleDto.Name;
            role.BaseSalary = roleDto.BaseSalary;
            role.Company = await _context.Company.Where(company => company.Id == roleDto.CompanyId).FirstOrDefaultAsync();;

            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            var responseDto = new GetRoleDTO()
            {
                Id = role.Id,
                Name = role.Name,
                BaseSalary = role.BaseSalary,
                Company = new GetCompanyDTO
                {
                    Id = role.Company.Id,
                    Name = role.Company.Name
                },
            };

            return CreatedAtAction("CreateRole", new { id = role.Id }, responseDto);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (_context.Role == null)
            {
                return NotFound();
            }
            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Role.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return (_context.Role?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
