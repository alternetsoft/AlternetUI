using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public virtual Image? GetSmallImage(TKey index)
        {
            return smallImages[index];
        }

        public virtual Image? GetLargeImage(TKey index)
        {
            return largeImages[index];
        }

        public virtual void SetLargeImage(TKey index, Image? image)
        {
            largeImages[index] = image;
        }

        public virtual void SetSmallImage(TKey index, Image? image)
        {
            smallImages[index] = image;
        }

        public void SetImage(TKey index, Image? image, bool isSmall)
        {
            if (isSmall)
                SetSmallImage(index, image);
            else
                SetLargeImage(index, image);
        }

        public Image? GetImage(TKey index, bool isSmall)
        {
            if (isSmall)
                return GetSmallImage(index);
            else
                return GetLargeImage(index);
        }

        public virtual void SetImageName(TKey index, string? name, bool isSmall = true)
        {
            if (isSmall)
                smallImageNames[index] = name;
            else
                largeImageNames[index] = name;
        }

        public virtual string? GetImageName(TKey index, bool isSmall = true)
        {
            if (isSmall)
                return smallImageNames[index];
            else
                return largeImageNames[index];
        }

        public virtual void AssignImageNames(bool fromSmallToLarge)
        {
            if (fromSmallToLarge)
                largeImageNames.Assign(smallImageNames);
            else
                smallImageNames.Assign(largeImageNames);
        }

        public virtual void FormatImageNames(string format, bool formatSmallImageNames = true)
        {
            if (formatSmallImageNames)
                smallImageNames.ConvertAllItems(StringUtils.SafeStringFormat, format);
            else
                largeImageNames.ConvertAllItems(StringUtils.SafeStringFormat, format);
        }

        public virtual void LoadFromResource(Assembly assembly, TKey imageKey, bool isSmallImage = true)
        {
            LoadImageFromResource(GetImageName(imageKey, isSmallImage));

            bool LoadImageFromResource(string? relativePath)
            {
                string url = AssemblyUtils.GetImageUrlInAssembly(assembly, relativePath);
                var image = Image.FromUrlOrNull(url);
                if (image is null || !image.IsOk)
                    return false;
                SetImage(imageKey, image, isSmallImage);
                return true;
            }
        }

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