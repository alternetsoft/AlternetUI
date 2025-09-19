using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements internal <see cref="ISystemSettingsHandler"/> provider.
    /// </summary>
    public class PlessSystemSettingsHandler : DisposableObject, ISystemSettingsHandler
    {
        private static readonly bool[] metricScaled = new bool[(int)SystemSettingsMetric.Max + 1];

        private readonly int[] metrics = new int[(int)SystemSettingsMetric.Max + 1];

        static PlessSystemSettingsHandler()
        {
            SetMetricScaled(SystemSettingsMetric.HScrollArrowX);
            SetMetricScaled(SystemSettingsMetric.HScrollArrowY);
            SetMetricScaled(SystemSettingsMetric.HThumbX);
            SetMetricScaled(SystemSettingsMetric.HScrollY);
            SetMetricScaled(SystemSettingsMetric.VScrollX);
            SetMetricScaled(SystemSettingsMetric.VScrollArrowX);
            SetMetricScaled(SystemSettingsMetric.VScrollArrowY);
            SetMetricScaled(SystemSettingsMetric.VThumbY);

            SetMetricScaled(SystemSettingsMetric.CursorX);
            SetMetricScaled(SystemSettingsMetric.CursorY);

            SetMetricScaled(SystemSettingsMetric.IconX);
            SetMetricScaled(SystemSettingsMetric.IconY);

            SetMetricScaled(SystemSettingsMetric.IconSpacingX);
            SetMetricScaled(SystemSettingsMetric.IconSpacingY);

            SetMetricScaled(SystemSettingsMetric.WindowMinX);
            SetMetricScaled(SystemSettingsMetric.WindowMinY);

            SetMetricScaled(SystemSettingsMetric.FrameSizeX);
            SetMetricScaled(SystemSettingsMetric.FrameSizeY);

            SetMetricScaled(SystemSettingsMetric.SmallIconX);
            SetMetricScaled(SystemSettingsMetric.SmallIconY);

            SetMetricScaled(SystemSettingsMetric.CaptionY);
            SetMetricScaled(SystemSettingsMetric.MenuY);
        }

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

        /// <summary>
        /// Sets whether metric value is scaled.
        /// </summary>
        /// <param name="index">Metric id.</param>
        /// <param name="value">Whether metric is scaled.</param>
        public static void SetMetricScaled(SystemSettingsMetric index, bool value = true)
        {
            metricScaled[(int)index] = value;
        }

        /// <summary>
        /// Gets whether metric value is scaled.
        /// </summary>
        /// <param name="index">Metric id.</param>
        public static bool IsMetricScaled(SystemSettingsMetric index)
        {
            return metricScaled[(int)index];
        }

        /// <inheritdoc/>
        public virtual IDisplayFactoryHandler CreateDisplayFactoryHandler()
        {
            return new PlessDisplayFactoryHandler();
        }

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
        public virtual int GetMetric(SystemSettingsMetric index, AbstractControl? control)
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
        /// Sets all scroll bar related metrics to the specified value.
        /// </summary>
        /// <param name="scrollMetric"></param>
        public virtual void SetAllScrollBarMetrics(int scrollMetric)
        {
            SetMetric(SystemSettingsMetric.HScrollArrowX, scrollMetric);
            SetMetric(SystemSettingsMetric.HScrollArrowY, scrollMetric);
            SetMetric(SystemSettingsMetric.HThumbX, scrollMetric);
            SetMetric(SystemSettingsMetric.HScrollY, scrollMetric);
            SetMetric(SystemSettingsMetric.VScrollX, scrollMetric);
            SetMetric(SystemSettingsMetric.VScrollArrowX, scrollMetric);
            SetMetric(SystemSettingsMetric.VScrollArrowY, scrollMetric);
            SetMetric(SystemSettingsMetric.VThumbY, scrollMetric);
        }

        /// <summary>
        /// Resets internal metrics to the default values.
        /// </summary>
        public virtual void Reset()
        {
            SetAllScrollBarMetrics(17); // 34

            SetMetric(SystemSettingsMetric.BorderX, 1); // same on high dpi
            SetMetric(SystemSettingsMetric.BorderY, 1); // same on high dpi

            SetMetric(SystemSettingsMetric.CursorX, 32); // 64
            SetMetric(SystemSettingsMetric.CursorY, 32); // 64

            SetMetric(SystemSettingsMetric.DClickX, 4);
            SetMetric(SystemSettingsMetric.DClickY, 4);

            SetMetric(SystemSettingsMetric.DragX, 4); // same on high dpi
            SetMetric(SystemSettingsMetric.DragY, 4); // same on high dpi

            SetMetric(SystemSettingsMetric.EdgeX, 2); // same on high dpi
            SetMetric(SystemSettingsMetric.EdgeY, 2); // same on high dpi

            SetMetric(SystemSettingsMetric.IconX, 32); // 64
            SetMetric(SystemSettingsMetric.IconY, 32); // 64

            SetMetric(SystemSettingsMetric.IconSpacingX, 75); // 150
            SetMetric(SystemSettingsMetric.IconSpacingY, 75); // 150

            SetMetric(SystemSettingsMetric.WindowMinX, 136); // 258
            SetMetric(SystemSettingsMetric.WindowMinY, 39); // 71

            SetMetric(SystemSettingsMetric.FrameSizeX, 4); // 5
            SetMetric(SystemSettingsMetric.FrameSizeY, 4); // 5

            SetMetric(SystemSettingsMetric.SmallIconX, 16); // 32
            SetMetric(SystemSettingsMetric.SmallIconY, 16); // 32

            SetMetric(SystemSettingsMetric.CaptionY, 23); // 45
            SetMetric(SystemSettingsMetric.MenuY, 20); // 39

            SetMetric(SystemSettingsMetric.MouseButtons, 8);
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
            return UIPlatformKind.Unspecified;
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
    }
}
