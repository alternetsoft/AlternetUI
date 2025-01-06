using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to methods and properties of the native combobox control.
    /// </summary>
    public interface IComboBoxHandler
    {
        /// <inheritdoc cref="ComboBox.PopupControl"/>
        VirtualListBox? PopupControl { set; }

        /// <inheritdoc cref="ComboBox.OwnerDrawStyle"/>
        ComboBox.OwnerDrawFlags OwnerDrawStyle { get; set; }

        /// <inheritdoc cref="ComboBox.TextMargin"/>
        PointI TextMargins { get; }

        /// <inheritdoc cref="ComboBox.EmptyTextHint"/>
        string? EmptyTextHint { get; set; }

        /// <inheritdoc cref="ComboBox.TextSelectionStart"/>
        int TextSelectionStart { get; }

        /// <inheritdoc cref="ComboBox.TextSelectionLength"/>
        int TextSelectionLength { get; }

        /// <inheritdoc cref="ComboBox.SelectTextRange"/>
        void SelectTextRange(int start, int length);

        /// <inheritdoc cref="ComboBox.SelectAllText"/>
        void SelectAllText();

        /// <summary>
        /// Closes popup window.
        /// </summary>
        void DismissPopup();

        /// <summary>
        /// Shows popup window.
        /// </summary>
        void ShowPopup();
    }
}
