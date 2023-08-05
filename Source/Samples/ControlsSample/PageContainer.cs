using Alternet.Base.Collections;
using Alternet.UI;

namespace ControlsSample
{
    public class PageContainer : Control
    {
        private readonly TreeView pagesControl;
        private readonly Control activePageHolder;
        private readonly Grid grid;

        public PageContainer()
        {
            grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            Children.Add(grid);

            pagesControl = new()
            {
                Width = 140,
            };
            pagesControl.MakeAsListBox();

            pagesControl.SelectionChanged += PagesListBox_SelectionChanged;
            grid.Children.Add(pagesControl);
            Grid.SetColumn(pagesControl, 0);

            activePageHolder = new Control();
            grid.Children.Add(activePageHolder);
            Grid.SetColumn(activePageHolder, 1);

            Pages.ItemInserted += Pages_ItemInserted;
        }

        public int? SelectedIndex
        {
            get => pagesControl?.SelectedItem?.Index;
            set
            {
                pagesControl.SelectedItem = pagesControl.Items[(int)value!];
                SetActivePageControl();
            }
        }

        public Collection<Page> Pages { get; } = new Collection<Page>();

        private void PagesListBox_SelectionChanged(object? sender, System.EventArgs e)
        {
            SetActivePageControl();
        }

        private void Pages_ItemInserted(object? sender, CollectionChangeEventArgs<Page> e)
        {
            pagesControl.Items.Insert(e.Index, new TreeViewItem(e.Item.Title));
        }

        private void SetActivePageControl()
        {
            activePageHolder.SuspendLayout();
            activePageHolder.Children.Clear();

            if (SelectedIndex != null)
                activePageHolder.Children.Add(Pages[SelectedIndex.Value].Control);
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
    }
}