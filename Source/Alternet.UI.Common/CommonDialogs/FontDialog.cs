using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a font.
    /// </summary>
    [ControlCategory("Dialogs")]
    public class FontDialog : CommonDialog
    {
        /// <summary>
        /// Gets default <see cref="FontDialog"/> instance.
        /// </summary>
        public static FontDialog Default = defaultDialog ??= new FontDialog();

        private static FontDialog? defaultDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="FontDialog"/>.
        /// </summary>
        public FontDialog()
        {
        }

        /// <summary>
        /// Under Windows, gets or sets a flag determining whether symbol fonts
        /// can be selected. Has no effect on other platforms.
        /// </summary>
        /// <remarks>
        /// The default value is true.
        /// </remarks>
        public virtual bool AllowSymbols
        {
            get
            {
                if (!App.IsWindowsOS)
                    return true;

                CheckDisposed();
                return Handler.AllowSymbols;
            }

            set
            {
                if (!App.IsWindowsOS)
                    return;
                CheckDisposed();
                Handler.AllowSymbols = value;
            }
        }

        /// <summary>
        /// Enables or disables "effects" under Windows or generic only.
        /// </summary>
        /// <remarks>
        /// This refers to the controls for manipulating colour,
        /// strikeout and underline properties. The default value is true.
        /// </remarks>
        public virtual bool EnableEffects
        {
            get
            {
                if (!App.IsWindowsOS)
                    return true;

                CheckDisposed();
                return Handler.EnableEffects;
            }

            set
            {
                if (!App.IsWindowsOS)
                    return;

                CheckDisposed();
                Handler.EnableEffects = value;
            }
        }

        /// <summary>
        /// Gets or sets the state of the flags restricting the selection.
        /// </summary>
        /// <remarks>
        /// Note that currently these flags are only effectively used in Windows.
        /// By default no restrictions are applied.
        /// </remarks>
        public virtual FontDialogRestrictSelection RestrictSelection
        {
            get
            {
                CheckDisposed();
                return Handler.RestrictSelection;
            }

            set
            {
                CheckDisposed();
                Handler.RestrictSelection = value;
            }
        }

        /// <summary>
        /// Gets or sets the color associated with the font dialog window.
        /// </summary>
        /// <remarks>
        /// The default value is black color.
        /// </remarks>
        [Browsable(false)]
        public virtual Color Color
        {
            get
            {
                CheckDisposed();
                return Handler.Color;
            }

            set
            {
                CheckDisposed();
                Handler.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the font selected by the font dialog window.
        /// </summary>
        [Browsable(false)]
        public virtual FontInfo FontInfo
        {
            get
            {
                return Handler.FontInfo;
            }

            set
            {
                Handler.FontInfo = value;
            }
        }

        /// <summary>
        /// Gets dialog handler.
        /// </summary>
        [Browsable(false)]
        public new IFontDialogHandler Handler => (IFontDialogHandler)base.Handler;

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
        public virtual void SetRange(int minRange, int maxRange)
        {
            if (!App.IsWindowsOS)
                return;

            CheckDisposed();
            Handler.SetRange(minRange, maxRange);
        }

        /// <inheritdoc/>
        protected override IDialogHandler CreateHandler()
        {
            return DialogFactory.Handler.CreateFontDialogHandler(this);
        }
    }
}