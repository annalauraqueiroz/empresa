using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetEmployeeDTO>>> GetEmployees()
        {
            return await _employeeService.GetEmployees(new List<int> { });
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<GetEmployeeDTO>>> GetEmployees(string name)
        {
            return await _employeeService.GetEmployees(name);
        }
        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<IEnumerable<GetEmployeeDTO>>> GetEmployeesByCompanyId(int companyId)
        {
            return await _employeeService.GetEmployeesByCompanyId(companyId);
        }
        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<List<GetEmployeeDTO>>> GetEmployeeByRoleId(int roleId)
        {
            return await _employeeService.GetEmployeesByRoleId(roleId);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetEmployeeDTO>> GetEmployee(int id)
        {
            return await _employeeService.GetEmployee(id);
        }
        [HttpPut("{id}")]
        public async Task<string> EditEmployee(int id, EditEmployeeDTO employee)
        {
            return await _employeeService.EditEmployee(employee);
        }

        [HttpPost]
        public async Task<ActionResult<CreateEmployeeDTO>> CreateEmployee(CreateEmployeeDTO employee)
        {
            return await _employeeService.CreateEmployee(employee);
        }

        [HttpDelete("{id}")]
        public async Task<string> DeleteEmployee(int id)
        {
            return await _employeeService.DeleteEmployee(id);
        }

    }
}
