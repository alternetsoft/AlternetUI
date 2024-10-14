using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal class SimpleStyledText : StyledText, IStyledText
    {
        private object text;
        private SizeD? measure;
        private Coord scaleFactor;
        private ObjectUniqueId defaultFont;

        public SimpleStyledText(object text)
        {
            this.text = text;
        }

        public SimpleStyledText()
        {
            text = string.Empty;
        }

        public virtual object Text
        {
            get
            {
                return text;
            }

            set
            {
                if (text == value)
                    return;
                text = value;
                Changed();
            }
        }

        public virtual void Draw(
            Graphics dc,
            PointD location,
            Font? font,
            Color? foreColor,
            Color? backColor)
        {
            dc.DrawText(
                Text.ToString(),
                location,
                SafeFont(font),
                SafeForeColor(foreColor),
                SafeBackColor(backColor));
        }

        public virtual void Draw(Graphics dc, PointD location, Font? font, Brush? foreBrush)
        {
            dc.DrawText(Text.ToString(), SafeFont(font), SafeForeBrush(foreBrush), location);
        }

        public virtual SizeD Measure(Graphics dc, Font? font)
        {
            font = SafeFont(font);
            var fontId = font.UniqueId;
            var newScaleFactor = dc.ScaleFactor;

            if (measure is null || defaultFont != fontId || scaleFactor != newScaleFactor)
            {
                defaultFont = fontId;
                measure = dc.MeasureText(Text.ToString(), font);
                scaleFactor = newScaleFactor;
            }

            return measure.Value;
        }

        public virtual void Changed()
        {
            measure = null;
        }
    }
}
