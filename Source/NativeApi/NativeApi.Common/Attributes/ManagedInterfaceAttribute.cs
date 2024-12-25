using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCommon
{
    [AttributeUsage(AttributeTargets.All)]
    public class ManagedInterfaceAttribute : Attribute
    {
        public ManagedInterfaceAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

}
