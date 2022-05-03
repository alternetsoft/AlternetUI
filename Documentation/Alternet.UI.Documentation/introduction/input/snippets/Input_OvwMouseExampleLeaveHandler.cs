private void OnMosueExampleMouseLeave(object sender, MouseEventArgs e)
{
    // Cast the source of the event to a Button.
    Button source = e.Source as Button;

    // If source is a Button.
    if (source != null)
    {
        source.Background = Brushes.AliceBlue;
    }
}