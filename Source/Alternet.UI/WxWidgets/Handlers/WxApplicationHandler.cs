using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    internal class WxApplicationHandler : DisposableObject, IApplicationHandler
    {
        private static Native.Application nativeApplication;
        private static readonly KeyboardInputProvider keyboardInputProvider;
        private static readonly MouseInputProvider mouseInputProvider;

        static WxApplicationHandler()
        {
            NativeDrawing.Default = new WxDrawing();

            if (BaseApplication.SupressDiagnostics)
                Native.Application.SuppressDiagnostics(-1);

            nativeApplication = new Native.Application();
            nativeApplication.Idle = BaseApplication.RaiseIdle;
            nativeApplication.LogMessage += NativeApplication_LogMessage;
            nativeApplication.Name = Path.GetFileNameWithoutExtension(
                Process.GetCurrentProcess()?.MainModule?.FileName!);

            keyboardInputProvider = new KeyboardInputProvider(
                nativeApplication.Keyboard);
            mouseInputProvider = new MouseInputProvider(nativeApplication.Mouse);

            Keyboard.PrimaryDevice = InputManager.UnsecureCurrent.PrimaryKeyboardDevice;
            Mouse.PrimaryDevice = InputManager.UnsecureCurrent.PrimaryMouseDevice;
        }

        public WxApplicationHandler()
        {
        }

        /// <summary>
        /// Allows the programmer to specify whether the application will exit when the
        /// top-level frame is deleted.
        /// Returns true if the application will exit when the top-level frame is deleted.
        /// </summary>
        public bool ExitOnFrameDelete
        {
            get => nativeApplication.GetExitOnFrameDelete();
            set => nativeApplication.SetExitOnFrameDelete(value);
        }

        /// <summary>
        /// Gets whether the application is active, i.e. if one of its windows is currently in
        /// the foreground.
        /// </summary>
        public bool IsActive => nativeApplication.IsActive();

        /// <inheritdoc/>
        public bool InUixmlPreviewerMode
        {
            get => nativeApplication.InUixmlPreviewerMode;
            set => nativeApplication.InUixmlPreviewerMode = value;
        }

        internal static Native.Clipboard NativeClipboard => nativeApplication.Clipboard;

        internal static Native.Keyboard NativeKeyboard => nativeApplication.Keyboard;

        internal static Native.Application NativeApplication => nativeApplication;

        internal static Native.Mouse NativeMouse => nativeApplication.Mouse;

        internal static string EventArgString => nativeApplication.EventArgString;

        public static IntPtr WxWidget(IControl? control)
        {
            if (control is null)
                return default;
            return ((UI.Native.Control)control.NativeControl).WxWidget;
        }

        /// <summary>
        /// Informs all message pumps that they must terminate, and then closes
        /// all application windows after the messages have been processed.
        /// </summary>
        public void Exit()
        {
            nativeApplication.Exit();
        }

        public bool HasPendingEvents()
        {
            return nativeApplication.HasPendingEvents();
        }

        public void Run(Window window)
        {
            nativeApplication.Run(
                ((WindowHandler)window.Handler).NativeControl);
        }

        public IDialogFactoryHandler CreateDialogFactoryHandler()
        {
            return new WxDialogFactoryHandler();
        }

        public IWebBrowserHandler CreateWebBrowserHandler(WebBrowser control)
        {
            return new WebBrowserHandler();
        }

        public IClipboardHandler CreateClipboardHandler()
        {
            return new WxClipboardHandler();
        }

        public IWebBrowserFactoryHandler CreateWebBrowserFactoryHandler()
        {
            return new WebBrowserFactoryHandler();
        }

        public void ProcessPendingEvents()
        {
            nativeApplication.ProcessPendingEvents();
        }

        public void ExitMainLoop()
        {
            nativeApplication.ExitMainLoop();
        }

        public void SetTopWindow(Window window)
        {
            nativeApplication.SetTopWindow(WxApplicationHandler.WxWidget(window));
        }

        public void WakeUpIdle()
        {
            nativeApplication.WakeUpIdle();
        }

        public void BeginInvoke(Action action)
        {
            nativeApplication.BeginInvoke(action);
        }

        private static void NativeApplication_LogMessage()
        {
            var s = nativeApplication.EventArgString;

            BaseApplication.LogNativeMessage(s);
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

        /// <inheritdoc/>
        public ISoundFactoryHandler CreateSoundFactoryHandler()
        {
            return new WxSoundFactoryHandler();
        }

        /// <inheritdoc/>
        public bool InvokeRequired => nativeApplication.InvokeRequired;

        /// <inheritdoc/>
        public IFontFactoryHandler CreateFontFactoryHandler()
        {
            return new WxFontFactoryHandler();
        }

        /// <inheritdoc/>
        public IControlPainterHandler CreateControlPainterHandler()
        {
            return new WxControlPainterHandler();
        }

        /// <inheritdoc/>
        public IMemoryHandler CreateMemoryHandler()
        {
            return new WxMemoryHandler();
        }

        /// <inheritdoc/>
        public ICursorFactoryHandler CreateCursorFactoryHandler()
        {
            return new WxCursorFactoryHandler();
        }

        /// <inheritdoc/>
        public IToolTipHandler CreateToolTipHandler()
        {
            return new WxToolTipHandler();
        }

        /// <inheritdoc/>
        public INotifyIconHandler CreateNotifyIconHandler()
        {
            return new UI.Native.NotifyIcon();
        }

        /// <inheritdoc/>
        public ITimerHandler CreateTimerHandler(Timer timer)
        {
            return new UI.Native.Timer();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            nativeApplication.Idle = null;
            nativeApplication.LogMessage = null;
            keyboardInputProvider.Dispose();
            mouseInputProvider.Dispose();
            nativeApplication.Dispose();
            nativeApplication = null!;
        }
    }
}
