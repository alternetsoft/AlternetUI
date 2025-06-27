using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.Drawing;

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
    public partial class SpeedButtonWithListPopup<T> : SpeedButton
        where T : VirtualListBox, new()
    {
        private object? data;
        private PopupListBox<T>? popupWindow;
        private string popupWindowTitle = string.Empty;
        private BaseCollection<ListControlItem>? items;

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
            // Ensure that control occupies space even if value is null.
            base.Text = " ";

            TextVisible = true;
            ShowComboBoxImageAtRight();
        }

        /// <summary>
        /// Occurs when <see cref="Value"/> property is changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets the collection of items in the list box control used within the popup window.
        /// </summary>
        public virtual BaseCollection<ListControlItem> Items
        {
            get
            {
                if (IsPopupWindowCreated)
                {
                    return ListBox.Items;
                }
                else
                {
                    return items ??= new();
                }
            }

            set
            {
                if (IsPopupWindowCreated)
                {
                    ListBox.Items = value;
                    items = null;
                }
                else
                {
                    items = value;
                }
            }
        }

        /// <summary>
        /// Gets the underlying <see cref="VirtualListBox"/> control used within the popup window.
        /// </summary>
        [Browsable(false)]
        public VirtualListBox ListBox
        {
            get
            {
                return PopupWindow.MainControl;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the popup window has been created.
        /// </summary>
        public bool IsPopupWindowCreated => popupWindow is not null;

        /// <summary>
        /// Gets or sets selected color.
        /// </summary>
        public virtual object? Value
        {
            get
            {
                return data;
            }

            set
            {
                if (data == value)
                    return;
                PerformLayoutAndInvalidate(() =>
                {
                    data = value;
                    base.Text = Text ?? " ";
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                });
            }
        }

        /// <summary>
        /// Gets attached popup window.
        /// </summary>
        [Browsable(false)]
        public virtual PopupListBox<T> PopupWindow
        {
            get
            {
                if (popupWindow is null)
                {
                    popupWindow = new();
                    popupWindow.Title = popupWindowTitle;
                    popupWindow.AfterHide += PopupWindowAfterHideHandler;
                    if(items is null)
                    {
                        ReloadItems();
                    }
                    else
                    {
                        SetItems(items);
                    }
                }

                return popupWindow;
            }
        }

        /// <summary>
        /// Gets or sets the title of the popup window.
        /// </summary>
        public virtual string PopupWindowTitle
        {
            get
            {
                return popupWindowTitle;
            }

            set
            {
                if (popupWindowTitle == value)
                    return;
                popupWindowTitle = value;
                if (IsPopupWindowCreated)
                    PopupWindow.Title = popupWindowTitle;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Value"/> as <see cref="string"/>.
        /// </summary>
        [Browsable(false)]
        public new string? Text
        {
            get
            {
                string s = GetValueAsString(data) ?? " ";
                if (string.IsNullOrEmpty(s))
                    s = " ";
                return s;
            }

            set
            {
            }
        }

        internal new Image? Image
        {
            get => base.Image;
            set => base.Image = value;
        }

        internal new Image? DisabledImage
        {
            get => base.DisabledImage;
            set => base.DisabledImage = value;
        }

        /// <summary>
        /// Casts selected item to <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="T2">Type of the result.</typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T2? ValueAs<T2>() => (T2?)Value;

        /// <summary>
        /// Adds a collection of items to the list control shown in the popup.
        /// </summary>
        public virtual void AddRange(IEnumerable items)
        {
            if (IsPopupWindowCreated)
            {
                ListBox.AddRange(items);
            }
            else
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
        /// Adds enum values to the items collection.
        /// </summary>
        /// <param name="type">Type of the enum which values are added.</param>
        /// <param name="selectValue">New selected item value.</param>
        public virtual void AddEnumValues(Type type, object? selectValue = default)
        {
            foreach (var item in Enum.GetValues(type))
                Add(new ListControlItem(item.ToString(), item));
            if (selectValue is not null)
                Value = selectValue;
        }

        /// <summary>
        /// Adds an item to the list control shown in the popup.
        /// </summary>
        /// <param name="title">Display text of the item which is shown to the user.</param>
        /// <param name="value">Value associated with the item.</param>
        public virtual void Add(string title, object value)
        {
            ListControlItem newItem = new()
            {
                Value = value,
                Text = title,
            };
            Add(newItem);
        }

        /// <summary>
        /// Adds an item to the list control shown in the popup.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <remarks>
        /// If item is not of type <see cref="ListControlItem"/>,
        /// a new <see cref="ListControlItem"/> will be created with the provided value.
        /// </remarks>
        public virtual void Add(object item)
        {
            if (item is ListControlItem listItem)
                Add(listItem);
            else
            {
                ListControlItem newItem = new()
                {
                    Value = item,
                };
                Add(newItem);
            }
        }

        /// <summary>
        /// Selects the first item in the list and sets its value
        /// to the <see cref="Value"/> property.
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
            if (IsPopupWindowCreated)
            {
                ListBox.Add(item);
            }
            else
            {
                Items.Add(item);
            }
        }

        /// <summary>
        /// Sets the items to be displayed in the list control shown in the popup.
        /// </summary>
        /// <remarks>This method updates the list control with the provided collection of items.
        /// If <paramref name="items"/> is <see langword="null"/>, the list
        /// control is cleared.</remarks>
        /// <param name="items">A collection of <see cref="ListControlItem"/> objects
        /// to populate the list control. If <paramref name="items"/> is
        /// <see langword="null"/>, all items are removed from the list.</param>
        public virtual void SetItems(BaseCollection<ListControlItem>? items)
        {
            if (IsPopupWindowCreated)
            {
                if (items is null)
                    ListBox.RemoveAll();
                else
                    ListBox.SetItemsFast(items, VirtualListBox.SetItemsKind.ChangeField);
            }
            else
            {
                this.items = items;
            }
        }

        /// <summary>
        /// Reloads the items in the associated list box.
        /// </summary>
        /// <remarks>This method updates the list box items by either clearing them
        /// or populating them with values.</remarks>
        public virtual void ReloadItems()
        {
            if (!IsPopupWindowCreated)
                return;
            var collection = GetItems();
            SetItems(collection);
        }

        /// <summary>
        /// Shows popup window with the list of enum values.
        /// </summary>
        public virtual void ShowPopup()
        {
            if (!Enabled)
                return;

            var index = ListBox.FindItemIndexWithValue(Value);
            ListBox.SelectItemAndScroll(index);
            PopupWindow.ShowPopup(this);

            App.InvokeIdle(() =>
            {
                ListBox.SelectItemAndScroll(index);
                ListBox.Refresh();
            });
        }

        /// <inheritdoc/>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ShowPopup();
        }

        /// <summary>
        /// Gets enum value as string.
        /// </summary>
        /// <returns></returns>
        protected virtual string? GetValueAsString(object? d)
        {
            return d?.ToString();
        }

        /// <inheritdoc/>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// Fired after popup window is closed. Applies color selected in the popup window
        /// to the control.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments</param>
        protected virtual void PopupWindowAfterHideHandler(object? sender, EventArgs e)
        {
            if (PopupWindow.PopupResult == ModalResult.Accepted)
                Value = PopupWindow.ResultItem?.Value;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDispose(ref popupWindow);

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
        protected virtual BaseCollection<ListControlItem>? GetItems()
        {
            return null;
        }
    }
}
