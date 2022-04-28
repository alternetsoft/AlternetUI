#nullable disable
using System;

namespace Alternet.UI.Markup
{
    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public interface IUixmlProvideValueTarget
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
    public interface IUixmlRootObjectProvider
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
    public interface IUixmlUriContext
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        Uri BaseUri { get; set; }
    }
    
    //internal interface IXamlTypeResolver
    //{
    //    Type Resolve (string qualifiedTypeName);
    //}

    
    //internal class ConstructorArgumentAttribute : Attribute
    //{
    //    public ConstructorArgumentAttribute(string name)
    //    {
            
    //    }
    //}
}
