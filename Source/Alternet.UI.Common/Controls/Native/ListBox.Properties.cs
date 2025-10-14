using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class ListBox
    {
        private bool horizontalScrollbar;
        private bool integralHeight = true;
        private bool scrollAlwaysVisible;

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex" /> property or the <see cref="SelectedIndices" />
        /// collection has changed.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? SelectedIndexChanged;

        /// <summary>
        /// Indicates whether or not the list box should display a horizontal scrollbar
        /// when the items extend beyond the right edge of the list box.
        /// If true, the scrollbar will automatically set its extent depending on the length
        /// of items in the list box. This property may not be supported on some platforms.
        /// Default is false.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Localizable(true)]
        public new virtual bool HorizontalScrollbar
        {
            get
            {
                return horizontalScrollbar;
            }

            set
            {
                if (value != horizontalScrollbar)
                {
                    horizontalScrollbar = value;

                    SetHandlerFlag(ListBoxHandlerFlags.ShowHorzScrollWhenNeeded, value);
                }
            }
        }

        /// <summary>
        /// Indicates if the list box should avoid showing partial items. If so,
        /// then only full items will be displayed, and the list box will be resized
        /// to prevent partial items from being shown.  Otherwise, they will be shown.
        /// Default is true.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Localizable(true)]
        [Description("Indicates whether the list box should avoid showing partial items.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual bool IntegralHeight
        {
            get
            {
                return integralHeight;
            }

            set
            {
                if (integralHeight != value)
                {
                    integralHeight = value;
                    SetHandlerFlag(ListBoxHandlerFlags.IntegralHeight, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the vertical scrollbar is shown at all times.
        /// Default is false, which means the scrollbar is shown only when needed.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool ScrollAlwaysVisible
        {
            get
            {
                return scrollAlwaysVisible;
            }

            set
            {
                if (scrollAlwaysVisible != value)
                {
                    scrollAlwaysVisible = value;
                    SetHandlerFlag(ListBoxHandlerFlags.AlwaysShowVertScroll, value);
                }
            }
        }

        /// <summary>
        /// The index of the currently selected item in the list, if there
        /// is one. If the value is -1, there is currently no selection. If the
        /// value is 0 or greater, than the value is the index of the currently
        /// selected item.
        /// selection.
        /// </summary>
        [Browsable(false)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int SelectedIndex
        {
            get
            {
                return GetSelection();
            }

            set
            {
                var oldSelectedIndex = SelectedIndex;
                if (oldSelectedIndex == value)
                    return;
                SetSelection(value);
            }
        }

        /// <summary>
        /// A collection of the indices of the selected items in the
        /// list box. If there are no selected items in the list box, the result is
        /// an empty collection.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IReadOnlyList<object> SelectedIndices
        {
            get
            {
                var result = new List<object>();
                PlatformControl.UpdateSelections();
                var count = PlatformControl.GetSelectionsCount();
                for (int i = 0; i < count; i++)
                    result.Add(PlatformControl.GetSelectionsItem(i));
                return result;
            }
        }

        /// <summary>
        /// The value of the currently selected item in the list, if there
        /// is one. If the value is null, there is currently no selection. If the
        /// value is non-null, then the value is that of the currently selected
        /// item. If the MultiSelect property on the list box is true, then a
        /// non-null return value for this method is the value of the first item selected.
        /// </summary>
        [Browsable(false)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object? SelectedItem
        {
            get
            {
                var index = SelectedIndex;
                if (index < 0 || index >= Items.Count)
                    return null;
                return Items[index];
            }

            set
            {
                if (value == null)
                {
                    SelectedIndex = -1;
                    return;
                }

                var index = Items.IndexOf(value);
                SelectedIndex = index;
            }
        }

        /// <summary>
        /// Raises the <see cref="SelectedIndexChanged"/> event and <see cref="OnSelectedIndexChanged"/> method.
        /// </summary>
        /// <remarks>This method invokes the <see cref="SelectedIndexChanged"/> event, allowing
        /// subscribers to handle changes to the selected index.
        /// It can be called to programmatically trigger the event.</remarks>
        public void RaiseSelectedIndexChanged()
        {
            OnSelectedIndexChanged(EventArgs.Empty);
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the <see cref="SelectedIndex" /> property or the <see cref="SelectedIndices" />
        /// property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Sets or clears the specified flag on the handler.
        /// </summary>
        /// <remarks>This method modifies the state of the handler by updating the specified flag.
        /// Derived classes can override this method to provide additional
        /// behavior when flags are modified.</remarks>
        /// <param name="flag">The <see cref="ListBoxHandlerFlags"/> value to set or clear.</param>
        /// <param name="value"><see langword="true"/> to set the specified flag; <see langword="false"/> to clear it.</param>
        protected virtual void SetHandlerFlag(ListBoxHandlerFlags flag, bool value)
        {
            if (value)
                PlatformControl.Flags |= flag;
            else
                PlatformControl.Flags &= ~flag;
        }
    }
}
