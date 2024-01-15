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
        internal static int ScreenShotCounter { get; set; } = 0;

        internal bool ProcessUIUpdates
        {
            get
            {
                return NativeControl?.ProcessUIUpdates ?? false;
            }

            set
            {
                if (NativeControl is not null)
                    NativeControl.ProcessUIUpdates = value;
            }
        }

        internal virtual bool IsDummy => false;

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

        /// <summary>
        /// Gets or sets border style of the control.
        /// </summary>
        internal virtual ControlBorderStyle BorderStyle
        {
            get
            {
                var nc = NativeControl;
                if (nc is null)
                    return ControlBorderStyle.Default;
                return (ControlBorderStyle)nc.BorderStyle;
            }

            set
            {
                var nc = NativeControl;
                if (nc is null)
                    return;
                nc.BorderStyle = (int)value;
            }
        }

        /// <summary>
        /// Gets <see cref="NativeControl"/> attached to this control.
        /// </summary>
        internal Native.Control NativeControl => Handler.NativeControl;

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
