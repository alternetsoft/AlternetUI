using System;
using System.Linq;
using System.Reflection;
using Alternet.UI;
using static System.Net.Mime.MediaTypeNames;

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
        }

        public IPageSite? Site { get; set; }

        private static string GetPageTitle(TabPage? page) =>
            page == null ? "<none>" : page.Title;

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

        private void TabControl_SelectedPageChanged(object sender, SelectedTabPageChangedEventArgs e)
        {
            Site?.LogEvent($"SelectedPageChanged; Old: {GetPageTitle(e.OldValue)}, New: {GetPageTitle(e.NewValue)}");
        }

        private void TabAlignmentComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            if ((string)tabAlignmentComboBox.SelectedItem! == "Top")
                tabControl.TabAlignment = TabAlignment.Top;
            if ((string)tabAlignmentComboBox.SelectedItem! == "Bottom")
                tabControl.TabAlignment = TabAlignment.Bottom;

            /*tabControl.TabAlignment = (TabAlignment)tabAlignmentComboBox.SelectedItem!;*/
        }
    }
}