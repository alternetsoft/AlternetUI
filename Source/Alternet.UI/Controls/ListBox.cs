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

            set
            {
                CheckDisposed();
                
                ClearSelectedCore();

                bool changed = false;
                foreach (var index in value)
                {
                    if (SetSelectedCore(index, true))
                        changed = true;
                }

                if (changed)
                    InvokeSelectionChanged(EventArgs.Empty);
            }
        }

        public void ClearSelected()
        {
            ClearSelectedCore();
            InvokeSelectionChanged(EventArgs.Empty);
        }

        private void ClearSelectedCore()
        {
            selectedIndices.Clear();
        }

        public void SetSelected(int index, bool value)
        {
            var changed = SetSelectedCore(index, value);

            if (changed)
                InvokeSelectionChanged(EventArgs.Empty);
        }

        private bool SetSelectedCore(int index, bool value)
        {
            bool changed;
            if (value)
                changed = selectedIndices.Add(index);
            else
                changed = selectedIndices.Remove(index);
            
            return changed;
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

        public event EventHandler? SelectionModeChanged;
        ListBoxSelectionMode selectionMode = ListBoxSelectionMode.Single;

        public ListBoxSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();

                return selectionMode;
            }

            set
            {
                CheckDisposed();

                if (selectionMode == value)
                    return;

                selectionMode = value;

                SelectionModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}