using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a user interface control that displays a single <see cref="ListControlItem"/> item.
    /// </summary>
    public partial class GenericListItemControl : HiddenGenericBorder, IListControlItemContainer
    {
        private readonly ListItemDrawable itemDrawable;

        private bool isTransparent = true;
        private ListControlItemDefaults itemDefaults;
        private ListControlItem? item;
        private IFormatProvider? formatProvider;
        private SvgImage? checkImageUnchecked;
        private SvgImage? checkImageChecked;
        private SvgImage? checkImageIndeterminate;
        private ImageList? imageList;

        /// <summary>
        /// Initializes a new instance of the GenericListItemControl class and associates it with the specified parent
        /// control, if provided.
        /// </summary>
        /// <remarks>Establishing a parent-child relationship between controls can be useful for managing
        /// layout, event routing, and resource ownership within a user interface.</remarks>
        /// <param name="parent">The parent control to associate with this item. This parameter can be null
        /// if the item does not have a parent control.</param>
        public GenericListItemControl(AbstractControl? parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the GenericListItemControl class.
        /// </summary>
        public GenericListItemControl()
        {
            itemDrawable = CreateItemDrawable();
            itemDrawable.Container = this;

            itemDefaults = CreateItemDefaults();
        }

        /// <summary>
        /// Gets or sets the item associated with the control.
        /// If the item has not been created, it is instantiated when accessed.
        /// </summary>
        /// <remarks>When a new item is assigned, the control's layout is updated to reflect the change.</remarks>
        [Browsable(false)]
        public virtual ListControlItem Item
        {
            get
            {
                item ??= CreateItem();
                OnItemCreatedOrAssigned(item);
                return item;
            }

            set
            {
                if (value == null || value == item)
                    return;
                item = value;
                OnItemCreatedOrAssigned(item);
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is transparent. Default is true.
        /// When true, the control's background is not drawn, allowing the parent control's
        /// background to show through. When false, the control's background
        /// is drawn normally. In both states, the control's border is drawn if applicable.
        /// </summary>
        /// <remarks>Setting this property to a new value will trigger a redraw of the object.</remarks>
        public virtual bool IsTransparent
        {
            get
            {
                return isTransparent;
            }

            set
            {
                if (isTransparent == value)
                    return;
                isTransparent = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the settings used when item is painted.
        /// </summary>
        /// <remarks>Assigning a new value updates the layout to reflect the changed item defaults.</remarks>
        [Browsable(false)]
        public virtual ListControlItemDefaults ItemDefaults
        {
            get
            {
                return itemDefaults;
            }

            set
            {
                if (value == null || value == itemDefaults)
                    return;
                itemDefaults = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <inheritdoc/>
        ObjectUniqueId IListControlItemContainer.UniqueId => UniqueId;

        /// <inheritdoc/>
        AbstractControl? IListControlItemContainer.Control => this;

        /// <inheritdoc/>
        IListControlItemDefaults IListControlItemContainer.Defaults => itemDefaults;

        /// <inheritdoc/>
        float? IListControlItemContainer.ColumnSeparatorWidth => null;

        /// <inheritdoc/>
        bool IListControlItemContainer.Focused => Focused;

        /// <inheritdoc/>
        bool IListControlItemContainer.HasColumns => false;

        /// <inheritdoc/>
        IReadOnlyList<ListControlColumn> IListControlItemContainer.Columns => Array.Empty<ListControlColumn>();

        /// <summary>
        /// Gets or sets the format provider used to control formatting operations for this control.
        /// When this property is set, the control will use the specified format provider for any culture-specific formatting operations,
        /// such as displaying dates, numbers, or other formatted values. If the format provider is null, the control will use
        /// the default formatting behavior based on the current culture or invariant culture.
        /// Setting this property allows for customization of how values are formatted and displayed by the control,
        /// providing flexibility in localization and presentation of data.
        /// </summary>
        [Browsable(false)]
        public virtual IFormatProvider? FormatProvider
        {
            get
            {
                return formatProvider;
            }

            set
            {
                formatProvider = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the image displayed when the checkbox is in the unchecked state.
        /// This is used when checkboxes are shown in the item.
        /// </summary>
        /// <remarks>Setting this property to null displays the default image for the unchecked state. Use
        /// this property to customize the appearance of the checkbox.</remarks>
        [Browsable(false)]
        public virtual SvgImage? CheckImageUnchecked
        {
            get => checkImageUnchecked;
            set
            {
                if (checkImageUnchecked == value)
                    return;
                checkImageUnchecked = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the image displayed when the checkbox is in the checked state.
        /// This is used when checkboxes are shown in the item.
        /// </summary>
        /// <remarks>Setting this property to null displays the default image for the checked state. Use
        /// this property to customize the appearance of the checkbox.</remarks>
        [Browsable(false)]
        public virtual SvgImage? CheckImageChecked
        {
            get => checkImageChecked;
            set
            {
                if (checkImageChecked == value)
                    return;
                checkImageChecked = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the image displayed when the checkbox is in the indeterminate state.
        /// This is used when checkboxes are shown in the item.
        /// </summary>
        /// <remarks>Setting this property to null displays the default image for the indeterminate state. Use
        /// this property to customize the appearance of the checkbox.</remarks>
        [Browsable(false)]
        public virtual SvgImage? CheckImageIndeterminate
        {
            get => checkImageIndeterminate;
            set
            {
                if (checkImageIndeterminate == value)
                    return;
                checkImageIndeterminate = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the collection of images associated with the control.
        /// </summary>
        /// <remarks>Setting this property updates the control's layout and refreshes its display. This
        /// property is not visible in the property grid.</remarks>
        [Browsable(false)]
        public virtual ImageList? ImageList
        {
            get => imageList;
            set
            {
                if (imageList == value)
                    return;
                imageList = value;
                if (Item.HasValidImageIndex)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Creates a new instance of a ListControlItem. Derived classes can override
        /// this method to provide custom item creation logic.
        /// </summary>
        /// <returns>A new instance of ListControlItem.</returns>
        protected virtual ListControlItem CreateItem()
        {
            return new();
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            var specifiedWidth = SuggestedWidth;
            var specifiedHeight = SuggestedHeight;
            if (!Coord.IsNaN(specifiedWidth) && !Coord.IsNaN(specifiedHeight))
                return new SizeD(specifiedWidth, specifiedHeight);

            var result = itemDrawable.GetPreferredSize(this);
            if (result != SizeD.Empty)
                return result + Padding.Size;

            return base.GetPreferredSize(context);
        }

        /// <inheritdoc/>
        int IListControlItemContainer.GetItemCount()
        {
            return 1;
        }

        /// <inheritdoc/>
        string IListControlItemContainer.GetItemText(int index, bool forDisplay)
        {
            if (index == 0)
                return GetItemText(Item, forDisplay);
            return string.Empty;
        }

        /// <inheritdoc/>
        public virtual string GetItemText(ListControlItem? item, bool forDisplay)
        {
            return ListControlItem.DefaultGetItemText(item, forDisplay, FormatProvider);
        }

        /// <inheritdoc/>
        ListControlItem? IListControlItemContainer.SafeItem(int index)
        {
            return index == 0 ? Item : null;
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            var flags = IsTransparent ? DrawDefaultBackgroundFlags.DrawBorder
                : DrawDefaultBackgroundFlags.DrawBorderAndBackground;

            DrawBorderAndBackground(e, flags);

            var rect = e.ClientRectangle;
            var dc = e.Graphics;

            itemDrawable.Bounds = (rect.Location + Padding.LeftTop, rect.Size - Padding.Size);
            itemDrawable.Draw(this, dc);
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            PerformLayoutAndInvalidate();
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
        }

        /// <summary>
        /// Creates and returns a drawable object which is responsible for rendering the visual representation of the item within the control.
        /// Derived classes can override this method to provide a custom drawable implementation
        /// that defines how the item should be drawn on the screen.
        /// </summary>
        /// <returns>A <see cref="ListItemDrawable"/> instance which is responsible for rendering the visual representation of the item.</returns>
        protected virtual ListItemDrawable CreateItemDrawable()
        { 
            return new ();
        }

        /// <summary>
        /// Creates and initializes a new instance of the ListControlItemDefaults class.
        /// </summary>
        /// <returns>A new instance of ListControlItemDefaults, which contains settings for items.</returns>
        protected virtual ListControlItemDefaults CreateItemDefaults()
        {
            return new ();
        }

        /// <summary>
        /// Invoked when a list item is created or assigned, allowing derived classes to perform custom processing or
        /// initialization for the item.
        /// </summary>
        /// <remarks>Override this method in a derived class to implement additional behavior when a new
        /// item is added to the control or an existing item is assigned. This method is called each time an item is
        /// created or assigned to the control.</remarks>
        /// <param name="item">The list item that has been created or assigned and will be associated with the control.</param>
        protected virtual void OnItemCreatedOrAssigned(ListControlItem item)
        {
            itemDrawable.Item = item;
        }
    }
}