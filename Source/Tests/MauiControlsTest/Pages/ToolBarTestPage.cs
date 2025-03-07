using AllQuickStarts.Pages;

namespace AllQuickStarts;

using Microsoft.Maui.Controls;

public partial class ToolBarTestPage : ContentPage
{
    public ToolBarTestPage()
    {
        var stackLayout = new HorizontalStackLayout();

        var searchButton = new Button
        {
            Text = "Search",
            Margin = 5,
        };

        var settingsButton = new Button
        {
            Text = "Settings",
            Margin = 5,
        };

        var zxButton = new Button
        {
            Text = "zx",
            Margin = 5,
        };

        stackLayout.Children.Add(settingsButton);
        stackLayout.Children.Add(searchButton);
        stackLayout.Children.Add(zxButton);

        AddVisualStates(searchButton);
        AddVisualStates(settingsButton);

        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
        grid.Children.Add(stackLayout);

        // Add the main content
        var mainContent = new StackLayout
        {
            Children =
                {
                    new Label
                    {
                        Text = "Welcome to .NET MAUI!",
                        FontSize = 24
                    }
                }
        };

        grid.SetRow(mainContent, 1);
        grid.Children.Add(mainContent);

        VisualStateManager.GoToState(searchButton, "Normal");
        VisualStateManager.GoToState(settingsButton, "Normal");
        searchButton.InvalidateMeasure();
        settingsButton.InvalidateMeasure();

        // Set the content of the page
        Content = grid;

    }

    private void AddVisualStates(Button button)
    {
        var visualStateGroup = new VisualStateGroup { Name = "CommonStates" };

        var normalState = new VisualState { Name = "Normal" };
        normalState.Setters.Add(new Setter { Property = Button.BackgroundColorProperty, Value = Colors.Transparent });
        normalState.Setters.Add(new Setter { Property = Button.BorderColorProperty, Value = Colors.Transparent });
        normalState.Setters.Add(new Setter { Property = Button.TextColorProperty, Value = Colors.Gray });
        normalState.Setters.Add(new Setter { Property = Button.BorderWidthProperty, Value = 1 });

        var pointerOverState = new VisualState { Name = "PointerOver" };
        pointerOverState.Setters.Add(new Setter { Property = Button.BackgroundColorProperty, Value = Colors.Transparent });
        pointerOverState.Setters.Add(new Setter { Property = Button.BorderColorProperty, Value = Colors.Gray });
        pointerOverState.Setters.Add(new Setter { Property = Button.TextColorProperty, Value = Colors.Gray });
        pointerOverState.Setters.Add(new Setter { Property = Button.BorderWidthProperty, Value = 1 });

        var pressedState = new VisualState { Name = "Pressed" };
        pressedState.Setters.Add(new Setter { Property = Button.BackgroundColorProperty, Value = Colors.Transparent });
        pressedState.Setters.Add(new Setter { Property = Button.BorderColorProperty, Value = Colors.DarkGray });
        pressedState.Setters.Add(new Setter { Property = Button.TextColorProperty, Value = Colors.Gray });
        pressedState.Setters.Add(new Setter { Property = Button.BorderWidthProperty, Value = 1 });

        visualStateGroup.States.Add(normalState);
        visualStateGroup.States.Add(pointerOverState);
        visualStateGroup.States.Add(pressedState);

        var vsGroups = VisualStateManager.GetVisualStateGroups(button);
        vsGroups.Clear();
        vsGroups.Add(visualStateGroup);
    }
}
