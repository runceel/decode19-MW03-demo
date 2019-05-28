using System;
using System.Collections.Generic;
using System.Text;

namespace PrismApp.Entities
{
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(int id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
