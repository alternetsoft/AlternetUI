using System;

namespace Alternet.UI
{
    internal class NativeCalendarHandler : ControlHandler
    {
        public new Native.Calendar NativeControl => (Native.Calendar)base.NativeControl!;

        public new Calendar Control => (Calendar)base.Control;

        /*/// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;*/

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Calendar();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.SelectionChanged += NativeControl_SelectionChanged;
            NativeControl.PageChanged += NativeControl_PageChanged;
            NativeControl.WeekNumberClick += NativeControl_WeekNumberClick;
            NativeControl.DayHeaderClick += NativeControl_DayHeaderClick;
            NativeControl.DayDoubleClick += NativeControl_DayDoubleClick;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.SelectionChanged -= NativeControl_SelectionChanged;
            NativeControl.PageChanged -= NativeControl_PageChanged;
            NativeControl.WeekNumberClick -= NativeControl_WeekNumberClick;
            NativeControl.DayHeaderClick -= NativeControl_DayHeaderClick;
            NativeControl.DayDoubleClick -= NativeControl_DayDoubleClick;
        }

        private void NativeControl_DayDoubleClick(object? sender, EventArgs e)
        {
            Control.RaiseDayDoubleClick(e);
        }

        private void NativeControl_DayHeaderClick(object? sender, EventArgs e)
        {
            Control.RaiseDayHeaderClick(e);
        }

        private void NativeControl_WeekNumberClick(object? sender, EventArgs e)
        {
            Control.RaiseWeekNumberClick(e);
        }

        private void NativeControl_PageChanged(object? sender, EventArgs e)
        {
            Control.RaisePageChanged(e);
        }

        private void NativeControl_SelectionChanged(object? sender, EventArgs e)
        {
            Control.RaiseSelectionChanged(e);
        }
    }
}