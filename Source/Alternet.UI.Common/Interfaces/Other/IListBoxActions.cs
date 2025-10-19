using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a set of actions that can be performed on a list box control.
    /// </summary>
    public interface IListBoxActions
    {
/*
        void SelectLastItemAndScroll();

        bool ScrollToFirstRow();

        bool ScrollToLastRow();

        void SelectFirstItemAndScroll();

        void SelectPreviousItem();

        void SelectNextItem();

        void SelectItemOnPreviousPage();

        void SelectItemOnNextPage();
*/

        /// <summary>
        /// Selects all items in the list box.
        /// </summary>
        void SelectAll();

        /// <summary>
        /// Unselects all items in the list box, clearing any selection.
        /// </summary>
        void UnselectAll();

        /// <summary>
        /// Removes all items from the list box.
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Selects the first item in the list box, if one exists.
        /// </summary>
        void SelectFirstItem();

        /// <summary>
        /// Selects the last item in the list box, if one exists.
        /// </summary>
        void SelectLastItem();
    }
}
