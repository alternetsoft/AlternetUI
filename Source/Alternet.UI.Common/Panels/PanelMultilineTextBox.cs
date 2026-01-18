using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements panel with <see cref="MultilineTextBox"/> and toolbar with
    /// text edit buttons.
    /// </summary>
    [ControlCategory("Panels")]
    public partial class PanelMultilineTextBox : PanelWithToolBar
    {
        private readonly MultilineTextBox textBox = new();

        private ObjectUniqueId buttonIdNew;
        private ObjectUniqueId buttonIdOpen;
        private ObjectUniqueId buttonIdSave;
        private ObjectUniqueId buttonIdUndo;
        private ObjectUniqueId buttonIdRedo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelMultilineTextBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PanelMultilineTextBox(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelMultilineTextBox"/> class.
        /// </summary>
        public PanelMultilineTextBox()
        {
            textBox.HasBorder = false;
            textBox.VerticalAlignment = VerticalAlignment.Fill;
            textBox.Parent = this;
        }

        /// <summary>
        /// Occurs when 'New' button is clicked on the toolbar.
        /// </summary>
        public event EventHandler? FileNewClick;

        /// <summary>
        /// Occurs when 'Open' button is clicked on the toolbar.
        /// </summary>
        public event EventHandler? FileOpenClick;

        /// <summary>
        /// Occurs when 'Save' button is clicked on the toolbar.
        /// </summary>
        public event EventHandler? FileSaveClick;

        /// <summary>
        /// Gets id of the 'New' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdNew => buttonIdNew;

        /// <summary>
        /// Gets id of the 'Open' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdOpen => buttonIdOpen;

        /// <summary>
        /// Gets id of the 'Save' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdSave => buttonIdSave;

        /// <summary>
        /// Gets id of the 'Undo' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdUndo => buttonIdUndo;

        /// <summary>
        /// Gets id of the 'Redo' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdRedo => buttonIdRedo;

        /// <summary>
        /// Gets <see cref="RichTextBox"/> control used in this panel.
        /// </summary>
        [Browsable(false)]
        public MultilineTextBox TextBox => textBox;

        /// <inheritdoc/>
        protected override void CreateToolbarItems()
        {
            buttonIdNew = ToolBar.AddSpeedBtn(KnownButton.New, FileNew_Click);
            buttonIdOpen = ToolBar.AddSpeedBtn(KnownButton.Open, FileOpen_Click);
            buttonIdSave = ToolBar.AddSpeedBtn(KnownButton.Save, FileSave_Click);
            buttonIdUndo = ToolBar.AddSpeedBtn(KnownButton.Undo, Undo_Click);
            buttonIdRedo = ToolBar.AddSpeedBtn(KnownButton.Redo, Redo_Click);
        }

        private void Undo_Click(object? sender, EventArgs e)
        {
            TextBox.Undo();
        }

        private void Redo_Click(object? sender, EventArgs e)
        {
            TextBox.Redo();
        }

        private void FileNew_Click(object? sender, EventArgs e)
        {
            FileNewClick?.Invoke(this, EventArgs.Empty);
        }

        private void FileOpen_Click(object? sender, EventArgs e)
        {
            FileOpenClick?.Invoke(this, EventArgs.Empty);
        }

        private void FileSave_Click(object? sender, EventArgs e)
        {
            FileSaveClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
