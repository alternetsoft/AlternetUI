using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        /// <summary>
        /// Language can be specified in xaml at any point using the xml language attribute xml:lang.
        /// This will make the culture pertain to the scope of the element where it is applied.  The
        /// XmlLanguage names follow the RFC 3066 standard. For example, U.S. English is "en-US".
        /// </summary>
        static public readonly DependencyProperty LanguageProperty =
                    DependencyProperty.RegisterAttached(
                                "Language",
                                typeof(object),//typeof(XmlLanguage),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                        /*XmlLanguage.GetLanguage("en-US")*/"en-US",
                                        FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public event RoutedEventHandler LostFocus;


        /// <summary>
        ///     DataContextChanged event
        /// </summary>
        /// <remarks>
        ///     When an element's DataContext changes, all data-bound properties
        ///     (on this element or any other element) whose Bindings use this
        ///     DataContext will change to reflect the new value.  There is no
        ///     guarantee made about the order of these changes relative to the
        ///     raising of the DataContextChanged event.  The changes can happen
        ///     before the event, after the event, or in any mixture.
        /// </remarks>
        public event DependencyPropertyChangedEventHandler DataContextChanged
        {
            add { EventHandlersStoreAdd(DataContextChangedKey, value); }
            remove { EventHandlersStoreRemove(DataContextChangedKey, value); }
        }

        internal void EventHandlersStoreAdd(EventPrivateKey key, Delegate handler)
        {
            EnsureEventHandlersStore();
            EventHandlersStore.Add(key, handler);
        }

        internal void EventHandlersStoreRemove(EventPrivateKey key, Delegate handler)
        {
            EventHandlersStore store = EventHandlersStore;
            if (store != null)
            {
                store.Remove(key, handler);
            }
        }

        /// <summary>
        ///     DataContext DependencyProperty
        /// </summary>
        public static readonly DependencyProperty DataContextProperty =
                    DependencyProperty.Register(
                                "DataContext",
                                typeof(object),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(null,
                                        FrameworkPropertyMetadataOptions.Inherits,
                                        new PropertyChangedCallback(OnDataContextChanged)));

        /// <summary>
        ///     DataContext Property
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public object DataContext
        {
            get { return GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        // Helper method to retrieve and fire Clr Event handlers for DependencyPropertyChanged event
        private void RaiseDependencyPropertyChanged(EventPrivateKey key, DependencyPropertyChangedEventArgs args)
        {
            EventHandlersStore store = EventHandlersStore;
            if (store != null)
            {
                Delegate handler = store.Get(key);
                if (handler != null)
                {
                    ((DependencyPropertyChangedEventHandler)handler)(this, args);
                }
            }
        }

        /// <summary>
        ///     DataContextChanged private key
        /// </summary>
        internal static readonly EventPrivateKey DataContextChangedKey = new EventPrivateKey();

        private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == BindingExpressionBase.DisconnectedItem)
                return;

            ((FrameworkElement)d).RaiseDependencyPropertyChanged(DataContextChangedKey, e);
        }

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