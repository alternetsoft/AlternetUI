using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Control
    {
        private CaretInfo? caretInfo;

        internal static int ScreenShotCounter { get; set; } = 0;

        internal CaretInfo? CaretInfo
        {
            get => caretInfo;

            set
            {
                if (caretInfo == value)
                    return;
                caretInfo = value;
                InvalidateCaret();
            }
        }

        internal bool ProcessUIUpdates
        {
            get
            {
                return Handler.ProcessUIUpdates;
            }

            set
            {
                Handler.ProcessUIUpdates = value;
            }
        }

        internal Thickness MinMargin
        {
            get
            {
                if (minMargin == null)
                {
                    minMargin = AllPlatformDefaults.
                        GetAsThickness(ControlKind, ControlDefaultsId.MinMargin);
                }

                return minMargin.Value;
            }
        }

        internal Thickness MinPadding
        {
            get
            {
                if (minPadding == null)
                {
                    minPadding = AllPlatformDefaults.
                        GetAsThickness(ControlKind, ControlDefaultsId.MinPadding);
                }

                return minPadding.Value;
            }
        }

        internal bool HasExtendedProps => extendedProps != null;

        internal ControlExtendedProps ExtendedProps
        {
            get
            {
                extendedProps ??= new();
                return extendedProps;
            }
        }
    }
}
