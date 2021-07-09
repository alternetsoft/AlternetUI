namespace Alternet.UI
{
    public class NativeControlHandlerFactory : IControlHandlerFactory
    {
        public ControlHandler CreateControlHandler(Control control) => control switch
        {
            Button => new NativeButtonHandler(),
            StackPanel => new StackPanelHandler(),
            Border => new GenericBorderHandler(),
            Label => new GenericLabelHandler(),
            TextBox => new NativeTextBoxHandler(),
            CheckBox => new NativeCheckBoxHandler(),
            RadioButton => new NativeRadioButtonHandler(),
            TabControl => new NativeTabControlHandler(),
            GroupBox => new NativeGroupBoxHandler(),
            ProgressBar => new NativeProgressBarHandler(),
            Slider => new NativeSliderHandler(),
            NumericUpDown => new NativeNumericUpDownHandler(),
            ListBox => new NativeListBoxHandler(),
            ComboBox => new NativeComboBoxHandler(),
            _ => new GenericControlHandler()
        };
    }
}