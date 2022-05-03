// Uses the Keyboard.GetKeyStates to determine if a key is down.
// A bitwise AND operation is used in the comparison.
// e is an instance of KeyEventArgs.
if ((Keyboard.GetKeyStates(Key.Enter) & KeyStates.Down) > 0)
{
    btnNone.Background = Brushes.Red;
}