#nullable disable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XamlX;
using XamlX.Ast;
using XamlX.Transform;
using XamlX.Transform.Transformers;
using XamlX.TypeSystem;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortXamlIlDataContextTypeTransformer : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (context.ParentNodes().FirstOrDefault() is UixmlPortXamlIlDataContextTypeMetadataNode)
            {
                // We've already resolved the data context type for this node.
                return node;
            }

            if (node is XamlAstConstructableObjectNode on)
            {
                UixmlPortXamlIlDataContextTypeMetadataNode inferredDataContextTypeNode = null;
                UixmlPortXamlIlDataContextTypeMetadataNode directiveDataContextTypeNode = null;

                for (int i = 0; i < on.Children.Count; ++i)
                {
                    var child = on.Children[i];
                    if (child is XamlAstXmlDirective directive)
                    {
                        if (directive.Namespace == XamlNamespaces.Xaml2006
                            && directive.Name == "DataType"
                            && directive.Values.Count == 1)
                        {
                            on.Children.RemoveAt(i);
                            i--;
                            if (directive.Values[0] is XamlAstTextNode text)
                            {
                                directiveDataContextTypeNode = new UixmlPortXamlIlDataContextTypeMetadataNode(on,
                                    TypeReferenceResolver.ResolveType(context, text.Text, isMarkupExtension: false, text, strict: true).Type);
                            }
                            else
                            {
                                throw new XamlX.XamlParseException("x:DataType should be set to a type name.", directive.Values[0]);
                            }
                        }
                    }
                    else if (child is XamlPropertyAssignmentNode pa)
                    {
                        if (pa.Property.Name == "DataContext"
                            && pa.Property.DeclaringType.Equals(context.GetUixmlPortTypes().StyledElement)
                            && pa.Values[0] is XamlMarkupExtensionNode ext
                            && ext.Value is XamlAstConstructableObjectNode obj)
                        {
                            inferredDataContextTypeNode = ParseDataContext(context, on, obj);
                        }
                        else if(context.GetUixmlPortTypes().DataTemplate.IsAssignableFrom(on.Type.GetClrType())
                            && pa.Property.Name == "DataType"
                            && pa.Values[0] is XamlTypeExtensionNode dataTypeNode)
                        {
                            inferredDataContextTypeNode = new UixmlPortXamlIlDataContextTypeMetadataNode(on, dataTypeNode.Value.GetClrType());
                        }
                    }
                }

                // If there is no x:DataType directive,
                // do more specialized inference
                if (directiveDataContextTypeNode is null)
                {
                    if (context.GetUixmlPortTypes().IDataTemplate.IsAssignableFrom(on.Type.GetClrType())
                        && inferredDataContextTypeNode is null)
                    {
                        // Infer data type from collection binding on a control that displays items.
                        var parentObject = context.ParentNodes().OfType<XamlAstConstructableObjectNode>().FirstOrDefault();
                        if (parentObject != null)
                        {
                            var parentType = parentObject.Type.GetClrType();

                            if (context.GetUixmlPortTypes().IItemsPresenterHost.IsDirectlyAssignableFrom(parentType)
                                || context.GetUixmlPortTypes().ItemsRepeater.IsDirectlyAssignableFrom(parentType))
                            {
                                inferredDataContextTypeNode = InferDataContextOfPresentedItem(context, on, parentObject);
                            }
                        }

                        if (inferredDataContextTypeNode is null)
                        {
                            inferredDataContextTypeNode = new UixmlPortXamlIlUninferrableDataContextMetadataNode(on);
                        }
                    }
                }

                return directiveDataContextTypeNode ?? inferredDataContextTypeNode ?? node;
            }

            return node;
        }

        private static UixmlPortXamlIlDataContextTypeMetadataNode InferDataContextOfPresentedItem(AstTransformationContext context, XamlAstConstructableObjectNode on, XamlAstConstructableObjectNode parentObject)
        {
            var parentItemsValue = parentObject
                                            .Children.OfType<XamlPropertyAssignmentNode>()
                                            .FirstOrDefault(pa => pa.Property.Name == "Items")
                                            ?.Values[0];
            if (parentItemsValue is null)
            {
                // We can't infer the collection type and the currently calculated type is definitely wrong.
                // Notify the user that we were unable to infer the data context type if they use a compiled binding.
                return new UixmlPortXamlIlUninferrableDataContextMetadataNode(on);
            }

            IXamlType itemsCollectionType = null;
            if (context.GetUixmlPortTypes().Binding.IsAssignableFrom(parentItemsValue.Type.GetClrType()))
            {
                if (parentItemsValue.Type.GetClrType().Equals(context.GetUixmlPortTypes().CompiledBindingExtension)
                    && parentItemsValue is XamlMarkupExtensionNode ext && ext.Value is XamlAstConstructableObjectNode parentItemsBinding)
                {
                    var parentItemsDataContext = context.ParentNodes().SkipWhile(n => n != parentObject).OfType<UixmlPortXamlIlDataContextTypeMetadataNode>().FirstOrDefault();
                    if (parentItemsDataContext != null)
                    {
                        itemsCollectionType = XamlIlBindingPathHelper.UpdateCompiledBindingExtension(context, parentItemsBinding, () => parentItemsDataContext.DataContextType, parentObject.Type.GetClrType());
                    }
                }
            }
            else
            {
                itemsCollectionType = parentItemsValue.Type.GetClrType();
            }

            if (itemsCollectionType != null)
            {
                foreach (var i in GetAllInterfacesIncludingSelf(itemsCollectionType))
                {
                    if (i.GenericTypeDefinition?.Equals(context.Configuration.WellKnownTypes.IEnumerableT) == true)
                    {
                        return new UixmlPortXamlIlDataContextTypeMetadataNode(on, i.GenericArguments[0]);
                    }
                }
            }
            // We can't infer the collection type and the currently calculated type is definitely wrong.
            // Notify the user that we were unable to infer the data context type if they use a compiled binding.
            return new UixmlPortXamlIlUninferrableDataContextMetadataNode(on);
        }

        private static UixmlPortXamlIlDataContextTypeMetadataNode ParseDataContext(AstTransformationContext context, XamlAstConstructableObjectNode on, XamlAstConstructableObjectNode obj)
        {
            var bindingType = context.GetUixmlPortTypes().Binding;
            if (!bindingType.IsAssignableFrom(obj.Type.GetClrType()) && !obj.Type.GetClrType().Equals(context.GetUixmlPortTypes().ReflectionBindingExtension))
            {
                return new UixmlPortXamlIlDataContextTypeMetadataNode(on, obj.Type.GetClrType());
            }
            else if (obj.Type.GetClrType().Equals(context.GetUixmlPortTypes().CompiledBindingExtension))
            {
                Func<IXamlType> startTypeResolver = () =>
                {
                    var dataTypeProperty = obj.Children.OfType<XamlPropertyAssignmentNode>().FirstOrDefault(c => c.Property.Name == "DataType");
                    if (dataTypeProperty?.Values.Count is 1 && dataTypeProperty.Values[0] is XamlAstTextNode text)
                    {
                        return TypeReferenceResolver.ResolveType(context, text.Text, isMarkupExtension: false, text, strict: true).Type;
                    }

                    var parentDataContextNode = context.ParentNodes().OfType<UixmlPortXamlIlDataContextTypeMetadataNode>().FirstOrDefault();
                    if (parentDataContextNode is null)
                    {
                        throw new XamlX.XamlParseException("Cannot parse a compiled binding without an explicit x:DataType directive to give a starting data type for bindings.", obj);
                    }

                    return parentDataContextNode.DataContextType;
                };

                var bindingResultType = XamlIlBindingPathHelper.UpdateCompiledBindingExtension(context, obj, startTypeResolver, on.Type.GetClrType());
                return new UixmlPortXamlIlDataContextTypeMetadataNode(on, bindingResultType);
            }

            return new UixmlPortXamlIlUninferrableDataContextMetadataNode(on);
        }

        private static IEnumerable<IXamlType> GetAllInterfacesIncludingSelf(IXamlType type)
        {
            if (type.IsInterface)
                yield return type;

            foreach (var i in type.GetAllInterfaces())
                yield return i;
        }
    }

    [DebuggerDisplay("DataType = {DataContextType}")]
    class UixmlPortXamlIlDataContextTypeMetadataNode : XamlValueWithSideEffectNodeBase
    {
        public virtual IXamlType DataContextType { get; }

        public UixmlPortXamlIlDataContextTypeMetadataNode(IXamlAstValueNode value, IXamlType targetType)
            : base(value, value)
        {
            DataContextType = targetType;
        }
    }

    [DebuggerDisplay("DataType = Unknown")]
    class UixmlPortXamlIlUninferrableDataContextMetadataNode : UixmlPortXamlIlDataContextTypeMetadataNode
    {
        public UixmlPortXamlIlUninferrableDataContextMetadataNode(IXamlAstValueNode value)
            : base(value, null)
        {
        }

        public override IXamlType DataContextType => throw new XamlTransformException("Unable to infer DataContext type for compiled bindings nested within this element.", Value);
    }
}
