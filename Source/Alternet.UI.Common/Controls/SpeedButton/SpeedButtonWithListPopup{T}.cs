using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a speed button control that displays a popup window containing
    /// a virtual list box.
    /// </summary>
    /// <remarks>This control combines the functionality of a button
    /// and a popup list box, allowing users to
    /// select values from a list. The popup window is created lazily
    /// when accessed via the <see cref="PopupWindow"/> property.</remarks>
    /// <typeparam name="T">The type of the virtual list box used within the popup window.
    /// Must inherit from <see cref="VirtualListBox"/>
    /// and have a parameterless constructor.</typeparam>
    public partial class SpeedButtonWithListPopup<T> : SpeedButtonWithPopup<PopupListBox<T>, T>
        where T : VirtualListBox, new()
    {
        /// <summary>
        /// Gets or sets the maximum number of items that can be displayed
        /// in the context menu when popup kind is <see cref="PickerPopupKind.Auto"/>.
        /// If the number of items exceeds this value, the control will use a popup window
        /// with a list box instead of a context menu.
        /// </summary>
        public static int MaxItemsUsingContextMenu = 15;

        /// <summary>
        /// The default decrement value, in chars, used to adjust the size of an expanded drop-down menu.
        /// </summary>
        /// <remarks>This value is typically used to reduce the width of a drop-down menu when
        /// it is expanded if <see cref="DefaultExpandDropDownMenuToWidth"/> is <c>true</c>.
        /// This value allows to take into account menu item margins
        /// when calculating the final width.</remarks>
        public static int DefaultExpandedDropDownMenuDecrement = 20;

        /// <summary>
        /// Gets or sets the default kind of popup window used by the control.
        /// </summary>
        public static PickerPopupKind DefaultPopupKind = PickerPopupKind.Auto;

        /// <summary>
        /// Gets or sets whether the default behavior is to expand drop-down menu width
        /// to match the width of their parent control.
        /// </summary>
        public static bool DefaultExpandDropDownMenuToWidth = true;

        private BaseCollection<ListControlItem>? items;
        private ObjectUniqueId? createdMenuId;
        private ListBoxItems? simpleItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedButtonWithListPopup{T}"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public SpeedButtonWithListPopup(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedButtonWithListPopup{T}"/> class.
        /// </summary>
        public SpeedButtonWithListPopup()
        {
            PopupWindowTitle = CommonStrings.Default.WindowTitleSelectValue;
        }

        /// <summary>
        /// Occurs when a lookup operation is performed and provides the resulting value.
        /// Implementation of this event should lookup 'Value' in the <see cref="ListBox"/>
        /// control and assign index of the found item to result
        /// in the event data.
        /// </summary>
        /// <remarks>This event is triggered to notify subscribers of the outcome of a lookup operation.
        /// The event arguments contain the lookup result, which may be null
        /// if the operation fails or no value is found.</remarks>
        public event EventHandler<BaseEventArgs<int?>>? LookupValue;

        /// <summary>
        /// Gets or sets a value indicating whether to perform lookup by value
        /// when popup window is opened.
        /// </summary>
        public virtual bool LookupByValue { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to perform an exact text lookup
        /// when popup window is opened.
        /// </summary>
        public virtual bool LookupExactText { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to ignore case when looking up items
        /// in the popup window.
        /// </summary>
        public virtual bool LookupIgnoreCase { get; set; } = true;

        /// <summary>
        /// Gets or sets the kind of popup window used by the control.
        /// Default is <c>null</c>.
        /// If not set, the <see cref="DefaultPopupKind"/> is used.
        /// </summary>
        public virtual PickerPopupKind? PopupKind { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use a context menu as the popup window.
        /// </summary>
        /// <remarks>
        /// If set to <see langword="true"/>, the popup window will be a context menu.
        /// Otherwise, it will use the default popup kind.
        /// </remarks>
        public virtual bool UseContextMenuAsPopup
        {
            get => PopupKind == PickerPopupKind.ContextMenu;

            set
            {
                if (value)
                    PopupKind = PickerPopupKind.ContextMenu;
                else
                    PopupKind = null;
            }
        }

        /// <summary>
        /// Gets simple items where item is <c>object</c>.
        /// It is mapped from <see cref="ListControlItem.Value"/> elements
        /// of the <see cref="Items"/> collection.
        /// </summary>
        public virtual ListBoxItems SimpleItems
        {
            get
            {
                return simpleItems ??= new(Items);
            }
        }

        /// <summary>
        /// Gets the collection of items used in the list box control within the popup window.
        /// </summary>
        public virtual BaseCollection<ListControlItem> Items
        {
            get
            {
                if (items is not null)
                    return items;
                items = GetItems();
                return items;
            }

            set
            {
                items = value;
                simpleItems = null;
                if(IsPopupWindowCreated)
                {
                    ListBox.SetItemsFastest(items);
                }
            }
        }

        /// <summary>
        /// Gets the underlying <see cref="VirtualListBox"/> control used within the popup window.
        /// </summary>
        [Browsable(false)]
        public virtual VirtualListBox ListBox
        {
            get
            {
                return (VirtualListBox)PopupWindow.MainControl;
            }
        }

        /// <summary>
        /// Gets the index of the value in the items collection.
        /// </summary>
        public virtual int? IndexOfValue
        {
            get
            {
                if (Value is null)
                    return null;
                return ListControlItem.FindItemIndexWithValue(Items, Value);
            }
        }

        /// <summary>
        /// Gets attached popup window.
        /// </summary>
        [Browsable(false)]
        public PopupListBox<T> PopupListBox => (PopupListBox<T>)PopupWindow;

        /// <summary>
        /// Adds a collection of items to the list control shown in the popup.
        /// </summary>
        public virtual void AddRange(IEnumerable items)
        {
            foreach (var item in items)
            {
                if (item is ListControlItem listItem)
                    Items.Add(listItem);
                else
                {
                    ListControlItem newItem = new();
                    newItem.Value = item;
                    Items.Add(newItem);
                }
            }
        }

        /// <summary>
        /// Adds enum values to the items collection.
        /// </summary>
        /// <typeparam name="T2">Type of the enum which values are added.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddEnumValues<T2>()
            where T2 : Enum
        {
            AddEnumValues(typeof(T));
        }

        /// <summary>
        /// Adds enum values to items collection.
        /// </summary>
        /// <typeparam name="T2">Type of the enum which values are added.</typeparam>
        /// <param name="selectValue">New selected item value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddEnumValues<T2>(T2 selectValue)
            where T2 : Enum
        {
            AddEnumValues(typeof(T2), selectValue);
        }

        /// <summary>
        /// Adds a list of font sizes to the associated list control and optionally
        /// selects a specified font size.
        /// </summary>
        /// <remarks>This method populates the list control with a predefined set
        /// of font sizes and updates the current value to the specified font size
        /// or the default font size if none is provided.</remarks>
        /// <param name="select">A value indicating whether the specified font size
        /// should be selected in the list control. If <see
        /// langword="true"/>, the specified font size will be selected; otherwise,
        /// no selection is made.</param>
        /// <param name="size">The font size to add and optionally select.
        /// If <see langword="null"/>, the default font size is used.</param>
        public virtual void AddFontSizesAndSelect(bool select = false, Coord? size = null)
        {
            size ??= Control.DefaultFont.Size;
            ListControlUtils.AddFontSizes(Items, size);
            if (select)
                Value = size;
        }

        /// <summary>
        /// Adds font names to the associated list control and optionally selects
        /// a specified font name.
        /// </summary>
        /// <remarks>If <paramref name="select"/> is <see langword="true"/>,
        /// value property is updated to the specified font name.</remarks>
        /// <param name="select"><see langword="true"/> to select the specified font name
        /// after adding it to the list; otherwise, <see langword="false"/>.</param>
        /// <param name="fontName">The name of the font to add and optionally select.
        /// If <paramref name="fontName"/> is <see langword="null"/>,
        /// the default font name is used.</param>
        public virtual void AddFontNamesAndSelect(bool select = false, string? fontName = default)
        {
            ListControlUtils.AddFontNames(Items);
            if (select)
                Value = fontName ?? Control.DefaultFont.Name;
        }

        /// <summary>
        /// Adds enum values to the items collection.
        /// </summary>
        /// <param name="type">Type of the enum which values are added.</param>
        /// <param name="selectValue">New selected item value.</param>
        public virtual void AddEnumValues(Type type, object? selectValue = default)
        {
            foreach (var item in Enum.GetValues(type))
            {
                var st = item.ToString();
                if(st is null)
                    continue;
                Add(new ListControlItem(st, item));
            }

            if (selectValue is not null)
                Value = selectValue;
        }

        /// <summary>
        /// Finds the item with <see cref="ListControlItem.Value"/> property which is
        /// equal to the specified value.
        /// </summary>
        /// <param name="value">Value to search for.</param>
        /// <returns></returns>
        public virtual ListControlItem? FindItemWithValue(object? value)
        {
            if (value is null)
                return null;

            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                if (value.Equals(item.Value))
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Adds an item to the list control shown in the popup.
        /// </summary>
        /// <param name="title">Display text of the item which is shown to the user.</param>
        /// <param name="value">Value associated with the item.</param>
        public virtual ListControlItem Add(string title, object value)
        {
            ListControlItem newItem = new()
            {
                Value = value,
                Text = title,
            };
            Add(newItem);
            return newItem;
        }

        /// <summary>
        /// Adds an item to the list control shown in the popup.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <remarks>
        /// If item is not of type <see cref="ListControlItem"/>,
        /// a new <see cref="ListControlItem"/> will be created with the provided value.
        /// </remarks>
        public virtual ListControlItem Add(object item)
        {
            if (item is ListControlItem listItem)
            {
                Add(listItem);
                return listItem;
            }
            else
            {
                ListControlItem newItem = new()
                {
                    Value = item,
                };
                Add(newItem);
                return newItem;
            }
        }

        /// <summary>
        /// Selects the first item in the list and sets its value to the control.
        /// </summary>
        public virtual void SelectFirstItem()
        {
            if (Items.Count > 0)
            {
                Value = Items[0].Value;
            }
            else
            {
                Value = null;
            }
        }

        /// <summary>
        /// Adds an item to the list control shown in the popup.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public virtual void Add(ListControlItem item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Reloads the items in the associated list box.
        /// </summary>
        /// <remarks>This method updates the list box items by either clearing them
        /// or populating them with values.</remarks>
        public virtual void ReloadItems()
        {
            items = GetItems();
        }

        /// <inheritdoc/>
        public override void ShowPopup()
        {
            if (!Enabled)
                return;

            var kind = PopupKind ?? DefaultPopupKind;

            if (kind == PickerPopupKind.None)
                return;

            if(App.IsLinuxOS)
                kind = PickerPopupKind.ListBox;

            RaiseBeforeShowPopup(EventArgs.Empty);

            switch (kind)
            {
                case PickerPopupKind.Auto:
                default:
                    if (Items.Count <= MaxItemsUsingContextMenu)
                        ShowPopupMenu();
                    else
                        ShowListBox();
                    break;
                case PickerPopupKind.ListBox:
                    ShowListBox();
                    break;
                case PickerPopupKind.ContextMenu:
                    ShowPopupMenu();
                    break;
            }

            void ShowPopupMenu()
            {
                if (DropDownMenu is null)
                {
                    DropDownMenu = new ContextMenu();
                    createdMenuId = DropDownMenu.UniqueId;
                }

                DropDownMenu.Items.SetCount(Items.Count, () => new MenuItem());

                var popupOwner = PopupOwner ?? this;

                var spaceWidth = popupOwner.MeasureCanvas.MeasureText(" ", RealFont).Width;

                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];
                    var menuItem = DropDownMenu.Items[i];

                    var s = item.DisplayText ?? item.Text ?? item.Value?.ToString() ?? string.Empty;

                    if(i == 0 && DefaultExpandDropDownMenuToWidth)
                    {
                        var itemWidth = popupOwner.MeasureCanvas.MeasureText(s, RealFont).Width;
                        var numOfSpaces = (int)((popupOwner.Width - itemWidth) / spaceWidth);
                        numOfSpaces -= DefaultExpandedDropDownMenuDecrement;

                        if (numOfSpaces > 0)
                            s += new string(' ', numOfSpaces);
                    }

                    menuItem.Text = s;
                    menuItem.Tag = item.Value ?? item;
                    menuItem.ClickAction = MenuItemClickHandler;

                    void MenuItemClickHandler()
                    {
                        Value = menuItem.Tag;
                    }
                }

                var valueChecked = DropDownMenu.CheckSingleItemWithTag(Value);

                if (!valueChecked)
                {
                    var checkValue = Value is not ListControlItem valueAsItem
                        ? Value : valueAsItem.Value ?? valueAsItem;

                    DropDownMenu.CheckSingleItemWithTag(
                        checkValue,
                        Menu.FindItemFlags.IgnoreCase | Menu.FindItemFlags.TrimText);
                }

                DropDownMenu.ShowAsDropDown(popupOwner);
            }

            void ShowListBox()
            {
                if (Items is null)
                    ListBox.RemoveAll();
                else
                    ListBox.SetItemsFastest(Items);

                int? index = null;

                if(LookupValue is not null)
                {
                    var lookupEventArgs = new BaseEventArgs<int?>();
                    LookupValue(this, lookupEventArgs);
                    index = lookupEventArgs.Value;
                }

                if (index is null)
                {
                    var v = Value;

                    if (LookupByValue && v is not null)
                    {
                        index = ListBox.FindItemIndexWithValue(v);
                    }

                    var s = Text;

                    if (index is null && !string.IsNullOrEmpty(s))
                    {
                        index = ListBox.FindAndSelect(
                         s,
                         startIndex: 0,
                         exact: LookupExactText,
                         ignoreCase: LookupIgnoreCase);
                    }
                }

                ListBox.SelectItemAndScroll(index);
                PopupWindow.ShowPopup(PopupOwner ?? this);

                App.InvokeIdle(() =>
                {
                    ListBox.SelectItemAndScroll(index);
                    ListBox.Refresh();
                });
            }
        }

        /// <inheritdoc/>
        protected override void OnPopupWindowClosed(object? sender, EventArgs e)
        {
            if (PopupWindow.PopupResult == ModalResult.Accepted)
            {
                Value = PopupWindow.ResultItem?.Value ?? PopupWindow.ResultItem;
            }

            var focusedControl = PopupWindow.PopupOwner;
            focusedControl?.SetFocusIdle();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if(DropDownMenu is not null && DropDownMenu.UniqueId == createdMenuId)
            {
                var menu = DropDownMenu;
                DropDownMenu = null;
                menu?.Dispose();
            }

            base.DisposeManaged();
        }

        /// <summary>
        /// Retrieves a collection of items to be displayed in a list control shown in the popup.
        /// </summary>
        /// <remarks>This method returns a collection of <see cref="ListControlItem"/> objects,
        /// or <see langword="null"/> if no items are available.
        /// Derived classes can override this method to provide a custom
        /// collection of items.</remarks>
        /// <returns>A <see cref="BaseCollection{T}"/> containing
        /// <see cref="ListControlItem"/> objects, or <see
        /// langword="null"/> if no items are available.</returns>
        protected virtual BaseCollection<ListControlItem> GetItems()
        {
            return new();
        }
    }
}
