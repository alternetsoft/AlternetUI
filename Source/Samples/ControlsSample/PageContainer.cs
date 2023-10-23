using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    public delegate Control CreateControlAction();

    public class PageContainer : Control
    {
        private readonly TreeView pagesControl;
        private readonly Control activePageHolder = new()
        {
        };

        private readonly Grid grid;
        private readonly VerticalStackPanel waitLabelContainer = new()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Size = new Size(400, 400),
        };
        private readonly Label waitLabel = new()
        {
            Text = "Page is loading...",
            Margin = new Thickness(100, 100, 0, 0),
        };

        private readonly AnimationPlayer waitAnination = new()
        {
            Margin = new Thickness(100, 100, 0, 0),
        };

        public PageContainer()
        {
            waitAnination.LoadFromUrl(AnimationPage.AnimationHourGlass);

            grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            Children.Add(grid);

            pagesControl = new()
            {
                SuggestedWidth = 140,
            };
            pagesControl.MakeAsListBox();

            pagesControl.SelectionChanged += PagesListBox_SelectionChanged;
            grid.Children.Add(pagesControl);
            Grid.SetColumn(pagesControl, 0);

            grid.Children.Add(activePageHolder);
            Grid.SetColumn(activePageHolder, 1);

            waitLabel.Parent = waitLabelContainer;

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

        public TreeView PagesControl => pagesControl;

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
            if (SelectedIndex == null)
                return;

            activePageHolder.SuspendLayout();
            try
            {
                activePageHolder.GetVisibleChildOrNull()?.Hide();
                var page = Pages[SelectedIndex.Value];
                var loaded = page.ControlCreated;

                if (!loaded)
                {
                    waitLabelContainer.Parent = activePageHolder;
                    waitLabelContainer.Visible = true;
                    waitLabelContainer.Update();
                    Application.DoEvents();
                }

                var control = page.Control;
                control.Parent = activePageHolder;
                control.Visible = true;
                control.PerformLayout();
                waitLabelContainer.Visible = false;
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

            public bool ControlCreated => control != null;

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