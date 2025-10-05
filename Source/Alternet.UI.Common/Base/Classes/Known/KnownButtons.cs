using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Contains text, image and other information for the known buttons.
    /// </summary>
    public static class KnownButtons
    {
        private static EnumArray<KnownButton, Func<Info?>?> data = new();
        private static bool loaded;

        static KnownButtons()
        {
        }

        /// <summary>
        /// Sets information about the specified known button.
        /// </summary>
        /// <param name="index">Index of the button.</param>
        /// <param name="func">Function used to get the information.</param>
        public static void SetInfo(KnownButton index, Func<Info?> func)
        {
            data[index] = func;
        }

        /// <summary>
        /// Gets information (text, image, etc.) about the specified known button.
        /// </summary>
        /// <param name="index">Index of the button.</param>
        /// <returns></returns>
        public static Info? GetInfo(KnownButton index)
        {
            Initialize();
            var info = data[index]?.Invoke();
            return info;
        }

        /// <summary>
        /// Retrieves the SVG image associated with the specified button index.
        /// </summary>
        /// <remarks>This method returns <see langword="null"/> if the specified button
        /// index does not
        /// have an associated image.</remarks>
        /// <param name="index">The button index for which to retrieve the associated SVG image.</param>
        /// <returns>The <see cref="SvgImage"/> associated with the specified button index,
        /// or <see langword="null"/>  if no
        /// image is available for the given index.</returns>
        public static SvgImage? GetImage(KnownButton index)
        {
            return GetInfo(index)?.SvgImage;
        }

        /// <summary>
        /// Retrieves the text associated with the specified button index.
        /// </summary>
        /// <remarks>Ensure that the button index provided is valid and corresponds
        /// to a known button. The
        /// method initializes necessary data before retrieving the text.</remarks>
        /// <param name="index">The index of the button whose text is to be retrieved.</param>
        /// <returns>A string containing the text of the specified button,
        /// or <see langword="null"/> if the text is not available.</returns>
        public static string? GetText(KnownButton index)
        {
            Initialize();
            var info = data[index]?.Invoke();
            return info?.Text?.ToString();
        }

        private static void Initialize()
        {
            if (loaded)
                return;
            loaded = true;

            var strings = CommonStrings.Default;

            data[KnownButton.Apply] = () => new(strings.ButtonApply);
            data[KnownButton.All] = () => new(strings.ButtonAll);

            data[KnownButton.Close] = () => new(strings.ButtonClose, KnownSvgImages.ImgClose);
            data[KnownButton.OK] = () => new(strings.ButtonOk, KnownSvgImages.ImgOk);
            data[KnownButton.Cancel] = () => new(strings.ButtonCancel, KnownSvgImages.ImgCancel);
            data[KnownButton.Yes] = () => new(strings.ButtonYes, KnownSvgImages.ImgYes);
            data[KnownButton.No] = () => new(strings.ButtonNo, KnownSvgImages.ImgNo);
            data[KnownButton.Abort] = () => new(strings.ButtonAbort, KnownSvgImages.ImgAbort);
            data[KnownButton.Retry] = () => new(strings.ButtonRetry, KnownSvgImages.ImgRetry);
            data[KnownButton.Ignore] = () => new(strings.ButtonIgnore, KnownSvgImages.ImgIgnore);
            data[KnownButton.Help] = () => new(strings.ButtonHelp, KnownSvgImages.ImgHelp);
            data[KnownButton.Add] = () => new(strings.ButtonAdd, KnownSvgImages.ImgAdd);
            data[KnownButton.Remove] = () => new(strings.ButtonRemove, KnownSvgImages.ImgRemove);
            data[KnownButton.Clear] = () => new(strings.ButtonClear, KnownSvgImages.ImgRemoveAll);
            data[KnownButton.AddChild]
                = () => new(strings.ButtonAddChild, KnownSvgImages.ImgAddChild);
            data[KnownButton.MoreItems]
                = () => new(strings.ToolbarSeeMore, KnownSvgImages.ImgMoreActionsHorz);
            data[KnownButton.New] = () => new(strings.ButtonNew, KnownSvgImages.ImgFileNew);
            data[KnownButton.Open] = () => new(strings.ButtonOpen, KnownSvgImages.ImgFileOpen);
            data[KnownButton.Save] = () => new(strings.ButtonSave, KnownSvgImages.ImgFileSave);
            data[KnownButton.Undo] = () => new(strings.ButtonUndo, KnownSvgImages.ImgUndo);
            data[KnownButton.Redo] = () => new(strings.ButtonRedo, KnownSvgImages.ImgRedo);
            data[KnownButton.Bold] = () => new(strings.ButtonBold, KnownSvgImages.ImgBold);
            data[KnownButton.Italic] = () => new(strings.ButtonItalic, KnownSvgImages.ImgItalic);
            data[KnownButton.Underline]
                = () => new(strings.ButtonUnderline, KnownSvgImages.ImgUnderline);
            data[KnownButton.Back] = () => new(strings.ButtonBack, KnownSvgImages.ImgBrowserBack);
            data[KnownButton.Forward]
                = () => new(strings.ButtonForward, KnownSvgImages.ImgBrowserForward);
            data[KnownButton.ZoomIn] = () => new(strings.ButtonZoomIn, KnownSvgImages.ImgZoomIn);
            data[KnownButton.ZoomOut] = () => new(strings.ButtonZoomOut, KnownSvgImages.ImgZoomOut);
            data[KnownButton.BrowserGo] = () => new(strings.ButtonGo, KnownSvgImages.ImgBrowserGo);

            data[KnownButton.Search] = () => new(strings.ButtonSearch, KnownSvgImages.ImgSearch);

            data[KnownButton.TextBoxEllipsis] = () => new(KnownSvgImages.ImgMoreActionsHorz);
            data[KnownButton.TextBoxCombo] = () => new(KnownSvgImages.ImgAngleDown);
            data[KnownButton.TextBoxUp] = () => new(KnownSvgImages.ImgTriangleArrowUp);
            data[KnownButton.TextBoxDown] = () => new(KnownSvgImages.ImgTriangleArrowDown);
            data[KnownButton.TextBoxPlus] = () => new(KnownSvgImages.ImgPlus);
            data[KnownButton.TextBoxMinus] = () => new(KnownSvgImages.ImgMinus);

            data[KnownButton.TextBoxShowPassword] = () => new(KnownSvgImages.ImgEyeOn);
            data[KnownButton.TextBoxHidePassword] = () => new(KnownSvgImages.ImgEyeOff);
        }

        /// <summary>
        /// Contains text, image and other data for the <see cref="KnownButton"/>.
        /// </summary>
        public class Info
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Info"/> class.
            /// </summary>
            public Info()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Info"/> class
            /// with the specified text.
            /// </summary>
            /// <param name="text">Button text.</param>
            public Info(object? text)
            {
                Text = text;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Info"/> class
            /// with the specified text and image.
            /// </summary>
            /// <param name="text">Button text.</param>
            /// <param name="svg">Button image.</param>
            public Info(object? text, SvgImage? svg)
            {
                Text = text;
                SvgImage = svg;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Info"/> class
            /// with the specified image.
            /// </summary>
            /// <param name="svg">Button image.</param>
            public Info(SvgImage svg)
            {
                SvgImage = svg;
            }

            /// <summary>
            /// Gets or sets known button text.
            /// </summary>
            public virtual object? Text { get; set; }

            /// <summary>
            /// Gets or sets known button svg.
            /// </summary>
            public virtual SvgImage? SvgImage { get; set; }
        }
    }
}