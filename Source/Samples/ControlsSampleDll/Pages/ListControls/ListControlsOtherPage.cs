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
            Add("Search for Members", () => WindowSearchForMembers.Default);
            Add("VirtualListBox as TreeView Sample", () => new ListBoxAsTreeWindow());
        }
    }
}
