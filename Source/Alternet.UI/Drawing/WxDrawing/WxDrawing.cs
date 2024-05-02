using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxDrawing : NativeDrawing
    {
        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;
            NativeDrawing.Default = new WxDrawing();
            initialized = true;
        }

        public override Graphics CreateGraphicsFromScreen()
        {
            return new WxGraphics(UI.Native.DrawingContext.FromScreen());
        }

        public override Graphics CreateGraphicsFromImage(Image image)
        {
            return new WxGraphics(
                UI.Native.DrawingContext.FromImage(
                    (UI.Native.Image)image.NativeObject));
        }

        /// <inheritdoc/>
        public override object CreatePen() => new UI.Native.Pen();

        /// <inheritdoc/>
        public override Color GetColor(SystemSettingsColor index)
        {
            return SystemSettings.GetColor(index);
        }

        /// <inheritdoc/>
        public override void UpdatePen(Pen pen)
        {
            ((UI.Native.Pen)pen.NativeObject).Initialize(
                (UI.Native.PenDashStyle)pen.DashStyle,
                pen.Color,
                pen.Width,
                (UI.Native.LineCap)pen.LineCap,
                (UI.Native.LineJoin)pen.LineJoin);
        }
    }
}