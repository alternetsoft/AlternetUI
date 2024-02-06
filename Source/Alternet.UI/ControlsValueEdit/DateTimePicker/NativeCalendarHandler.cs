using System;

namespace Alternet.UI
{
    internal class NativeCalendarHandler : ControlHandler
    {
        public new Native.Calendar NativeControl => (Native.Calendar)base.NativeControl!;

        public new Calendar Control => (Calendar)base.Control;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Calendar();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.SelectionChanged = NativeControl_SelectionChanged;
            NativeControl.PageChanged = NativeControl_PageChanged;
            NativeControl.WeekNumberClick = NativeControl_WeekNumberClick;
            NativeControl.DayHeaderClick = NativeControl_DayHeaderClick;
            NativeControl.DayDoubleClick = NativeControl_DayDoubleClick;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.SelectionChanged = null;
            NativeControl.PageChanged = null;
            NativeControl.WeekNumberClick = null;
            NativeControl.DayHeaderClick = null;
            NativeControl.DayDoubleClick = null;
        }

        private void NativeControl_DayDoubleClick()
        {
            Control.RaiseDayDoubleClick(EventArgs.Empty);
        }

        private void NativeControl_DayHeaderClick()
        {
            Control.RaiseDayHeaderClick(EventArgs.Empty);
        }

        private void NativeControl_WeekNumberClick()
        {
            Control.RaiseWeekNumberClick(EventArgs.Empty);
        }

        private void NativeControl_PageChanged()
        {
            Control.RaisePageChanged(EventArgs.Empty);
        }

        private void NativeControl_SelectionChanged()
        {
            Control.RaiseSelectionChanged(EventArgs.Empty);
        }
    }
}