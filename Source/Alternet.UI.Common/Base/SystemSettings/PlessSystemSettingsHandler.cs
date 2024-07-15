using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements dummy <see cref="ISystemSettingsHandler"/> provider.
    /// </summary>
    public abstract class PlessSystemSettingsHandler : DisposableObject, ISystemSettingsHandler
    {
        private readonly int[] metrics = new int[(int)SystemSettingsMetric.Max + 1];

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessSystemSettingsHandler"/> class.
        /// </summary>
        public PlessSystemSettingsHandler()
        {
            Reset();
        }

        /// <inheritdoc/>
        public virtual string AppName { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual string AppDisplayName { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual string AppClassName { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual string VendorName { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual string VendorDisplayName { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual bool UseBestVisual { get; set; } = true;

        /// <inheritdoc/>
        public abstract IDisplayFactoryHandler CreateDisplayFactoryHandler();

        /// <inheritdoc/>
        public virtual bool GetAppearanceIsDark()
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual string GetAppearanceName()
        {
            return "Default";
        }

        /// <inheritdoc/>
        public virtual Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return SystemColors.Window;
        }

        /// <inheritdoc/>
        public virtual Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return SystemColors.WindowText;
        }

        /// <inheritdoc/>
        public virtual Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual ColorStruct? GetColor(KnownSystemColor index)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual LangDirection GetLangDirection()
        {
            return LangDirection.LeftToRight;
        }

        /// <inheritdoc/>
        public virtual string GetLibraryVersionString()
        {
            return "1.0";
        }

        /// <inheritdoc/>
        public virtual int GetMetric(SystemSettingsMetric index, Control? control)
        {
            return GetMetric(index);
        }

        /// <summary>
        /// Sets metric value.
        /// </summary>
        /// <param name="index">Metric id.</param>
        /// <param name="value">New metric value.</param>
        public virtual void SetMetric(SystemSettingsMetric index, int value)
        {
            metrics[(int)index] = value;
        }

        /// <inheritdoc/>
        public virtual int GetMetric(SystemSettingsMetric index)
        {
            return metrics[(int)index];
        }

        /// <summary>
        /// Resets internal metrics to the default values.
        /// </summary>
        public virtual void Reset()
        {
            SetMetric(SystemSettingsMetric.MouseButtons, 8);
            SetMetric(SystemSettingsMetric.BorderX, 1);
            SetMetric(SystemSettingsMetric.BorderY, 1);
            SetMetric(SystemSettingsMetric.CursorX, 64);
            SetMetric(SystemSettingsMetric.CursorY, 64);
            SetMetric(SystemSettingsMetric.DClickX, 4);
            SetMetric(SystemSettingsMetric.DClickY, 4);
            SetMetric(SystemSettingsMetric.DragX, 4);
            SetMetric(SystemSettingsMetric.DragY, 4);
            SetMetric(SystemSettingsMetric.EdgeX, 2);
            SetMetric(SystemSettingsMetric.EdgeY, 2);
            SetMetric(SystemSettingsMetric.HScrollArrowX, 34);
            SetMetric(SystemSettingsMetric.HScrollArrowY, 34);
            SetMetric(SystemSettingsMetric.HThumbX, 34);
            SetMetric(SystemSettingsMetric.IconX, 64);
            SetMetric(SystemSettingsMetric.IconY, 64);
            SetMetric(SystemSettingsMetric.IconSpacingX, 150);
            SetMetric(SystemSettingsMetric.IconSpacingY, 150);
            SetMetric(SystemSettingsMetric.WindowMinX, 258);
            SetMetric(SystemSettingsMetric.WindowMinY, 71);
            SetMetric(SystemSettingsMetric.FrameSizeX, 5);
            SetMetric(SystemSettingsMetric.FrameSizeY, 5);
            SetMetric(SystemSettingsMetric.SmallIconX, 32);
            SetMetric(SystemSettingsMetric.SmallIconY, 32);
            SetMetric(SystemSettingsMetric.HScrollY, 34);
            SetMetric(SystemSettingsMetric.VScrollX, 34);
            SetMetric(SystemSettingsMetric.VScrollArrowX, 34);
            SetMetric(SystemSettingsMetric.VScrollArrowY, 34);
            SetMetric(SystemSettingsMetric.VThumbY, 34);
            SetMetric(SystemSettingsMetric.CaptionY, 45);
            SetMetric(SystemSettingsMetric.MenuY, 39);
            SetMetric(SystemSettingsMetric.NetworkPresent, 1);
            SetMetric(SystemSettingsMetric.PenWindowsPresent, 0);
            SetMetric(SystemSettingsMetric.ShowSounds, 0);
            SetMetric(SystemSettingsMetric.SwapButtons, 0);
            SetMetric(SystemSettingsMetric.DClickMSec, 500);
            SetMetric(SystemSettingsMetric.CaretOnMSec, 530);
            SetMetric(SystemSettingsMetric.CaretOffMSec, 530);
            SetMetric(SystemSettingsMetric.CaretTimeoutMSec, -1);
        }

        /// <inheritdoc/>
        public virtual UIPlatformKind GetPlatformKind()
        {
            return UIPlatformKind.Plarformless;
        }

        /// <inheritdoc/>
        public virtual string? GetUIVersion()
        {
            return "1.0";
        }

        /// <inheritdoc/>
        public virtual bool HasFeature(SystemSettingsFeature index)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool IsUsingDarkBackground()
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool SetNativeTheme(string theme)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual void SetSystemOption(string name, int value)
        {
        }

        /// <inheritdoc/>
        public virtual void SetUseBestVisual(bool flag, bool forceTrueColour = false)
        {
        }

        /// <inheritdoc/>
        public virtual void SuppressBellOnError(bool value)
        {
        }
    }
}
