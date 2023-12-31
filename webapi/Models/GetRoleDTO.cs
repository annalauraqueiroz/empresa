﻿namespace webapi.Models
{
    public class GetRoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double BaseSalary { get; set; }
        public bool IsDeleted { get; set; }
        public GetCompanyDTO Company { get; set; }
    }

    public class CreateRoleDTO
    {
        public string? Name { get; set; }
        public double BaseSalary { get; set; }
        public int CompanyId { get; set; }
    }

    public class EditRoleDTO : CreateRoleDTO 
    {
        public int Id { get; set; }
    }
    


}

