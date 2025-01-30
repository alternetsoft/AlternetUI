using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Prompts the user to select a location for saving a file.
    /// </summary>
    [ControlCategory("Dialogs")]
    public class SaveFileDialog : FileDialog
    {
        /// <summary>
        /// Gets or sets default value for the <see cref="AllowNullFileName"/> property.
        /// </summary>
        public static bool DefaultAllowNullFileName = false;

        /// <summary>
        /// Gets or sets default value for the <see cref="OverwritePrompt"/> property.
        /// </summary>
        public static bool DefaultOverwritePrompt = true;

        /// <summary>
        /// Gets default <see cref="SaveFileDialog"/> instance.
        /// </summary>
        public static SaveFileDialog Default = defaultDialog ??= new SaveFileDialog();

        private static SaveFileDialog? defaultDialog;

        /// <summary>
        /// Gets or sets prompt for a confirmation if a file will be overwritten.
        /// </summary>
        /// <remarks>
        /// This style is for Save dialog only, is always enabled on MacOs and cannot be disabled.
        /// </remarks>
        public virtual bool OverwritePrompt
        {
            get
            {
                CheckDisposed();
                return Handler.OverwritePrompt;
            }

            set
            {
                CheckDisposed();
                Handler.OverwritePrompt = value;
            }
        }

        /// <summary>
        /// Gets or sets whether an empty file name is allowed when dialog result
        /// is <see cref="ModalResult.Accepted"/>.
        /// </summary>
        /// <remarks>
        /// Default value is <c>false</c>. When empty file name is not allowed, dialog
        /// result is set to <see cref="ModalResult.Canceled"/> when file name is empty or
        /// <c>null</c>.
        /// </remarks>
        public virtual bool AllowNullFileName { get; set; } = DefaultAllowNullFileName;

        /// <summary>
        /// Gets dialog handler.
        /// </summary>
        [Browsable(false)]
        public new ISaveFileDialogHandler Handler => (ISaveFileDialogHandler)base.Handler;

        /// <inheritdoc/>
        public override void Reset()
        {
            base.Reset();
            AllowNullFileName = DefaultAllowNullFileName;
            OverwritePrompt = DefaultOverwritePrompt;
        }

        /// <inheritdoc/>
        protected override bool CoerceDialogResult(bool dialogResult)
        {
            if (dialogResult)
            {
                if (!AllowNullFileName)
                {
                    if (string.IsNullOrEmpty(FileName))
                        return false;
                }
            }

            return dialogResult;
        }

        /// <inheritdoc/>
        protected override IDialogHandler CreateHandler()
        {
            return DialogFactory.Handler.CreateSaveFileDialogHandler(this);
        }
    }
}