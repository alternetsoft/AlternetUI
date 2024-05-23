using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    internal class WxApplicationHandler : DisposableObject, IApplicationHandler
    {
        public static IntPtr WxWidget(IControl? control)
        {
            if (control is null)
                return default;
            return ((UI.Native.Control)control.NativeControl).WxWidget;
        }

        public IWebBrowserHandler CreateWebBrowserHandler(WebBrowser control)
        {
            return new WebBrowserHandler();
        }

        public IWebBrowserFactoryHandler CreateWebBrowserFactoryHandler()
        {
            return new WebBrowserFactoryHandler();
        }

        public void ProcessPendingEvents()
        {
            Application.Current?.nativeApplication.ProcessPendingEvents();
        }

        public void ExitMainLoop()
        {
            Application.Current.NativeApplication.ExitMainLoop();
        }

        public ISystemSettingsHandler CreateSystemSettingsHandler()
        {
            return new WxSystemSettingsHandler();
        }

        public IRichTextBoxHandler CreateRichTextBoxHandler(RichTextBox editor)
        {
            return new RichTextBoxHandler();
        }

        /// <inheritdoc/>
        public void NotifyCaptureLost()
        {
            Native.Control.NotifyCaptureLost();
        }

        /// <inheritdoc/>
        public Control? GetFocusedControl()
        {
            var focusedNativeControl = Native.Control.GetFocusedControl();
            if (focusedNativeControl == null)
                return null;

            var handler = WxControlHandler.NativeControlToHandler(focusedNativeControl);
            if (handler == null || !handler.IsAttached)
                return null;

            return handler.Control;
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
            return new RadioButtonHandler();
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
            return new ButtonHandler();
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
            return new ScrollBarHandler();
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

        public IPropertyGridChoices CreateChoices()
        {
            return new PropertyGridChoices();
        }

        /// <inheritdoc/>
        public IPropertyGridHandler CreatePropertyGridHandler(PropertyGrid control)
        {
            return new PropertyGridHandler();
        }

        /// <inheritdoc/>
        public IOpenFileDialogHandler CreateOpenFileDialogHandler(OpenFileDialog dialog)
        {
            return new UI.Native.FileDialog()
            {
                Mode = Native.FileDialogMode.Open,
            };
        }

        public IPropertyGridVariant CreateVariant()
        {
            return new PropertyGridVariant();
        }

        /// <inheritdoc/>
        public ISaveFileDialogHandler CreateSaveFileDialogHandler(SaveFileDialog dialog)
        {
            return new UI.Native.FileDialog()
            {
                Mode = Native.FileDialogMode.Save,
            };
        }

        /// <inheritdoc/>
        public IColorDialogHandler CreateColorDialogHandler(ColorDialog dialog)
        {
            return new UI.Native.ColorDialog();
        }

        /// <inheritdoc/>
        public ISelectDirectoryDialogHandler CreateSelectDirectoryDialogHandler(
            SelectDirectoryDialog dialog)
        {
            return new UI.Native.SelectDirectoryDialog();
        }

        /// <inheritdoc/>
        public IFontDialogHandler CreateFontDialogHandler(FontDialog dialog)
        {
            return new UI.Native.FontDialog();
        }

        /// <inheritdoc/>
        public IPenHandler CreatePenHandler(Pen pen) => new UI.Native.Pen();

        /// <inheritdoc/>
        public IBrushHandler CreateTransparentBrushHandler(Brush brush)
            => new UI.Native.Brush();

        /// <inheritdoc/>
        public IHatchBrushHandler CreateHatchBrushHandler(HatchBrush brush)
            => new UI.Native.HatchBrush();

        /// <inheritdoc/>
        public ILinearGradientBrushHandler CreateLinearGradientBrushHandler(
            LinearGradientBrush brush)
            => new UI.Native.LinearGradientBrush();

        /// <inheritdoc/>
        public IRadialGradientBrushHandler CreateRadialGradientBrushHandler(
            RadialGradientBrush brush)
            => new UI.Native.RadialGradientBrush();

        /// <inheritdoc/>
        public ISolidBrushHandler CreateSolidBrushHandler(SolidBrush brush)
            => new UI.Native.SolidBrush();

        /// <inheritdoc/>
        public ITextureBrushHandler CreateTextureBrushHandler(TextureBrush brush)
            => new UI.Native.TextureBrush();

        /// <inheritdoc/>
        public Window? GetActiveWindow()
        {
            var activeWindow = Native.Window.ActiveWindow;
            if (activeWindow == null)
                return null;

            var handler = WxControlHandler.NativeControlToHandler(activeWindow) ??
                throw new InvalidOperationException();
            return ((WindowHandler)handler).Control;
        }

        public IPrinterSettingsHandler CreatePrinterSettingsHandler()
        {
            return new UI.Native.PrinterSettings();
        }

        public IPageSettingsHandler CreatePageSettingsHandler()
        {
            return new UI.Native.PageSettings();
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

        public IRichToolTipHandler CreateRichToolTipHandler(
            string title,
            string message,
            bool useGeneric)
        {
            Native.WxOtherFactory.RichToolTipUseGeneric = useGeneric;
            return new RichToolTipHandler(title, message);
        }

        public IPrintDocumentHandler CreatePrintDocumentHandler()
        {
            return new PrintDocumentHandler();
        }

        public IPrintDialogHandler CreatePrintDialogHandler()
        {
            return new UI.Native.PrintDialog();
        }

        public IPrintPreviewDialogHandler CreatePrintPreviewDialogHandler()
        {
            return new UI.Native.PrintPreviewDialog();
        }

        public IPageSetupDialogHandler CreatePageSetupDialogHandler()
        {
            return new UI.Native.PageSetupDialog();
        }
    }
}
