using Alternet.Drawing;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines functionality that enables transformations in a 2-D plane.
    /// </summary>
    public struct Transform
    {
        // todo

        private Transform(SizeF translation) => Translation = translation;

        /// <summary>
        /// Gets the translation part of the transform.
        /// </summary>
        public SizeF Translation { get; }

        /// <summary>
        /// Creates a new transform from a translation value.
        /// </summary>
        public static Transform FromTranslation(SizeF translation) => new Transform(translation);

        /// <summary>
        /// Creates a new transform from a translation value.
        /// </summary>
        public static Transform FromTranslation(PointF translation) => new Transform(new SizeF(translation.X, translation.Y));
    }
}