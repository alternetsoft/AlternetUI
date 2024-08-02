private void OnTextInputKeyDown(object sender, KeyEventArgs e)
{
    if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
    {
        Handle();
        e.Handled = true;
    }
}

private void OnTextInputButtonClick(object sender, EventArgs e)
{
    Handle();
    e.Handled = true;
}

public void Handle()
{
    MessageBox.Show("Pretend this opens a file");
}