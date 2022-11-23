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
            page1.Title += "X";
        }

        private void InsertLastPageSiblingButton_Click(object sender, System.EventArgs e)
        {
            if (!tabControl.Pages.Any())
                return;

            var lastPage = tabControl.Pages.Last();
            tabControl.Pages.Insert(lastPage.Index ?? throw new Exception(), new TabPage(lastPage.Title + " Sibling"));
        }
    }
}