using System;
using System.Collections.Generic;
using System.Text;

namespace HRFunction.Models
{
    public class EmployeeResourceCollection
    {
        public EmployeeResource[] value { get; set; }

        public EmployeeResourceCollection()
        {
        }
        public EmployeeResourceCollection(string requestPath, Employee[] employees)
        {
            if (employees != null)
            {
                this.value = new EmployeeResource[employees.Length];
                for (int i = 0; i < employees.Length; i++) this.value[i] = new EmployeeResource(requestPath, employees[i]);
            }
        }
    }

    public class EmployeeResource
    {
        public const string ResourceTypeName = "Microsoft.CustomProviders/resourceproviders/employees";
        
        public class EmployeeResourceProperties
        {
            public string id { get; set; }
            public string name { get; set; }
            public string location { get; set; }
            public string department { get; set; }

            public EmployeeResourceProperties()
            {
            }
            public EmployeeResourceProperties(Employee employee)
            {
                this.id = employee.ID.ToString();
                this.name = employee.Name;
                this.location = employee.Location;
                this.department = employee.Department;
            }
        }

        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public EmployeeResourceProperties properties { get; set; }

        public EmployeeResource()
        {
        }

        public EmployeeResource(string requestPath, Employee employee)
        {
            this.name = employee.ID.ToString();
            this.type = ResourceTypeName;
            this.id = (string.IsNullOrWhiteSpace(requestPath)) ? this.name : requestPath + "/" + this.name;

            this.properties = new EmployeeResourceProperties(employee);
        }
    }
}
