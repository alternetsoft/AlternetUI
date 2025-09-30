using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace ControlsSample
{
    internal class ListBoxHeaderTestPage : Panel
    {
        public ListBoxHeaderTestPage()
        {
            Padding = 20;
            Layout = LayoutStyle.Vertical;
            var header = new ListBoxHeader();
            header.HasBorder = true;

            var c1 = header.AddColumn("Column 1");
            
            header.AddColumn("Column 2", 100);
            
            var c3 = header.AddColumn("Column 3", 100);
            
            header.AddColumn("Column 4", 100);

            header.DeleteColumn(c3);

            var control1 = header.GetColumnControl(c1);

            if(control1 is not null)
            {
                control1.SetText("First Column");
                control1.SetSvgImage(MessageBoxSvg.Information, null);
                header.SetColumnWidth(c1, null);
            }

            var c4 = header.InsertColumn(0, "Column A", 100, () =>
            {
                App.Log("Column A clicked");
            });

            var c5 = header.InsertColumn(header.ColumnCount - 1, "Column B", 100, () =>
            {
                App.Log("Column B clicked");
            });

            header.Parent = this;
        }
    }
}
