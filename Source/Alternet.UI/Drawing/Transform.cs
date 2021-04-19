using System.Drawing;

namespace Alternet.UI
{
    public struct Transform
    {
        // todo

        private Transform(SizeF translation) => Translation = translation;

        public SizeF Translation { get; }

        public static Transform FromTranslation(SizeF translation) => new Transform(translation);
        public static Transform FromTranslation(PointF translation) => new Transform(new SizeF(translation.X, translation.Y));
    }
}