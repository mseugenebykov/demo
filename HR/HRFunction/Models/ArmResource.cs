using System;
using System.Collections.Generic;
using System.Text;

namespace HRFunction.Models
{
    internal interface IArmResourceProperties
    {
        string GetArmResourceTypeName();
        string GetArmResourceName();
    }

    internal class ArmResourceCollection<T> where T : IArmResourceProperties
    {
        public delegate T CreateResourceDelegate<R>(R value);

        public ArmResource<T>[] value { get; set; }

        public static ArmResourceCollection<T> Create<R>(ArmRequest request, R[] resources, CreateResourceDelegate<R> create)
        {
            var result = new ArmResourceCollection<T>();
            if (resources != null)
            {
                result.value = new ArmResource<T>[resources.Length];
                for (int i = 0; i < resources.Length; i++)
                {
                    result.value[i] = ArmResource<T>.Create(request, create(resources[i]));
                }
            }
            else
            {
                result.value = null;
            }

            return result;
        }
    }

    internal class ArmResource<T>: ArmResourceBase<T> where T: IArmResourceProperties
    {
        private const string ResourceTypeName = "Microsoft.CustomProviders/resourceproviders/";

        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }

        public ArmResource()
        {
        }

        protected ArmResource(ArmRequest request, T resource)
            :base(resource)
        {
            this.name = resource.GetArmResourceName();
            if (string.IsNullOrWhiteSpace(this.name)) throw new ArgumentNullException(resource.GetArmResourceName());

            var val = resource.GetArmResourceTypeName();
            if (string.IsNullOrWhiteSpace(val)) throw new ArgumentNullException(resource.GetArmResourceTypeName());
            this.type = ResourceTypeName + val;

            this.id = request.GetResourceId(this.name);
        }

        public static ArmResource<T> Create(ArmRequest request, T resource)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            return new ArmResource<T>(request, resource);
        }
    }
}
