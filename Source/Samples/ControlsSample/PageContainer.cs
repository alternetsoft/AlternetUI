using Alternet.Base.Collections;
using Alternet.UI;

namespace ControlsSample
{
    public delegate Control CreateControlAction();

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

        private void Pages_ItemInserted(object? sender, int index, Page item)
        {
            pagesControl.Items.Insert(index, new TreeViewItem(item.Title));
        }

        private void SetActivePageControl()
        {
            activePageHolder.SuspendLayout();
            try
            {
                activePageHolder.Children.Clear();

                if (SelectedIndex != null)
                    activePageHolder.Children.Add(Pages[SelectedIndex.Value].Control);
            }
            finally
            {
                activePageHolder.ResumeLayout();
            }
        }

        public class Page
        {
            private readonly CreateControlAction action;
            private Control? control;

            public Page(string title, CreateControlAction action)
            {
                Title = title;
                this.action = action;
            }

            public string Title { get; }

            public Control Control
            {
                get
                {
                    control ??= action();
                    return control;
                }
            }
        }
    }
}