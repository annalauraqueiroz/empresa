﻿using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Data;

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
            
            var company = await _context.Company.Where(company => company.Id == roleDTO.CompanyId).AsNoTracking().FirstOrDefaultAsync();

            await _context.Role.Where(role => role.Id == roleDTO.Id).ForEachAsync(role => 
                {
                    role.Name = string.IsNullOrEmpty(roleDTO.Name) ? role.Name : roleDTO.Name;
                    role.BaseSalary = roleDTO.BaseSalary != 0 ? roleDTO.BaseSalary : role.BaseSalary;
                    role.Company = roleDTO.CompanyId != 0 ? new Company { Id = company.Id, Name = company.Name } : role.Company;
                }
            );

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(roleDTO.Id).Result)
                {
                    throw new InvalidOperationException("role is null");
                }
                else
                {
                    throw new InvalidOperationException("role id is not valid");
                }
               
            }

            return true;
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
