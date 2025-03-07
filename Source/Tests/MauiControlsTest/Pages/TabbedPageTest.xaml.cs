namespace AllQuickStarts
{
    public partial class TabbedPageTest : ContentPage
    {
        public TabbedPageTest()
        {
            InitializeComponent();
            UpdateTabSelection(Tab1Button);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnTabClicked;

            Tab1Button.GestureRecognizers.Add(tapGestureRecognizer);
            Tab2Button.GestureRecognizers.Add(tapGestureRecognizer);
        }

        private void OnTabClicked(object? sender, EventArgs e)
        {
            if (sender is Label button)
            {
                UpdateTabSelection(button);

                if (button == Tab1Button)
                {
                    Tab1Content.IsVisible = true;
                    Tab2Content.IsVisible = false;
                }
                else if (button == Tab2Button)
                {
                    Tab1Content.IsVisible = false;
                    Tab2Content.IsVisible = true;
                }
            }
        }

        private void UpdateTabSelection(Label selectedButton)
        {
            if(Tab1Button != selectedButton)
            {
                Tab1Button.FontAttributes = FontAttributes.None;
                Tab1ButtonUnderline.Color = Colors.Transparent;
            }
            else
            {
                Tab1ButtonUnderline.Color = Colors.Gray;
            }

            if (Tab2Button != selectedButton)
            {
                Tab2Button.FontAttributes = FontAttributes.None;
                Tab2ButtonUnderline.Color = Colors.Transparent;
            }
            else
            {
                Tab2ButtonUnderline.Color = Colors.Gray;
            }

            selectedButton.FontAttributes = FontAttributes.Bold;
        }
    }

}
