using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    public interface IApplicationHandler : IDisposable
    {
        /// <summary>
        /// Allows the programmer to specify whether the application will exit when the
        /// top-level frame is deleted.
        /// Returns true if the application will exit when the top-level frame is deleted.
        /// </summary>
        bool ExitOnFrameDelete { get; set; }

        /// <summary>
        /// Gets whether the application is active, i.e. if one of its windows is currently in
        /// the foreground.
        /// </summary>
        bool IsActive { get; }

        bool InUixmlPreviewerMode { get; set; }

        bool InvokeRequired { get; }

        Window? GetActiveWindow();

        Control? GetFocusedControl();

        void Run(Window window);

        void SetTopWindow(Window window);

        void WakeUpIdle();

        void BeginInvoke(Action action);

        bool HasPendingEvents();

        void NotifyCaptureLost();

        void ExitMainLoop();

        void Exit();

        ICursorFactoryHandler CreateCursorFactoryHandler();

        ITimerHandler CreateTimerHandler(Timer timer);

        IFontFactoryHandler CreateFontFactoryHandler();

        ISoundFactoryHandler CreateSoundFactoryHandler();

        /// <summary>
        /// Creates transparent brush handler.
        /// </summary>
        /// <returns></returns>
        IBrushHandler CreateTransparentBrushHandler(Brush brush);

        /// <summary>
        /// Creates hatch brush handler.
        /// </summary>
        /// <returns></returns>
        IHatchBrushHandler CreateHatchBrushHandler(HatchBrush brush);

        /// <summary>
        /// Creates linear gradient brush handler.
        /// </summary>
        /// <returns></returns>
        ILinearGradientBrushHandler CreateLinearGradientBrushHandler(LinearGradientBrush brush);

        /// <summary>
        /// Creates radial gradient brush handler.
        /// </summary>
        /// <returns></returns>
        IRadialGradientBrushHandler CreateRadialGradientBrushHandler(RadialGradientBrush brush);

        IPenHandler CreatePenHandler(Pen pen);

        /// <summary>
        /// Creates solid brush handler.
        /// </summary>
        /// <returns></returns>
        ISolidBrushHandler CreateSolidBrushHandler(SolidBrush brush);

        /// <summary>
        /// Creates texture brush handler.
        /// </summary>
        /// <returns></returns>
        ITextureBrushHandler CreateTextureBrushHandler(TextureBrush brush);

        ISelectDirectoryDialogHandler CreateSelectDirectoryDialogHandler(SelectDirectoryDialog dialog);

        IOpenFileDialogHandler CreateOpenFileDialogHandler(OpenFileDialog dialog);

        ISaveFileDialogHandler CreateSaveFileDialogHandler(SaveFileDialog dialog);

        IFontDialogHandler CreateFontDialogHandler(FontDialog dialog);

        IColorDialogHandler CreateColorDialogHandler(ColorDialog dialog);

        IRichTextBoxHandler CreateRichTextBoxHandler(RichTextBox editor);

        ISystemSettingsHandler CreateSystemSettingsHandler();

        ILinkLabelHandler CreateLinkLabelHandler(LinkLabel control);

        IAnimationPlayerHandler CreateAnimationPlayerHandler(AnimationPlayer control);

        ICalendarHandler CreateCalendarHandler(Calendar control);

        IListViewHandler CreateListViewHandler(ListView control);

        IDateTimePickerHandler CreateDateTimePickerHandler(DateTimePicker control);

        INumericUpDownHandler CreateNumericUpDownHandler(NumericUpDown control);

        IWebBrowserHandler CreateWebBrowserHandler(WebBrowser control);

        IWebBrowserFactoryHandler CreateWebBrowserFactoryHandler();

        IPropertyGridHandler CreatePropertyGridHandler(PropertyGrid control);

        IPropertyGridVariant CreateVariant();

        ICheckBoxHandler CreateCheckBoxHandler(CheckBox control);

        IButtonHandler CreateButtonHandler(Button control);

        IColorPickerHandler CreateColorPickerHandler(ColorPicker control);

        ISliderHandler CreateSliderHandler(Slider control);

        IProgressBarHandler CreateProgressBarHandler(ProgressBar control);

        IGroupBoxHandler CreateGroupBoxHandler(GroupBox control);

        IRadioButtonHandler CreateRadioButtonHandler(RadioButton control);

        ITextBoxHandler CreateTextBoxHandler(TextBox control);

        IComboBoxHandler CreateComboBoxHandler(ComboBox control);

        ILabelHandler CreateLabelHandler(Label control);

        IScrollBarHandler CreateScrollBarHandler(ScrollBar control);

        IMenuItemHandler CreateMenuItemHandler(MenuItem control);

        IContextMenuHandler CreateContextMenuHandler(ContextMenu control);

        IMainMenuHandler CreateMainMenuHandler(MainMenu control);

        IListBoxHandler CreateListBoxHandler(ListBox control);

        IWindowHandler CreateWindowHandler(Window window);

        IScrollViewerHandler CreateScrollViewerHandler(ScrollViewer control);

        ITreeViewHandler CreateTreeViewHandler(TreeView control);

        IStatusBarHandler CreateStatusBarHandler(StatusBar control);

        IControlHandler CreateControlHandler(Control control);

        ICheckListBoxHandler CreateCheckListBoxHandler(CheckListBox control);

        IVListBoxHandler CreateVListBoxHandler(VListBox control);

        void ProcessPendingEvents();

        IPropertyGridChoices CreateChoices();

        IPrintDocumentHandler CreatePrintDocumentHandler();

        IPrinterSettingsHandler CreatePrinterSettingsHandler();

        IPrintDialogHandler CreatePrintDialogHandler();

        IPageSettingsHandler CreatePageSettingsHandler();

        IValueValidatorText CreateValueValidatorText(ValueValidatorTextStyle style);

        IValueValidatorText CreateValueValidatorNum(
            ValueValidatorNumStyle numericType,
            int valueBase = 10);

        IPageSetupDialogHandler CreatePageSetupDialogHandler();

        IPrintPreviewDialogHandler CreatePrintPreviewDialogHandler();

        IRichToolTipHandler CreateRichToolTipHandler(
            string title,
            string message,
            bool useGeneric);
    }
}
