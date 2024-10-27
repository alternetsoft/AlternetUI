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
            Add("VirtualListBox with BigData", () => new ListBoxBigDataWindow());
        }
    }
}
