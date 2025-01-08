using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class MauiComboBoxHandler : MauiControlHandler, IComboBoxHandler
    {
        public ComboBox.OwnerDrawFlags OwnerDrawStyle { get; set; }

        public PointI TextMargins { get; }

        public string? EmptyTextHint { get; set; }

        public int TextSelectionStart { get; }

        public int TextSelectionLength { get; }

        public VirtualListBox? PopupControl { get;  set; }

        public void DefaultOnDrawBackground()
        {
        }

        public void DefaultOnDrawItem()
        {
        }

        public void SelectAllText()
        {
        }

        public void SelectTextRange(int start, int length)
        {
        }

        public void DismissPopup()
        {
        }

        public void ShowPopup()
        {
        }
    }
}
