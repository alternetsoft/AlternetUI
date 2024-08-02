// Create the UI elements.
StackPanel textInputStackPanel = new StackPanel();
Button textInputButton = new Button();
TextBox textInputTextBox = new TextBox();
textInputButton.Text = "Open";

// Attach elements to StackPanel.
textInputStackPanel.Children.Add(textInputeButton);
textInputStackPanel.Children.Add(textInputTextBox);

// Attach event handlers.
textInputStackPanel.KeyDown += new KeyEventHandler(OnTextInputKeyDown);
textInputButton.Click += new RoutedEventHandler(OnTextInputButtonClick);