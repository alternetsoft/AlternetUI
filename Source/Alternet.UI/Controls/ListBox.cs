using System;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI
{
    public class ListBox : ListControl
    {
        HashSet<int> selectedIndices = new HashSet<int>();

        public IReadOnlyList<int> SelectedIndices
        {
            get
            {
                CheckDisposed();
                return selectedIndices.ToArray();
            }
        }

        public void ClearSelected()
        {
            selectedIndices.Clear();
            InvokeSelectionChanged(EventArgs.Empty);
        }

        public void SetSelected(int index, bool value)
        {
            if (value)
                selectedIndices.Add(index);
            else
                selectedIndices.Remove(index);

            InvokeSelectionChanged(EventArgs.Empty);
        }

        public int? SelectedIndex
        {
            get
            {
                CheckDisposed();
                return selectedIndices.FirstOrDefault();
            }

            set
            {
                CheckDisposed();

                ClearSelected();
                if (value != null)
                    SetSelected(value.Value, true);
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

        ListBoxSelectionMode selectionMode = ListBoxSelectionMode.Single;
    }
}