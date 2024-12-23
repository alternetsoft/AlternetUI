using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class ScrollBar
    {
        /// <summary>
        /// Gets size of the scrollbar corner for the specified control.
        /// </summary>
        /// <param name="control">Control to use for getting scrollbar metrics.</param>
        /// <returns></returns>
        public static SizeD GetCornerSize(AbstractControl control)
        {
            var hScrollY = SystemSettings.GetMetric(SystemSettingsMetric.HScrollY, control);
            var vScrollX = SystemSettings.GetMetric(SystemSettingsMetric.VScrollX, control);

            SizeD result = new();
            result.Width = GraphicsFactory.PixelToDip(vScrollX, control.ScaleFactor);
            result.Height = GraphicsFactory.PixelToDip(hScrollY, control.ScaleFactor);
            return result;
        }

        /// <summary>
        /// Contains properties which specify different scrollbar metrics.
        /// </summary>
        /// <typeparam name="T">Type of the metrics value.</typeparam>
        public class MetricsInfo<T>
            where T : struct
        {
            /// <summary>
            /// Height of horizontal scrollbar in pixels.
            /// </summary>
            public T HScrollY;

            /// <summary>
            /// Width of vertical scrollbar in pixels.
            /// </summary>
            public T VScrollX;

            /// <summary>
            /// Width of arrow bitmap on a vertical scrollbar.
            /// </summary>
            public T VScrollArrowX;

            /// <summary>
            /// Height of arrow bitmap on a vertical scrollbar.
            /// </summary>
            public T VScrollArrowY;

            /// <summary>
            /// Height of vertical scrollbar thumb.
            /// </summary>
            public T VThumbY;

            /// <summary>
            /// Width of arrow bitmap on horizontal scrollbar.
            /// </summary>
            public T HScrollArrowX;

            /// <summary>
            /// Height of arrow bitmap on horizontal scrollbar.
            /// </summary>
            public T HScrollArrowY;

            /// <summary>
            /// Width of horizontal scrollbar thumb.
            /// </summary>
            public T HThumbX;
        }

        /// <summary>
        /// Contains properties which specify different scrollbar metrics.
        /// </summary>
        public class MetricsInfo : MetricsInfo<int>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MetricsInfo"/> struct.
            /// </summary>
            public MetricsInfo(AbstractControl control)
            {
                Reset(control);
            }

            /// <summary>
            /// Logs scroll bar metrics.
            /// </summary>
            public void Log()
            {
                var info = this;

                App.LogSection(
                    () =>
                    {
                        App.LogNameValue(
                            "HScrollY",
                            info.HScrollY,
                            null,
                            "Height of horizontal scrollbar in pixels");

                        App.LogNameValue(
                            "VScrollX",
                            info.VScrollX,
                            null,
                            "Width of vertical scrollbar in pixels");

                        App.LogNameValue(
                            "VScrollArrowX",
                            info.VScrollArrowX,
                            null,
                            "Width of arrow bitmap on a vertical scrollbar");

                        App.LogNameValue(
                            "VScrollArrowY",
                            info.VScrollArrowY,
                            null,
                            "Height of arrow bitmap on a vertical scrollbar");

                        App.LogNameValue(
                            "VThumbY",
                            info.VThumbY,
                            null,
                            "Height of vertical scrollbar thumb");

                        App.LogNameValue(
                            "HScrollArrowX",
                            info.HScrollArrowX,
                            null,
                            "Width of arrow bitmap on horizontal scrollbar");

                        App.LogNameValue(
                            "HScrollArrowY",
                            info.HScrollArrowY,
                            null,
                            "Height of arrow bitmap on horizontal scrollbar");

                        App.LogNameValue(
                            "HThumbX",
                            info.HThumbX,
                            null,
                            "Width of horizontal scrollbar thumb");

                        App.LogNameValue("info.GetPreferredSize(vert)", info.GetPreferredSize(true));
                        App.LogNameValue("info.GetPreferredSize(horz)", info.GetPreferredSize(false));
                        App.LogNameValue("info.GetArrowBitmapSize(vert)", info.GetArrowBitmapSize(true));
                        App.LogNameValue("info.GetArrowBitmapSize(horz)", info.GetArrowBitmapSize(false));
                        App.LogNameValue(
                            "info.GetThumbSize(vert,(50,50))",
                            info.GetThumbSize(true, (50, 50)));
                        App.LogNameValue(
                            "info.GetThumbSize(horz,(50,50))",
                            info.GetThumbSize(false, (50, 50)));
                    },
                    "ScrollBar metrics");
            }

            /// <summary>
            /// Resets all properties, reloading them from the system settings.
            /// </summary>
            public virtual void Reset(AbstractControl control)
            {
                HScrollY = SystemSettings.GetMetric(SystemSettingsMetric.HScrollY, control);
                VScrollX = SystemSettings.GetMetric(SystemSettingsMetric.VScrollX, control);
                VScrollArrowX = SystemSettings.GetMetric(SystemSettingsMetric.VScrollArrowX, control);
                VScrollArrowY = SystemSettings.GetMetric(SystemSettingsMetric.VScrollArrowY, control);
                VThumbY = SystemSettings.GetMetric(SystemSettingsMetric.VThumbY, control);
                HScrollArrowX = SystemSettings.GetMetric(SystemSettingsMetric.HScrollArrowX, control);
                HScrollArrowY = SystemSettings.GetMetric(SystemSettingsMetric.HScrollArrowY, control);
                HThumbX = SystemSettings.GetMetric(SystemSettingsMetric.HThumbX, control);
            }

            /// <summary>
            /// Gets preferred size of the scrollbar.
            /// Height for the vertical scroll bar is returned as NaN.
            /// Width for the horizontal scrollbar is returned as Nan.
            /// </summary>
            /// <returns></returns>
            public SizeD GetPreferredSize(bool isVertical, Coord? scaleFactor = null)
            {
                Coord width;
                Coord height;

                if (isVertical)
                {
                    width = GraphicsFactory.PixelToDip(VScrollX, scaleFactor);
                    height = Coord.NaN;
                }
                else
                {
                    width = Coord.NaN;
                    height = GraphicsFactory.PixelToDip(HScrollY, scaleFactor);
                }

                return new(width, height);
            }

            /// <summary>
            /// Gets size of the arrow bitmap.
            /// </summary>
            /// <returns></returns>
            public SizeD GetArrowBitmapSize(bool isVertical, Coord? scaleFactor = null)
            {
                Coord width;
                Coord height;

                if (isVertical)
                {
                    width = GraphicsFactory.PixelToDip(VScrollArrowX, scaleFactor);
                    height = GraphicsFactory.PixelToDip(VScrollArrowY, scaleFactor);
                }
                else
                {
                    width = GraphicsFactory.PixelToDip(HScrollArrowX, scaleFactor);
                    height = GraphicsFactory.PixelToDip(HScrollArrowY, scaleFactor);
                }

                return new(width, height);
            }

            /// <summary>
            /// Gets size of the scroll thumb from the system metrics.
            /// </summary>
            /// <returns></returns>
            /// <param name="isVertical">Whether scroll bar is vertical or horizontal.</param>
            /// <param name="size">Size of the scroll bar.</param>
            /// <param name="scaleFactor">Scaling factor. Optional.</param>
            /// <returns></returns>
            public SizeD GetThumbSize(
                bool isVertical,
                SizeD size,
                Coord? scaleFactor = null)
            {
                Coord width;
                Coord height;

                if (isVertical)
                {
                    width = size.Width;
                    height = GraphicsFactory.PixelToDip(VThumbY, scaleFactor);
                }
                else
                {
                    width = GraphicsFactory.PixelToDip(HThumbX, scaleFactor);
                    height = size.Height;
                }

                return new(width, height);
            }
        }
    }
}
