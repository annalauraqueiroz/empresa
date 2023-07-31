using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        [InverseProperty("Company")]
        public List<Role> Roles { get; set; } = new List<Role>();
        public Company()
        {

        }
        public Company(int id, string name)
        {
            Id = id;
            Name = name;
        }

    }
}
