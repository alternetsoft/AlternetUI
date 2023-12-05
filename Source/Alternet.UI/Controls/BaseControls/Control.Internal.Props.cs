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

        /// <summary>
        /// Gets or sets value indicating whether this control accepts
        /// input or not (i.e. behaves like a static text) and so doesn't need focus.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        internal bool AcceptsFocus
        {
            get
            {
                var result = NativeControl?.AcceptsFocus;
                return result ?? false;
            }

            set
            {
                if (NativeControl is null)
                    return;
                NativeControl.AcceptsFocus = value;
            }
        }

        /// <summary>
        /// Gets or sets value indicating whether this control accepts
        /// focus from keyboard or not.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        /// <returns>
        /// Return false to indicate that while this control can,
        /// in principle, have focus if the user clicks
        /// it with the mouse, it shouldn't be included
        /// in the TAB traversal chain when using the keyboard.
        /// </returns>
        internal bool AcceptsFocusFromKeyboard
        {
            get
            {
                var result = NativeControl?.AcceptsFocusFromKeyboard;
                return result ?? false;
            }

            set
            {
                if (NativeControl is null)
                    return;
                NativeControl.AcceptsFocusFromKeyboard = value;
            }
        }

        /// <summary>
        /// Indicates whether this control or one of its children accepts focus.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        internal bool AcceptsFocusRecursively
        {
            get
            {
                var result = NativeControl?.AcceptsFocusRecursively;
                return result ?? false;
            }

            set
            {
                if (NativeControl is null)
                    return;
                NativeControl.AcceptsFocusRecursively = value;
            }
        }

        internal bool AcceptsFocusAll
        {
            get
            {
                var result = NativeControl?.AcceptsFocusAll;
                return result ?? false;
            }

            set
            {
                if (NativeControl is null)
                    return;
                NativeControl.AcceptsFocusAll = value;
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
