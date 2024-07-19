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
    public interface IComboBoxHandler : IControlHandler
    {
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

        /// <inheritdoc cref="ComboBox.HasBorder"/>
        bool HasBorder { get; set; }

        /// <inheritdoc cref="ComboBox.SelectTextRange"/>
        void SelectTextRange(int start, int length);

        /// <inheritdoc cref="ComboBox.SelectAllText"/>
        void SelectAllText();

        /// <summary>
        /// Default method for the background drawing. Can be used inside paint event.
        /// </summary>
        void DefaultOnDrawBackground();

        /// <summary>
        /// Default method for the item drawing. Can be used inside paint event.
        /// </summary>
        void DefaultOnDrawItem();
    }
}
