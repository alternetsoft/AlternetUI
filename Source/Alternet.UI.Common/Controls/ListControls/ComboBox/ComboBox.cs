using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a combo box control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A <see cref="ComboBox"/> displays a text box combined with a
    /// <see cref="ListBox"/>, which enables the user
    /// to select items from the list or enter a new value.
    /// The <see cref="IsEditable"/> property specifies whether the text portion
    /// can be edited.
    /// </para>
    /// <para>
    /// To add or remove objects in the list at run time, use methods of the
    /// <see cref="Collection{Object}" /> class
    /// (through the <see cref="ListControl.Items"/> property of the
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
    /// within the items of the list. With the <see cref="Control.BeginUpdate"/>
    /// and <see cref="Control.EndUpdate"/> methods, you can add a large number
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
    public partial class ComboBox : ListControl
    {
        /// <summary>
        /// Gets or sets default vertical offset of the item's image for the items with images.
        /// </summary>
        public static double DefaultImageVerticalOffset = 3;

        /// <summary>
        /// Gets or sets default distance between image and text in the item.
        /// </summary>
        public static double DefaultImageTextDistance = 3;

        /// <summary>
        /// Gets or sets default color of the image border.
        /// </summary>
        public static Color DefaultImageBorderColor = SystemColors.GrayText;

        private int? selectedIndex;
        private bool isEditable = true;
        private IComboBoxItemPainter? painter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBox"/> class.
        /// </summary>
        public ComboBox()
        {
            if (BaseApplication.IsWindowsOS && BaseApplication.PlatformKind == UIPlatformKind.WxWidgets)
            {
                UserPaint = true;
            }
        }

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
            /// Specifies whether to draw background.
            /// </summary>
            ItemBackground = 1,

            /// <summary>
            /// Specifies whether to draw item.
            /// </summary>
            Item = 2,
        }

        /// <summary>
        /// Gets the starting index of text selected in the combo box.
        /// </summary>
        /// <value>The zero-based index of the first character in the string
        /// of the current text selection.</value>
        public virtual int TextSelectionStart => Handler.TextSelectionStart;

        /// <summary>
        /// Gets the number of characters selected in the editable portion
        /// of the combo box.
        /// </summary>
        /// <value>The number of characters selected in the combo box.</value>
        public virtual int TextSelectionLength => Handler.TextSelectionLength;

        /// <summary>
        /// Gets or sets a hint shown in an empty unfocused text control.
        /// </summary>
        public virtual string? EmptyTextHint
        {
            get
            {
                return Handler.EmptyTextHint;
            }

            set
            {
                Handler.EmptyTextHint = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed in the <see cref="ComboBox"/>.
        /// </summary>
        /// <remarks>
        /// Setting the <see cref="Text"/> property to an empty string ("")
        /// sets the <see cref="SelectedIndex"/> to <c>null</c>.
        /// Setting the <see cref="Text"/> property to a value that is in the
        /// <see cref="ListControl.Items"/> collection sets the
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
        public override int? SelectedIndex
        {
            get
            {
                return selectedIndex;
            }

            set
            {
                CheckDisposed();
                if (value < 0 || value >= Items.Count)
                    value = null;
                if (selectedIndex == value)
                    return;
                selectedIndex = value;
                Text = GetItemText(SelectedItem);
                RaiseSelectedItemChanged(EventArgs.Empty);
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
        public override object? SelectedItem
        {
            get
            {
                if (selectedIndex == null || selectedIndex < 0 || selectedIndex >= Items.Count)
                    return null;
                return Items[selectedIndex.Value];
            }

            set
            {
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
        public virtual bool IsEditable
        {
            get
            {
                return isEditable;
            }

            set
            {
                if (isEditable == value)
                    return;
                CheckDisposed();
                isEditable = value;
                IsEditableChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public virtual bool HasBorder
        {
            get => Handler.HasBorder;
            set
            {
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
                var margins = Handler.TextMargins;
                var result = PixelToDip(margins);
                return result;
            }
        }

        /// <summary>
        /// Gets or sets whether background of the item is owner drawn.
        /// </summary>
        [Browsable(false)]
        public bool OwnerDrawItemBackground
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

        /// <summary>
        /// Gets or sets whether item is owner drawn.
        /// </summary>
        [Browsable(false)]
        public bool OwnerDrawItem
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

        internal OwnerDrawFlags OwnerDrawStyle
        {
            get
            {
                return Handler.OwnerDrawStyle;
            }

            set
            {
                if (OwnerDrawStyle == value)
                    return;
                Handler.OwnerDrawStyle = value;
                Invalidate();
            }
        }

        internal new IComboBoxHandler Handler
        {
            get
            {
                CheckDisposed();
                return (IComboBoxHandler)base.Handler;
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
            => Handler.SelectTextRange(start, length);

        /// <summary>
        /// Selects all the text in the editable portion of the ComboBox.
        /// </summary>
        public virtual void SelectAllText() => Handler.SelectAllText();

        /// <summary>
        /// Raises the <see cref="SelectedItemChanged"/> event and calls
        /// <see cref="OnSelectedItemChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public virtual void RaiseSelectedItemChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnSelectedItemChanged(e);
            SelectedItemChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Binds property specified with <paramref name="instance"/> and
        /// <paramref name="propName"/> to the <see cref="ComboBox"/>.
        /// After binding <see cref="ComboBox"/> will edit the specified property.
        /// </summary>
        /// <param name="instance">Object.</param>
        /// <param name="propName">Property name.</param>
        /// <remarks>
        /// Property must have the <see cref="Enum"/> type. Value of the binded
        /// property will be changed automatically after <see cref="SelectedItem"/>
        /// is changed.
        /// </remarks>
        /// <remarks>
        /// Items property of the <see cref="ComboBox"/> is filled with <see cref="Enum"/>
        /// elements using <see cref="PropertyGrid.GetPropChoices"/>. So, it is possible
        /// to localize labels and limit displayed enum elements.
        /// </remarks>
        public virtual void BindEnumProp(object instance, string propName)
        {
            var choices = BasePropertyGrid.GetPropChoices(instance, propName);
            if (choices is null)
                return;
            IsEditable = false;

            var propInfo = AssemblyUtils.GetPropInfo(instance, propName);
            object? result = propInfo?.GetValue(instance, null);
            int selectIndex = -1;

            for (int i = 0; i < choices.Count; i++)
            {
                var label = choices.GetLabel(i);
                var value = choices.GetValue(i);
                var item = new ListControlItem(label, value);
                var index = Add(item);
                if (result is not null)
                {
                    if (value == (int)result)
                        selectIndex = index;
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

        /// <summary>
        /// Default item paint method.
        /// </summary>
        /// <param name="e">Paint arguments.</param>
        public virtual void DefaultItemPaint(ComboBoxItemPaintEventArgs e)
        {
            if (e.IsPaintingBackground || ShouldPaintHintText())
            {
                e.DefaultPaint();
                return;
            }

            var font = Font ?? Control.DefaultFont;
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
                    color = SystemColors.GrayText;
                }

                var size = e.Graphics.MeasureText(s, font);

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
                    color,
                    (e.ClipRectangle.X + 2, e.ClipRectangle.Y));
            }
        }

        /// <summary>
        /// Gets suggested rectangles of the item's image and text.
        /// </summary>
        /// <param name="e">Item painting paramaters.</param>
        /// <returns></returns>
        public virtual (RectD ImageRect, RectD TextRect) GetItemImageRect(
            ComboBoxItemPaintEventArgs e)
        {
            var offset = DefaultImageVerticalOffset;
            if (e.IsPaintingControl)
                offset++;

            var size = e.ClipRectangle.Height - (TextMargin.Y * 2) - (offset * 2);
            var imageRect = new RectD(
                e.ClipRectangle.X + TextMargin.X,
                e.ClipRectangle.Y + TextMargin.Y + offset,
                size,
                size);

            var itemRect = e.ClipRectangle;
            itemRect.X += imageRect.Width + DefaultImageTextDistance;
            itemRect.Width -= imageRect.Width + DefaultImageTextDistance;

            return (imageRect, itemRect);
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
        protected override IControlHandler CreateHandler()
        {
            return NativePlatform.Default.CreateComboBoxHandler(this);
        }

        /// <summary>
        /// Called when the value of the <see cref="SelectedItem"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
        }
    }
}