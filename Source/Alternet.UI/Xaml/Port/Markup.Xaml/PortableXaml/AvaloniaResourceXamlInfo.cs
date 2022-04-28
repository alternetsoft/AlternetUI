#nullable disable
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alternet.UI.Markup.Xaml.PortableXaml
{
    [DataContract]
    class UixmlPortResourceXamlInfo
    {
        [DataMember]
        public Dictionary<string, string> ClassToResourcePathIndex { get; set; } = new Dictionary<string, string>();
    }
}
