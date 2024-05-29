#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Description:
//      Implements some helper functions.
//

using System;
using System.Collections.ObjectModel; // Collection<T>

using System.Diagnostics;

using System.Reflection;
using Alternet.UI.Markup;
using XamlX;

namespace Alternet.UI.Port
{
    // Miscellaneous (and internal) helper functions.
    internal static class Helper
    {
        // build a format string suitable for String.Format from the given argument,
        // by expanding the convenience form, if necessary
        internal static string GetEffectiveStringFormat(string stringFormat)
        {
            if (stringFormat.IndexOf('{') < 0)
            {
                // convenience syntax - build a composite format string with one parameter
                stringFormat = @"{0:" + stringFormat + @"}";
            }

            return stringFormat;
        }

        /// <summary>
        ///     Return true if a TSF composition is in progress on the given
        ///     property of the given element.
        /// </summary>
        internal static bool IsComposing(DependencyObject d, DependencyProperty dp)
        {
            return false; // yezo
            //if (dp != TextBox.TextProperty)
            //    return false;

            //return IsComposing(d as TextBoxBase);
        }

        /// <summary>
        ///     This method finds the mentor by looking up the InheritanceContext
        ///     links starting from the given node until it finds an FE/FCE. This
        ///     mentor will be used to do a FindResource call while evaluating this
        ///     expression.
        /// </summary>
        /// <remarks>
        ///     This method is invoked by the ResourceReferenceExpression
        ///     and BindingExpression
        /// </remarks>
        internal static DependencyObject FindMentor(DependencyObject d)
        {
            // Find the nearest FE/FCE InheritanceContext
            while (d != null)
            {
                FrameworkElement fe;
                //FrameworkContentElement fce;
                Helper.DowncastToFEorFCE(d, out fe, /*out fce,*/ false);

                if (fe != null)
                {
                    return fe;
                }
                //else if (fce != null)
                //{
                //    return fce;
                //}
                else
                {
                    d = d.InheritanceContext;
                }
            }

            return null;
        }

        /// <summary>
        /// Downcast the given DependencyObject into FrameworkElement or
        /// FrameworkContentElement, as appropriate.
        /// </summary>
        internal static void DowncastToFEorFCE(DependencyObject d,
                                    out FrameworkElement fe/*, out FrameworkContentElement fce*/,
                                    bool throwIfNeither)
        {
            if (FrameworkElement.DType.IsInstanceOfType(d))
            {
                fe = (FrameworkElement)d;
                //fce = null;
            }
            //else if (FrameworkContentElement.DType.IsInstanceOfType(d))
            //{
            //    fe = null;
            //    //fce = (FrameworkContentElement)d;
            //}
            else if (throwIfNeither)
            {
                throw new InvalidOperationException(SR.Get(SRID.MustBeFrameworkDerived, d.GetType()));
            }
            else
            {
                fe = null;
                //fce = null;
            }
        }

        /// <summary>
        /// Find the XmlDataProvider (if any) that is associated with the
        /// given DependencyObject.
        /// This method only works when the DO is part of the generated content
        /// of an ItemsControl or TableRowGroup.
        /// </summary>
        internal static XmlDataProvider XmlDataProviderForElement(DependencyObject d)
        {
            return null; /* yezo
            IGeneratorHost host = Helper.GeneratorHostForElement(d);
            System.Windows.Controls.ItemCollection ic = (host != null) ? host.View : null;
            ICollectionView icv = (ic != null) ? ic.CollectionView : null;
            MS.Internal.Data.XmlDataCollection xdc = (icv != null) ? icv.SourceCollection as MS.Internal.Data.XmlDataCollection : null;

            return (xdc != null) ? xdc.ParentXmlDataProvider : null;*/
        }

        /// <summary>
        /// Return true if the given property is not set locally or from a style
        /// </summary>
        internal static bool HasDefaultValue(DependencyObject d, DependencyProperty dp)
        {
            return HasDefaultOrInheritedValueImpl(d, dp, false, true);
        }

        /// <summary>
        /// Return true if the given property is not set locally or from a style or by inheritance
        /// </summary>
        internal static bool HasDefaultOrInheritedValue(DependencyObject d, DependencyProperty dp)
        {
            return HasDefaultOrInheritedValueImpl(d, dp, true, true);
        }

        /// <summary>
        /// Return true if the given property is not set locally or from a style
        /// </summary>
        internal static bool HasUnmodifiedDefaultValue(DependencyObject d, DependencyProperty dp)
        {
            return HasDefaultOrInheritedValueImpl(d, dp, false, false);
        }

        /// <summary>
        /// Return true if the given property is not set locally or from a style or by inheritance
        /// </summary>
        internal static bool HasUnmodifiedDefaultOrInheritedValue(DependencyObject d, DependencyProperty dp)
        {
            return HasDefaultOrInheritedValueImpl(d, dp, true, false);
        }

        /// <summary>
        /// Return true if the given property is not set locally or from a style
        /// </summary>
        private static bool HasDefaultOrInheritedValueImpl(DependencyObject d, DependencyProperty dp,
                                                                bool checkInherited,
                                                                bool ignoreModifiers)
        {
            PropertyMetadata metadata = dp.GetMetadata(d);
            bool hasModifiers;
            BaseValueSourceInternal source = d.GetValueSource(dp, metadata, out hasModifiers);

            if (source == BaseValueSourceInternal.Default ||
                (checkInherited && source == BaseValueSourceInternal.Inherited))
            {
                if (ignoreModifiers)
                {
                    // ignore modifiers on FE/FCE, for back-compat
                    if (d is FrameworkElement/* yezo || d is FrameworkContentElement*/)
                    {
                        hasModifiers = false;
                    }
                }

                // a default or inherited value might be animated or coerced.  We should
                // return false in that case - the hasModifiers flag tests this.
                // (An expression modifier can't apply to a default or inherited value.)
                return !hasModifiers;
            }

            return false;
        }

        /// <summary>
        /// Checks if the given IProvideValueTarget can receive
        /// a DynamicResource or Binding MarkupExtension.
        /// </summary>
        internal static void CheckCanReceiveMarkupExtension(
                MarkupExtension     markupExtension,
                IServiceProvider    serviceProvider,
            out DependencyObject    targetDependencyObject,
            out DependencyProperty  targetDependencyProperty)
        {
            targetDependencyObject = null;
            targetDependencyProperty = null;

            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget == null)
            {
                return;
            }

            object targetObject = provideValueTarget.TargetObject;

            if (targetObject == null)
            {
                return;
            }

            Type targetType = targetObject.GetType();
            object targetProperty = provideValueTarget.TargetProperty;

            if (targetProperty != null)
            {
                targetDependencyProperty = targetProperty as DependencyProperty;
                if (targetDependencyProperty != null)
                {
                    // This is the DependencyProperty case

                    targetDependencyObject = targetObject as DependencyObject;
                    Debug.Assert(targetDependencyObject != null, "DependencyProperties can only be set on DependencyObjects");
                }
                else
                {
                    MemberInfo targetMember = targetProperty as MemberInfo;
                    if (targetMember != null)
                    {
                        // This is the Clr Property case
                        PropertyInfo propertyInfo = targetMember as PropertyInfo;

                        /* yezo: Xaml

                        // Setters, Triggers, DataTriggers & Conditions are the special cases of
                        // Clr properties where DynamicResource & Bindings are allowed. Normally
                        // these cases are handled by the parser calling the appropriate
                        // ReceiveMarkupExtension method.  But a custom MarkupExtension
                        // that delegates ProvideValue will end up here.
                        // So we handle it similarly to how the parser does it.

                        EventHandler<XamlSetMarkupExtensionEventArgs> setMarkupExtension
                            = LookupSetMarkupExtensionHandler(targetType);

                        if (setMarkupExtension != null && propertyInfo != null)
                        {
                            System.Xaml.IXamlSchemaContextProvider scp = serviceProvider.GetService(typeof(System.Xaml.IXamlSchemaContextProvider)) as System.Xaml.IXamlSchemaContextProvider;
                            if (scp != null)
                            {
                                System.Xaml.XamlSchemaContext sc = scp.SchemaContext;
                                System.Xaml.XamlType xt = sc.GetXamlType(targetType);
                                if (xt != null)
                                {
                                    System.Xaml.XamlMember member = xt.GetMember(propertyInfo.Name);
                                    if (member != null)
                                    {
                                        var eventArgs = new System.Windows.Markup.XamlSetMarkupExtensionEventArgs(member, markupExtension, serviceProvider);

                                        // ask the target object whether it accepts MarkupExtension
                                        setMarkupExtension(targetObject, eventArgs);
                                        if (eventArgs.Handled)
                                            return;     // if so, all is well
                                    }
                                }
                            }
                        }
                        */

                        // Find the MemberType

                        Debug.Assert(targetMember is PropertyInfo || targetMember is MethodInfo,
                            "TargetMember is either a Clr property or an attached static settor method");

                        Type memberType;

                        if (propertyInfo != null)
                        {
                            memberType = propertyInfo.PropertyType;
                        }
                        else
                        {
                            MethodInfo methodInfo = (MethodInfo)targetMember;
                            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                            Debug.Assert(parameterInfos.Length == 2, "The signature of a static settor must contain two parameters");
                            memberType = parameterInfos[1].ParameterType;
                        }

                        // Check if the MarkupExtensionType is assignable to the given MemberType
                        // This check is to allow properties such as the following
                        // - DataTrigger.Binding
                        // - Condition.Binding
                        // - HierarchicalDataTemplate.ItemsSource
                        // - GridViewColumn.DisplayMemberBinding

                        if (!typeof(MarkupExtension).IsAssignableFrom(memberType) ||
                             !memberType.IsAssignableFrom(markupExtension.GetType()))
                        {
                            throw new XamlParseException(SR.Get(SRID.MarkupExtensionDynamicOrBindingOnClrProp,
                                                                markupExtension.GetType().Name,
                                                                targetMember.Name,
                                                                targetType.Name));
                        }
                    }
                    else
                    {
                        // This is the Collection ContentProperty case
                        // Example:
                        // <DockPanel>
                        //   <Button />
                        //   <DynamicResource ResourceKey="foo" />
                        // </DockPanel>

                        // Collection<BindingBase> used in MultiBinding is a special
                        // case of a Collection that can contain a Binding.

                        if (!typeof(BindingBase).IsAssignableFrom(markupExtension.GetType()) ||
                            !typeof(Collection<BindingBase>).IsAssignableFrom(targetProperty.GetType()))
                        {
                            throw new XamlParseException(SR.Get(SRID.MarkupExtensionDynamicOrBindingInCollection,
                                                                markupExtension.GetType().Name,
                                                                targetProperty.GetType().Name));
                        }
                    }
                }
            }
            else
            {
                // This is the explicit Collection Property case
                // Example:
                // <DockPanel>
                // <DockPanel.Children>
                //   <Button />
                //   <DynamicResource ResourceKey="foo" />
                // </DockPanel.Children>
                // </DockPanel>

                // Collection<BindingBase> used in MultiBinding is a special
                // case of a Collection that can contain a Binding.

                if (!typeof(BindingBase).IsAssignableFrom(markupExtension.GetType()) ||
                    !typeof(Collection<BindingBase>).IsAssignableFrom(targetType))
                {
                    throw new XamlParseException(SR.Get(SRID.MarkupExtensionDynamicOrBindingInCollection,
                                                        markupExtension.GetType().Name,
                                                        targetType.Name));
                }
            }
        }
    }
}


