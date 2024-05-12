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
        ComboBox.OwnerDrawFlags OwnerDrawStyle { get; set; }

        PointI TextMargins { get; }

        string? EmptyTextHint { get; set; }

        int TextSelectionStart { get; }

        int TextSelectionLength { get; }

        bool HasBorder { get; set; }

        void SelectTextRange(int start, int length);

        void SelectAllText();

        void DefaultOnDrawBackground();

        void DefaultOnDrawItem();
    }
}
