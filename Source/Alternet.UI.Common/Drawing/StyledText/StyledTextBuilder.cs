using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Allows to build sequence of <see cref="StyledText"/> objects.
    /// </summary>
    internal abstract class StyledTextBuilder : BaseObject
    {
        public StyledTextBuilder()
        {
        }

        public virtual StyledTextBuilder Text(object text)
        {
            return Add(StyledText.Create(text));
        }

        public virtual StyledTextBuilder Text(string[] value)
        {
            return Add(StyledText.Create(value));
        }

        public virtual StyledTextBuilder Text(
            IEnumerable<object> strings,
            Coord distance = 0,
            bool isVertical = true)
        {
            return Add(StyledText.Create(strings, distance, isVertical));
        }

        public virtual StyledTextBuilder Block(
                IEnumerable<StyledText> items,
                Coord distance = 0,
                bool isVertical = true)
        {
            return Add(StyledText.Create(items, distance, isVertical));
        }

        public virtual StyledTextBuilder Text(object s, FontStyle fontStyle, Brush? brush)
        {
            return Add(StyledText.Create(s, fontStyle, brush));
        }

        public virtual StyledTextBuilder Text(object s, Font? font, Brush? brush)
        {
            return Add(StyledText.Create(s, font, brush));
        }

        public virtual StyledTextBuilder Text(
            object s,
            Font? font,
            Color? foreColor,
            Color? backColor)
        {
            return Add(StyledText.Create(s, font, foreColor, backColor));
        }

        public virtual StyledTextBuilder Text(
            object s,
            FontStyle fontStyle,
            Color? foreColor,
            Color? backColor)
        {
            return Add(StyledText.Create(s, fontStyle, foreColor, backColor));
        }

        public abstract StyledTextBuilder Add(StyledText item);

        public abstract StyledTextBuilder Image(Image image);

        public abstract StyledTextBuilder Svg(SvgImage? image, SizeD size);

        public abstract StyledTextBuilder PushStyle();

        public abstract StyledTextBuilder PopStyle();

        /*
                public abstract StyledTextBuilder UrlStyle(Style style);

                public abstract StyledTextBuilder Style(Style style);

                public abstract StyledTextBuilder BeginStyle(Style style);
        */

        public abstract StyledText ToStyledText();

        public abstract StyledTextBuilder Url(object title, object url);

        public abstract StyledTextBuilder Url(object title, Action clickAction);

        public abstract StyledTextBuilder EndStyle();

        public abstract StyledTextBuilder Brush(Brush? value);

        public abstract StyledTextBuilder Font(Font? value);

        public abstract StyledTextBuilder FontStyle(FontStyle? value);

        public abstract StyledTextBuilder ForeColor(Color? value);

        public abstract StyledTextBuilder BackColor(Color? value);

        public abstract StyledTextBuilder FontAndColor(IReadOnlyFontAndColor? value);

        public abstract StyledTextBuilder Spacer(SizeD size);

        public abstract StyledTextBuilder BreakLine();

        public abstract StyledTextBuilder BeginBold();

        public abstract StyledTextBuilder EndBold();

        public abstract StyledTextBuilder BeginUnderlined();

        public abstract StyledTextBuilder EndUnderlined();

        public abstract StyledTextBuilder BeginFontStyle(FontStyle style);

        public abstract StyledTextBuilder EndFontStyle();

        public abstract StyledTextBuilder BeginBackColor();

        public abstract StyledTextBuilder EndBackColor();

        public abstract StyledTextBuilder BeginForeColor();

        public abstract StyledTextBuilder EndForeColor();

        public abstract StyledTextBuilder BeginForeBrush();

        public abstract StyledTextBuilder EndForeBrush();
    }
}
