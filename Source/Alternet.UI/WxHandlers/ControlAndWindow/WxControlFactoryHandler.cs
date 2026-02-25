using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxControlFactoryHandler : DisposableObject
        , IControlFactoryHandler, IWxControlFactoryHandler
    {
        public IControlHandler CreatePanelHandler(ContainerControl control)
        {
            return new WxPanelHandler();
        }

        public IControlHandler CreateRichTextBoxHandler(RichTextBox editor)
        {
            return new WxRichTextBoxHandler();
        }

        public IPropertyGridChoices CreateChoices()
        {
            return new WxPropertyGridChoices();
        }

        /// <inheritdoc/>
        public IControlHandler CreatePropertyGridHandler(PropertyGrid control)
        {
            return new WxPropertyGridHandler();
        }

        public IPropertyGridVariant CreateVariant()
        {
            return new WxPropertyGridVariant();
        }

        public IWebBrowserFactoryHandler CreateWebBrowserFactoryHandler()
        {
            return new WxWebBrowserFactoryHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateCalendarHandler(Calendar control)
        {
            return new WxCalendarHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateProgressBarHandler(ProgressBar control)
        {
            return new WxProgressBarHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateRadioButtonHandler(RadioButton control)
        {
            return new WxRadioButtonHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateWindowHandler(Window window)
        {
            if (window.GetWindowKind() == WindowKind.Control)
                return new WxWindowAsControlHandler();
            return new WxWindowHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateListViewHandler(ListView control)
        {
            return new WxListViewHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateCheckBoxHandler(CheckBox control)
        {
            return new WxCheckBoxHandler();
        }

        /// <inheritdoc/>
        public IControlHandler? CreateButtonHandler(Button control)
        {
            return new WxButtonHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateAnimationPlayerHandler(AnimationPlayer control)
        {
            return new WxAnimationPlayerHandler(useGeneric: true);
        }

        /// <inheritdoc/>
        public IControlHandler CreateGroupBoxHandler(GroupBox control)
        {
            return new WxControlHandler<GroupBox, Native.GroupBox>();
        }

        /// <inheritdoc/>
        public IControlHandler CreateTextBoxHandler(TextBox control)
        {
            return new WxTextBoxHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateComboBoxHandler(ComboBox control)
        {
            return new WxComboBoxHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateScrollBarHandler(ScrollBar control)
        {
            return new WxScrollBarHandler();
        }

        /// <inheritdoc/>
        public IStatusBarHandler CreateStatusBarHandler(StatusBar control)
        {
            return new WxStatusBarHandler(control);
        }

        /// <inheritdoc/>
        public IControlHandler CreateControlHandler(AbstractControl control)
        {
            return new WxControlHandler();
        }

        public IControlHandler CreateTreeViewHandler(Control control)
        {
            return new WxTreeViewHandler();
        }

        public IControlHandler CreateSliderHandler(Slider control)
        {
            return new WxSliderHandler();
        }

        public IControlHandler CreateListBoxHandler(ListBox control)
        {
            return new WxListBoxHandler();
        }
    }
}
