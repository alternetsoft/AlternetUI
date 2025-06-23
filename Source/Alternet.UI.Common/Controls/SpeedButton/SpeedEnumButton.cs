using System;
using System.ComponentModel;

using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="SpeedButton"/> for selecting of the enum values.
    /// </summary>
    public partial class SpeedEnumButton : SpeedButton
    {
        private PopupListBox? popupWindow;
        private Type? enumType;
        private object? data;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedEnumButton"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public SpeedEnumButton(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedEnumButton"/> class.
        /// </summary>
        public SpeedEnumButton()
        {
            TextVisible = true;
            ImageVisible = false;
        }

        /// <summary>
        /// Occurs when <see cref="Value"/> property is changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets the type of the enumeration which values are shown in the popup.
        /// </summary>
        [Browsable(false)]
        public virtual Type? EnumType
        {
            get
            {
                return enumType;
            }

            set
            {
                if (enumType == value)
                    return;
                enumType = value;

                if (enumType is null)
                    ListBox.RemoveAll();
                else
                {
                    var collection = new BaseCollection<ListControlItem>();
                    var values = Enum.GetValues(enumType);
                    foreach(var v in values)
                    {
                        ListControlItem item = new();
                        item.Value = v;
                        item.Text = GetValueAsString(v) ?? string.Empty;
                        collection.Add(item);
                    }

                    ListBox.SetItemsFast(collection, VirtualListBox.SetItemsKind.ChangeField);
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
        /// Gets attached popup window with <see cref="VirtualListBox"/>.
        /// </summary>
        [Browsable(false)]
        public virtual PopupListBox PopupWindow
        {
            get
            {
                if (popupWindow is null)
                {
                    popupWindow = new();
                    popupWindow.AfterHide += PopupWindowAfterHideHandler;
                }

                return popupWindow;
            }
        }

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
                data = value;
                base.Text = GetValueAsString(data) ?? string.Empty;
                ValueChanged?.Invoke(this, EventArgs.Empty);
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
                return GetValueAsString(data);
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
        /// Shows popup window with the list of enum values.
        /// </summary>
        public virtual void ShowPopup()
        {
            if (!Enabled)
                return;

            var index = ListBox.FindItemIndexWithValue(data);
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
    }
}