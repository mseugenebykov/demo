using System;
using System.Collections.Generic;
using System.Text;

namespace HRFunction.Models
{
    public class Employee
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
    }
}
