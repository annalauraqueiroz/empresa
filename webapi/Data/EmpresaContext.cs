using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data
{
    public class EmpresaContext : DbContext
    {
        public EmpresaContext (DbContextOptions<EmpresaContext> options)
            : base(options) 
        {
        }
        public DbSet<Empresa> Empresa { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empresa>().ToTable("empresa");
        }
    }
}
