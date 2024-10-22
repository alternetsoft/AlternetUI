using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains known svg images for the message box dialogs.
    /// </summary>
    public static class MessageBoxSvg
    {
        private static EnumArray<MessageBoxIcon, SvgImage?> svg = new();
        private static EnumArray<MessageBoxIcon, Func<SvgImage?>?> svgActions = new();

        static MessageBoxSvg()
        {
            svgActions[MessageBoxIcon.Error] = () => KnownColorSvgImages.ImgError;
            svgActions[MessageBoxIcon.Information] = () => KnownColorSvgImages.ImgInformation;
            svgActions[MessageBoxIcon.Warning] = () => KnownColorSvgImages.ImgWarning;
        }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Information"/>.
        /// </summary>
        public static SvgImage? Information
        {
            get
            {
                return GetImage(MessageBoxIcon.Information);
            }

            set
            {
                SetImage(MessageBoxIcon.Information, value);
            }
        }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Warning"/>.
        /// </summary>
        public static SvgImage? Warning
        {
            get
            {
                return GetImage(MessageBoxIcon.Warning);
            }

            set
            {
                SetImage(MessageBoxIcon.Warning, value);
            }
        }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Error"/>.
        /// </summary>
        public static SvgImage? Error
        {
            get
            {
                return GetImage(MessageBoxIcon.Error);
            }

            set
            {
                SetImage(MessageBoxIcon.Error, value);
            }
        }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Question"/>.
        /// </summary>
        public static SvgImage? Question
        {
            get
            {
                return GetImage(MessageBoxIcon.Question);
            }

            set
            {
                SetImage(MessageBoxIcon.Question, value);
            }
        }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Hand"/>.
        /// Image normally contains a white X in a circle with a red background.
        /// </summary>
        public static SvgImage? Hand
        {
            get
            {
                return GetImage(MessageBoxIcon.Hand);
            }

            set
            {
                SetImage(MessageBoxIcon.Hand, value);
            }
        }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Exclamation"/>.
        /// Image normally exclamation point in a
        /// triangle with a yellow background.
        /// </summary>
        public static SvgImage? Exclamation
        {
            get
            {
                return GetImage(MessageBoxIcon.Exclamation);
            }

            set
            {
                SetImage(MessageBoxIcon.Exclamation, value);
            }
        }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Asterisk"/>.
        /// Image normally contains a symbol consisting of
        /// a lowercase letter i in a circle.
        /// </summary>
        public static SvgImage? Asterisk
        {
            get
            {
                return GetImage(MessageBoxIcon.Asterisk);
            }

            set
            {
                SetImage(MessageBoxIcon.Asterisk, value);
            }
        }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Stop"/>.
        /// Image normally contains a symbol consisting
        /// of white X in a circle with a red background.
        /// </summary>
        public static SvgImage? Stop
        {
            get
            {
                return GetImage(MessageBoxIcon.Stop);
            }

            set
            {
                SetImage(MessageBoxIcon.Stop, value);
            }
        }

        /// <summary>
        /// Sets message box icon with the specified index.
        /// </summary>
        /// <param name="messabeBoxIcon">The message box icon index specified using
        /// <see cref="MessageBoxIcon"/> enum.</param>
        /// <param name="image">Svg image.</param>
        public static void SetImage(MessageBoxIcon messabeBoxIcon, SvgImage? image)
        {
            svg[messabeBoxIcon] = image;
            svgActions[messabeBoxIcon] = null;
        }

        /// <summary>
        /// Gets message box icon as <see cref="SvgImage"/> with the specified index.
        /// </summary>
        /// <param name="messabeBoxIcon">The message box icon index specified using
        /// <see cref="MessageBoxIcon"/> enum.</param>
        /// <returns></returns>
        public static SvgImage? GetImage(MessageBoxIcon messabeBoxIcon)
        {
            var result = svg[messabeBoxIcon] ?? svgActions[messabeBoxIcon]?.Invoke();
            return result;
        }

        /// <summary>
        /// Removes empty message box icon indexes from the items of the specified list control.
        /// </summary>
        public static void RemoveEmptyMessageBoxIconIndexes<T>(ListControl<T> container)
            where T : class, new()
        {
            RemoveItem(MessageBoxIcon.Information);
            RemoveItem(MessageBoxIcon.Warning);
            RemoveItem(MessageBoxIcon.Error);
            RemoveItem(MessageBoxIcon.Question);
            RemoveItem(MessageBoxIcon.Hand);
            RemoveItem(MessageBoxIcon.Exclamation);
            RemoveItem(MessageBoxIcon.Asterisk);
            RemoveItem(MessageBoxIcon.Stop);

            void RemoveItem(MessageBoxIcon icon)
            {
                container.RemoveItemWithValueIf(icon, GetImage(icon) is null);
            }
        }
    }
}