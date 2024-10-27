using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace ControlsSample
{
    internal class ListControlsOtherPage : PanelFormSelector
    {
        protected override void AddDefaultItems()
        {
            Add("ComboBox with VirtualListBox in the popup", () => new VComboBoxWindow());
        }
    }
}
