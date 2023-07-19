using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Services
{
    public class RoleService
    {
        private readonly CompanyContext _context;
        public RoleService(CompanyContext context)
        {
            _context = context;
        }

        public async Task<GetRoleDTO> GetRole(int roleId)
        {
            var roles = await this.GetRoles(new List<int>() { roleId });

            return roles.FirstOrDefault();
        }

        public async Task<List<GetRoleDTO>> GetRoles(List<int> roleIds = null)
        {
            IQueryable<Role> roleAsync = _context.Role;

            if (roleIds != null || roleIds.Count > 0)
            {
                roleAsync = roleAsync.Where(role => roleIds.Contains(role.Id));
            }

            return await roleAsync.Select(role => new GetRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                BaseSalary = role.BaseSalary,
                Company = new GetCompanyDTO
                {
                    Id = role.Company.Id,
                    Name = role.Company.Name,
                }
            }).ToListAsync();
        }


        public async Task<bool> RoleIsValid(CreateEditRoleDTO role)
        {
            return false;
        }
    }
}
