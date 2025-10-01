using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains small and large images indexed by the enum type.
    /// </summary>
    /// <typeparam name="TKey">Type of the enum.</typeparam>
    public class EnumImages<TKey> : ImmutableObject
        where TKey : struct, Enum, IConvertible
    {
        private EnumArray<TKey, string?> smallImageNames = new();
        private EnumArray<TKey, string?> largeImageNames = new();
        private EnumArray<TKey, Image?> smallImages = new();
        private EnumArray<TKey, Image?> largeImages = new();
        private EnumArray<TKey, Color?> svgColors = new();
        private EnumArray<TKey, SvgImage?> svgImages = new();

        /// <summary>
        /// Gets or sets size of the small svg image.
        /// </summary>
        public virtual int SmallSvgSize { get; set; } = 16;

        /// <summary>
        /// Gets or sets size of the large svg image.
        /// </summary>
        public virtual int LargeSvgSize { get; set; } = 32;

        /// <summary>
        /// Sets color of the svg image.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public virtual void SetSvgColor(TKey index, Color? value)
        {
            GetSvgImage(index)?.SetColorOverride(KnownSvgColor.Normal, value);
            svgColors[index] = value;
        }

        /// <summary>
        /// Associates the specified SVG image with the given key in the collection.
        /// </summary>
        /// <remarks>If an SVG image is already associated with the specified key, it will be replaced
        /// with the new value.</remarks>
        /// <param name="index">The key used to identify the SVG image in the collection.
        /// Cannot be null.</param>
        /// <param name="value">The SVG image to associate with the specified key.
        /// Can be null to remove the association.</param>
        public virtual void SetSvgImage(TKey index, SvgImage? value)
        {
            svgImages[index] = value;
        }

        /// <summary>
        /// Retrieves the <see cref="SvgImage"/> associated with the specified key.
        /// </summary>
        /// <param name="index">The key used to locate the <see cref="SvgImage"/>.
        /// Must not be null.</param>
        /// <returns>The <see cref="SvgImage"/> associated with the specified key,
        /// or <see langword="null"/> if the key does not
        /// exist.</returns>
        public virtual SvgImage? GetSvgImage(TKey index)
        {
            return svgImages[index];
        }

        /// <summary>
        /// Gets small image for the specified index.
        /// </summary>
        /// <param name="index">The index of the image to return.</param>
        /// <returns></returns>
        public virtual Image? GetSmallImage(TKey index)
        {
            return smallImages[index];
        }

        /// <summary>
        /// Gets large image for the specified index.
        /// </summary>
        /// <param name="index">The index of the image to return.</param>
        /// <returns></returns>
        public virtual Image? GetLargeImage(TKey index)
        {
            return largeImages[index];
        }

        /// <summary>
        /// Sets large image for the specified index.
        /// </summary>
        /// <param name="index">The index of the image to set.</param>
        /// <param name="image">The new image to be set at the
        /// position specified by the index.</param>
        /// <returns></returns>
        public virtual void SetLargeImage(TKey index, Image? image)
        {
            largeImages[index] = image;
        }

        /// <summary>
        /// Sets small image for the specified index.
        /// </summary>
        /// <param name="index">The index of the image to set.</param>
        /// <param name="image">The new image to be set at the position
        /// specified by the index.</param>
        /// <returns></returns>
        public virtual void SetSmallImage(TKey index, Image? image)
        {
            smallImages[index] = image;
        }

        /// <summary>
        /// Sets small or large image for the specified index.
        /// </summary>
        /// <param name="isSmall">The flag specifying whether small or
        /// large image is set.</param>
        /// <param name="index">The index of the image to set.</param>
        /// <param name="image">The new image to be set at the position
        /// specified by the index.</param>
        /// <returns></returns>
        public void SetImage(TKey index, Image? image, bool isSmall)
        {
            if (isSmall)
                SetSmallImage(index, image);
            else
                SetLargeImage(index, image);
        }

        /// <summary>
        /// Gets small or large image for the specified index.
        /// </summary>
        /// <param name="isSmall">The flag specifying whether small
        /// or large image is get.</param>
        /// <param name="index">The index of the image to set.</param>
        /// <returns></returns>
        public Image? GetImage(TKey index, bool isSmall)
        {
            if (isSmall)
                return GetSmallImage(index);
            else
                return GetLargeImage(index);
        }

        /// <summary>
        /// Sets small or large image name for the specified index.
        /// </summary>
        /// <param name="isSmall">The flag specifying whether small
        /// or large image name is set.</param>
        /// <param name="index">The index of the image which name is set.</param>
        /// <param name="name">The new name of the image.</param>
        /// <returns></returns>
        public virtual void SetImageName(TKey index, string? name, bool isSmall = true)
        {
            if (isSmall)
                smallImageNames[index] = name;
            else
                largeImageNames[index] = name;
        }

        /// <summary>
        /// Gets small or large image name for the specified index.
        /// </summary>
        /// <param name="isSmall">The flag specifying whether small
        /// or large image name is get.</param>
        /// <param name="index">The index of the image which name is returned.</param>
        /// <returns></returns>
        public virtual string? GetImageName(TKey index, bool isSmall = true)
        {
            if (isSmall)
                return smallImageNames[index];
            else
                return largeImageNames[index];
        }

        /// <summary>
        /// Assigns all image names from small to large images or backwards.
        /// </summary>
        /// <param name="fromSmallToLarge">The flag which specifies
        /// how images are assigned.</param>
        public virtual void AssignImageNames(bool fromSmallToLarge)
        {
            if (fromSmallToLarge)
                largeImageNames.Assign(smallImageNames);
            else
                smallImageNames.Assign(largeImageNames);
        }

        /// <summary>
        /// Calls <see cref="string.Format(string, object)"/> on small or large image names.
        /// </summary>
        /// <param name="format">The format template.</param>
        /// <param name="formatSmallImageNames">The flag which specifies whether to format
        /// small or large image names.</param>
        public virtual void FormatImageNames(string format, bool formatSmallImageNames = true)
        {
            if (formatSmallImageNames)
                smallImageNames.ConvertAllItems(StringUtils.SafeStringFormat, format);
            else
                largeImageNames.ConvertAllItems(StringUtils.SafeStringFormat, format);
        }

        /// <summary>
        /// Reloads an image from an SVG source and updates it with the specified size and color.
        /// </summary>
        /// <remarks>This method retrieves the SVG image associated with the specified
        /// <paramref name="imageKey"/>, applies the corresponding color, and resizes it based
        /// on the <paramref name="isSmallImage"/> parameter.
        /// The updated image is then set in the appropriate storage.</remarks>
        /// <param name="imageKey">The key identifying the SVG image to reload.</param>
        /// <param name="isSmallImage">A value indicating whether the
        /// image should be reloaded as a small image. If <see langword="true"/>, the
        /// image is resized to a smaller dimension; otherwise,
        /// it is resized to a larger dimension.</param>
        public virtual void ReloadFromSvg(TKey imageKey, bool isSmallImage)
        {
            var svg = svgImages[imageKey];
            if (svg is not null)
            {
                var size = GetSvgSize(isSmallImage);
                var svgColor = svgColors[imageKey];
                var image = svg.ImageWithColor(size, svgColor);
                SetImage(imageKey, image, isSmallImage);
            }
        }

        /// <summary>
        /// Reloads all images from their corresponding SVG sources.
        /// </summary>
        /// <remarks>This method clears the current collections of small
        /// and large images and reloads them
        /// from the SVG sources. It ensures that both small and large
        /// versions of each image are  reloaded. Use this
        /// method to refresh all images when the underlying SVG sources
        /// have changed.</remarks>
        public virtual void ReloadAllFromSvg()
        {
            ReloadSmallImagesFromSvg();
            ReloadLargeImagesFromSvg();
        }

        /// <summary>
        /// Reloads and regenerates large images from their corresponding SVG sources.
        /// </summary>
        /// <remarks>This method clears the current collection of large images
        /// and regenerates them based
        /// on the existing SVG image sources.
        /// It processes each SVG source and updates the large image collection
        /// accordingly.</remarks>
        public virtual void ReloadLargeImagesFromSvg()
        {
            largeImages = new();
            svgImages.ForEachKey((key) => ReloadFromSvg(key, false));
        }

        /// <summary>
        /// Reloads and regenerates the small-sized images from their corresponding SVG sources.
        /// </summary>
        /// <remarks>This method clears the current collection of small images
        /// and regenerates them based
        /// on the existing SVG image sources. It processes each SVG source and creates
        /// a corresponding small-sized image.</remarks>
        public virtual void ReloadSmallImagesFromSvg()
        {
            smallImages = new();
            svgImages.ForEachKey((key) => ReloadFromSvg(key, true));
        }

        /// <summary>
        /// Determines whether the collection contains any non-null SVG images.
        /// </summary>
        /// <returns><see langword="true"/> if the collection contains
        /// at least one non-null SVG image; otherwise, <see langword="false"/>.</returns>
        public virtual bool HasSvgImages()
        {
            bool result = false;
            svgImages.ForEachKey((key) =>
            {
                if (svgImages[key] != null)
                    result = true;
            });

            return result;
        }

        /// <summary>
        /// Resets all cached images within the current instance and its child elements.
        /// This is done only if the current instance contains any SVG images.
        /// </summary>
        /// <remarks>This method clears any cached image data associated with
        /// the current instance and
        /// recursively resets cached images for all child elements.
        /// It is typically used to ensure that updated or
        /// refreshed image data is loaded the next time the images are accessed.</remarks>
        public virtual void ResetCachedImages()
        {
            svgImages.ForEachKey((key) =>
            {
                var svg = svgImages[key];
                svg?.ResetCachedImages();
            });

            ReloadAllFromSvg();
        }

        /// <summary>
        /// Loads the specified image from the assembly resource.
        /// </summary>
        /// <param name="assembly">The assembly from where image is loaded.</param>
        /// <param name="imageKey">The key of the image to load.</param>
        /// <param name="isSmallImage">The flag which specifies whether
        /// to load small or large image.</param>
        /// <remarks>
        /// Image name is used as a resource url.
        /// </remarks>
        public virtual bool LoadFromResource(
            Assembly assembly,
            TKey imageKey,
            bool isSmallImage = true)
        {
            return LoadImageFromResource(GetImageName(imageKey, isSmallImage));

            bool LoadImageFromResource(string? relativePath)
            {
                if (relativePath is null)
                    return false;
                string url = AssemblyUtils.GetImageUrlInAssembly(assembly, relativePath);

                Image? image;

                if (url.EndsWith(".svg"))
                {
                    var svg = new MonoSvgImage(url, SvgImageDataKind.Url, false);
                    SetSvgImage(imageKey, svg);
                    var size = GetSvgSize(isSmallImage);
                    var svgColor = svgColors[imageKey];
                    svg.SetColorOverride(KnownSvgColor.Normal, svgColor);
                    image = svg.ImageWithColor(size, svgColor);
                }
                else
                {
                    image = Image.FromUrlOrNull(url);
                }

                if (image is null || !image.IsOk)
                    return false;
                SetImage(imageKey, image, isSmallImage);
                return true;
            }
        }

        /// <summary>
        /// Loads an SVG image from an embedded resource within the specified assembly.
        /// </summary>
        /// <remarks>The method resolves the resource path based on the provided
        /// <paramref
        /// name="imageKey"/> and  <paramref name="isSmallImage"/> flag.
        /// If the resource path is null or the resource is
        /// not an SVG file,  the method returns <see langword="false"/>.</remarks>
        /// <param name="assembly">The assembly containing the embedded resource.</param>
        /// <param name="imageKey">The key used to identify the image.</param>
        /// <param name="isSmallImage">A value indicating whether to
        /// load the small version of the image.  If <see langword="true"/>, the method
        /// attempts to load the small image; otherwise, the regular image.</param>
        /// <returns><see langword="true"/> if the SVG image was successfully loaded and set;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool LoadSvgFromResource(Assembly assembly, TKey imageKey, bool isSmallImage = true)
        {
            var relativePath = GetImageName(imageKey, isSmallImage);

            if (relativePath is null)
                return false;

            string url = AssemblyUtils.GetImageUrlInAssembly(assembly, relativePath);

            if (url.EndsWith(".svg"))
            {
                var svg = new MonoSvgImage(url, SvgImageDataKind.Url, false);
                var svgColor = svgColors[imageKey];
                svg.SetColorOverride(KnownSvgColor.Normal, svgColor);
                SetSvgImage(imageKey, svg);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets size of the svg image.
        /// </summary>
        /// <param name="isSmallImage">Whether returned size is for small or large image.</param>
        /// <returns></returns>
        public virtual int GetSvgSize(bool isSmallImage)
        {
            if (isSmallImage)
                return SmallSvgSize;
            else
                return LargeSvgSize;
        }

        /// <summary>
        /// Loads both small and large images from the specified assembly's embedded resources.
        /// </summary>
        /// <remarks>This method loads two sets of images: small and large.
        /// The specific resources to load
        /// are determined by the implementation of the resource-loading logic.
        /// Ensure that the provided assembly
        /// contains the required resources in the expected format.</remarks>
        /// <param name="assembly">The assembly containing the embedded
        /// resources to load the images from.</param>
        public virtual void LoadSmallAndLargeImagesFromResource(Assembly assembly)
        {
            LoadImagesFromResource(assembly, true);
            LoadImagesFromResource(assembly, false);
        }

        /// <summary>
        /// Loads large images from the specified assembly's embedded resources.
        /// </summary>
        /// <remarks>This method loads only the large images from the assembly's resources.
        /// Ensure that the assembly contains the expected resource format.</remarks>
        /// <param name="assembly">The assembly containing the embedded resources
        /// to load images from. Cannot be <see langword="null"/>.</param>
        public void LoadLargeImagesFromResource(Assembly assembly)
        {
            LoadImagesFromResource(assembly, loadSmallImageNames: false);
        }

        /// <summary>
        /// Loads small images from the specified assembly's embedded resources.
        /// </summary>
        /// <remarks>This method loads only the small images from the assembly's resources.
        /// Ensure that the assembly contains the expected resource format.</remarks>
        /// <param name="assembly">The assembly containing the embedded resources
        /// to load images from. Cannot be <see langword="null"/>.</param>
        public void LoadSmallImagesFromResource(Assembly assembly)
        {
            LoadImagesFromResource(assembly, loadSmallImageNames: true);
        }

        /// <summary>
        /// Loads SVG images from the specified assembly and processes them based
        /// on the provided size option.
        /// </summary>
        /// <remarks>This method iterates through all keys in the internal collection
        /// of SVG images and
        /// loads the corresponding SVG resources from the specified assembly.
        /// The size of the images is  determined by
        /// the <paramref name="isSmallImage"/> parameter.</remarks>
        /// <param name="assembly">The assembly containing the embedded SVG resources to load.</param>
        /// <param name="isSmallImage">A boolean value indicating whether
        /// to load the SVG images as small-sized images.  <see langword="true"/> to
        /// load small images; otherwise, <see langword="false"/>.</param>
        public void LoadSvgFromResource(Assembly assembly, bool isSmallImage = true)
        {
            svgImages.ForEachKey((key) => LoadSvgFromResource(assembly, key, isSmallImage));
        }

        /// <summary>
        /// Loads all small or large images from the assembly resource.
        /// </summary>
        /// <param name="assembly">The assembly from where image is loaded.</param>
        /// <param name="loadSmallImageNames">The flag which specifies whether
        /// to load all small or large images.</param>
        /// <remarks>
        /// Image names are used as a resource urls.
        /// </remarks>
        public virtual void LoadImagesFromResource(Assembly assembly, bool loadSmallImageNames = true)
        {
            if (loadSmallImageNames)
                smallImages.ForEachKey((key) => LoadFromResource(assembly, key, true));
            else
                largeImages.ForEachKey((key) => LoadFromResource(assembly, key, false));
        }

        /// <summary>
        /// Logs object information.
        /// </summary>
        [Conditional("DEBUG")]
        public virtual void Log()
        {
            Log("Small", true);
            Log("Large", false);

            void Log(string section, bool isSmall)
            {
                App.LogBeginSection(section);
                smallImages.ForEachKey((key) =>
                {
                    var s = $"{key} = '{GetImageName(key, isSmall)}' = {GetImage(key, isSmall)?.IsOk}>";
                    App.Log(s);
                });
                App.LogEndSection();
            }
        }
    }
}