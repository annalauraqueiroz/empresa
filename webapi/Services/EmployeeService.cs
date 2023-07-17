using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Services
{
    public class EmployeeService
    {
        private readonly CompanyContext _context;
        public EmployeeService(CompanyContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> FindAllAsync()
        {
            return await _context.Employee.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
