using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Encapsulates text layout information (such as alignment and orientation)
    /// display manipulations (such
    /// as ellipsis insertion). This class cannot be inherited.
    /// </summary>
    public class TextFormat : BaseObject
    {
        /// <summary>
        /// Gets default horizontal alignment of the text;
        /// </summary>
        public const TextHorizontalAlignment DefaultHorizontalAlignment = TextHorizontalAlignment.Left;

        /// <summary>
        /// Gets default vertical alignment of the text;
        /// </summary>
        public const TextVerticalAlignment DefaultVerticalAlignment = TextVerticalAlignment.Top;

        /// <summary>
        /// Gets default trimming style of the text;
        /// </summary>
        public const TextTrimming DefaultTrimming = TextTrimming.None;

        /// <summary>
        /// Gets default wrapping style of the text;
        /// </summary>
        public const TextWrapping DefaultWrapping = TextWrapping.Character;

        private TextHorizontalAlignment horizontalAlignment = DefaultHorizontalAlignment;
        private TextVerticalAlignment verticalAlignment = DefaultVerticalAlignment;
        private TextTrimming trimming = DefaultTrimming;
        private TextWrapping wrapping = DefaultWrapping;

        /// <summary>
        /// Gets or sets horizontal alignment of the text.
        /// Default is <see cref="TextHorizontalAlignment.Left"/>.
        /// </summary>
        /// <value>
        /// A <see cref="TextHorizontalAlignment"/> enumeration that specifies the
        /// horizontal alignment of the string.
        /// </value>
        public virtual TextHorizontalAlignment HorizontalAlignment
        {
            get
            {
                return horizontalAlignment;
            }

            set
            {
                if (horizontalAlignment == value)
                    return;
                horizontalAlignment = value;
                Changed();
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the text.
        /// </summary>
        /// <value>
        /// A <see cref="TextVerticalAlignment"/> enumeration that represents the vertical alignment.
        /// Default is <see cref="TextVerticalAlignment.Top"/>.
        /// </value>
        public virtual TextVerticalAlignment VerticalAlignment
        {
            get
            {
                return verticalAlignment;
            }

            set
            {
                if (verticalAlignment == value)
                    return;
                verticalAlignment = value;
                Changed();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TextTrimming"/> enumeration value for this object.
        /// Default is <see cref="TextTrimming.None"/>.
        /// </summary>
        /// <value>
        /// A <see cref="TextTrimming"/> enumeration that indicates how text
        /// is trimmed when it exceeds the edges of the layout rectangle.
        /// </value>
        public virtual TextTrimming Trimming
        {
            get
            {
                return trimming;
            }

            set
            {
                if (trimming == value)
                    return;
                trimming = value;
                Changed();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TextWrapping"/> enumeration value for this object.
        /// Default is <see cref="TextWrapping.Character"/>.
        /// </summary>
        /// <value>
        /// A <see cref="TextWrapping"/> enumeration that indicates how text is
        /// wrapped when it exceeds the edges
        /// of the layout rectangle.
        /// </value>
        public virtual TextWrapping Wrapping
        {
            get
            {
                return wrapping;
            }

            set
            {
                if (wrapping == value)
                    return;
                wrapping = value;
                Changed();
            }
        }

        /// <summary>
        /// Called to reset internal structures when properties are changed.
        /// </summary>
        public virtual void Changed()
        {
        }

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public virtual TextFormat Clone()
        {
            TextFormat result = new();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Assigns this object with properties of other <see cref="TextFormat"/>.
        /// </summary>
        /// <param name="value">Source of the property values to assign.</param>
        public virtual void Assign(TextFormat value)
        {
            horizontalAlignment = value.horizontalAlignment;
            verticalAlignment = value.verticalAlignment;
            trimming = value.trimming;
            wrapping = value.wrapping;
            Changed();
        }
    }
}