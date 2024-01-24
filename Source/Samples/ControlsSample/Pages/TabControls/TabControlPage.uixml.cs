using System;
using System.Linq;
using System.Reflection;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class TabControlPage : Control
    {
        private int newItemIndex = 3;

        public TabControlPage()
        {
            InitializeComponent();
            tabAlignmentComboBox.Items.Add("Top");
            tabAlignmentComboBox.Items.Add("Bottom");
            tabAlignmentComboBox.SelectedIndex = 0;
            foreach(var page in tabControl.Pages)
            {
                page.VisibleChanged += Page_VisibleChanged;
            }
        }

        private void TabControl_HandleCreated(object? sender, EventArgs e)
        {
            Application.LogIf("TabControl_HandleCreated", true);
        }

        private void TabControl_HandleDestroyed(object? sender, EventArgs e)
        {
            Application.LogIf("TabControl_HandleDestroyed", true);
        }

        private void Page_VisibleChanged(object? sender, EventArgs e)
        {
            if (sender is not TabPage tabPage)
                return;
            Application.Log($"TabPage '{tabPage.Title}' VisibleChanged: {tabPage.Visible}");
        }

        private void TabControl_PageAdded(object? sender, EventArgs e)
        {
            Application.Log("TabControl:PageAdded");
        }

        private void TabControl_SizeChanged(object? sender, EventArgs e)
        {
            Application.Log("TabControl:SizeChanged");
        }

        private void Children_ItemInserted(object? sender, int index, Control item)
        {
        }

        private int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        private void ModifyFirstPageTitleButton_Click(
            object? sender, 
            EventArgs e)
        {
            if (!tabControl.Pages.Any())
                return;

            tabControl.Pages.First().Title += "X";
        }

        private void InsertLastPageSiblingButton_Click(
            object? sender,
            System.EventArgs e)
        {
            if (!tabControl.Pages.Any())
                return;

            var lastPage = tabControl.Pages.Last();

            InsertPage(lastPage.Index);
        }

        private void RemoveSelectedPageButton_Click(object sender, System.EventArgs e)
        {
            var selectedPage = tabControl.SelectedPage;
            if (selectedPage == null)
                return;

            tabControl.Pages.Remove(selectedPage);
        }

        private void AppendPageButton_Click(object sender, System.EventArgs e)
        {
            InsertPage();
        }

        private void InsertPage(int? index = null)
        {
            var s = "Page " + GenItemIndex();
            var page = new TabPage(s) 
            {
                Padding = 5,
            };
            page.VisibleChanged += Page_VisibleChanged;

            VerticalStackPanel panel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Padding = 5,
            };

            page.Children.Add(panel);

            for (int i=1; i<4; i++)
            {
                var button = new Button()
                {
                    Text = s + " Button " + i.ToString(),
                    Margin = 5,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
                panel.Children.Add(button);
            }
            if(index == null)
                tabControl.Pages.Add(page);
            else
                tabControl.Pages.Insert(index.Value, page);
        }

        private void ClearPagesButton_Click(object sender, System.EventArgs e)
        {
            tabControl.Pages.Clear();
        }

        private void TabControl_SelectedPageChanged(object sender, EventArgs e)
        {
            Application.Log($"SelectedPageChanged");
        }

        private void TabAlignmentComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            if ((string)tabAlignmentComboBox.SelectedItem! == "Top")
                tabControl.TabAlignment = TabAlignment.Top;
            if ((string)tabAlignmentComboBox.SelectedItem! == "Bottom")
                tabControl.TabAlignment = TabAlignment.Bottom;
        }
    }
}