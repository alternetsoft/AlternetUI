namespace Alternet.Drawing
{
    /// <summary>
    /// Defines functionality that enables transformations in a 2-D plane.
    /// </summary>
    public struct Transform
    {
        // todo

        private Transform(Size translation) => Translation = translation;

        /// <summary>
        /// Gets the translation part of the transform.
        /// </summary>
        public Size Translation { get; }

        /// <summary>
        /// Creates a new transform from a translation value.
        /// </summary>
        public static Transform FromTranslation(Size translation) => new Transform(translation);

        /// <summary>
        /// Creates a new transform from a translation value.
        /// </summary>
        public static Transform FromTranslation(Point translation) => new Transform(new Size(translation.X, translation.Y));
    }
}