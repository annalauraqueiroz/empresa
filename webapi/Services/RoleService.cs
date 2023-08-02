using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                },
                Employees = role.Employees.Where(emp => emp.IsDeleted == false).Select(emp => new GetEmployeeDTO
                {
                    Id = emp.Id,
                    Name = emp.Name,

                }).ToList(),
            }).ToListAsync();
        }
        public async Task<List<GetRoleDTO>> GetRoles(string name)
        {
            IQueryable<Role> roleAsync = _context.Role
                .Where(role => role.IsDeleted == false);

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
                },
                Employees = role.Employees.Where(emp => emp.IsDeleted == false && emp.Role.Id == role.Id).Select(emp => new GetEmployeeDTO
                {
                    Id = role.Id,
                    Name = role.Name,

                }).ToList(),
            }).ToListAsync();
        }
        public async Task<List<GetRoleDTO>> GetRolesByCompanyId(int companyId)
        {
            IQueryable<Role> roleAsync = _context.Role
                .Where(role => role.IsDeleted == false && role.Company.Id == companyId);


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
                },
                Employees = role.Employees.Where(emp => emp.IsDeleted == false && emp.Role.Id == role.Id).Select(emp => new GetEmployeeDTO
                {
                    Id = role.Id,
                    Name = role.Name,

                }).ToList(),
            }).ToListAsync();
        }
        public async Task<CreateRoleDTO> CreateRole(CreateRoleDTO roleDTO)
        {
            var role = new Role();
           
            try
            {
                role.Name = roleDTO.Name;
                role.BaseSalary = roleDTO.BaseSalary;
                role.IsDeleted = false;
                role.Company = await _context.Company.Where(company => company.Id == roleDTO.CompanyId && !company.IsDeleted).FirstOrDefaultAsync();
                _context.Role.Add(role);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (!RoleIsValid(roleDTO).Result)
                {
                    throw new Exception("Role is not valid");
                }else if(role.Company == null)
                {
                    throw new Exception("Company is not valid.");
                }
            }
            var responseDto = new CreateRoleDTO()
            {
                Name = role.Name,
                BaseSalary = role.BaseSalary,
                CompanyId = role.Company.Id
                
            };

            return responseDto;
            
        }
        public async Task<string> EditRole(EditRoleDTO roleDTO)
        {
            try
            {
                var role = await _context.Role.Where(role => role.Id == roleDTO.Id && !role.IsDeleted).FirstOrDefaultAsync();
                
                role.Name = string.IsNullOrEmpty(roleDTO.Name) ? role.Name : roleDTO.Name;
                role.BaseSalary = roleDTO.BaseSalary != 0 ? roleDTO.BaseSalary : role.BaseSalary;

                if (roleDTO.CompanyId > 0)
                {
                    var company = await _context.Company.Where(company => company.Id == roleDTO.CompanyId && !company.IsDeleted).FirstOrDefaultAsync();
                    if(company == null)
                    {
                        throw new NullReferenceException("company is invalid");
                    }
                    role.Company.Id = company.Id;
                    role.Company.Name = company.Name;
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (!RoleExists(roleDTO.Id).Result)
                {
                    throw new InvalidOperationException("role is invalid, null or not found");
                }
                    return e.Message;
            }

            return "Data successfuly changed";
        }
        public async Task<string> DeleteRole(int id)
        {
            if (!RoleExists(id).Result)
            {
                return "Role id is invalid";
            }
            var role = await _context.Role.Where(role => role.Id == id).FirstOrDefaultAsync();

            if (role.Employees.Any())
            {
                return "Cannot delete role if there is employees attached to position";
            }
            role.IsDeleted = true;

            await _context.SaveChangesAsync();

            return "Role successfuly deleted";
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
