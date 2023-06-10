using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class TabControlPage : Control
    {
        public TabControlPage()
        {
            InitializeComponent();

            foreach (var item in Enum.GetValues(typeof(TabAlignment)))
                tabAlignmentComboBox.Items.Add(item ?? throw new Exception());
            tabAlignmentComboBox.SelectedIndex = 0;
        }

        public IPageSite? Site { get; set; }

        private static string GetPageTitle(TabPage? page) =>
            page == null ? "<none>" : page.Title;

        private void ModifyFirstPageTitleButton_Click(object sender, System.EventArgs e)
        {
            if (!tabControl.Pages.Any())
                return;

            tabControl.Pages.First().Title += "X";
        }

        private void InsertLastPageSiblingButton_Click(object sender, System.EventArgs e)
        {
            if (!tabControl.Pages.Any())
                return;

            var lastPage = tabControl.Pages.Last();
            tabControl.Pages.Insert(lastPage.Index ?? throw new Exception(), new TabPage(lastPage.Title + " Sibling"));
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
            tabControl.Pages.Add(new TabPage("Page " + (tabControl.Pages.Count + 1)));
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
            tabControl.TabAlignment = (TabAlignment)tabAlignmentComboBox.SelectedItem!;
        }
    }
}