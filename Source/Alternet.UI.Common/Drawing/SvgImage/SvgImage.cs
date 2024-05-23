using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Impements methods and properties to work with toolbar svg images.
    /// These are rectangular images with small size. This class allows
    /// to speed up loading and getting of different states (and sizes) of the
    /// same image.
    /// </summary>
    public class SvgImage : BaseObject
    {
        private string? url;
        private string? svg;
        private Data?[] data = new Data?[16];
        private bool wasLoaded;
        private Color?[]? colorOverridesLight;
        private Color?[]? colorOverridesDark;

        internal SvgImage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImage"/> class.
        /// </summary>
        /// <param name="urlOrData">Image url or data.</param>
        /// <param name="kind">Image data kind.</param>
        protected SvgImage(string urlOrData, SvgImageDataKind kind = SvgImageDataKind.Url)
        {
            switch (kind)
            {
                case SvgImageDataKind.Url:
                    url = urlOrData;
                    break;
                case SvgImageDataKind.Data:
                    wasLoaded = true;
                    svg = urlOrData;
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImage"/> class.
        /// </summary>
        /// <param name="stream">Stream with image data.</param>
        protected SvgImage(Stream stream)
        {
            wasLoaded = true;
            using var reader = new StreamReader(stream);
            svg = reader.ReadToEnd();
        }

        /// <summary>
        /// Gets whether image has single color.
        /// </summary>
        public virtual bool IsMono => NumOfColors == SvgImageNumOfColors.One;

        /// <summary>
        /// Gets number of colors in svg, passed in the constructor.
        /// </summary>
        public virtual SvgImageNumOfColors NumOfColors => SvgImageNumOfColors.Uknown;

        /// <summary>
        /// Gets image url.
        /// </summary>
        public virtual string? Url => url;

        /// <summary>
        /// Gets svg image as <see cref="ImageSet"/> with default toolbar image size.
        /// </summary>
        public virtual ImageSet? AsImageSet()
            => AsImageSet(ToolBarUtils.GetDefaultImageSize().Width);

        /// <summary>
        /// Gets svg image as <see cref="Image"/> with default toolbar image size.
        /// </summary>
        public virtual Image? AsImage()
            => AsImage(ToolBarUtils.GetDefaultImageSize().Width);

        /// <summary>
        /// Gets svg image as <see cref="Image"/>.
        /// </summary>
        /// <param name="size">Image size</param>
        /// <returns></returns>
        public virtual Image? AsImage(int size)
            => AsImageSet(size)?.AsImage();

        /// <summary>
        /// Gets svg image as <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="size">Image size</param>
        /// <returns></returns>
        public virtual ImageSet? AsImageSet(int size)
        {
            Resize(size);
            var result = data[size]?.OriginalImage;
            if (result is not null)
                return result;
            result = LoadImage(size);
            data[size] ??= new();
            data[size]!.OriginalImage = result;
            return result;
        }

        /// <summary>
        /// Gets image with the specified size and <see cref="KnownSvgColor.Normal"/> color.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual ImageSet? AsNormal(int size, bool isDark)
        {
            return AsImageSet(size, KnownSvgColor.Normal, isDark);
        }

        /// <summary>
        /// Gets image with the specified size and <see cref="KnownSvgColor.Disabled"/> color.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual ImageSet? AsDisabled(int size, bool isDark)
        {
            return AsImageSet(size, KnownSvgColor.Disabled, isDark);
        }

        /// <summary>
        /// Gets image with the specified size and <see cref="KnownSvgColor.Normal"/> color.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual Image? AsNormalImage(int size, bool isDark)
        {
            return AsImageSet(size, KnownSvgColor.Normal, isDark)?.AsImage();
        }

        /// <summary>
        /// Gets image with the specified size and <see cref="KnownSvgColor.Disabled"/> color.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual Image? AsDisabledImage(int size, bool isDark)
        {
            return AsImageSet(size, KnownSvgColor.Disabled, isDark)?.AsImage();
        }

        /// <summary>
        /// Gets image with the specified size and known svg color.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="knownColor">Known image color.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual ImageSet? AsImageSet(int size, KnownSvgColor knownColor, bool isDark)
        {
            if (!IsMono && knownColor == KnownSvgColor.Normal)
                return AsImageSet(size);

            Resize(size);

            ImageSet? result;

            if (isDark)
                result = data[size]?.KnownColorImagesDark[(int)knownColor];
            else
                result = data[size]?.KnownColorImagesLight[(int)knownColor];

            if (result is not null)
                return result;

            var color = GetSvgColor(knownColor, isDark);
            result = LoadImage(size, color);
            data[size] ??= new();

            if(isDark)
                data[size]!.KnownColorImagesDark[(int)knownColor] = result;
            else
                data[size]!.KnownColorImagesLight[(int)knownColor] = result;
            return result;
        }

        /// <summary>
        /// Gets mono svg image as <see cref="ImageSet"/> filled using the specified color.
        /// Svg images with two or more colors are returned as is.
        /// </summary>
        /// <param name="size">Svg image size.</param>
        /// <param name="color">Svg image color.</param>
        /// <returns></returns>
        public virtual ImageSet? ImageSetWithColor(int size, Color color)
        {
            if(!IsMono)
                return AsImageSet(size);

            Resize(size);

            data[size]!.ColoredImages ??= new();

            var images = data[size]!.ColoredImages!;

            if (images.TryGetValue(color, out var result))
                return result;

            result = LoadImage(size, color);
            if(result is not null)
                images.Add(color, result);
            return result;
        }

        /// <summary>
        /// Gets mono svg image as <see cref="Image"/> filled using the specified color.
        /// Svg images with two or more colors are returned as is.
        /// </summary>
        /// <param name="size">Svg image size.</param>
        /// <param name="color">Svg image color.</param>
        /// <returns></returns>
        public virtual Image? ImageWithColor(int size, Color? color)
        {
            if (color is null)
                return AsImage(size);
            return ImageSetWithColor(size, color)?.AsImage();
        }

        /// <summary>
        /// Gets real color value for the specified known svg color.
        /// Uses <see cref="SvgColors.GetSvgColor(KnownSvgColor, bool)"/> and color overrides
        /// specified with <see cref="SetColorOverride(KnownSvgColor, bool, Color?)"/>.
        /// </summary>
        /// <param name="knownColor">Known svg color</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual Color GetSvgColor(KnownSvgColor knownColor, bool isDark)
        {
            Color? result = null;

            if (isDark)
            {
                if (colorOverridesDark is not null)
                    result = colorOverridesDark[(int)knownColor];
            }
            else
            {
                if (colorOverridesLight is not null)
                    result = colorOverridesLight[(int)knownColor];
            }

            if(result is null)
                return SvgColors.GetSvgColor(knownColor, isDark);
            return result;
        }

        /// <summary>
        /// Clones this object.
        /// </summary>
        /// <returns></returns>
        public virtual SvgImage Clone()
        {
            SvgImage result = (SvgImage)Activator.CreateInstance(GetType())!;

            result.url = url;
            result.svg = svg;
            result.wasLoaded = wasLoaded;

            if (colorOverridesLight is not null)
                result.colorOverridesLight = (Color?[]?)colorOverridesLight.Clone();
            if (colorOverridesDark is not null)
                result.colorOverridesDark = (Color?[]?)colorOverridesDark.Clone();

            return result;
        }

        /// <summary>
        /// Sets color overrides used instead of default known svg colors.
        /// Override is used only for this svg image.
        /// </summary>
        /// <param name="knownColor">Known svg color</param>
        /// <param name="value">Override color value.</param>
        /// <param name="isDark">Whether override is set for dark or light color theme.</param>
        public virtual void SetColorOverride(KnownSvgColor knownColor, bool isDark, Color? value)
        {
            if (isDark)
            {
                colorOverridesDark ??= new Color?[(int)KnownSvgColor.MaxValue + 1];
                colorOverridesDark[(int)knownColor] = value;
            }
            else
            {
                colorOverridesLight ??= new Color?[(int)KnownSvgColor.MaxValue + 1];
                colorOverridesLight[(int)knownColor] = value;
            }
        }

        /// <summary>
        /// Sets color overrides used instead of default known svg colors.
        /// Override is used only for this svg image.
        /// </summary>
        /// <param name="knownColor">Known svg color</param>
        /// <param name="value">Override color value.</param>
        public virtual void SetColorOverride(KnownSvgColor knownColor, LightDarkColor? value)
        {
            SetColorOverride(knownColor, true, value?.Dark);
            SetColorOverride(knownColor, false, value?.Light);
        }

        /// <summary>
        /// Creates new <see cref="ImageSet"/> and loads there this svg image
        /// with the specified size and color.
        /// </summary>
        /// <param name="size">Svg image size in pixels.</param>
        /// <param name="color">Color of the mono svg image. Optional.</param>
        /// <returns></returns>
        public virtual ImageSet? LoadImage(int size, Color? color = null)
        {
            LoadImage();

            if (svg is null)
                return ImageSet.Empty;
            else
                return ImageSet.FromSvgString(svg, size, size, color);
        }

        /// <summary>
        /// Creates new <see cref="ImageSet"/> and loads there this svg image
        /// with the specified size and color.
        /// </summary>
        /// <param name="size">Svg image size in pixels.</param>
        /// <param name="color">Color of the mono svg image. Optional.</param>
        /// <returns></returns>
        public virtual ImageSet? LoadImage(SizeI size, Color? color = null)
        {
            LoadImage();

            if (svg is null)
                return ImageSet.Empty;
            else
                return ImageSet.FromSvgString(svg, size.Width, size.Height, color);
        }

        /// <summary>
        /// Loads image if it was not yet loaded from the url or stream
        /// specified in the constructor.
        /// </summary>
        public virtual void LoadImage()
        {
            if (svg is null && url is not null && !wasLoaded)
            {
                wasLoaded = true;
                using var stream = ResourceLoader.StreamFromUrl(url);
                using var reader = new StreamReader(stream);
                svg = reader.ReadToEnd();
            }
        }

        internal void Resize(int size)
        {
            if (size > short.MaxValue || size < 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            if (data.Length <= size)
                Array.Resize(ref data, size + 1);
        }

        internal class Data
        {
            public ImageSet? OriginalImage;
            public ImageSet?[] KnownColorImagesLight;
            public ImageSet?[] KnownColorImagesDark;
            public Dictionary<Color, ImageSet>? ColoredImages;

            public Data()
            {
                KnownColorImagesLight = new ImageSet?[(int)KnownSvgColor.MaxValue + 1];
                KnownColorImagesDark = new ImageSet?[(int)KnownSvgColor.MaxValue + 1];
            }
        }
    }
}
