using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Alternet.UI.Integration.IntelliSense.AssemblyMetadata;

namespace Alternet.UI.Integration.IntelliSense
{

    public class Metadata
    {
        public Dictionary<string, Dictionary<string, MetadataType>> Namespaces { get; } = new Dictionary<string, Dictionary<string, MetadataType>>();

        public void AddType(string ns, MetadataType type) => Namespaces.GetOrCreate(ns)[type.Name] = type;
    }

    [DebuggerDisplay("{Name}")]
    public class MetadataType
    {
        public bool IsEnum { get; set; }
        public bool IsMarkupExtension { get; set; }
        public bool IsStatic { get; set; }
        public bool HasHintValues { get; set; }
        public string[] HintValues { get; set; }

        //assembly, type, property
        public Func<string, MetadataType, MetadataProperty, bool> IsValidForXamlContextFunc { get; set; }
        //assembly, type, property
        public Func<string, MetadataType, MetadataProperty, IEnumerable<string>> XamlContextHintValuesFunc { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; } = "";
        public List<MetadataProperty> Properties { get; set; } = new List<MetadataProperty>();
        public List<MetadataEvent> Events { get; set; } = new List<MetadataEvent>();
        public List<MetadataMethod> Methods { get; set; } = new List<MetadataMethod>();
        public bool HasAttachedProperties { get; set; }
        public bool HasAttachedEvents { get; set; }
        public bool HasStaticGetProperties { get; set; }
        public bool HasSetProperties { get; set; }
        public bool IsAvaloniaObjectType { get; set; }
        public MetadataTypeCtorArgument SupportCtorArgument { get; set; }
        public bool IsCompositeValue { get; set; }
        public bool IsGeneric { get; set; }
        public bool IsXamlDirective { get; set; }

        public MetadataType CloneAs(string name, string fullName)
        {
            var result = (MetadataType)MemberwiseClone();
            result.Name = name;
            result.FullName = fullName;
            return result;
        }
    }

    public enum MetadataTypeCtorArgument
    {
        None,
        Type,
        Object,
        TypeAndObject,
        HintValues
    }

    [DebuggerDisplay("{Name} from {DeclaringType}")]
    public class MetadataProperty
    {
        public string Name { get; }
        public MetadataType Type { get; set; }
        public MetadataType DeclaringType { get; }
        public bool IsAttached { get; }
        public bool IsStatic { get; }
        public bool HasGetter { get; }
        public bool HasSetter { get; }

        public MetadataProperty(string name, MetadataType type, MetadataType declaringType, bool isAttached, bool isStatic, bool hasGetter, bool hasSetter)
        {
            Name = name;
            Type = type;
            DeclaringType = declaringType;
            IsAttached = isAttached;
            IsStatic = isStatic;
            HasGetter = hasGetter;
            HasSetter = hasSetter;
        }
    }

    [DebuggerDisplay("{DeclaringType} {Name}")]
    public class MetadataParameter
    {
        public MetadataParameter(string name, MetadataType parameterType)
        {
            Name = name;
            ParameterType = parameterType;
        }

        public string Name { get; }
        public MetadataType ParameterType { get; }
    }

    [DebuggerDisplay("{Name} from {DeclaringType}")]
    public class MetadataMethod
    {
        public MetadataMethod(string name, MetadataType returnType, MetadataParameter[] parameters, MetadataType declaringType, bool isStatic)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
            DeclaringType = declaringType;
            IsStatic = isStatic;
        }

        public string Name { get; }
        public MetadataType ReturnType { get; }
        public MetadataParameter[] Parameters { get; }
        public MetadataType DeclaringType { get; }
        public bool IsStatic { get; }
    }

    public class MetadataEvent
    {
        public MetadataEvent(string name, MetadataType type, MetadataType declaringType, bool isAttached)
        {
            Name = name;
            Type = type;
            DeclaringType = declaringType;
            IsAttached = isAttached;
        }
        public string Name { get; }
        public bool IsAttached { get; }
        public MetadataType Type { get; }
        public MetadataType DeclaringType { get; }
    }

}
