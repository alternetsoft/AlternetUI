using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class TabControlPage : Control
    {
        public TabControlPage()
        {
            InitializeComponent();
        }

        public IPageSite? Site { get; set; }

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
            string GetPageTitle(TabPage? page) => page == null ? "<none>" : page.Title;
            Site?.LogEvent($"SelectedPageChanged; Old: {GetPageTitle(e.OldValue)}, New: {GetPageTitle(e.NewValue)}");
        }
    }
}