namespace webapi.Models
{
    public class GetCompanyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public List<GetRoleDTO> Roles { get; set; } = new List<GetRoleDTO>();
    }
    public class CreateCompanyDTO
    {
        public string? Name { get; set; }
    }

    public class EditCompanyDTO : CreateCompanyDTO
    {
        public int Id { get; set; }
    }
}
