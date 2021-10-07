using HelloWorldSample;

internal partial class Window
{
    private void GenericLightButton_Click(object? sender, EventArgs e)
    {
        Application.Current.VisualTheme = StockVisualThemes.GenericLight;
        MessageBox.Show("Hello", "Light Theme");
    }
}