using System;

namespace Alternet.UI
{
    /// <summary>
    /// Attribute to associate a ValueSerializer class with a value type or to override
    /// which value serializer to use for a property. A value serializer can be associated
    /// with an attached property by placing the attribute on the static accessor for the
    /// attached property.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Interface
        | AttributeTargets.Struct | AttributeTargets.Enum
        | AttributeTargets.Property | AttributeTargets.Method,
        AllowMultiple = false,
        Inherited = true)]
    public sealed class ValueSerializerAttribute : Attribute
    {
        private readonly string? valueSerializerTypeName;
        private Type? valueSerializerType;

        /// <summary>
        /// Constructor for the ValueSerializerAttribute
        /// </summary>
        /// <param name="valueSerializerType">Type of the value serializer being
        /// associated with a type or property</param>
        public ValueSerializerAttribute(Type valueSerializerType)
        {
            this.valueSerializerType = valueSerializerType;
        }

        /// <summary>
        /// Constructor for the ValueSerializerAttribute
        /// </summary>
        /// <param name="valueSerializerTypeName">Fully qualified type name of
        /// the value serializer being associated with a type or property</param>
        public ValueSerializerAttribute(string valueSerializerTypeName)
        {
            this.valueSerializerTypeName = valueSerializerTypeName;
        }

        /// <summary>
        /// The type of the value serializer to create for this type or property.
        /// </summary>
        public Type? ValueSerializerType
        {
            get
            {
                if (valueSerializerType == null && valueSerializerTypeName != null)
                    valueSerializerType = Type.GetType(valueSerializerTypeName);
                return valueSerializerType;
            }
        }

        /// <summary>
        /// The assembly qualified name of the value serializer type for this type or property.
        /// </summary>
        public string? ValueSerializerTypeName
        {
            get
            {
                if (valueSerializerType != null)
                    return valueSerializerType.AssemblyQualifiedName;
                else
                    return valueSerializerTypeName;
            }
        }
    }
}
