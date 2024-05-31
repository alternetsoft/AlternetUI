using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class MauiControlFactoryHandler : DisposableObject, IControlFactoryHandler
    {
        IAnimationPlayerHandler IControlFactoryHandler.CreateAnimationPlayerHandler(AnimationPlayer control)
        {
            throw new NotImplementedException();
        }

        IButtonHandler IControlFactoryHandler.CreateButtonHandler(Button control)
        {
            throw new NotImplementedException();
        }

        ICalendarHandler IControlFactoryHandler.CreateCalendarHandler(Calendar control)
        {
            throw new NotImplementedException();
        }

        ICheckBoxHandler IControlFactoryHandler.CreateCheckBoxHandler(CheckBox control)
        {
            throw new NotImplementedException();
        }

        ICheckListBoxHandler IControlFactoryHandler.CreateCheckListBoxHandler(CheckListBox control)
        {
            throw new NotImplementedException();
        }

        IPropertyGridChoices IControlFactoryHandler.CreateChoices()
        {
            throw new NotImplementedException();
        }

        IColorPickerHandler IControlFactoryHandler.CreateColorPickerHandler(ColorPicker control)
        {
            throw new NotImplementedException();
        }

        IComboBoxHandler IControlFactoryHandler.CreateComboBoxHandler(ComboBox control)
        {
            throw new NotImplementedException();
        }

        IContextMenuHandler IControlFactoryHandler.CreateContextMenuHandler(ContextMenu control)
        {
            return new PlessContextMenuHandler();
        }

        IControlHandler IControlFactoryHandler.CreateControlHandler(Control control)
        {
            return new MauiControlHandler();
        }

        IDateTimePickerHandler IControlFactoryHandler.CreateDateTimePickerHandler(DateTimePicker control)
        {
            throw new NotImplementedException();
        }

        IGroupBoxHandler IControlFactoryHandler.CreateGroupBoxHandler(GroupBox control)
        {
            throw new NotImplementedException();
        }

        ILabelHandler IControlFactoryHandler.CreateLabelHandler(Label control)
        {
            throw new NotImplementedException();
        }

        ILinkLabelHandler IControlFactoryHandler.CreateLinkLabelHandler(LinkLabel control)
        {
            throw new NotImplementedException();
        }

        IListBoxHandler IControlFactoryHandler.CreateListBoxHandler(ListBox control)
        {
            throw new NotImplementedException();
        }

        IListViewHandler IControlFactoryHandler.CreateListViewHandler(ListView control)
        {
            throw new NotImplementedException();
        }

        IMainMenuHandler IControlFactoryHandler.CreateMainMenuHandler(MainMenu control)
        {
            return new PlessMainMenuHandler();
        }

        IMenuItemHandler IControlFactoryHandler.CreateMenuItemHandler(MenuItem control)
        {
            return new PlessMenuItemHandler();
        }

        INumericUpDownHandler IControlFactoryHandler.CreateNumericUpDownHandler(NumericUpDown control)
        {
            throw new NotImplementedException();
        }

        IProgressBarHandler IControlFactoryHandler.CreateProgressBarHandler(ProgressBar control)
        {
            throw new NotImplementedException();
        }

        IPropertyGridHandler IControlFactoryHandler.CreatePropertyGridHandler(PropertyGrid control)
        {
            throw new NotImplementedException();
        }

        IRadioButtonHandler IControlFactoryHandler.CreateRadioButtonHandler(RadioButton control)
        {
            throw new NotImplementedException();
        }

        IRichTextBoxHandler IControlFactoryHandler.CreateRichTextBoxHandler(RichTextBox editor)
        {
            throw new NotImplementedException();
        }

        IScrollBarHandler IControlFactoryHandler.CreateScrollBarHandler(ScrollBar control)
        {
            throw new NotImplementedException();
        }

        IScrollViewerHandler IControlFactoryHandler.CreateScrollViewerHandler(ScrollViewer control)
        {
            throw new NotImplementedException();
        }

        ISliderHandler IControlFactoryHandler.CreateSliderHandler(Slider control)
        {
            throw new NotImplementedException();
        }

        IStatusBarHandler IControlFactoryHandler.CreateStatusBarHandler(StatusBar control)
        {
            throw new NotImplementedException();
        }

        ITextBoxHandler IControlFactoryHandler.CreateTextBoxHandler(TextBox control)
        {
            throw new NotImplementedException();
        }

        ITreeViewHandler IControlFactoryHandler.CreateTreeViewHandler(TreeView control)
        {
            throw new NotImplementedException();
        }

        IValueValidatorText IControlFactoryHandler.CreateValueValidatorNum(ValueValidatorNumStyle numericType, int valueBase)
        {
            throw new NotImplementedException();
        }

        IValueValidatorText IControlFactoryHandler.CreateValueValidatorText(ValueValidatorTextStyle style)
        {
            throw new NotImplementedException();
        }

        IPropertyGridVariant IControlFactoryHandler.CreateVariant()
        {
            throw new NotImplementedException();
        }

        IVListBoxHandler IControlFactoryHandler.CreateVListBoxHandler(VListBox control)
        {
            throw new NotImplementedException();
        }

        IWebBrowserFactoryHandler IControlFactoryHandler.CreateWebBrowserFactoryHandler()
        {
            throw new NotImplementedException();
        }

        IWindowHandler IControlFactoryHandler.CreateWindowHandler(Window window)
        {
            return new MauiWindowHandler();
        }
    }
}
