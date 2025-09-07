using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Allows to build sequence of drawable elements.
    /// </summary>
    internal abstract class DrawableElementBuilder : BaseObject
    {
        private readonly List<IDrawableElement> items = new();
        private readonly DrawableStackElement root;

        public DrawableElementBuilder(
            CoordAlignment alignment = CoordAlignment.Near,
            Coord distance = 0,
            bool isVertical = true)
        {
            root = new DrawableStackElement(items, alignment, distance, isVertical);
        }

        public bool IsVertical
        {
            get
            {
                return root.IsVertical;
            }

            set
            {
                root.IsVertical = value;
            }
        }

        public virtual Coord Distance
        {
            get
            {
                return root.Distance;
            }

            set
            {
                root.Distance = value;
            }
        }

        public virtual IDrawableElement Root
        {
            get
            {
                return root;
            }
        }

        public virtual void Clear()
        {
            items.Clear();
        }

        /*
        public virtual DrawableElementBuilder Text(object text)
        {
            return Add(DrawableTextElement.Create(text));
        }

        public virtual DrawableElementBuilder Text(string[] value)
        {
            return Add(DrawableTextElement.Create(value));
        }

        public virtual DrawableElementBuilder Text(
            IEnumerable<object> strings,
            Coord distance = 0,
            bool isVertical = true)
        {
            return Add(DrawableTextElement.Create(strings, distance, isVertical));
        }

        public virtual DrawableElementBuilder Block(
                IEnumerable<DrawableTextElement> items,
                Coord distance = 0,
                bool isVertical = true)
        {
            return Add(DrawableTextElement.Create(items, distance, isVertical));
        }

        public virtual DrawableElementBuilder Text(object s, FontStyle fontStyle, Brush? brush)
        {
            return Add(DrawableTextElement.Create(s, fontStyle, brush));
        }

        public virtual DrawableElementBuilder Text(object s, Font? font, Brush? brush)
        {
            return Add(root.Create(s, font, brush));
        }

        public virtual DrawableElementBuilder Text(
            object s,
            Font? font,
            Color? foreColor,
            Color? backColor)
        {
            return Add(Root.Create(s, font, foreColor, backColor));
        }

        public virtual DrawableElementBuilder Text(
            object s,
            FontStyle fontStyle,
            Color? foreColor,
            Color? backColor)
        {
            return Add(Root.Create(s, fontStyle, foreColor, backColor));
        }*/

        public virtual DrawableElementBuilder Add(DrawableTextElement item)
        {
            items.Add(item);
            return this;
        }

        /*
        public abstract StyledTextBuilder GetLastId(ref ObjectUniqueId)

        public abstract StyledTextBuilder GetLastText(ref StyledText)

        public abstract StyledTextBuilder Image(Image image);

        public abstract StyledTextBuilder Svg(SvgImage? image, SizeD size);

        public abstract StyledTextBuilder PushStyle();

        public abstract StyledTextBuilder PopStyle();

        public abstract StyledTextBuilder UrlStyle(Style style);

        public abstract StyledTextBuilder Style(Style style);

        public abstract StyledTextBuilder BeginStyle(Style style);

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
        */
    }
}
