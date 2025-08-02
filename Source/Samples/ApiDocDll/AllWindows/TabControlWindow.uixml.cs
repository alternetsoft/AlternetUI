using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class TabControlWindow : Window
    {
        public TabControlWindow()
        {
            InitializeComponent();
            MinWidth = 600;
            TabControlExample1();
            SetSizeToContent();
        }

        public void TabControlExample1()
        {
            #region TabControlCSharpCreation
            var tc = new Alternet.UI.TabControl();
            tc.SuggestedHeight = 300;
            tc.Margin = 10;

            var page1 = new TabPage
            {
                Title = "Page 1",
            };

            var content1 = new StdListBox
            {
                HasBorder = false,
                Items = new string[] { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" },
            };

            content1.Parent = page1;

            tc.Pages.Add(page1);
            tc.Pages.Add(new TabPage { Title = "Page 2" });
            tc.Pages.Add(new TabPage { Title = "Page 3" });
            tc.Pages.Add(new TabPage { Title = "Page 4" });
            tc.Pages.Add(new TabPage { Title = "Page 5" });
            tc.Parent = mainPanel;

            tc.SelectedPageChanged += (s, e) =>
            {
                Title = $"Selected page: {tc.SelectedPage?.Title}";
            };

            #endregion
        }
    }
}