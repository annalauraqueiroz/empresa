using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Models;
using Microsoft.IdentityModel.Tokens;

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
            IQueryable<Role> roleAsync = _context.Role.Where(role => role.IsDeleted == false);

            if (!roleIds.IsNullOrEmpty()
                || roleIds.Count > 0)
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

        public async Task<GetRoleDTO> CreateRole(CreateEditRoleDTO roleDTO)
        {
            
            if (!RoleIsValid(roleDTO).Result)
            {
                throw new Exception("Role is not valid");
            }

            var role = new Role();
            role.Id = roleDTO.Id;
            role.Name = roleDTO.Name;
            role.BaseSalary = roleDTO.BaseSalary;
            role.IsDeleted = false;
            role.Company = await _context.Company.Where(company => company.Id == roleDTO.CompanyId).FirstOrDefaultAsync();

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

            return responseDto;
            
        }
        public async Task<bool> RoleIsValid(CreateEditRoleDTO roleDto)
        {
            if (roleDto == null)
            {
                throw new ArgumentNullException("É necessário fornecer as informações do cargo");

            }

            if (string.IsNullOrEmpty(roleDto.Name) == true)
            {
                
                throw new ArgumentNullException("É necessário o nome do cargo");
            }

            if (roleDto.CompanyId <= 0)
            {
                throw new ArgumentNullException("É necessário informar a empresa que o cargo pertence");
               
            }

            var alreadyExist = await _context.Role.Where(role => role.Id == roleDto.Id).AnyAsync();

            if (alreadyExist == true)
            {
                
                throw new ArgumentException("O ID deste cargo já existe. Por favor, utilize outro ID.");
            }
            
            return true;
        }
    }
}
