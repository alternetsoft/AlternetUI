using Alternet.UI;

namespace CustomControlsSample
{
    public class CustomColorPicker : ColorPicker
    {
        protected override ControlHandler CreateHandler()
        {
            return new CustomColorPickerHandler();
        }
    }
}