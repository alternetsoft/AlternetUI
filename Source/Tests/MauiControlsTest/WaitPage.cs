namespace AllQuickStarts;

public class WaitPage : ContentPage
{
	public WaitPage()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Please wait..."
				}
			}
		};

    }
}