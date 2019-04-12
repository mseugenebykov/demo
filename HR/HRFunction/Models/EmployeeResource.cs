using System;
using System.Collections.Generic;
using System.Text;

namespace HRFunction.Models
{
    public class EmployeeResource
    {
        public const string ResourceTypeName = "Microsoft.CustomProviders/resourceproviders/Employee";

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
