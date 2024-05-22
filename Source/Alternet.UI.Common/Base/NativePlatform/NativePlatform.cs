using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    /// <summary>
    /// Declares platform related methods.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="Default"/> property until native platform
    /// is initialized.
    /// </remarks>
    public abstract partial class NativePlatform : BaseObject
    {
        /// <summary>
        /// Gets default native platform implementation.
        /// </summary>
        public static NativePlatform Default = new NotImplementedPlatform();

        public abstract IFontFactoryHandler FontFactory { get; }

        public abstract ILinkLabelHandler CreateLinkLabelHandler(LinkLabel control);

        public abstract IAnimationPlayerHandler CreateAnimationPlayerHandler(AnimationPlayer control);

        public abstract ICalendarHandler CreateCalendarHandler(Calendar control);

        public abstract IListViewHandler CreateListViewHandler(ListView control);

        public abstract IDateTimePickerHandler CreateDateTimePickerHandler(DateTimePicker control);

        public abstract INumericUpDownHandler CreateNumericUpDownHandler(NumericUpDown control);

        public abstract IPropertyGridHandler CreatePropertyGridHandler(PropertyGrid control);

        public abstract ICheckBoxHandler CreateCheckBoxHandler(CheckBox control);

        public abstract IButtonHandler CreateButtonHandler(Button control);

        public abstract IColorPickerHandler CreateColorPickerHandler(ColorPicker control);

        public abstract ISliderHandler CreateSliderHandler(Slider control);

        public abstract IProgressBarHandler CreateProgressBarHandler(ProgressBar control);

        public abstract IGroupBoxHandler CreateGroupBoxHandler(GroupBox control);

        public abstract IRadioButtonHandler CreateRadioButtonHandler(RadioButton control);

        public abstract ITextBoxHandler CreateTextBoxHandler(TextBox control);

        public abstract IComboBoxHandler CreateComboBoxHandler(ComboBox control);

        public abstract ILabelHandler CreateLabelHandler(Label control);

        public abstract IScrollBarHandler CreateScrollBarHandler(ScrollBar control);

        public abstract IMenuItemHandler CreateMenuItemHandler(MenuItem control);

        public abstract IContextMenuHandler CreateContextMenuHandler(ContextMenu control);

        public abstract IMainMenuHandler CreateMainMenuHandler(MainMenu control);

        public abstract IListBoxHandler CreateListBoxHandler(ListBox control);

        public abstract IWindowHandler CreateWindowHandler(Window window);

        public abstract IScrollViewerHandler CreateScrollViewerHandler(ScrollViewer control);

        public abstract ITreeViewHandler CreateTreeViewHandler(TreeView control);

        public abstract IStatusBarHandler CreateStatusBarHandler(StatusBar control);

        public abstract IControlHandler CreateControlHandler(Control control);

        public abstract ICheckListBoxHandler CreateCheckListBoxHandler(CheckListBox control);

        public abstract IVListBoxHandler CreateVListBoxHandler(VListBox control);

        public abstract IControl? GetFocusedControl();

        public abstract CustomControlPainter GetPainter();

        public abstract void NotifyCaptureLost();

        public abstract Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

        public abstract Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

        public abstract Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

        public abstract Window? GetActiveWindow();

        /// <summary>
        /// Creates transparent brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract IBrushHandler CreateTransparentBrushHandler(Brush brush);

        /// <summary>
        /// Creates hatch brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract IHatchBrushHandler CreateHatchBrushHandler(HatchBrush brush);

        /// <summary>
        /// Creates linear gradient brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract ILinearGradientBrushHandler CreateLinearGradientBrushHandler(LinearGradientBrush brush);

        /// <summary>
        /// Creates radial gradient brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract IRadialGradientBrushHandler CreateRadialGradientBrushHandler(RadialGradientBrush brush);

        public abstract IPenHandler CreatePenHandler(Pen pen);

        /// <summary>
        /// Creates solid brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract ISolidBrushHandler CreateSolidBrushHandler(SolidBrush brush);

        /// <summary>
        /// Creates texture brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract ITextureBrushHandler CreateTextureBrushHandler(TextureBrush brush);

        public abstract ISelectDirectoryDialogHandler CreateSelectDirectoryDialogHandler(
            SelectDirectoryDialog dialog);

        public abstract IOpenFileDialogHandler CreateOpenFileDialogHandler(OpenFileDialog dialog);

        public abstract ISaveFileDialogHandler CreateSaveFileDialogHandler(SaveFileDialog dialog);

        public abstract IFontDialogHandler CreateFontDialogHandler(FontDialog dialog);

        public abstract IColorDialogHandler CreateColorDialogHandler(ColorDialog dialog);

        public abstract IRichTextBoxHandler CreateRichTextBoxHandler(RichTextBox editor);

        public abstract void ShowDeveloperTools();

        public abstract ISystemSettingsHandler CreateSystemSettingsHandler();

        public abstract void ExitMainLoop();

        public abstract bool ShowExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true);

        public abstract bool IsBusyCursor();

        public abstract void BeginBusyCursor();

        public abstract void EndBusyCursor();

        public abstract void ProcessPendingEvents();

        public abstract IDataObject? ClipboardGetDataObject();

        public abstract void ClipboardSetDataObject(IDataObject value);

        public abstract DialogResult ShowMessageBox(MessageBoxInfo info);

        public abstract void StopSound();

        public abstract void Bell();

        public abstract void MessageBeep(SystemSoundType soundType);

        public abstract void TimerSetTick(Timer timer, Action? value);

        public abstract object CreateTimer();

        public abstract bool TimerGetEnabled(Timer timer);

        public abstract void TimerSetEnabled(Timer timer, bool value);

        public abstract int TimerGetInterval(Timer timer);

        public abstract void TimerSetInterval(Timer timer, int value);

        public abstract IPropertyGridChoices CreateChoices();

        public abstract IPrintDocumentHandler CreatePrintDocumentHandler();

        public abstract IPrinterSettingsHandler CreatePrinterSettingsHandler();

        public abstract IPrintDialogHandler CreatePrintDialogHandler();

        public abstract IPageSettingsHandler CreatePageSettingsHandler();

        public abstract IValueValidatorText CreateValueValidatorText(ValueValidatorTextStyle style);

        public abstract IValueValidatorText CreateValueValidatorNum(
            ValueValidatorNumStyle numericType,
            int valueBase = 10);

        public abstract IPageSetupDialogHandler CreatePageSetupDialogHandler();

        public abstract void RegisterDefaultPreviewControls(PreviewFile preview);

        public abstract IPrintPreviewDialogHandler CreatePrintPreviewDialogHandler();

        public abstract IRichToolTipHandler CreateRichToolTipHandler(
            string title,
            string message,
            bool useGeneric);

        public abstract string? GetTextFromUser(
            string message,
            string caption,
            string defaultValue,
            Control? parent,
            int x,
            int y,
            bool centre);

        public abstract long? GetNumberFromUser(
            string message,
            string prompt,
            string caption,
            long value,
            long min,
            long max,
            Control? parent,
            PointI pos);
    }
}
