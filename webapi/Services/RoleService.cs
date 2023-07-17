using webapi.Data;

namespace webapi.Services
{
    public class RoleService
    {
        private readonly CompanyContext _context;
        public RoleService(CompanyContext context)
        {
            _context = context;
        }

    }
}
