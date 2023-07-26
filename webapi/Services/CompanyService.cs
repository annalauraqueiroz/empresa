using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using webapi.Data;
using webapi.Models;

namespace webapi.Services
{
    public class CompanyService
    {
        private readonly CompanyContext _context;
        public CompanyService(CompanyContext context)
        {
            _context = context;
        }

        public async Task<GetCompanyDTO> GetCompany(int companyId)
        {
            var companies = await GetCompanies(new List<int>() { companyId });

            return companies.FirstOrDefault();
        }
        public async Task<List<GetCompanyDTO>> GetCompanies(List<int> companyIds = null)
        {
            IQueryable<Company> companyAsync = _context.Company.Where(company => company.IsDeleted == false).OrderBy(c => c.Name);

            if (!companyIds.IsNullOrEmpty()
                || companyIds.Count > 0)
            {
                companyAsync = companyAsync.Where(company => companyIds.Contains(company.Id));
            }


            return await companyAsync.Select(company => new GetCompanyDTO
            {
                Id = company.Id,
                Name = company.Name,
                IsDeleted = company.IsDeleted,
                Roles = company.Roles.Where(role => role.IsDeleted == false).Select(role => new GetRoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,

                }).ToList()
            }).ToListAsync();
        }

        public async Task<CreateCompanyDTO> CreateCompany(CreateCompanyDTO companyDTO)
        {

            if (!CompanyIsValid(companyDTO).Result)
            {
                throw new Exception("Company is not valid");
            }

            var company = new Company();
            company.Name = companyDTO.Name;
            company.IsDeleted = false;
            
            _context.Company.Add(company);
            await _context.SaveChangesAsync();

            var responseDto = new CreateCompanyDTO()
            {
                Name = company.Name,
            };

            return responseDto;

        }
        public async Task<bool> EditCompany(EditCompanyDTO companyDTO)
        {
            var response = true;
            var company = await _context.Company.Where(company => company.Id == companyDTO.Id && !company.IsDeleted).FirstOrDefaultAsync();

            try
            {
                company.Name = string.IsNullOrEmpty(companyDTO.Name) ? company.Name : companyDTO.Name;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (!CompanyExists(companyDTO.Id).Result)
                {
                    response = false;
                    throw new InvalidOperationException("Company is invalid, null or not found");
                }
                else
                {
                    response = false;
                    throw new InvalidOperationException("Company id is not valid");
                }

            }

            return response;
        }
        public async Task<bool> DeleteCompany(int id)
        {
            if(!CompanyExists(id).Result)
            {
                return false;
            }
            await _context.Company.Where(comp => comp.Id == id).ForEachAsync(comp =>
                { 
                    comp.IsDeleted = true;
                }
            );
            await _context.Role.Where(role => role.Company.Id == id).ForEachAsync(role =>
                role.IsDeleted = true
            );
            

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CompanyExists(int id)
        {
            var companyDTO = await GetCompany(id);

            return !(companyDTO == null || companyDTO.IsDeleted == true);
        }
        public async Task<bool> CompanyIsValid(CreateCompanyDTO companyDto)
        {
            if (companyDto == null)
            {
                throw new ArgumentNullException("Company info are necessary to create a new company");

            }

            if (string.IsNullOrEmpty(companyDto.Name) == true)
            {

                throw new ArgumentNullException("Field name cannot be null");
            }


            return true;
        }



    }
}
