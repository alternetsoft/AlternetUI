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
            Padding = new Thickness(10);
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            StartLocation = WindowStartLocation.CenterOwner;
            AlwaysOnTop = true;

            var detailsTextBox = new TextBox
            {
                Text = details,
                ReadOnly = true,
                Multiline = true
            };
            Children.Add(detailsTextBox);
            EndInit();
        }
    }
}