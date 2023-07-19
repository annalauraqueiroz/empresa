using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double BaseSalary { get; set; }
        [InverseProperty("Roles")]
        public Company Company { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public Role() { }
        public Role(int id, string name, double baseSalary, Company company)
        {
            Id = id;
            Name = name;
            BaseSalary = baseSalary;
            Company = company;
        }
    }
}
