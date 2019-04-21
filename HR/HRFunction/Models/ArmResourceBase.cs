using System;
using System.Collections.Generic;
using System.Text;

namespace HRFunction.Models
{
    internal class ArmResourceBase<T>
    {
        public T properties { get; set; }

        public ArmResourceBase()
        {
        }

        protected ArmResourceBase(T resource)
        {
            this.properties = resource;
        }

        public static ArmResourceBase<T> Create(T resource)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            return new ArmResourceBase<T>(resource);
        }
    }
}
