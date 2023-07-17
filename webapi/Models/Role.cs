using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double BaseSalary { get; set; }
        [InverseProperty("Roles")]
        public Company Company { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public Role() { }
        public Role(int id, string name, double baseSalary)
        {
            Id = id;
            Name = name;
            BaseSalary = baseSalary;
        }
    }
}
