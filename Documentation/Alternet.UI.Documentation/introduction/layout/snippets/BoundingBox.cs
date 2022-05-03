private void ShowBoundingBoxButton_Click(object? sender, EventArgs e)
{
    border1.BorderBrush = Brushes.Orange;
    txt2.Text = border1.Bounds.ToString();
}