using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Runtime.CompilerServices;

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
                IsDeleted = role.IsDeleted,
                Company = new GetCompanyDTO
                {
                    Id = role.Company.Id,
                    Name = role.Company.Name,
                    IsDeleted = role.Company.IsDeleted,
                }
            }).ToListAsync();
        }
        public async Task<List<GetRoleDTO>> GetRoles(string name)
        {
            IQueryable<Role> roleAsync = _context.Role.Where(role => role.IsDeleted == false);

            if (!name.IsNullOrEmpty())
            {
                roleAsync = roleAsync.Where(role => role.Name.Contains(name));
            }

            return await roleAsync.Select(role => new GetRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                BaseSalary = role.BaseSalary,
                IsDeleted = role.IsDeleted,
                Company = new GetCompanyDTO
                {
                    Id = role.Company.Id,
                    Name = role.Company.Name,
                    IsDeleted = role.Company.IsDeleted,
                }
            }).ToListAsync();
        }

        public async Task<CreateRoleDTO> CreateRole(CreateRoleDTO roleDTO)
        {
            
            if (!RoleIsValid(roleDTO).Result)
            {
                throw new Exception("Role is not valid");
            }

            var role = new Role();
            role.Name = roleDTO.Name;
            role.BaseSalary = roleDTO.BaseSalary;
            role.IsDeleted = false;
            role.Company = await _context.Company.Where(company => company.Id == roleDTO.CompanyId).FirstOrDefaultAsync();

            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            var responseDto = new CreateRoleDTO()
            {
                Name = role.Name,
                BaseSalary = role.BaseSalary,
                CompanyId = role.Company.Id
                
            };

            return responseDto;
            
        }
        public async Task<bool> EditRole(EditRoleDTO roleDTO)
        {
            var response = true;
            var role = await _context.Role.Where(role => role.Id == roleDTO.Id && !role.IsDeleted).FirstOrDefaultAsync();

            try
            {
                role.Name = string.IsNullOrEmpty(roleDTO.Name) ? role.Name : roleDTO.Name;
                role.BaseSalary = roleDTO.BaseSalary != 0 ? roleDTO.BaseSalary : role.BaseSalary;

                if (roleDTO.CompanyId > 0)
                {
                    var company = await _context.Company.Where(company => company.Id == roleDTO.CompanyId).FirstOrDefaultAsync();
                    if (company == null)
                    {
                        response = false;
                        throw new InvalidOperationException("company is null");
                    }
                    role.Company = company;
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (!RoleExists(roleDTO.Id).Result)
                {
                    response = false;
                    throw new InvalidOperationException("role is invalid, null or not found");
                }
                else
                {
                    response = false;
                    throw new InvalidOperationException("role id is not valid");
                }
               
            }

            return response;
        }
        public async Task<bool> DeleteRole(int id)
        {
            if (!RoleExists(id).Result)
            {
                return false;
            }

            await _context.Role.Where(role => role.Id == id).ForEachAsync(role =>
                role.IsDeleted = true
            );

            await _context.SaveChangesAsync();

            return true;
        }
    
        public async Task<bool> RoleExists(int id)
        {
            var roleDTO = await GetRole(id);

            return !(roleDTO == null || roleDTO.IsDeleted == true);
        }
        public async Task<bool> RoleIsValid(CreateRoleDTO roleDto)
        {
            if (roleDto == null)
            {
                throw new ArgumentNullException("Role info are necessary to create a new role");

            }

            if (string.IsNullOrEmpty(roleDto.Name) == true)
            {
                
                throw new ArgumentNullException("Field name cannot be null");
            }

            if (roleDto.CompanyId <= 0)
            {
                throw new ArgumentNullException("Field Company cannot be null");
               
            }
            
            
            return true;
        }

    }
}
