using System;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI
{
    public class ComboBox : ListControl
    {
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

                InvokeSelectionChanged(EventArgs.Empty);
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

        public void InvokeSelectionChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnSelectionChanged(e);
            SelectionChanged?.Invoke(this, e);
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
        }

        public event EventHandler? SelectionChanged;

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