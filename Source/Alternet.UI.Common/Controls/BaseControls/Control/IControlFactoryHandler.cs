using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IControlFactoryHandler : IDisposable
    {
        ICheckBoxHandler CreateCheckBoxHandler(CheckBox control);

        IButtonHandler CreateButtonHandler(Button control);

        IColorPickerHandler CreateColorPickerHandler(ColorPicker control);

        ISliderHandler CreateSliderHandler(Slider control);

        IProgressBarHandler CreateProgressBarHandler(ProgressBar control);

        IGroupBoxHandler CreateGroupBoxHandler(GroupBox control);

        IRadioButtonHandler CreateRadioButtonHandler(RadioButton control);

        ITextBoxHandler CreateTextBoxHandler(TextBox control);

        IRichTextBoxHandler CreateRichTextBoxHandler(RichTextBox editor);

        IPropertyGridVariant CreateVariant();

        IPropertyGridChoices CreateChoices();

        IValueValidatorText CreateValueValidatorText(ValueValidatorTextStyle style);

        IValueValidatorText CreateValueValidatorNum(
            ValueValidatorNumStyle numericType,
            int valueBase = 10);

        IPropertyGridHandler CreatePropertyGridHandler(PropertyGrid control);

        IWebBrowserFactoryHandler CreateWebBrowserFactoryHandler();

        IComboBoxHandler CreateComboBoxHandler(ComboBox control);

        ILabelHandler CreateLabelHandler(Label control);

        IScrollBarHandler CreateScrollBarHandler(ScrollBar control);

        IMenuItemHandler CreateMenuItemHandler(MenuItem control);

        IContextMenuHandler CreateContextMenuHandler(ContextMenu control);

        IMainMenuHandler CreateMainMenuHandler(MainMenu control);

        IListBoxHandler CreateListBoxHandler(ListBox control);

        IWindowHandler CreateWindowHandler(Window window);

        IScrollViewerHandler CreateScrollViewerHandler(ScrollViewer control);

        ITreeViewHandler CreateTreeViewHandler(TreeView control);

        IStatusBarHandler CreateStatusBarHandler(StatusBar control);

        IControlHandler CreateControlHandler(Control control);

        ICheckListBoxHandler CreateCheckListBoxHandler(CheckListBox control);

        IVListBoxHandler CreateVListBoxHandler(VListBox control);

        ILinkLabelHandler CreateLinkLabelHandler(LinkLabel control);

        IAnimationPlayerHandler CreateAnimationPlayerHandler(AnimationPlayer control);

        ICalendarHandler CreateCalendarHandler(Calendar control);

        IListViewHandler CreateListViewHandler(ListView control);

        IDateTimePickerHandler CreateDateTimePickerHandler(DateTimePicker control);

        INumericUpDownHandler CreateNumericUpDownHandler(NumericUpDown control);
    }
}
