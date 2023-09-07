using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a font.
    /// </summary>
    public sealed class FontDialog : CommonDialog
    {
        private readonly Native.FontDialog nativeDialog;
        private FontInfo fontInfo = Control.DefaultFont;

        /// <summary>
        /// Initializes a new instance of <see cref="FontDialog"/>.
        /// </summary>
        public FontDialog()
        {
            nativeDialog = new Native.FontDialog();
        }

        /// <summary>
        /// Under Windows, gets or sets a flag determining whether symbol fonts
        /// can be selected. Has no effect on other platforms.
        /// </summary>
        /// <remarks>
        /// The default value is true.
        /// </remarks>
        public bool AllowSymbols
        {
            get
            {
                if (!Application.IsWindowsOS)
                    return true;

                CheckDisposed();
                return nativeDialog.AllowSymbols;
            }

            set
            {
                if (!Application.IsWindowsOS)
                    return;
                CheckDisposed();
                nativeDialog.AllowSymbols = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the Help button will be shown (Windows only).
        /// </summary>
        /// <remarks>
        /// The default value is false.
        /// </remarks>
        public bool ShowHelp
        {
            get
            {
                if (!Application.IsWindowsOS)
                    return false;

                CheckDisposed();
                return nativeDialog.ShowHelp;
            }

            set
            {
                if (!Application.IsWindowsOS)
                    return;

                CheckDisposed();
                nativeDialog.ShowHelp = value;
            }
        }

        /// <summary>
        /// Enables or disables "effects" under Windows or generic only.
        /// </summary>
        /// <remarks>
        /// This refers to the controls for manipulating colour,
        /// strikeout and underline properties. The default value is true.
        /// </remarks>
        public bool EnableEffects
        {
            get
            {
                if (!Application.IsWindowsOS)
                    return true;

                CheckDisposed();
                return nativeDialog.EnableEffects;
            }

            set
            {
                if (!Application.IsWindowsOS)
                    return;

                CheckDisposed();
                nativeDialog.EnableEffects = value;
            }
        }

        /// <summary>
        /// Gets or sets the state of the flags restricting the selection.
        /// </summary>
        /// <remarks>
        /// Note that currently these flags are only effectively used in Windows.
        /// By default no restrictions are applied.
        /// </remarks>
        public FontDialogRestrictSelection RestrictSelection
        {
            get
            {
                CheckDisposed();
                return (FontDialogRestrictSelection)Enum.ToObject(
                    typeof(FontDialogRestrictSelection),
                    nativeDialog.RestrictSelection);
            }

            set
            {
                if (!Application.IsWindowsOS)
                    return;

                CheckDisposed();
                nativeDialog.RestrictSelection = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the color associated with the font dialog window.
        /// </summary>
        /// <remarks>
        /// The default value is black color.
        /// </remarks>
        public Color Color
        {
            get
            {
                CheckDisposed();
                return nativeDialog.Color;
            }

            set
            {
                CheckDisposed();
                nativeDialog.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the font selected by the font dialog window.
        /// </summary>
        [Browsable(false)]
        public FontInfo FontInfo
        {
            get
            {
                return fontInfo;
            }

            set
            {
                fontInfo = value;
            }
        }

        private protected override string? TitleCore
        {
            get
            {
                CheckDisposed();
                return nativeDialog.Title;
            }

            set
            {
                CheckDisposed();
                nativeDialog.Title = value;
            }
        }

        /// <summary>
        /// Sets the valid range for the font point size (Windows only).
        /// </summary>
        /// <param name="minRange">New minimal font point size.
        /// Pass 0 here to unrestrict minimal font point size.</param>
        /// <param name="maxRange">New maximal font point size.
        /// Pass 0 here to unrestrict maximal point size.</param>
        /// <remarks>
        /// The default is 0, 0 (unrestricted range).
        /// </remarks>
        public void SetRange(int minRange, int maxRange)
        {
            if (!Application.IsWindowsOS)
                return;

            CheckDisposed();
            nativeDialog.SetRange(minRange, maxRange);
        }

        private protected override ModalResult ShowModalCore(Window? owner)
        {
            CheckDisposed();
            var nativeOwner = owner == null ?
                null : ((NativeWindowHandler)owner.Handler).NativeControl;

            string fontName = fontInfo.FontFamily.Name;
            var style = (UI.Native.FontStyle)fontInfo.Style;
            var genericFamily = Font.ToNativeGenericFamily(
                fontInfo.FontFamily.GenericFamily);

            nativeDialog.SetInitialFont(
                genericFamily,
                fontName,
                fontInfo.SizeInPoints,
                style);

            var result = (ModalResult)nativeDialog.ShowModal(nativeOwner);

            fontInfo.Style = (FontStyle)nativeDialog.ResultFontStyle;
            fontInfo.SizeInPoints = nativeDialog.ResultFontSizeInPoints;
            fontInfo.FontFamily = new FontFamily(nativeDialog.ResultFontName);

            return result;
        }
    }
}