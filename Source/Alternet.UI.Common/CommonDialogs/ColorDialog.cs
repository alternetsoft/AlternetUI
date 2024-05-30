using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a color.
    /// </summary>
    [ControlCategory("Dialogs")]
    public class ColorDialog : CommonDialog
    {
        /// <summary>
        /// Gets default <see cref="ColorDialog"/> instance.
        /// </summary>
        public static ColorDialog Default = defaultDialog ??= new ColorDialog();

        private static ColorDialog? defaultDialog;

        /// <summary>
        /// Gets or sets the color selected by the color dialog window.
        /// </summary>
        public virtual Color Color
        {
            get
            {
                CheckDisposed();
                return ((IColorDialogHandler)Handler).Color;
            }

            set
            {
                CheckDisposed();
                ((IColorDialogHandler)Handler).Color = value;
            }
        }

        /// <inheritdoc/>
        protected override IDialogHandler CreateHandler()
        {
            return DialogFactory.Handler.CreateColorDialogHandler(this);
        }
    }
}