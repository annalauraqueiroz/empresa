using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Company>> FindAllAsync()
        {
            return await _context.Company.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
