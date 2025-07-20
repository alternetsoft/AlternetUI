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
            header.Parent = this;
            header.ParentBackColor = false;
            header.BackColor = DefaultColors.ControlBackColor;

            BackColor = Color.WhiteSmoke;

            SpeedTextButton label1 = new()
            {
                Text = "Column 1",
                Width = 100,
                Dock = DockStyle.Left,
            };
            label1.SetContentHorizontalAlignment(HorizontalAlignment.Left);

            SpeedTextButton label2 = new()
            {
                Text = "Column 2",
                Width = 100,
                Dock = DockStyle.Left,
            };
            label2.SetContentHorizontalAlignment(HorizontalAlignment.Left);

            SpeedTextButton label3 = new()
            {
                Text = "Column 3",
                Width = 100,
                Dock = DockStyle.Left,
            };
            label3.SetContentHorizontalAlignment(HorizontalAlignment.Left);

            Splitter splitter1 = new()
            {
                Dock = DockStyle.Left,
                ParentBackColor = true,
            };

            Splitter splitter2 = new()
            {
                Dock = DockStyle.Left,
                ParentBackColor = true,
            };

            Splitter splitter3 = new()
            {
                Dock = DockStyle.Left,
                ParentBackColor = true,
            };

            label1.Parent = header;
            splitter1.Parent = header;
            label2.Parent = header;
            splitter2.Parent = header;
            label3.Parent = header;
            splitter3.Parent = header;
        }
    }
}
