using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace webapi.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        [InverseProperty("Employees")]
        public Role Role { get; set; }

        public Employee() { }
        public Employee(int id, string name, Role role)
        {
            Id = id;
            Name = name;
            Role = role;
            IsDeleted = false;
        }
    }
}
