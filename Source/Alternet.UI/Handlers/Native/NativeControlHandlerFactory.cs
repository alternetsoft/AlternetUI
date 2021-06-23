namespace Alternet.UI
{
    public class NativeControlHandlerFactory : IControlHandlerFactory
    {
        public ControlHandler CreateControlHandler(Control control) => control switch
        {
            Button => new NativeButtonHandler(),
            StackPanel => new StackPanelHandler(),
            Border => new GenericBorderHandler(),
            TextBlock => new GenericTextBlockHandler(),
            TextBox => new NativeTextBoxHandler(),
            CheckBox => new NativeCheckBoxHandler(),
            RadioButton => new NativeRadioButtonHandler(),
            TabControl => new NativeTabControlHandler(),
            GroupBox => new NativeGroupBoxHandler(),
            ProgressBar => new NativeProgressBarHandler(),
            Slider => new NativeSliderHandler(),
            NumericUpDown => new NativeNumericUpDownHandler(),
            _ => new GenericControlHandler()
        };
    }
}