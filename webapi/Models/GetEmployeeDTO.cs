namespace webapi.Models
{
    public class GetEmployeeDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public Role Role { get; set; }
        public Company Company { get; set; }
    }
}
