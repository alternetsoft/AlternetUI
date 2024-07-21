using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which create controls of the different types.
    /// </summary>
    public interface IControlFactoryHandler : IDisposable
    {
        /// <summary>
        /// Creates <see cref="ICheckBoxHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        ICheckBoxHandler CreateCheckBoxHandler(CheckBox control);

        /// <summary>
        /// Creates <see cref="IButtonHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IButtonHandler CreateButtonHandler(Button control);

        /// <summary>
        /// Creates <see cref="IColorPickerHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IColorPickerHandler CreateColorPickerHandler(ColorPicker control);

        /// <summary>
        /// Creates <see cref="ISliderHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        ISliderHandler CreateSliderHandler(Slider control);

        /// <summary>
        /// Creates <see cref="IProgressBarHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IProgressBarHandler CreateProgressBarHandler(ProgressBar control);

        /// <summary>
        /// Creates <see cref="IGroupBoxHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IGroupBoxHandler CreateGroupBoxHandler(GroupBox control);

        /// <summary>
        /// Creates <see cref="IRadioButtonHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IRadioButtonHandler CreateRadioButtonHandler(RadioButton control);

        /// <summary>
        /// Creates <see cref="ITextBoxHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        ITextBoxHandler CreateTextBoxHandler(TextBox control);

        /// <summary>
        /// Creates <see cref="IRichTextBoxHandler"/> interface provider.
        /// </summary>
        /// <param name="editor">Owner.</param>
        /// <returns></returns>
        IRichTextBoxHandler CreateRichTextBoxHandler(RichTextBox editor);

        /// <summary>
        /// Creates <see cref="IPropertyGridVariant"/> interface provider.
        /// </summary>
        /// <returns></returns>
        IPropertyGridVariant CreateVariant();

        /// <summary>
        /// Creates <see cref="IPropertyGridChoices"/> interface provider.
        /// </summary>
        /// <returns></returns>
        IPropertyGridChoices CreateChoices();

        /// <summary>
        /// Creates <see cref="IValueValidatorText"/> interface provider.
        /// </summary>
        /// <returns></returns>
        /// <param name="style"></param>
        IValueValidatorText CreateValueValidatorText(ValueValidatorTextStyle style);

        /// <summary>
        /// Creates <see cref="IValueValidatorText"/> interface provider.
        /// </summary>
        /// <param name="numericType"></param>
        /// <param name="valueBase"></param>
        /// <returns></returns>
        IValueValidatorText CreateValueValidatorNum(
            ValueValidatorNumStyle numericType,
            int valueBase = 10);

        /// <summary>
        /// Creates <see cref="IPropertyGridHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IPropertyGridHandler CreatePropertyGridHandler(PropertyGrid control);

        /// <summary>
        /// Creates <see cref="IWebBrowserFactoryHandler"/> interface provider.
        /// </summary>
        /// <returns></returns>
        IWebBrowserFactoryHandler CreateWebBrowserFactoryHandler();

        /// <summary>
        /// Creates <see cref="IComboBoxHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IComboBoxHandler CreateComboBoxHandler(ComboBox control);

        /// <summary>
        /// Creates <see cref="ILabelHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        ILabelHandler CreateLabelHandler(Label control);

        /// <summary>
        /// Creates <see cref="IScrollBarHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IScrollBarHandler CreateScrollBarHandler(ScrollBar control);

        /// <summary>
        /// Creates <see cref="IMenuItemHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IMenuItemHandler CreateMenuItemHandler(MenuItem control);

        /// <summary>
        /// Creates <see cref="IContextMenuHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IContextMenuHandler CreateContextMenuHandler(ContextMenu control);

        /// <summary>
        /// Creates <see cref="IMainMenuHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IMainMenuHandler CreateMainMenuHandler(MainMenu control);

        /// <summary>
        /// Creates <see cref="IListBoxHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IListBoxHandler CreateListBoxHandler(ListBox control);

        /// <summary>
        /// Creates <see cref="IWindowHandler"/> interface provider.
        /// </summary>
        /// <param name="window">Owner.</param>
        /// <returns></returns>
        IWindowHandler CreateWindowHandler(Window window);

        /// <summary>
        /// Creates <see cref="IScrollViewerHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IScrollViewerHandler CreateScrollViewerHandler(ScrollViewer control);

        /// <summary>
        /// Creates <see cref="ITreeViewHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        ITreeViewHandler CreateTreeViewHandler(TreeView control);

        /// <summary>
        /// Creates <see cref="IStatusBarHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IStatusBarHandler CreateStatusBarHandler(StatusBar control);

        /// <summary>
        /// Creates <see cref="IControlHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateControlHandler(Control control);

        /// <summary>
        /// Creates <see cref="ICheckListBoxHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        ICheckListBoxHandler CreateCheckListBoxHandler(CheckListBox control);

        /// <summary>
        /// Creates <see cref="IVListBoxHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IVListBoxHandler CreateVListBoxHandler(VListBox control);

        /// <summary>
        /// Creates <see cref="ILinkLabelHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        ILinkLabelHandler CreateLinkLabelHandler(LinkLabel control);

        /// <summary>
        /// Creates <see cref="IAnimationPlayerHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IAnimationPlayerHandler CreateAnimationPlayerHandler(AnimationPlayer control);

        /// <summary>
        /// Creates <see cref="ICalendarHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        ICalendarHandler CreateCalendarHandler(Calendar control);

        /// <summary>
        /// Creates <see cref="IListViewHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IListViewHandler CreateListViewHandler(ListView control);

        /// <summary>
        /// Creates <see cref="IDateTimePickerHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IDateTimePickerHandler CreateDateTimePickerHandler(DateTimePicker control);

        /// <summary>
        /// Creates <see cref="INumericUpDownHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        INumericUpDownHandler CreateNumericUpDownHandler(NumericUpDown control);
    }
}
