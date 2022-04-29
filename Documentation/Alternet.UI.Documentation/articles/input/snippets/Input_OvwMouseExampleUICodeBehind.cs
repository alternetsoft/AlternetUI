// Create the UI elements.
StackPanel mouseMoveStackPanel = new StackPanel();
Button mouseMoveButton = new Button();

// Set properties on Button.
mouseMoveButton.Background = Brushes.AliceBlue;
mouseMoveButton.Text = "Button";

// Attach Buttons to StackPanel.
mouseMoveStackPanel.Children.Add(mouseMoveButton);

// Attach event handler.
mouseMoveButton.MouseEnter += new MouseEventHandler(OnMouseExampleMouseEnter);
mouseMoveButton.MouseLeave += new MouseEventHandler(OnMosueExampleMouseLeave);