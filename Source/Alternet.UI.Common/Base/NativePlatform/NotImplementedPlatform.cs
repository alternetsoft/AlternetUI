﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    internal class NotImplementedPlatform : NativePlatform
    {
        public override IFontFactoryHandler FontFactory => throw new NotImplementedException();

        public override IBrushHandler CreateTransparentBrushHandler(Brush brush)
            => NotImplemented<IBrushHandler>();

        public override IHatchBrushHandler CreateHatchBrushHandler(HatchBrush brush)
            => NotImplemented<IHatchBrushHandler>();

        public override ILinearGradientBrushHandler CreateLinearGradientBrushHandler(
            LinearGradientBrush brush)
            => NotImplemented<ILinearGradientBrushHandler>();

        public override IRadialGradientBrushHandler CreateRadialGradientBrushHandler(
            RadialGradientBrush brush)
            => NotImplemented<IRadialGradientBrushHandler>();

        public override ISolidBrushHandler CreateSolidBrushHandler(SolidBrush brush)
            => NotImplemented<ISolidBrushHandler>();

        public override ITextureBrushHandler CreateTextureBrushHandler(TextureBrush brush)
            => NotImplemented<ITextureBrushHandler>();

        public override void BeginBusyCursor()
        {
            throw new NotImplementedException();
        }

        public override void Bell()
        {
            throw new NotImplementedException();
        }

        public override IDataObject? ClipboardGetDataObject()
        {
            throw new NotImplementedException();
        }

        public override void ClipboardSetDataObject(IDataObject value)
        {
            throw new NotImplementedException();
        }

        public override IAnimationPlayerHandler CreateAnimationPlayerHandler(AnimationPlayer control)
        {
            throw new NotImplementedException();
        }

        public override IButtonHandler CreateButtonHandler(Button control)
        {
            throw new NotImplementedException();
        }

        public override ICalendarHandler CreateCalendarHandler(Calendar control)
        {
            throw new NotImplementedException();
        }

        public override ICheckBoxHandler CreateCheckBoxHandler(CheckBox control)
        {
            throw new NotImplementedException();
        }

        public override ICheckListBoxHandler CreateCheckListBoxHandler(CheckListBox control)
        {
            throw new NotImplementedException();
        }

        public override IPropertyGridChoices CreateChoices()
        {
            throw new NotImplementedException();
        }

        public override IColorPickerHandler CreateColorPickerHandler(ColorPicker control)
        {
            throw new NotImplementedException();
        }

        public override IComboBoxHandler CreateComboBoxHandler(ComboBox control)
        {
            throw new NotImplementedException();
        }

        public override IContextMenuHandler CreateContextMenuHandler(ContextMenu control)
        {
            throw new NotImplementedException();
        }

        public override IDateTimePickerHandler CreateDateTimePickerHandler(DateTimePicker control)
        {
            throw new NotImplementedException();
        }

        public override IGroupBoxHandler CreateGroupBoxHandler(GroupBox control)
        {
            throw new NotImplementedException();
        }

        public override ILabelHandler CreateLabelHandler(Label control)
        {
            throw new NotImplementedException();
        }

        public override ILinkLabelHandler CreateLinkLabelHandler(LinkLabel control)
        {
            throw new NotImplementedException();
        }

        public override IListBoxHandler CreateListBoxHandler(ListBox control)
        {
            throw new NotImplementedException();
        }

        public override IListViewHandler CreateListViewHandler(ListView control)
        {
            throw new NotImplementedException();
        }

        public override IMainMenuHandler CreateMainMenuHandler(MainMenu control)
        {
            throw new NotImplementedException();
        }

        public override IMenuItemHandler CreateMenuItemHandler(MenuItem control)
        {
            throw new NotImplementedException();
        }

        public override INumericUpDownHandler CreateNumericUpDownHandler(NumericUpDown control)
        {
            throw new NotImplementedException();
        }

        public override IPageSettingsHandler CreatePageSettingsHandler()
        {
            throw new NotImplementedException();
        }

        public override IPageSetupDialogHandler CreatePageSetupDialogHandler()
        {
            throw new NotImplementedException();
        }

        public override IControlHandler CreateControlHandler(Control control)
        {
            throw new NotImplementedException();
        }

        public override IPrintDialogHandler CreatePrintDialogHandler()
        {
            throw new NotImplementedException();
        }

        public override IPrintDocumentHandler CreatePrintDocumentHandler()
        {
            throw new NotImplementedException();
        }

        public override IPrinterSettingsHandler CreatePrinterSettingsHandler()
        {
            throw new NotImplementedException();
        }

        public override IPrintPreviewDialogHandler CreatePrintPreviewDialogHandler()
        {
            throw new NotImplementedException();
        }

        public override IProgressBarHandler CreateProgressBarHandler(ProgressBar control)
        {
            throw new NotImplementedException();
        }

        public override IRadioButtonHandler CreateRadioButtonHandler(RadioButton control)
        {
            throw new NotImplementedException();
        }

        public override IRichToolTipHandler CreateRichToolTipHandler(
            string title,
            string message,
            bool useGeneric)
        {
            throw new NotImplementedException();
        }

        public override IScrollBarHandler CreateScrollBarHandler(ScrollBar control)
        {
            throw new NotImplementedException();
        }

        public override IScrollViewerHandler CreateScrollViewerHandler(ScrollViewer control)
        {
            throw new NotImplementedException();
        }

        public override ISliderHandler CreateSliderHandler(Slider control)
        {
            throw new NotImplementedException();
        }

        public override ITextBoxHandler CreateTextBoxHandler(TextBox control)
        {
            throw new NotImplementedException();
        }

        public override object CreateTimer()
        {
            throw new NotImplementedException();
        }

        public override ITreeViewHandler CreateTreeViewHandler(TreeView control)
        {
            throw new NotImplementedException();
        }

        public override IValueValidatorText CreateValueValidatorNum(ValueValidatorNumStyle numericType, int valueBase = 10)
        {
            throw new NotImplementedException();
        }

        public override IValueValidatorText CreateValueValidatorText(ValueValidatorTextStyle style)
        {
            throw new NotImplementedException();
        }

        public override IVListBoxHandler CreateVListBoxHandler(VListBox control)
        {
            throw new NotImplementedException();
        }

        public override IWindowHandler CreateWindowHandler(Window window)
        {
            throw new NotImplementedException();
        }

        public override void EndBusyCursor()
        {
            throw new NotImplementedException();
        }

        public override void ExitMainLoop()
        {
            throw new NotImplementedException();
        }

        public override Window? GetActiveWindow()
        {
            throw new NotImplementedException();
        }

        public override Color GetClassDefaultAttributesBgColor(ControlTypeId controlType, ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            throw new NotImplementedException();
        }

        public override Color GetClassDefaultAttributesFgColor(ControlTypeId controlType, ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            throw new NotImplementedException();
        }

        public override Font? GetClassDefaultAttributesFont(ControlTypeId controlType, ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            throw new NotImplementedException();
        }

        public override IControl? GetFocusedControl()
        {
            throw new NotImplementedException();
        }

        public override LangDirection GetLangDirection()
        {
            throw new NotImplementedException();
        }

        public override string GetLibraryVersionString()
        {
            throw new NotImplementedException();
        }

        public override long? GetNumberFromUser(string message, string prompt, string caption, long value, long min, long max, Control? parent, PointI pos)
        {
            throw new NotImplementedException();
        }

        public override CustomControlPainter GetPainter()
        {
            throw new NotImplementedException();
        }

        public override UIPlatformKind GetPlatformKind()
        {
            throw new NotImplementedException();
        }

        public override string? GetTextFromUser(string message, string caption, string defaultValue, Control? parent, int x, int y, bool centre)
        {
            throw new NotImplementedException();
        }

        public override string? GetUIVersion()
        {
            throw new NotImplementedException();
        }

        public override bool IsBusyCursor()
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryAlloc(int size)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryAllocLong(ulong size)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryCopy(IntPtr dest, IntPtr src, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryCopyLong(IntPtr dest, IntPtr src, ulong count)
        {
            throw new NotImplementedException();
        }

        public override void MemoryFree(IntPtr ptr)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryMove(IntPtr dest, IntPtr src, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryMoveLong(IntPtr dest, IntPtr src, ulong count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryRealloc(IntPtr ptr, int newSize)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryReallocLong(IntPtr ptr, ulong newSize)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemorySet(IntPtr dest, byte fillByte, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemorySetLong(IntPtr dest, byte fillByte, ulong count)
        {
            throw new NotImplementedException();
        }

        public override void MessageBeep(SystemSoundType soundType)
        {
            throw new NotImplementedException();
        }

        public override void NotifyCaptureLost()
        {
            throw new NotImplementedException();
        }

        public override void ProcessPendingEvents()
        {
            throw new NotImplementedException();
        }

        public override void RegisterDefaultPreviewControls(PreviewFile preview)
        {
            throw new NotImplementedException();
        }

        public override void SetSystemOption(string name, int value)
        {
            throw new NotImplementedException();
        }

        public override void ShowDeveloperTools()
        {
            throw new NotImplementedException();
        }

        public override bool ShowExceptionWindow(Exception exception, string? additionalInfo = null, bool canContinue = true)
        {
            throw new NotImplementedException();
        }

        public override DialogResult ShowMessageBox(MessageBoxInfo info)
        {
            throw new NotImplementedException();
        }

        public override void StopSound()
        {
            throw new NotImplementedException();
        }

        public override bool SystemSettingsAppearanceIsDark()
        {
            throw new NotImplementedException();
        }

        public override string SystemSettingsAppearanceName()
        {
            throw new NotImplementedException();
        }

        public override Font SystemSettingsGetFont(SystemSettingsFont systemFont)
        {
            throw new NotImplementedException();
        }

        public override int SystemSettingsGetMetric(SystemSettingsMetric index)
        {
            throw new NotImplementedException();
        }

        public override int SystemSettingsGetMetric(SystemSettingsMetric index, IControl? control)
        {
            throw new NotImplementedException();
        }

        public override bool SystemSettingsHasFeature(SystemSettingsFeature index)
        {
            throw new NotImplementedException();
        }

        public override bool SystemSettingsIsUsingDarkBackground()
        {
            throw new NotImplementedException();
        }

        public override bool TimerGetEnabled(Timer timer)
        {
            throw new NotImplementedException();
        }

        public override int TimerGetInterval(Timer timer)
        {
            throw new NotImplementedException();
        }

        public override void TimerSetEnabled(Timer timer, bool value)
        {
            throw new NotImplementedException();
        }

        public override void TimerSetInterval(Timer timer, int value)
        {
            throw new NotImplementedException();
        }

        public override void TimerSetTick(Timer timer, Action? value)
        {
            throw new NotImplementedException();
        }

        public override void ValidatorSuppressBellOnError(bool value)
        {
            throw new NotImplementedException();
        }

        public override IStatusBarHandler CreateStatusBarHandler(StatusBar control)
        {
            throw new NotImplementedException();
        }

        public override IPenHandler CreatePenHandler(Pen pen)
        {
            throw new NotImplementedException();
        }

        public override IColorDialogHandler CreateColorDialogHandler(ColorDialog dialog)
        {
            throw new NotImplementedException();
        }

        public override IFontDialogHandler CreateFontDialogHandler(FontDialog dialog)
        {
            throw new NotImplementedException();
        }

        public override ISelectDirectoryDialogHandler CreateSelectDirectoryDialogHandler(SelectDirectoryDialog dialog)
        {
            throw new NotImplementedException();
        }

        public override IOpenFileDialogHandler CreateOpenFileDialogHandler(OpenFileDialog dialog)
        {
            throw new NotImplementedException();
        }

        public override ISaveFileDialogHandler CreateSaveFileDialogHandler(SaveFileDialog dialog)
        {
            throw new NotImplementedException();
        }

        public override IRichTextBoxHandler CreateRichTextBoxHandler(RichTextBox editor)
        {
            throw new NotImplementedException();
        }
    }
}
