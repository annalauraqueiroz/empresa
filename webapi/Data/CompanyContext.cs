using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using webapi.Models;

namespace webapi.Data
{
    public class CompanyContext : DbContext
    {
        public CompanyContext(DbContextOptions<CompanyContext> options)
            : base(options)
        {
        }
        public DbSet<Company> Company { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Role> Role { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("company");
            modelBuilder.Entity<Employee>().ToTable("employee");
            modelBuilder.Entity<Role>().ToTable("role");
        }
    }
}
