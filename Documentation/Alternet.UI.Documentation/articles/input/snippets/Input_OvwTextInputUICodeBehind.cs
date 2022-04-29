// Create the UI elements.
StackPanel textInputStackPanel = new StackPanel();
Button textInputeButton = new Button();
TextBox textInputTextBox = new TextBox();
textInputeButton.Content = "Open";

// Attach elements to StackPanel.
textInputStackPanel.Children.Add(textInputeButton);
textInputStackPanel.Children.Add(textInputTextBox);

// Attach event handlers.
textInputStackPanel.KeyDown += new KeyEventHandler(OnTextInputKeyDown);
textInputeButton.Click += new RoutedEventHandler(OnTextInputButtonClick);