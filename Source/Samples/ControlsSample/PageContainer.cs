using Alternet.Base.Collections;
using Alternet.UI;


namespace ControlsSample
{
    internal class PageContainer : Control
    {
        ListBox pagesListBox;
        Control activePageHolder;
        Grid grid;

        public PageContainer()
        {
            grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Children.Add(grid);

            pagesListBox = new ListBox();
            pagesListBox.SelectionChanged += PagesListBox_SelectionChanged;
            grid.Children.Add(pagesListBox);
            Grid.SetColumn(pagesListBox, 0);

            activePageHolder = new Control();
            grid.Children.Add(activePageHolder);
            Grid.SetColumn(activePageHolder, 1);

            Pages.ItemInserted += Pages_ItemInserted;
        }

        private void PagesListBox_SelectionChanged(object? sender, System.EventArgs e)
        {
            SetActivePageControl();
        }

        private void Pages_ItemInserted(object? sender, CollectionChangeEventArgs<Page> e)
        {
            pagesListBox.Items.Insert(e.Index, e.Item.Title);
        }

        public int? SelectedIndex
        {
            get => pagesListBox.SelectedIndex;
            set
            {
                if (pagesListBox.SelectedIndex == value)
                    return;

                pagesListBox.SelectedIndex = value;
                SetActivePageControl();
            }
        }

        private void SetActivePageControl()
        {
            activePageHolder.SuspendLayout();
            activePageHolder.Children.Clear();
            
            var selectedIndex = pagesListBox.SelectedIndex;
            if (selectedIndex != null)
                activePageHolder.Children.Add(Pages[selectedIndex.Value].Control);
            activePageHolder.ResumeLayout();
        }

        public class Page
        {
            public Page(string title, Control control)
            {
                Title = title;
                Control = control;
            }

            public string Title { get; }
            public Control Control { get; }
        }

        public Collection<Page> Pages { get; } = new Collection<Page>();
    }
}