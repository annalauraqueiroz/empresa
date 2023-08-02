namespace webapi.Models
{
    public class GetEmployeeDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public GetRoleDTO Role { get; set; }
    }
    public class CreateEmployeeDTO
    {
        public string? Name { get; set; }
        public int RoleId { get; set; }
    }
    public class EditEmployeeDTO : CreateEmployeeDTO
    {
        public int Id { get; set; }
    }
}
