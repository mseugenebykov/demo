using System;
using System.Collections.Generic;
using System.Text;

namespace HRFunction.Models
{
    internal class ArmEmployeeResource: EmployeeResource, IArmResourceProperties
    {
        private const string ResourceTypeName = "employees";

        public string GetArmResourceTypeName() {  return ResourceTypeName; }

        public string GetArmResourceName() { return this.id.ToString(); }

        public ArmEmployeeResource()
            :base()
        {
        }

        public ArmEmployeeResource(Employee employee)
            :base(employee)
        {
        }
    }


}
