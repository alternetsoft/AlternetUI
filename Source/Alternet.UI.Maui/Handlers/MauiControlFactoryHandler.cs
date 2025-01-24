﻿using System;
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
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateAnimationPlayerHandler(AnimationPlayer control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateButtonHandler(Button control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateCalendarHandler(Calendar control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateCheckBoxHandler(CheckBox control)
        {
            return new HandlerForDisposed();
        }

        IPropertyGridChoices IControlFactoryHandler.CreateChoices()
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateColorPickerHandler(Control control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateComboBoxHandler(ComboBox control)
        {
            return new HandlerForDisposed();
        }

        IContextMenuHandler IControlFactoryHandler.CreateContextMenuHandler(ContextMenu control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateControlHandler(AbstractControl control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateDateTimePickerHandler(DateTimePicker control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateGroupBoxHandler(GroupBox control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateLabelHandler(Label control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateLinkLabelHandler(LinkLabel control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateListViewHandler(ListView control)
        {
            return new HandlerForDisposed();
        }

        IMainMenuHandler IControlFactoryHandler.CreateMainMenuHandler(MainMenu control)
        {
            return new HandlerForDisposed();
        }

        IMenuItemHandler IControlFactoryHandler.CreateMenuItemHandler(MenuItem control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateNumericUpDownHandler(NumericUpDown control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateProgressBarHandler(ProgressBar control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreatePropertyGridHandler(PropertyGrid control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateRadioButtonHandler(RadioButton control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateRichTextBoxHandler(RichTextBox editor)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateScrollBarHandler(ScrollBar control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateSliderHandler(Slider control)
        {
            return new HandlerForDisposed();
        }

        IStatusBarHandler IControlFactoryHandler.CreateStatusBarHandler(StatusBar control)
        {
            return new PlessStatusBarHandler();
        }

        IControlHandler IControlFactoryHandler.CreateTextBoxHandler(TextBox control)
        {
            return new HandlerForDisposed();
        }

        IControlHandler IControlFactoryHandler.CreateTreeViewHandler(TreeView control)
        {
            return new HandlerForDisposed();
        }

        IPropertyGridVariant IControlFactoryHandler.CreateVariant()
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateVListBoxHandler(VirtualListBox control)
        {
            return new MauiVListBoxHandler();
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
