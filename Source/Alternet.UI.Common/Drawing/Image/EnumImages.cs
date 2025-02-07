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
            svgColors[index] = value;
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
        /// Loads the specified image from the assemblie's resource.
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
                    var svg = new MonoSvgImage(url);
                    var size = GetSvgSize(isSmallImage);
                    var svgColor = svgColors[imageKey];
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
        /// Loads all small or large images from the assemblie's resource.
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