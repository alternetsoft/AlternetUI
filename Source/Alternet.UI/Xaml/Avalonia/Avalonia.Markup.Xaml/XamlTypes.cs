#nullable disable
using System;

namespace Avalonia.Markup.Xaml
{
    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public interface IProvideValueTarget
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        object TargetObject { get; }
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        object TargetProperty { get; }
    }

    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public interface IRootObjectProvider
    {
        /// <summary>
        /// The root object of the xaml file
        /// </summary>
        object RootObject { get; }
        /// <summary>
        /// The "current" root object, contains either the root of the xaml file
        /// or the root object of the control/data template 
        /// </summary>
        object IntermediateRootObject { get; }
    }

    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public interface IUriContext
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        Uri BaseUri { get; set; }
    }
    
    internal interface IXamlTypeResolver
    {
        Type Resolve (string qualifiedTypeName);
    }

    
    internal class ConstructorArgumentAttribute : Attribute
    {
        public ConstructorArgumentAttribute(string name)
        {
            
        }
    }
}
