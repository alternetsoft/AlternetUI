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
            return new RichTextBoxHandler();
        }

        public IPropertyGridChoices CreateChoices()
        {
            return new PropertyGridChoices();
        }

        /// <inheritdoc/>
        public IControlHandler CreatePropertyGridHandler(PropertyGrid control)
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
        public IControlHandler CreateCalendarHandler(Calendar control)
        {
            return new CalendarHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateProgressBarHandler(ProgressBar control)
        {
            return new ProgressBarHandler();
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
                return new WindowAsControlHandler();
            return new WindowHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateListViewHandler(ListView control)
        {
            return new ListViewHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateCheckBoxHandler(CheckBox control)
        {
            return new CheckBoxHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateButtonHandler(Button control)
        {
            return new WxButtonHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateAnimationPlayerHandler(AnimationPlayer control)
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
        public IControlHandler CreateGroupBoxHandler(GroupBox control)
        {
            return new WxControlHandler<GroupBox, Native.GroupBox>();
        }

        /// <inheritdoc/>
        public IControlHandler CreateTextBoxHandler(TextBox control)
        {
            return new TextBoxHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateComboBoxHandler(ComboBox control)
        {
            return new ComboBoxHandler();
        }

        /// <inheritdoc/>
        public IControlHandler CreateScrollBarHandler(ScrollBar control)
        {
            return new WxScrollBarHandler();
        }

        /// <inheritdoc/>
        public IMenuItemHandler CreateMenuItemHandler(MenuItem control)
        {
            return new Native.MenuItem(control);
        }

        /// <inheritdoc/>
        public IContextMenuHandler CreateContextMenuHandler(ContextMenu control)
        {
            return new Native.Menu(control);
        }

        /// <inheritdoc/>
        public IMainMenuHandler CreateMainMenuHandler(MainMenu control)
        {
            return new Native.MainMenu(control);
        }

        /// <inheritdoc/>
        public IStatusBarHandler CreateStatusBarHandler(StatusBar control)
        {
            return new StatusBarHandler(control);
        }

        /// <inheritdoc/>
        public IControlHandler CreateControlHandler(AbstractControl control)
        {
            return new WxControlHandler();
        }

        public IControlHandler CreateTreeViewHandler(Control control)
        {
            return new TreeViewHandler();
        }

        public IControlHandler CreateSliderHandler(Slider control)
        {
            return new SliderHandler();
        }
    }
}
