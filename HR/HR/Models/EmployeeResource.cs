using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Models
{
    internal class EmployeeResource
    {
        public string id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string department { get; set; }

        public EmployeeResource()
        {
        }

        public EmployeeResource(Employee employee)
        {
            this.id = employee.ID.ToString();
            this.name = employee.Name;
            this.location = employee.Location;
            this.department = employee.Department;
        }

        public Employee GetValue()
        {
            return new Employee
            {
                ID = Guid.Parse(this.id),
                Name = this.name,
                Location = this.location,
                Department = this.department
            };
        }
    }
}
