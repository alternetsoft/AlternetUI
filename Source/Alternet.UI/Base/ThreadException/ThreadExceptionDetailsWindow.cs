using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class ThreadExceptionDetailsWindow : DialogWindow
    {
        public ThreadExceptionDetailsWindow(string details)
        {
            InitializeControls(details);
        }

        private void InitializeControls(string details)
        {
            BeginInit();
            Title = CommonStrings.Default.WindowTitleExceptionDetails;
            Padding = new Thickness(10);
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            StartLocation = WindowStartLocation.CenterOwner;
            TopMost = true;

            var mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var detailsTextBox = new TextBox
            {
                Text = details,
                ReadOnly = true,
                Multiline = true,
            };
            mainGrid.Children.Add(detailsTextBox);

            var closeButton = new Button
            {
                Text = CommonStrings.Default.ButtonClose,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0),
            };
            closeButton.Click += (o, e) => Close();
            mainGrid.Children.Add(closeButton);
            Grid.SetRow(closeButton, 1);

            Children.Add(mainGrid);
            EndInit();
        }
    }
}