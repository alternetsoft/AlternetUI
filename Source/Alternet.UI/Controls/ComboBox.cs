using System;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI
{
    public class ComboBox : ListControl
    {
        private string text = "";

        public string Text
        {
            get
            {
                CheckDisposed();
                return text;
            }

            set
            {
                CheckDisposed();
                if (text == value)
                    return;

                text = value;
                InvokeTextChanged(EventArgs.Empty);
            }
        }

        int? selectedIndex;

        public int? SelectedIndex
        {
            get
            {
                CheckDisposed();
                return selectedIndex;
            }

            set
            {
                CheckDisposed();

                if (selectedIndex == value)
                    return;

                selectedIndex = value;

                InvokeSelectedItemChanged(EventArgs.Empty);
            }
        }

        public object? SelectedItem
        {
            get
            {
                CheckDisposed();
                
                if (SelectedIndex == null)
                    return null;

                return Items[SelectedIndex.Value];
            }

            set
            {
                CheckDisposed();

                if (value == null)
                {
                    SelectedIndex = -1;
                    return;
                }

                var index = Items.IndexOf(value);
                if (index != -1)
                    SelectedIndex = index;
            }
        }

        public void InvokeSelectedItemChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnSelectedItemChanged(e);
            SelectedItemChanged?.Invoke(this, e);
        }

        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
        }

        public event EventHandler? SelectedItemChanged;

        public void InvokeTextChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
            TextChanged?.Invoke(this, e);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
        }

        public event EventHandler? TextChanged;

        public event EventHandler? IsEditableChanged;

        bool isEditable = true;

        public bool IsEditable
        {
            get
            {
                CheckDisposed();
                return isEditable;
            }

            set
            {
                CheckDisposed();

                if (isEditable == value)
                    return;

                isEditable = value;

                IsEditableChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}