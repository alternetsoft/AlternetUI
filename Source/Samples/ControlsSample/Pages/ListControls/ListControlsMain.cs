using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class ListControlsMain : GenericTabControl
    {
        public ListControlsMain()
            : base()
        {
            Padding = 5;
            Add("ListBox", () => new ListBoxPage());
            Add("CheckListBox", () => new CheckListBoxPage());
            Add("ComboBox", () => new ComboBoxPage());
            Add("Popups", () => new ListControlsPopups());
            SelectFirstTab();
        }
    }
}
