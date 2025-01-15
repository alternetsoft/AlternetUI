﻿using System;
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
        /// Creates platform control for the <see cref="Panel"/>.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreatePanelHandler(ContainerControl control);

        /// <summary>
        /// Creates <see cref="ICheckBoxHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateCheckBoxHandler(CheckBox control);

        /// <summary>
        /// Creates <see cref="IButtonHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateButtonHandler(Button control);

        /// <summary>
        /// Creates <see cref="IControlHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateColorPickerHandler(Control control);

        /// <summary>
        /// Creates <see cref="ISliderHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateSliderHandler(Slider control);

        /// <summary>
        /// Creates <see cref="IControlHandler"/> interface provider.
        /// for the <see cref="ProgressBar"/>.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateProgressBarHandler(ProgressBar control);

        /// <summary>
        /// Creates <see cref="IControlHandler"/> interface provider
        /// for the <see cref="GroupBox"/>.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateGroupBoxHandler(GroupBox control);

        /// <summary>
        /// Creates <see cref="IRadioButtonHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateRadioButtonHandler(RadioButton control);

        /// <summary>
        /// Creates <see cref="ITextBoxHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateTextBoxHandler(TextBox control);

        /// <summary>
        /// Creates <see cref="IControlHandler"/> interface provider.
        /// </summary>
        /// <param name="editor">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateRichTextBoxHandler(RichTextBox editor);

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
        /// Creates <see cref="IPropertyGridHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreatePropertyGridHandler(PropertyGrid control);

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
        IControlHandler CreateComboBoxHandler(ComboBox control);

        /// <summary>
        /// Creates <see cref="IControlHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateLabelHandler(Label control);

        /// <summary>
        /// Creates <see cref="IScrollBarHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateScrollBarHandler(ScrollBar control);

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
        /// Creates <see cref="IWindowHandler"/> interface provider.
        /// </summary>
        /// <param name="window">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateWindowHandler(Window window);

        /// <summary>
        /// Creates <see cref="ITreeViewHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateTreeViewHandler(TreeView control);

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
        IControlHandler CreateControlHandler(AbstractControl control);

        /// <summary>
        /// Creates <see cref="IVListBoxHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateVListBoxHandler(VirtualListBox control);

        /// <summary>
        /// Creates <see cref="ILinkLabelHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateLinkLabelHandler(LinkLabel control);

        /// <summary>
        /// Creates <see cref="IAnimationPlayerHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateAnimationPlayerHandler(AnimationPlayer control);

        /// <summary>
        /// Creates <see cref="ICalendarHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateCalendarHandler(Calendar control);

        /// <summary>
        /// Creates <see cref="IListViewHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateListViewHandler(ListView control);

        /// <summary>
        /// Creates <see cref="IDateTimePickerHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateDateTimePickerHandler(DateTimePicker control);

        /// <summary>
        /// Creates <see cref="IControlHandler"/> interface provider.
        /// </summary>
        /// <param name="control">Owner.</param>
        /// <returns></returns>
        IControlHandler CreateNumericUpDownHandler(NumericUpDown control);
    }
}
