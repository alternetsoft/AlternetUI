using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Alternet.UI
{
    public class FrameworkElement : UIElement
    {
        /// <summary>
        /// Gets or sets the identifying name of the control.
        /// The name provides a reference so that code-behind, such as event handler code,
        /// can refer to a markup control after it is constructed during processing by a UIXML processor.
        /// </summary>
        /// <value>The name of the control. The default is <c>null</c>.</value>
        public string? Name { get; set; } // todo: maybe use Site.Name?
        public static DependencyProperty? BindingGroupProperty { get; internal set; }
        public bool IsInitialized { get; internal set; }
        public FrameworkElement Parent { get; internal set; }
        public bool IsParentAnFE { get; internal set; }
        public object DataContext { get; internal set; }
        public static DependencyProperty LanguageProperty { get; internal set; }

        public event RoutedEventHandler LostFocus;

        public event DependencyPropertyChangedEventHandler DataContextChanged;

        /// <summary>
        ///     DataContext DependencyProperty
        /// </summary>
        public static readonly DependencyProperty DataContextProperty; // yezo todo: replace with real property

        internal static void AddHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            throw new NotImplementedException();
        }

        internal static void RemoveHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            throw new NotImplementedException();
        }

        internal void WriteInternalFlag(object isInitialized, bool v)
        {
            throw new NotImplementedException();
        }

        internal static DependencyObject GetFrameworkParent(object current)
        {
            FrameworkObject fo = new FrameworkObject(current as DependencyObject);

            fo = fo.FrameworkParent;

            return fo.DO;
        }
    }
}