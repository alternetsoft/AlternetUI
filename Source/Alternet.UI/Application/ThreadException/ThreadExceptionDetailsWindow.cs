namespace Alternet.UI
{
    internal class ThreadExceptionDetailsWindow : Window
    {
        public ThreadExceptionDetailsWindow(string details)
        {
            InitializeControls(details);
        }

        private void InitializeControls(string details)
        {
            // Cannot use UIXML in the Alternet.UI assembly itself, so populate the controls from code.

            BeginInit();
            Title = "Exception Details";
            Padding = new Thickness(10);
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            StartLocation = WindowStartLocation.CenterOwner;
            AlwaysOnTop = true;

            var mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var detailsTextBox = new TextBox
            {
                Text = details,
                ReadOnly = true,
                Multiline = true
            };
            mainGrid.Children.Add(detailsTextBox);

            var closeButton = new Button
            {
                Text = "&Close",
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };
            closeButton.Click += (o, e) => Close();
            mainGrid.Children.Add(closeButton);
            Grid.SetRow(closeButton, 1);

            Children.Add(mainGrid);
            EndInit();
        }
    }
}