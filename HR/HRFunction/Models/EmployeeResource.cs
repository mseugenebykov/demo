using System;
using System.Collections.Generic;
using System.Text;

namespace HRFunction.Models
{
    internal class EmployeeResource: IArmResourceProperties
    {
        private const string ResourceTypeName = "employees";

        public string id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string department { get; set; }

        public string GetArmResourceTypeName() {  return ResourceTypeName; }

        public string GetArmResourceName() { return this.id.ToString(); }

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
    }


}
