﻿using System;
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

        /// <summary>
        /// Gets a <see cref="WxControlHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal virtual BaseControlHandler Handler
        {
            get
            {
                EnsureHandlerCreated();
                return handler ?? throw new InvalidOperationException();
            }
        }

        internal bool ProcessUIUpdates
        {
            get
            {
                return GetNative().GetProcessUIUpdates(this);
            }

            set
            {
                GetNative().SetProcessUIUpdates(this, value);
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
                return GetNative().GetBorderStyle(this);
            }

            set
            {
                GetNative().SetBorderStyle(this, value);
            }
        }

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
