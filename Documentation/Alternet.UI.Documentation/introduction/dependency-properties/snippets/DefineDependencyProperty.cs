public static readonly DependencyProperty IsSpinningProperty = DependencyProperty.Register(
    "IsSpinning", typeof(bool),
    typeof(MainWindow)
    );

public bool IsSpinning
{
    get => (bool)GetValue(IsSpinningProperty);
    set => SetValue(IsSpinningProperty, value);
}