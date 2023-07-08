using Alternet.UI;

namespace ControlsTest
{
    public partial class EmptyWindow : Window
    {
        public EmptyWindow()
        {
            InitializeComponent();

            var addControls = true;

            if (addControls)
            {
                StackPanel stackPanel = new ();
                Children.Add(stackPanel);

                Label newLabel = new ()
                {
                    Text = "Hello",
                };
                stackPanel.Children.Add(newLabel);

                Button newButton = new ()
                {
                    Text = "Hello Button",
                };
                stackPanel.Children.Add(newButton);
            }

            this.VisibleChanged += EmptyWindow_VisibleChanged;
        }

        private void EmptyWindow_VisibleChanged(object? sender, System.EventArgs e)
        {
        }
    }
}
