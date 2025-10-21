using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class ListBox
    {
        public void OnPlatformEventCheckedChanged()
        {
            (UIControl as UI.CheckedListBox)?.RaiseCheckedItemsChanged();
        }

        public void OnPlatformEventSelectionChanged()
        {
            (UIControl as UI.ListBox)?.RaiseSelectedIndexChanged();
        }
    }
}
