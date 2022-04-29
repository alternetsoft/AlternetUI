void MakeButton()
{
    Button b2 = new Button();
    b2.AddHandler(UIElement.MouseMoveEvent, new RoutedEventHandler(Onb2MouseMove));
}

void Onb2MouseMove(object sender, RoutedEventArgs e)
{
    // logic to handle the MouseMove event
}