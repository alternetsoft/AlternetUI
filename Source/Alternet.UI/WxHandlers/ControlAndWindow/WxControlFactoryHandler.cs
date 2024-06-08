using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxControlFactoryHandler : DisposableObject, IControlFactoryHandler
    {
        public IRichTextBoxHandler CreateRichTextBoxHandler(RichTextBox editor)
        {
            return new RichTextBoxHandler();
        }

        public IValueValidatorText CreateValueValidatorText(ValueValidatorTextStyle style)
        {
            return new ValueValidatorText(style);
        }

        public IValueValidatorText CreateValueValidatorNum(
            ValueValidatorNumStyle numericType,
            int valueBase = 10)
        {
            return new ValueValidatorNumProp(numericType, valueBase);
        }

        public IPropertyGridChoices CreateChoices()
        {
            return new PropertyGridChoices();
        }

        /// <inheritdoc/>
        public IPropertyGridHandler CreatePropertyGridHandler(PropertyGrid control)
        {
            return new PropertyGridHandler();
        }

        public IPropertyGridVariant CreateVariant()
        {
            return new PropertyGridVariant();
        }

        public IWebBrowserFactoryHandler CreateWebBrowserFactoryHandler()
        {
            return new WebBrowserFactoryHandler();
        }

        /// <inheritdoc/>
        public ICalendarHandler CreateCalendarHandler(Calendar control)
        {
            return new CalendarHandler();
        }

        /// <inheritdoc/>
        public IProgressBarHandler CreateProgressBarHandler(ProgressBar control)
        {
            return new ProgressBarHandler();
        }

        /// <inheritdoc/>
        public IRadioButtonHandler CreateRadioButtonHandler(RadioButton control)
        {
            return new WxRadioButtonHandler();
        }

        /// <inheritdoc/>
        public IWindowHandler CreateWindowHandler(Window window) => new WindowHandler();

        /// <inheritdoc/>
        public IListViewHandler CreateListViewHandler(ListView control)
        {
            return new ListViewHandler();
        }

        /// <inheritdoc/>
        public IDateTimePickerHandler CreateDateTimePickerHandler(DateTimePicker control)
        {
            return new DateTimePickerHandler();
        }

        /// <inheritdoc/>
        public INumericUpDownHandler CreateNumericUpDownHandler(NumericUpDown control)
        {
            return new NumericUpDownHandler();
        }

        /// <inheritdoc/>
        public ICheckBoxHandler CreateCheckBoxHandler(CheckBox control)
        {
            return new CheckBoxHandler();
        }

        /// <inheritdoc/>
        public IButtonHandler CreateButtonHandler(Button control)
        {
            return new WxButtonHandler();
        }

        /// <inheritdoc/>
        public ILinkLabelHandler CreateLinkLabelHandler(LinkLabel control)
        {
            Native.LinkLabel.UseGenericControl = LinkLabel.UseGenericControl;
            return new LinkLabelHandler();
        }

        /// <inheritdoc/>
        public IAnimationPlayerHandler CreateAnimationPlayerHandler(AnimationPlayer control)
        {
            switch (AnimationPlayer.DefaultHandlerKind)
            {
                case AnimationPlayer.KnownHandler.Native:
                    return new AnimationPlayerHandler(false);
                case AnimationPlayer.KnownHandler.Generic:
                default:
                    return new AnimationPlayerHandler(true);
                case AnimationPlayer.KnownHandler.WebBrowser:
                    throw new NotImplementedException(
                        "KnownDriver.WebBrowser is not currently supported.");
            }
        }

        /// <inheritdoc/>
        public ISliderHandler CreateSliderHandler(Slider control)
        {
            return new SliderHandler();
        }

        /// <inheritdoc/>
        public IColorPickerHandler CreateColorPickerHandler(ColorPicker control)
        {
            return new ColorPickerHandler();
        }

        /// <inheritdoc/>
        public IGroupBoxHandler CreateGroupBoxHandler(GroupBox control)
        {
            return new GroupBoxHandler();
        }

        /// <inheritdoc/>
        public ITextBoxHandler CreateTextBoxHandler(TextBox control)
        {
            return new TextBoxHandler();
        }

        /// <inheritdoc/>
        public IComboBoxHandler CreateComboBoxHandler(ComboBox control)
        {
            return new ComboBoxHandler();
        }

        /// <inheritdoc/>
        public ILabelHandler CreateLabelHandler(Label control)
        {
            return new LabelHandler();
        }

        /// <inheritdoc/>
        public IScrollBarHandler CreateScrollBarHandler(ScrollBar control)
        {
            return new WxScrollBarHandler();
        }

        /// <inheritdoc/>
        public IMenuItemHandler CreateMenuItemHandler(MenuItem control)
        {
            return new MenuItemHandler();
        }

        /// <inheritdoc/>
        public IContextMenuHandler CreateContextMenuHandler(ContextMenu control)
        {
            return new ContextMenuHandler();
        }

        /// <inheritdoc/>
        public IMainMenuHandler CreateMainMenuHandler(MainMenu control)
        {
            return new MainMenuHandler();
        }

        /// <inheritdoc/>
        public ITreeViewHandler CreateTreeViewHandler(TreeView control)
        {
            return new TreeViewHandler();
        }

        /// <inheritdoc/>
        public IScrollViewerHandler CreateScrollViewerHandler(ScrollViewer control)
        {
            return new ScrollViewerHandler();
        }

        /// <inheritdoc/>
        public IVListBoxHandler CreateVListBoxHandler(VListBox control)
        {
            return new VListBoxHandler();
        }

        /// <inheritdoc/>
        public ICheckListBoxHandler CreateCheckListBoxHandler(CheckListBox control)
        {
            return new CheckListBoxHandler();
        }

        /// <inheritdoc/>
        public IListBoxHandler CreateListBoxHandler(ListBox control)
        {
            return new ListBoxHandler();
        }

        /// <inheritdoc/>
        public IStatusBarHandler CreateStatusBarHandler(StatusBar control)
        {
            return new StatusBarHandler(control);
        }

        /// <inheritdoc/>
        public IControlHandler CreateControlHandler(Control control)
        {
            return new WxControlHandler();
        }
    }
}
