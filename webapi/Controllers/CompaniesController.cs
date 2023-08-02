using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompaniesController(CompanyService service)
        {
            _companyService = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<GetCompanyDTO>>> GetCompany()
        {
            return await _companyService.GetCompanies(new List<int> { });
        }
       /* [HttpGet]
        public async Task<ActionResult<List<GetEmployeeDTO>>> GetEmployees(int companyId)
        {
            return await _companyService.GetEmployees(companyId);
        }
       */
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCompanyDTO>> GetCompany(int id)
        {
            return await _companyService.GetCompany(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> PutCompany(EditCompanyDTO companyDTO)
        {
           return await _companyService.EditCompany(companyDTO) ? "Data changed successfully" : "There was a problem editing data";
        }
        [HttpPost]
        public async Task<ActionResult<CreateCompanyDTO>> CreateCompany(CreateCompanyDTO companyDTO)
        {
            return await _companyService.CreateCompany(companyDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCompany(int id)
        {
            return await _companyService.DeleteCompany(id);
        }

    }
}
