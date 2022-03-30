using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that manages a related set of tab pages.
    /// </summary>
    public class TabControl : Control
    {
        /// <summary>
        /// Gets the collection of tab pages in this tab control.
        /// </summary>
        /// <value>A <see cref="Collection{TabPage}"/> that contains the <see cref="TabPage"/> objects in this <see cref="TabControl"/>.</value>
        /// <remarks>The order of tab pages in this collection reflects the order the tabs appear in the control.</remarks>
        [Content]
        public Collection<TabPage> Pages { get; } = new Collection<TabPage>();

        /// <summary>
        ///     SelectedItem DependencyProperty
        /// </summary>
        public static readonly DependencyProperty SelectedPageProperty =
                DependencyProperty.Register(
                        "SelectedPage",
                        typeof(TabPage),
                        typeof(TabControl),
                        new FrameworkPropertyMetadata(
                                null,
                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                new PropertyChangedCallback(OnSelectedPageChanged),
                                new CoerceValueCallback(CoerceSelectedPage)));

        /// <summary>
        ///  Gets or sets the currently selected tab page.
        /// </summary>
        [Bindable(true), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TabPage? SelectedPage
        {
            get { return (TabPage?)GetValue(SelectedPageProperty); }
            set { SetValue(SelectedPageProperty, value); }
        }

        private static void OnSelectedPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TabControl)d).OnSelectedPageChanged(new RoutedEventArgs(SelectedPageChangedEvent, d));
        }

        /// <summary>
        /// A virtual function that is called when the selection is changed. Default behavior
        /// is to raise a SelectedPageChangedEvent
        /// </summary>
        /// <param name="e">The inputs for this event. Can be raised (default behavior) or processed in some other way.</param>
        protected virtual void OnSelectedPageChanged(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        /// <summary>
        ///     An event fired when the selection changes.
        /// </summary>
        public static readonly RoutedEvent SelectedPageChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(SelectedPageChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabControl));

        /// <summary>
        /// Occurs when the <see cref="SelectedPage"/> property has changed.
        /// </summary>
        [Category("Behavior")]
        public event RoutedEventHandler SelectedPageChanged
        {
            add { AddHandler(SelectedPageChangedEvent, value); }
            remove { RemoveHandler(SelectedPageChangedEvent, value); }
        }

        private static object CoerceSelectedPage(DependencyObject d, object value)
        {
            //Selector s = (Selector)d;
            //if (value == null || s.SkipCoerceSelectedItemCheck)
            //    return value;

            //int selectedIndex = s.SelectedIndex;

            //if ((selectedIndex > -1 && selectedIndex < s.Items.Count && s.Items[selectedIndex] == value)
            //    || s.Items.Contains(value))
            //{
            //    return value;
            //}

            //return DependencyProperty.UnsetValue;
            return value;
        }

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection => Pages;
    }
}