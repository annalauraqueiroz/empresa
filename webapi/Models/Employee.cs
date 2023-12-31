﻿using System.Data;

namespace webapi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public Company Company { get; set; }

        public Employee() { }
        public Employee(int id, string name, Role role, Company company)
        {
            Id = id;
            Name = name;
            Role = role;
            Company = company;
        }
    }
}
