using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace SiGen.Configuration
{
    public class ShouldSerializeContractResolver : DefaultContractResolver
    {
        public bool ForceSerialize { get; set; }

        public ShouldSerializeContractResolver()
        {
        }

        public ShouldSerializeContractResolver(bool forceSerialize)
        {
            ForceSerialize = forceSerialize;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var jsonPropInfo = member.GetCustomAttribute<JsonPropertyAttribute>();

            if (ForceSerialize)
            {
                property.ShouldSerialize = instance => true;
            }
            else if (property != null && jsonPropInfo != null && 
                jsonPropInfo.DefaultValueHandling == DefaultValueHandling.Include)
            {
                property.ShouldSerialize = instance => true;
            }

            return property;
        }
    }
}
