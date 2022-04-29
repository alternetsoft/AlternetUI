private void OnTextInputKeyDown(object sender, KeyEventArgs e)
{
    if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
    {
        handle();
        e.Handled = true;
    }
}

private void OnTextInputButtonClick(object sender, RoutedEventArgs e)
{
    handle();
    e.Handled = true;
}

public void handle()
{
    MessageBox.Show("Pretend this opens a file");
}