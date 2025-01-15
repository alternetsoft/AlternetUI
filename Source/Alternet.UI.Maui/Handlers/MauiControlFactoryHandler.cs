using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal partial class MauiControlFactoryHandler : DisposableObject, IControlFactoryHandler
    {
        public IControlHandler CreatePanelHandler(ContainerControl control)
        {
            return new MauiControlHandler();
        }

        IControlHandler IControlFactoryHandler.CreateAnimationPlayerHandler(AnimationPlayer control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateButtonHandler(Button control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateCalendarHandler(Calendar control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateCheckBoxHandler(CheckBox control)
        {
            throw new NotImplementedException();
        }

        IPropertyGridChoices IControlFactoryHandler.CreateChoices()
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateColorPickerHandler(Control control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateComboBoxHandler(ComboBox control)
        {
            return new MauiComboBoxHandler();
        }

        IContextMenuHandler IControlFactoryHandler.CreateContextMenuHandler(ContextMenu control)
        {
            return new PlessContextMenuHandler();
        }

        IControlHandler IControlFactoryHandler.CreateControlHandler(AbstractControl control)
        {
            return new MauiControlHandler();
        }

        IControlHandler IControlFactoryHandler.CreateDateTimePickerHandler(DateTimePicker control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateGroupBoxHandler(GroupBox control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateLabelHandler(Label control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateLinkLabelHandler(LinkLabel control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateListViewHandler(ListView control)
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

        IControlHandler IControlFactoryHandler.CreateNumericUpDownHandler(NumericUpDown control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateProgressBarHandler(ProgressBar control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreatePropertyGridHandler(PropertyGrid control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateRadioButtonHandler(RadioButton control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateRichTextBoxHandler(RichTextBox editor)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateScrollBarHandler(ScrollBar control)
        {
            return new PlessScrollBarHandler();
        }

        IControlHandler IControlFactoryHandler.CreateSliderHandler(Slider control)
        {
            throw new NotImplementedException();
        }

        IStatusBarHandler IControlFactoryHandler.CreateStatusBarHandler(StatusBar control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateTextBoxHandler(TextBox control)
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateTreeViewHandler(TreeView control)
        {
            throw new NotImplementedException();
        }

        IPropertyGridVariant IControlFactoryHandler.CreateVariant()
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateVListBoxHandler(VirtualListBox control)
        {
            throw new NotImplementedException();
        }

        IWebBrowserFactoryHandler IControlFactoryHandler.CreateWebBrowserFactoryHandler()
        {
            return new MauiWebBrowserFactoryHandler();
        }

        IControlHandler IControlFactoryHandler.CreateWindowHandler(Window window)
        {
            return new MauiWindowHandler();
        }
    }
}
