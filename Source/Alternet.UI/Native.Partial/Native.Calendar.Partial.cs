using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class Calendar
    {
        public void OnPlatformEventSelectionChanged()
        {
            (UIControl as UI.Calendar)?.RaiseSelectionChanged(EventArgs.Empty);
        }
        
        public void OnPlatformEventPageChanged()
        {
            (UIControl as UI.Calendar)?.RaisePageChanged(EventArgs.Empty);
        }

        public void OnPlatformEventWeekNumberClick()
        {
            (UIControl as UI.Calendar)?.RaiseWeekNumberClick(EventArgs.Empty);
        }

        public void OnPlatformEventDayHeaderClick()
        {
            (UIControl as UI.Calendar)?.RaiseDayHeaderClick(EventArgs.Empty);
        }

        public void OnPlatformEventDayDoubleClick()
        {
            (UIControl as UI.Calendar)?.RaiseDayDoubleClick(EventArgs.Empty);
        }
    }
}