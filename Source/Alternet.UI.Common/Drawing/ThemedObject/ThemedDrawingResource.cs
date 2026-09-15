using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines an interface for a themed drawing resource that can be defined by a brush, pen, or color.
    /// </summary>
    public interface IThemedDrawingResource : IThemedDrawingObject<IDrawingResource>
    {
        /// <summary>
        /// Gets or sets the title of the themed drawing resource.
        /// </summary>
        public string? Title
        {
            get => Light.Title;
            set
            {
                Light.Title = value;
                Dark.Title = value;
            }
        }
    }

    /// <summary>
    /// Represents a two-value class that has different values for light and dark themes for <see cref="IDrawingResource"/> type.
    /// </summary>
    public class ThemedDrawingResource : ThemedDrawingObject<IDrawingResource>, IThemedDrawingResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the same light and dark values.
        /// </summary>
        /// <param name="value">The value to use for both the light and dark theme variants.</param>
        public ThemedDrawingResource(IDrawingResource value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the specified light and dark values.
        /// </summary>
        /// <param name="light">The value to use for the light theme variant.</param>
        /// <param name="dark">The value to use for the dark theme variant.</param>
        public ThemedDrawingResource(IDrawingResource light, IDrawingResource dark)
            : base(light, dark)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the specified light and dark values.
        /// </summary>
        /// <param name="light">The value to use for the light theme variant.</param>
        /// <param name="dark">The value to use for the dark theme variant.</param>
        /// <param name="title">The title of the themed drawing resource.</param>
        public ThemedDrawingResource(Brush light, Brush dark, string title)
            : this(new DrawingResource(light), new DrawingResource(dark))
        {
            Light.Title = title;
            Dark.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the specified light and dark values.
        /// </summary>
        /// <param name="light">The value to use for the light theme variant.</param>
        /// <param name="dark">The value to use for the dark theme variant.</param>
        /// <param name="title">The title of the themed drawing resource.</param>
        public ThemedDrawingResource(Pen light, Pen dark, string title)
                    : this(new DrawingResource(light), new DrawingResource(dark))
        {
            Light.Title = title;
            Dark.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the specified light and dark values.
        /// </summary>
        /// <param name="light">The value to use for the light theme variant.</param>
        /// <param name="dark">The value to use for the dark theme variant.</param>
        /// <param name="title">The title of the themed drawing resource.</param>
        public ThemedDrawingResource(Color light, Color dark, string title)
                    : this(new DrawingResource(light), new DrawingResource(dark))
        {
            Light.Title = title;
            Dark.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the specified themed color.
        /// </summary>
        /// <param name="color">The themed color to use for the light and dark theme variants.</param>
        /// <param name="title">The title of the themed drawing resource.</param>
        public ThemedDrawingResource(ThemedColor color, string title)
                    : this(new DrawingResource(color.Light), new DrawingResource(color.Dark))
        {
            Light.Title = title;
            Dark.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the specified themed brush.
        /// </summary>
        /// <param name="brush">The themed brush to use for the light and dark theme variants.</param>
        /// <param name="title">The title of the themed drawing resource.</param>
        public ThemedDrawingResource(ThemedBrush brush, string title)
                    : this(new DrawingResource(brush.Light), new DrawingResource(brush.Dark))
        {
            Light.Title = title;
            Dark.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the specified themed pen.
        /// </summary>
        /// <param name="pen">The themed pen to use for the light and dark theme variants.</param>
        /// <param name="title">The title of the themed drawing resource.</param>
        public ThemedDrawingResource(ThemedPen pen, string title)
                    : this(new DrawingResource(pen.Light), new DrawingResource(pen.Dark))
        {
            Light.Title = title;
            Dark.Title = title;
        }

        /// <summary>
        /// Gets or sets the title of the themed drawing resource.
        /// </summary>
        public string? Title
        {
            get => Light.Title;
            set
            {
                Light.Title = value;
                Dark.Title = value;
            }
        }
    }
}
