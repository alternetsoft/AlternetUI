using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a combo box control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A <see cref="ComboBox"/> displays a text box combined with a
    /// <see cref="StdListBox"/>, which enables the user
    /// to select items from the list or enter a new value.
    /// The <see cref="IsEditable"/> property specifies whether the text portion
    /// can be edited.
    /// </para>
    /// <para>
    /// To add or remove objects in the list at run time, use methods of the
    /// <see cref="Collection{Object}" /> class
    /// (through the <see cref="ListControl{T}.Items"/> property of the
    /// <see cref="ComboBox" />).
    /// The list then displays the default string value for each object.
    /// You can add individual objects with the
    /// <see cref="ICollection{Object}.Add"/> method.
    /// You can delete items with the <see cref="ICollection{Object}.Remove"/>
    /// method or clear the entire list with the
    /// <see cref="ICollection{Object}.Clear"/> method.
    /// </para>
    /// <para>
    /// In addition to display and selection functionality, the
    /// <see cref="ComboBox" /> also provides features that enable you to
    /// efficiently add items to the <see cref="ComboBox" /> and to find text
    /// within the items of the list. With the <see cref="AbstractControl.BeginUpdate"/>
    /// and <see cref="AbstractControl.EndUpdate"/> methods, you can add a large number
    /// of items to the <see cref="ComboBox" /> without the control
    /// being repainted each time an item is added to the list.
    /// </para>
    /// <para>
    /// You can use the <see cref="Text"/> property to specify the string
    /// displayed in the editing field,
    /// the <see cref="SelectedIndex"/> property to get or set the current item,
    /// and the <see cref="SelectedItem"/> property to get or set a reference
    /// to the selected object.
    /// </para>
    /// </remarks>
    [ControlCategory("Common")]
    public partial class ComboBox : Control, IListControl, IListControlItemContainer
    {
        /// <summary>
        /// Gets or sets default vertical offset of the item's image for the items with images.
        /// </summary>
        public static Coord DefaultImageVerticalOffset = 2;

        /// <summary>
        /// Gets or sets default distance between image and text in the item.
        /// </summary>
        public static Coord DefaultImageTextDistance = 3;

        /// <summary>
        /// Gets or sets default color of the image border.
        /// </summary>
        public static Color DefaultImageBorderColor = SystemColors.GrayText;

        /// <summary>
        /// Gets or sets default disabled text color.
        /// </summary>
        public static Color DefaultDisabledTextColor = SystemColors.GrayText;

        private bool allowMouseWheel = true;
        private int? selectedIndex;
        private bool isEditable = true;
        private IComboBoxItemPainter? painter;
        private ListControlItemDefaults? itemDefaults;
        private WeakReferenceValue<ImageList> imageList = new();
        private bool droppedDown;
        private int? reportedSelectedIndex;
        private SizeD? savedBestSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ComboBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBox"/> class.
        /// </summary>
        public ComboBox()
        {
            Handler.Required();
        }

        /// <summary>
        /// Occurs when the drop-down portion of the control is no longer visible.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? DropDownClosed;

        /// <summary>
        /// Occurs when the drop-down portion of the control is shown.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? DropDown;

        /// <summary>
        /// Occurs when the <see cref="SelectedItem"/> property value changes.
        /// </summary>
        /// <remarks>
        /// This event is raised if the <see cref="SelectedItem"/> property
        /// is changed by either a programmatic modification or user interaction.
        /// You can create an event handler for this event to determine when
        /// the selected index in the <see cref="ComboBox"/> has been changed.
        /// This can be useful when you need to display information in other
        /// controls based on the current selection in the <see cref="ComboBox"/>.
        /// You can use the event handler for this event to load the information
        /// in the other controls.
        /// </remarks>
        public event EventHandler? SelectedItemChanged;

        /// <summary>
        /// Same as <see cref="SelectedItemChanged"/>. Added for the compatibility.
        /// </summary>
        public event EventHandler? SelectedIndexChanged;

        /// <summary>
        /// Occurs when the <see cref="IsEditable"/> property value changes.
        /// </summary>
        public event EventHandler? IsEditableChanged;

        /// <summary>
        /// Enumerates possible owner draw flags.
        /// </summary>
        [Flags]
        public enum OwnerDrawFlags
        {
            /// <summary>
            /// Owner draw is off.
            /// </summary>
            None = 0,

            /// <summary>
            /// Specifies whether to draw background.
            /// </summary>
            ItemBackground = 1,

            /// <summary>
            /// Specifies whether to draw item.
            /// </summary>
            Item = 2,
        }

        /// <summary>
        /// Gets or sets the <see cref="ImageList"/> associated with the control.
        /// </summary>
        /// <value>An <see cref="ImageList"/> that contains the images to be used by the control.
        /// If no <see cref="ImageList"/> is set, this property returns null.</value>
        public virtual ImageList? ImageList
        {
            get
            {
                return imageList.Value;
            }

            set
            {
                if (ImageList == value)
                    return;
                imageList.Value = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets defaults which are used when items are painted in the popup list box
        /// in the case when item is <see cref="ListControlItem"/> and owner draw mode is turned on.
        /// </summary>
        [Browsable(false)]
        public virtual ListControlItemDefaults OwnerDrawItemDefaults
        {
            get
            {
                itemDefaults ??= new ListControlItemDefaults();
                return itemDefaults;
            }

            set
            {
                itemDefaults = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the combo box is displaying its drop-down portion.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the drop-down portion is displayed;
        /// otherwise, <see langword="false" />. The default is false.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool DroppedDown
        {
            get
            {
                return droppedDown;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (DroppedDown == value)
                    return;
                if (value)
                    PlatformControl.ShowPopup();
                else
                    PlatformControl.DismissPopup();
            }
        }

        /// <summary>
        /// Gets the starting index of text selected in the combo box.
        /// </summary>
        /// <value>The zero-based index of the first character in the string
        /// of the current text selection.</value>
        public virtual int TextSelectionStart
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return PlatformControl.TextSelectionStart;
            }
        }

        /// <summary>
        /// Gets the number of characters selected in the editable portion
        /// of the combo box.
        /// </summary>
        /// <value>The number of characters selected in the combo box.</value>
        public virtual int TextSelectionLength
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return PlatformControl.TextSelectionLength;
            }
        }

        /// <summary>
        /// Gets or sets a hint shown in an empty unfocused text control.
        /// </summary>
        public virtual string? EmptyTextHint
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return PlatformControl.EmptyTextHint;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                PlatformControl.EmptyTextHint = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed in the <see cref="ComboBox"/>.
        /// </summary>
        /// <remarks>
        /// Setting the <see cref="Text"/> property to an empty string ("")
        /// sets the <see cref="SelectedIndex"/> to <c>null</c>.
        /// Setting the <see cref="Text"/> property to a value that is in the
        /// <see cref="ListControl{T}.Items"/> collection sets the
        /// <see cref="SelectedIndex"/> to the index of that item.
        /// Setting the <see cref="Text"/> property to a value that is not in
        /// the collection leaves the <see cref="SelectedIndex"/> unchanged.
        /// Reading the <see cref="Text"/> property returns the text of
        /// <see cref="SelectedItem"/>, if it is not <c>null</c>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <c>value</c> is
        /// <c>null</c>.</exception>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (Text == value)
                    return;

                value ??= string.Empty;
                base.Text = value;

                if (value == string.Empty)
                    SelectedIndex = null;

                var foundIndex = FindStringExact(value);
                if (foundIndex != null)
                    SelectedIndex = foundIndex.Value;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ComboBox;

        /// <summary>
        /// Gets or sets the index specifying the currently selected item.
        /// </summary>
        /// <value>A zero-based index of the currently selected item. A value
        /// of <c>null</c> is returned if no item is selected.</value>
        /// <remarks>This property indicates the zero-based index of
        /// the currently selected item in the combo box list.
        /// Setting a new index raises the <see cref="SelectedItemChanged"/>
        /// event.</remarks>
        public virtual int? SelectedIndex
        {
            get
            {
                return selectedIndex;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (value < 0 || value >= Items.Count)
                    value = null;
                if (selectedIndex == value)
                    return;
                selectedIndex = value;
                Text = GetItemText(SelectedItem, false);
                RaiseSelectedItemChanged();
            }
        }

        /// <summary>
        /// Gets or sets currently selected item in the combo box.
        /// </summary>
        /// <value>The object that is the currently selected item or <c>null</c>
        /// if there is no currently selected item.</value>
        /// <remarks>
        /// When you set the <see cref="SelectedItem"/> property to an object,
        /// the <see cref="ComboBox"/> attempts to
        /// make that object the currently selected one in the list.
        /// If the object is found in the list, it is displayed in the edit
        /// portion of the <see cref="ComboBox"/> and
        /// the <see cref="SelectedIndex"/> property is set to the
        /// corresponding index.
        /// If the object does not exist in the list, the
        /// <see cref="SelectedIndex"/> property is left at its current value.
        /// </remarks>
        public virtual object? SelectedItem
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                if (selectedIndex == null || selectedIndex < 0 || selectedIndex >= Items.Count)
                    return null;
                return Items[selectedIndex.Value];
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (SelectedItem == value)
                    return;
                if (value == null)
                {
                    SelectedIndex = null;
                    return;
                }

                var index = Items.IndexOf(value);
                if (index != -1)
                    SelectedIndex = index;
            }
        }

        /// <summary>
        /// Gets or a value that enables or disables editing of the text in
        /// text box area of the <see cref="ComboBox"/>.
        /// </summary>
        /// <value><c>true</c> if the <see cref="ComboBox"/> can be edited;
        /// otherwise <c>false</c>. The default is <c>false</c>.</value>
        [Category("Appearance")]
        [DefaultValue(true)]
        public virtual bool IsEditable
        {
            get
            {
                return isEditable;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (isEditable == value)
                    return;
                CheckDisposed();
                isEditable = value;
                IsEditableChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value specifying the style of the combo box.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ComboBoxStyle" /> values.
        /// The default is <see langword="DropDown" />.
        /// </returns>
        [Category("Appearance")]
        [DefaultValue(ComboBoxStyle.DropDown)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Browsable(false)]
        public virtual ComboBoxStyle DropDownStyle
        {
            get
            {
                if (IsEditable)
                    return ComboBoxStyle.DropDown;
                else
                    return ComboBoxStyle.DropDownList;
            }

            set
            {
                if (DropDownStyle == value)
                    return;
                switch (value)
                {
                    case ComboBoxStyle.DropDown:
                        IsEditable = true;
                        break;
                    case ComboBoxStyle.DropDownList:
                        IsEditable = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        [Browsable(true)]
        public override bool HasBorder
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.HasBorder;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (HasBorder == value)
                    return;
                Handler.HasBorder = value;
            }
        }

        /// <summary>
        /// Gets or sets item painter associated with the control.
        /// </summary>
        [Browsable(false)]
        public virtual IComboBoxItemPainter? ItemPainter
        {
            get
            {
                return painter;
            }

            set
            {
                if (painter == value)
                    return;
                painter = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets text margin. It is the empty space between borders
        /// of control and the text itself.
        /// </summary>
        [Browsable(false)]
        public virtual PointD TextMargin
        {
            get
            {
                var margins = PlatformControl.TextMargins;
                var result = PixelToDip(margins);
                if (result.X < 1)
                    result.X = 1;
                if (result.Y < 1)
                    result.Y = 1;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets whether background of the item is owner drawn.
        /// </summary>
        [Browsable(false)]
        public virtual bool OwnerDrawItemBackground
        {
            get
            {
                return OwnerDrawStyle.HasFlag(OwnerDrawFlags.ItemBackground);
            }

            set
            {
                OwnerDrawStyle |= OwnerDrawFlags.ItemBackground;
            }
        }

        IListControlItemDefaults IListControlItemContainer.Defaults
        {
            get
            {
                return OwnerDrawItemDefaults;
            }
        }

        /// <summary>
        /// Gets or sets whether item is owner drawn.
        /// </summary>
        [Browsable(false)]
        public virtual bool OwnerDrawItem
        {
            get
            {
                return OwnerDrawStyle.HasFlag(OwnerDrawFlags.Item);
            }

            set
            {
                OwnerDrawStyle |= OwnerDrawFlags.Item;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether your code or the operating system
        /// will handle drawing of elements in the list.
        /// </summary>
        /// <returns>
        /// One of the <see cref="DrawMode" /> enumeration values. The default
        /// is <see cref="DrawMode.Normal" />.
        /// </returns>
        /// <remarks>
        /// This property is added for the compatibility with legacy code.
        /// Use <see cref="OwnerDrawStyle"/> property as it has more options.
        /// </remarks>
        [Category("Behavior")]
        [DefaultValue(DrawMode.Normal)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Browsable(false)]
        public virtual DrawMode DrawMode
        {
            get
            {
                if (OwnerDrawItemBackground || OwnerDrawItem)
                    return UI.DrawMode.OwnerDrawVariable;
                else
                    return UI.DrawMode.Normal;
            }

            set
            {
                if (DrawMode == value)
                    return;
                if(value == UI.DrawMode.Normal)
                {
                    OwnerDrawStyle = OwnerDrawFlags.None;
                }
                else
                {
                    OwnerDrawStyle = OwnerDrawFlags.ItemBackground;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse wheel can
        /// be used to change the selected item.
        /// </summary>
        public virtual bool AllowMouseWheel
        {
            get
            {
                return allowMouseWheel;
            }

            set
            {
                if (allowMouseWheel == value)
                    return;
                allowMouseWheel = value;
                PlatformControl.AllowMouseWheel = value;
            }
        }

        SvgImage? IListControlItemContainer.CheckImageUnchecked { get; }

        SvgImage? IListControlItemContainer.CheckImageChecked { get; }

        SvgImage? IListControlItemContainer.CheckImageIndeterminate { get; }

        AbstractControl? IListControlItemContainer.Control => this;

        internal OwnerDrawFlags OwnerDrawStyle
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return PlatformControl.OwnerDrawStyle;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (OwnerDrawStyle == value)
                    return;
                PlatformControl.OwnerDrawStyle = value;
                Invalidate();
            }
        }

        internal IComboBoxHandler PlatformControl
        {
            get
            {
                CheckDisposed();
                return (IComboBoxHandler)Handler;
            }
        }

        /// <summary>
        /// Selects a range of text in the editable portion of the ComboBox.
        /// </summary>
        /// <param name="start">The position of the first character in the
        /// current text selection within the text
        /// box.</param>
        /// <param name="length">The number of characters to select.</param>
        /// <remarks>
        /// If you want to set the start position to the first character
        /// in the control's text, set the start parameter
        /// to zero. You can use this method to select a substring of text,
        /// such as when searching through the text of
        /// the control and replacing information.
        /// </remarks>
        public virtual void SelectTextRange(int start, int length)
        {
            if (DisposingOrDisposed)
                return;
            PlatformControl.SelectTextRange(start, length);
        }

        /// <summary>
        /// Selects all the text in the editable portion of the ComboBox.
        /// </summary>
        public virtual void SelectAllText()
        {
            if (DisposingOrDisposed)
                return;
            PlatformControl.SelectAllText();
        }

        /// <summary>
        /// Raises the <see cref="SelectedItemChanged"/> event and calls
        /// <see cref="OnSelectedItemChanged(EventArgs)"/>.
        /// </summary>
        [Browsable(false)]
        public void RaiseSelectedItemChanged()
        {
            if (DisposingOrDisposed)
                return;
            var newIndex = SelectedIndex;

            if (newIndex == reportedSelectedIndex)
                return;

            reportedSelectedIndex = newIndex;

            OnSelectedItemChanged(EventArgs.Empty);
            SelectedItemChanged?.Invoke(this, EventArgs.Empty);
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="DropDown"/> event and calls <see cref="OnDropDown"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseDropDown()
        {
            if (DisposingOrDisposed)
                return;
            droppedDown = true;
            OnDropDown(EventArgs.Empty);
            DropDown?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="DropDownClosed"/> event and
        /// calls <see cref="OnDropDownClosed"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseDropDownClosed()
        {
            if (DisposingOrDisposed)
                return;
            droppedDown = false;
            OnDropDownClosed(EventArgs.Empty);
            DropDownClosed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Binds property specified with <paramref name="instance"/> and
        /// <paramref name="propName"/> to the <see cref="ComboBox"/>.
        /// After binding <see cref="ComboBox"/> will edit the specified property.
        /// </summary>
        /// <param name="instance">Object.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="addToItems">Optional function which returns whether to add
        /// specified enum value to the items.</param>
        /// <remarks>
        /// Property must have the <see cref="Enum"/> type. Value of the bound
        /// property will be changed automatically after <see cref="SelectedItem"/>
        /// is changed.
        /// </remarks>
        /// <remarks>
        /// Items property of the <see cref="ComboBox"/> is filled with <see cref="Enum"/>
        /// elements using <see cref="PropertyGrid.GetPropChoices"/>. So, it is possible
        /// to localize labels and limit displayed enum elements.
        /// </remarks>
        public virtual void BindEnumProp(
            object instance,
            string propName,
            Func<object, bool>? addToItems = null)
        {
            if (DisposingOrDisposed)
                return;
            var choices = PropertyGrid.GetPropChoices(instance, propName);
            if (choices is null)
                return;
            IsEditable = false;

            var propInfo = AssemblyUtils.GetPropInfo(instance, propName);
            if (propInfo is null)
                return;
            object? result = propInfo.GetValue(instance, null);
            int selectIndex = -1;

            for (int i = 0; i < choices.Count; i++)
            {
                var label = choices.GetLabel(i);
                var value = choices.GetValue(i);
                var enumValue = Enum.ToObject(propInfo.PropertyType, value);

                if(addToItems?.Invoke(enumValue) ?? true)
                {
                    var item = new ListControlItem(label, enumValue);
                    var index = Add(item);
                    if (result is not null)
                    {
                        if (value == (int)result)
                            selectIndex = index;
                    }
                }
            }

            if (selectIndex >= 0)
                SelectedIndex = selectIndex;

            SelectedItemChanged += Editor_SelectedItemChanged;

            void Editor_SelectedItemChanged(object? sender, EventArgs e)
            {
                var item = (sender as ComboBox)?.SelectedItem;
                if (item is null)
                    return;
                object? value = null;
                if (item is ListControlItem lcItem)
                    value = lcItem.Value;
                else
                    value = item;
                propInfo?.SetValue(instance, value);
            }
        }

        /// <inheritdoc/>
        public override void PerformLayout(bool layoutParent = true)
        {
            base.PerformLayout(layoutParent);
        }

        /// <inheritdoc/>
        public override void RaiseFontChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            InvalidateBestSize();
            base.RaiseFontChanged(e);
        }

        /// <summary>
        /// Gets text color when control is disabled.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetDisabledTextColor()
        {
            return DefaultDisabledTextColor;
        }

        /// <summary>
        /// Default item paint method.
        /// </summary>
        /// <param name="e">Paint arguments.</param>
        public virtual void DefaultItemPaint(ComboBoxItemPaintEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (ShouldPaintHintText())
            {
                e.DefaultPaint();
                return;
            }

            var item = e.Item as ListControlItem;
            if (item is not null && !e.IsPaintingControl)
            {
                ListBoxItemPaintEventArgs listEventArgs = new(
                    this,
                    e.Graphics,
                    e.ClipRectangle,
                    e.ItemIndex);
                listEventArgs.IsSelected = e.IsSelected;
                listEventArgs.IsCurrent = e.IsSelected;

                if (e.IsPaintingBackground)
                {
                    item.DrawBackground(this, listEventArgs);
                }
                else
                {
                    item.DrawForeground(this, listEventArgs);
                }

                return;
            }

            if (e.IsPaintingBackground)
            {
                e.DefaultPaint();
                return;
            }

            var font = RealFont;
            Color color;
            color = ForegroundColor ?? SystemColors.WindowText;

            if (e.IsPaintingControl)
            {
                var s = Text;
                if (Enabled)
                {
                }
                else
                {
                    color = GetDisabledTextColor();
                }

                var size = e.Graphics.GetTextExtent(s, font);

                var offsetX = TextMargin.X;
                var offsetY = (e.ClipRectangle.Height - size.Height) / 2;
                var rect = e.ClipRectangle;
                rect.Inflate(-offsetX, -offsetY);

                e.Graphics.DrawText(
                    s,
                    font,
                    color.AsBrush,
                    rect);
            }
            else
            {
                if (e.IsSelected)
                    color = SystemColors.HighlightText;

                var s = Items[e.ItemIndex].ToString() ?? string.Empty;
                e.Graphics.DrawText(
                    s,
                    font,
                    color.AsBrush,
                    (e.ClipRectangle.X + 2, e.ClipRectangle.Y));
            }
        }

        void IListControl.Add(ListControlItem item)
        {
            Items.Add(item);
        }

        object? IListControl.GetItemAsObject(int index)
        {
            return GetItem(index);
        }

        /// <summary>
        /// Adds enum values to the items collection.
        /// </summary>
        /// <typeparam name="T">Type of the enum which values are added.</typeparam>
        public virtual void AddEnumValues<T>()
            where T : Enum
        {
            AddEnumValues(typeof(T));
        }

        /// <summary>
        /// Adds enum values to items collection.
        /// </summary>
        /// <typeparam name="T">Type of the enum which values are added.</typeparam>
        /// <param name="selectValue">New selected item value.</param>
        public virtual void AddEnumValues<T>(T selectValue)
            where T : Enum
        {
            AddEnumValues(typeof(T), selectValue);
        }

        /// <summary>
        /// Adds enum values to the items collection.
        /// </summary>
        /// <param name="type">Type of the enum which values are added.</param>
        /// <param name="selectValue">New selected item value.</param>
        public virtual void AddEnumValues(Type type, object? selectValue = default)
        {
            foreach (var item in Enum.GetValues(type))
                Items.Add(item);
            if (selectValue is not null)
                SelectedItem = selectValue;
        }

        /// <summary>
        /// Adds <paramref name="text"/> with <paramref name="data"/> to the end of
        /// the items collection.
        /// </summary>
        /// <param name="text">Item text (title).</param>
        /// <param name="data">Item data.</param>
        /// <remarks>
        /// This method creates <see cref="ListControlItem"/>, assigns its properties with
        /// <paramref name="text"/> and <paramref name="data"/>. Created object is added to
        /// the items collection.
        /// </remarks>
        public virtual int Add(string text, object? data)
        {
            return Add(new ListControlItem(text, data));
        }

        /// <summary>
        /// Adds <paramref name="text"/> with <paramref name="action"/> to the end of
        /// the items collection.
        /// </summary>
        /// <param name="text">Item text (title).</param>
        /// <param name="action">Action associated with the item.</param>
        /// <remarks>
        /// This method creates <see cref="ListControlItem"/>, assigns its properties with
        /// <paramref name="text"/> and <paramref name="action"/>. Created object is added to
        /// the collection.
        /// </remarks>
        public virtual int Add(string text, Action action)
        {
            return Add(new ListControlItem(text, action));
        }

        /// <summary>
        /// Gets suggested rectangles of the item's image and text.
        /// </summary>
        /// <param name="e">Item painting parameters.</param>
        /// <returns></returns>
        public virtual (RectD ImageRect, RectD TextRect) GetItemImageRect(
            ComboBoxItemPaintEventArgs e)
        {
            var offset = DefaultImageVerticalOffset;
            if (e.IsPaintingControl)
                offset++;
            else
            {
            }

            var textMargin = TextMargin;
            var size = e.ClipRectangle.Height - (textMargin.Y * 2) - (offset * 2);
            var imageRect = new RectD(
                e.ClipRectangle.X + textMargin.X,
                e.ClipRectangle.Y + textMargin.Y + offset,
                size,
                size);

            var itemRect = e.ClipRectangle;
            var horzOffset = imageRect.Right + DefaultImageTextDistance;
            itemRect.X += horzOffset;
            itemRect.Width -= imageRect.Right + DefaultImageTextDistance;

            return (imageRect, itemRect);
        }

        int IListControlItemContainer.GetItemCount()
        {
            return Items.Count;
        }

        /// <summary>
        /// Gets whether hint text should be painted.
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldPaintHintText()
        {
            bool noHintText = string.IsNullOrEmpty(EmptyTextHint);
            bool noText = string.IsNullOrEmpty(Text);

            return !Focused && noText && !noHintText;
        }

        /// <inheritdoc/>
        public override void InvalidateBestSize()
        {
            savedBestSize = null;
            base.InvalidateBestSize();
        }

        /// <inheritdoc/>
        protected override SizeD GetBestSizeWithoutPadding(SizeD availableSize)
        {
            SizeD result = base.GetBestSizeWithoutPadding(availableSize);
            if (Count > 0)
            {
                result.Width = Math.Max(CalcBestSizeCached().Width, result.Width);
            }

            return result;
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateComboBoxHandler(this);
        }

        /// <summary>
        /// Called when the <see cref="DropDown"/> event is fired.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnDropDown(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        /// <summary>
        /// Called when the <see cref="DropDownClosed"/> event is fired.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnDropDownClosed(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="SelectedItem"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
        }

        private SizeD CalcBestSize(int startIndex, int count)
        {
            var dc = MeasureCanvas;
            SizeD result = SizeD.Empty;

            for (int i = startIndex; i < count; i++)
            {
                var size = ListControlItem.DefaultMeasureItemSize(this, dc, i);
                result.Width = Math.Max(result.Width, size.Width);
                result.Height = Math.Max(result.Height, size.Height);
            }

            result.Width += 2 + PixelToDip(
                SystemSettings.GetMetric(SystemSettingsMetric.VScrollX, this));
            result.Height += 2;

            return result;
        }

        private SizeD CalcBestSizeCached()
        {
            SizeD result;
            if (savedBestSize is null)
            {
                result = CalcBestSize(0, Math.Min(Count, 512));
                savedBestSize = result;
            }
            else
                result = savedBestSize.Value;
            return result;
        }

        /// <summary>
        /// Default item painter for the owner draw <see cref="ComboBox"/> items.
        /// </summary>
        public class DefaultItemPainter : IComboBoxItemPainter
        {
            /// <inheritdoc/>
            public virtual Coord GetHeight(ComboBox sender, int index, Coord defaultHeight)
            {
                var size = ListControlItem.DefaultMeasureItemSize(sender, sender.MeasureCanvas, index);
                return size.Height;
            }

            /// <inheritdoc/>
            public virtual Coord GetWidth(ComboBox sender, int index, Coord defaultWidth)
            {
                var size = ListControlItem.DefaultMeasureItemSize(sender, sender.MeasureCanvas, index);
                return size.Width;
            }

            /// <inheritdoc/>
            public virtual void Paint(ComboBox sender, ComboBoxItemPaintEventArgs e)
            {
                sender.DefaultItemPaint(e);
            }
        }
    }
}