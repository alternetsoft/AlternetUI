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
    public partial class GenericItemControl : HiddenGenericBorder, IListControlItemContainer, ICommandSource
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
        private bool? reportedChecked;
        private Action? clickAction;
        private CommandSourceStruct commandSource;

        /// <summary>
        /// Initializes a new instance of the GenericListItemControl class and associates it with the specified parent
        /// control, if provided.
        /// </summary>
        /// <remarks>Establishing a parent-child relationship between controls can be useful for managing
        /// layout, event routing, and resource ownership within a user interface.</remarks>
        /// <param name="parent">The parent control to associate with this item. This parameter can be null
        /// if the item does not have a parent control.</param>
        public GenericItemControl(AbstractControl? parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the GenericListItemControl class.
        /// </summary>
        public GenericItemControl()
        {
            commandSource = new(this);
            commandSource.Changed = () =>
            {
                Enabled = commandSource.CanExecute;
            };

            itemDrawable = CreateItemDrawable();
            itemDrawable.Container = this;

            itemDefaults = CreateItemDefaults();

            ParentBackColor = true;
            ParentForeColor = true;
            HorizontalAlignment = HorizontalAlignment.Left;
        }

        /// <inheritdoc/>
        public virtual ICommand? Command
        {
            get
            {
                return commandSource.Command;
            }

            set
            {
                commandSource.Command = value;
            }
        }

        /// <inheritdoc/>
        public virtual object? CommandParameter
        {
            get
            {
                return commandSource.CommandParameter;
            }

            set
            {
                commandSource.CommandParameter = value;
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public virtual object? CommandTarget
        {
            get
            {
                return commandSource.CommandParameter;
            }

            set
            {
                commandSource.CommandParameter = value;
            }
        }

        /// <summary>
        /// Gets or sets whether user can set the checkbox to
        /// the third state by clicking.
        /// </summary>
        /// <remarks>
        /// By default a user can't set a 3-state checkbox to the third state. It can only
        /// be done from code. Using this flags allows the user to set the checkbox to
        /// the third state by clicking.
        /// </remarks>
        [DefaultValue(false)]
        public virtual bool AllowAllStatesForUser
        {
            get
            {
                return ItemDefaults.CheckBoxAllowAllStatesForUser;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (AllowAllStatesForUser == value)
                    return;
                ItemDefaults.CheckBoxAllowAllStatesForUser = value;
            }
        }

        /// <summary>
        /// Gets or set a value indicating whether the control is in the checked state.
        /// </summary>
        /// <value><c>true</c> if the control is in the checked
        /// state; otherwise, <c>false</c>.
        /// The default value is <c>false</c>.</value>
        /// <remarks>When the value is <c>true</c>, the control
        /// portion of the control displays a check mark.</remarks>
        [DefaultValue(false)]
        public virtual bool IsChecked
        {
            get
            {
                return CheckState == CheckState.Checked;
            }

            set
            {
                if (value)
                    CheckState = CheckState.Checked;
                else
                    CheckState = CheckState.Unchecked;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsChecked"/> property changes.
        /// </summary>
        public event EventHandler? CheckedChanged;

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
                if (item is null)
                {
                    item = CreateItem();
                    OnItemCreatedOrAssigned(item);
                }

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
        /// Gets or sets visibility of the text.
        /// </summary>
        public virtual bool TextVisible
        {
            get
            {
                return ItemDefaults.TextVisible;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (ItemDefaults.TextVisible == value)
                    return;
                ItemDefaults.TextVisible = value;
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
        /// Gets or sets a value indicating whether the control will
        /// allow three check states rather than two.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the control is able to display
        /// three check states; otherwise, <see langword="false" />. The default value
        /// is <see langword="false"/>.
        /// </returns>
        [DefaultValue(false)]
        public virtual bool ThreeState
        {
            get
            {
                return ItemDefaults.CheckBoxThreeState;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (ThreeState == value)
                    return;
                if (!value && CheckState == CheckState.Indeterminate)
                {
                    CheckState = CheckState.Unchecked;
                }

                ItemDefaults.CheckBoxThreeState = value;
            }
        }

        /// <summary>
        /// Gets or sets the state of the control.
        /// </summary>
        /// <returns>
        /// One of the <see cref="UI.CheckState"/> enumeration values. The default value
        /// is <see cref="CheckState.Unchecked"/>.
        /// </returns>
        [DefaultValue(CheckState.Unchecked)]
        [RefreshProperties(RefreshProperties.All)]
        public virtual CheckState CheckState
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Item.CheckState;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (!ThreeState && value == CheckState.Indeterminate)
                    value = CheckState.Unchecked;
                if (CheckState == value)
                    return;
                Item.CheckState = value;
                Invalidate();
                RaiseCheckedChanged();
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

        /// <summary>
        /// Gets or sets <see cref="Action"/> which will be executed when
        /// this control is clicked by the user.
        /// </summary>
        [Browsable(false)]
        public virtual Action? ClickAction
        {
            get => clickAction;
            set
            {
                clickAction = value;
            }
        }

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
                if (formatProvider == value)
                    return;
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
        /// Same as <see cref="IsChecked"/>. This property is added for the compatibility with legacy code.
        /// </summary>
        [Browsable(false)]
        public bool Checked
        {
            get => IsChecked;
            set => IsChecked = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        [Browsable(false)]
        internal new string Title
        {
            get => base.Title;
            set => base.Title = value;
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

            var result = itemDrawable.GetPreferredSize();
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

        /// <summary>
        /// Returns the zero-based index of the item, if specified coordinates are over checkbox;
        /// otherwise returns <c>null</c>.
        /// </summary>
        /// <param name="position">A <see cref="PointD"/> object containing
        /// the coordinates used to obtain the item
        /// index.</param>
        public virtual bool HitTestCheckBox(PointD position)
        {
            var rect = ClientRectangle;
            rect = rect.DeflatedWithPadding(Padding);
            rect = rect.DeflatedWithPadding(Item.ForegroundMargin);

            var info = Item.GetCheckBoxInfo(this, rect);
            if (info is null || !info.IsCheckBoxVisible)
                return false;
            var checkRect = info.CheckRect;
            checkRect.Inflate(2);
            var isOverCheck = checkRect.Contains(position);
            return isOverCheck;
        }

        /// <summary>
        /// Toggles checked state of the item clicked at the specified coordinates.
        /// </summary>
        /// <param name="location">A <see cref="PointD"/> object containing
        /// the coordinates where the item was clicked.</param>
        /// <param name="hitTestCheckBox">A boolean value indicating whether to perform a hit test on the checkbox.
        /// If true, the method will only toggle the check state if the click occurred on the checkbox.
        /// If false, the method will toggle the check state regardless of the click location.
        /// This allows to change checked state when item is clicked on the item text.</param>
        public virtual bool ToggleItemCheckState(PointD location, bool hitTestCheckBox)
        {
            if (hitTestCheckBox && !HitTestCheckBox(location))
                return false;
            var result = Item.ToggleCheckState(this);
            if (result)
            {
                Invalidate();
                RaiseCheckedChanged();
            }

            return result;
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
            itemDrawable.VisualState = VisualState;

            if (itemDrawable.VisualState != VisualControlState.Normal)
            {
            }

            itemDrawable.Draw(this, dc);
        }

        /// <summary>
        /// Binds property specified with <paramref name="instance"/> and
        /// <paramref name="propName"/> to the <see cref="Checked"/> property of the control.
        /// After binding is set up, changes to the <see cref="Checked"/> property will automatically update the value
        /// of the bound property on the specified instance.
        /// </summary>
        /// <param name="instance">Object.</param>
        /// <param name="propName">Property name.</param>
        /// <remarks>Property must have the <see cref="bool"/> type. Value of the bound
        /// property will be changed automatically after
        /// <see cref="IsChecked"/> is changed.</remarks>
        public virtual GenericItemControl BindChecked(object instance, string propName)
        {
            var propInfo = AssemblyUtils.GetPropInfo(instance, propName);
            if (propInfo is null)
                return this;
            object? result = propInfo?.GetValue(instance, null);
            IsChecked = result is true;

            CheckedChanged += InternalCheckedChanged;

            void InternalCheckedChanged(object? sender, EventArgs e)
            {
                propInfo?.SetValue(instance, IsChecked);
            }

            return this;
        }

        /// <summary>
        /// Raises the <see cref="CheckedChanged"/> event and calls
        /// <see cref="OnCheckedChanged(EventArgs)"/>.
        /// </summary>
        public virtual void RaiseCheckedChanged()
        {
            if (DisposingOrDisposed)
                return;
            var newChecked = IsChecked;

            if (reportedChecked == newChecked)
                return;
            reportedChecked = newChecked;
            OnCheckedChanged(EventArgs.Empty);
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the <see cref="IsChecked"/> property.
        /// </summary>
        /// <param name="isChecked">The new value.</param>
        public void SetChecked(bool isChecked)
        {
            IsChecked = isChecked;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            commandSource.Changed = null;
            base.DisposeManaged();
        }

        /// <inheritdoc />
        protected override void OnClick(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            base.OnClick(e);
            clickAction?.Invoke();
            commandSource.Execute();
        }

        /// <summary>
        /// Called when the value of the <see cref="IsChecked"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Item.Text = Text;
            PerformLayoutAndInvalidate();
        }

        /// <summary>
        /// Creates and returns a drawable object which is responsible for rendering the visual representation of the item within the control.
        /// Derived classes can override this method to provide a custom drawable implementation
        /// that defines how the item should be drawn on the screen.
        /// </summary>
        /// <returns>A <see cref="ListItemDrawable"/> instance which is responsible for rendering the visual representation of the item.</returns>
        protected virtual ListItemDrawable CreateItemDrawable()
        {
            return new();
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            if (DisposingOrDisposed || !Enabled)
                return;

            if (ItemDefaults.CheckBoxVisible)
            {
                if (ToggleItemCheckState(e.Location, hitTestCheckBox: false))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Creates and initializes a new instance of the ListControlItemDefaults class.
        /// </summary>
        /// <returns>A new instance of ListControlItemDefaults, which contains settings for items.</returns>
        protected virtual ListControlItemDefaults CreateItemDefaults()
        {
            return new();
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