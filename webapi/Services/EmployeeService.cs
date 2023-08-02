using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.Design;
using System.Data;
using webapi.Data;
using webapi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace webapi.Services
{
    public class EmployeeService
    {
        private readonly CompanyContext _context;
        public EmployeeService(CompanyContext context)
        {
            _context = context;
        }
        public async Task<GetEmployeeDTO> GetEmployee(int id)
        {
            var employees = await GetEmployees(new List<int> { id });

            return employees.FirstOrDefault();
        }
        public async Task<List<GetEmployeeDTO>> GetEmployees(string name)
        {
            IQueryable<Employee> empAsync = _context.Employee.Where(emp => emp.IsDeleted == false).OrderBy(x => x.Name);

            if (!name.IsNullOrEmpty())
            {
                empAsync = empAsync.Where(emp => emp.Name.Contains(name));
            }

            return await empAsync.Select(emp => new GetEmployeeDTO
            {
                Id = emp.Id,
                Name = emp.Name,
                IsDeleted = emp.IsDeleted,
                Role = new GetRoleDTO
                {
                    Id = emp.Role.Id,
                    Name = emp.Role.Name,
                    BaseSalary = emp.Role.BaseSalary,
                    Company = new GetCompanyDTO
                    {
                        Id = emp.Role.Company.Id,
                        Name = emp.Role.Company.Name
                    }
                },

            }).ToListAsync();
        }
        public async Task<List<GetEmployeeDTO>> GetEmployees(List<int> empIds = null)
        {
            IQueryable<Employee> empAsync = _context.Employee.Where(emp => emp.IsDeleted == false).OrderBy(x => x.Name);

            if (!empIds.IsNullOrEmpty()
                || empIds.Count > 0)
            {
                empAsync = empAsync.Where(emp => empIds.Contains(emp.Id));
            }

            return await empAsync.Select(emp => new GetEmployeeDTO
            {
                Id = emp.Id,
                Name = emp.Name,
                IsDeleted = emp.IsDeleted,
                Role = new GetRoleDTO
                {
                    Id = emp.Role.Id,
                    Name = emp.Role.Name,
                    BaseSalary = emp.Role.BaseSalary,
                    Company = new GetCompanyDTO
                    {
                        Id = emp.Role.Company.Id,
                        Name = emp.Role.Company.Name
                    }
                },
                
            }).ToListAsync();
        }
        public async Task<List<GetEmployeeDTO>> GetEmployeesByCompanyId(int companyId)
        {
            IQueryable<Employee> empAsync = _context.Employee
                .Include(emp => emp.Role)
                .ThenInclude(role => role.Company)
                .Where(emp => emp.IsDeleted == false && emp.Role.Company.Id == companyId)
                .OrderBy(x => x.Name);

            return await empAsync.Select(emp => new GetEmployeeDTO
            {
                Id = emp.Id,
                Name = emp.Name,
                Role = new GetRoleDTO
                {
                    Id = emp.Role.Id,
                    Name = emp.Role.Name,
                    BaseSalary = emp.Role.BaseSalary,
                    Company = new GetCompanyDTO
                    {
                        Id = emp.Role.Company.Id,
                        Name = emp.Role.Company.Name,
                    }
                },

            }).ToListAsync();
        }
        public async Task<List<GetEmployeeDTO>> GetEmployeesByRoleId(int roleId)
        {
            IQueryable<Employee> empAsync = _context.Employee
                .Include(emp => emp.Role)
                .ThenInclude(role => role.Company)
                .Where(emp => emp.IsDeleted == false && emp.Role.Id == roleId)
                .OrderBy(x => x.Name);

            return await empAsync.Select(emp => new GetEmployeeDTO
            {
                Id = emp.Id,
                Name = emp.Name,
                Role = new GetRoleDTO
                {
                    Id = emp.Role.Id,
                    Name = emp.Role.Name,
                    BaseSalary = emp.Role.BaseSalary,
                    Company = new GetCompanyDTO
                    {
                        Id = emp.Role.Company.Id,
                        Name = emp.Role.Company.Name,
                    }
                },

            }).ToListAsync();
        }
        public async Task<CreateEmployeeDTO> CreateEmployee(CreateEmployeeDTO employeeDTO)
        {
            var employee = new Employee();
            try
            {
                employee.Name = employeeDTO.Name;
                employee.IsDeleted = false;
                employee.Role = await _context.Role.Where(role => role.Id == employeeDTO.RoleId).FirstOrDefaultAsync();


                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();
            }
            catch (NullReferenceException)
            {
                throw new Exception("Role id is invalid");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            

            var responseDto = new CreateEmployeeDTO()
            {
                Name = employee.Name,
                RoleId = employeeDTO.RoleId,
            };

            return responseDto;

        }
        public async Task<string> EditEmployee(EditEmployeeDTO employeeDTO)
        {
            var employee = await _context.Employee
                .Where(emp => !emp.IsDeleted && emp.Id == employeeDTO.Id)
                .FirstOrDefaultAsync();

            try
            {
                employee.Name = string.IsNullOrEmpty(employeeDTO.Name) ? employee.Name : employeeDTO.Name;
                if(employeeDTO.RoleId > 0 )
                {
                    var role = await _context.Role.Where(role => role.Id == employeeDTO.RoleId && role.IsDeleted == false).FirstOrDefaultAsync();
                    if (role == null)
                    {
                        throw new InvalidOperationException("role is invalid");
                    }
                    employee.Role = role;
                }
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                if (!EmployeeExists(employeeDTO.Id).Result)
                {
                    throw new InvalidOperationException("employee is invalid, null or not found");
                }
                return ex.Message;
            }

            return "Employee data successfuly changed";
        }
        public async Task<string> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _context.Employee.Where(emp => emp.Id == id && !emp.IsDeleted).FirstOrDefaultAsync();
                if (employee == null)
                {
                    throw new InvalidOperationException("employee id is not valid");
                }
                employee.IsDeleted = true;

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
            return "Employee deleted";
        }
        public async Task<bool> EmployeeExists(int id)
        {
            var employee = await GetEmployee(id);

            return !(employee == null || employee.IsDeleted == true);
        }

    }


    
}
