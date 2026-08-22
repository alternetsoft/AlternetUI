using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements panel with <see cref="RichTextBox"/> and toolbar with
    /// text edit buttons.
    /// </summary>
    [ControlCategory(KnownControlCategory.Panels)]
    public partial class PanelRichTextBox : PanelWithToolBar
    {
        /// <summary>
        /// Gets or sets default margin for textbox control in this panel.
        /// </summary>
        public static Thickness DefaultTextBoxMargin = 5;

        /// <summary>
        /// Gets or sets a value indicating whether to use the background color of the textbox control
        /// as background color of the panel.
        /// </summary>
        public static bool UseTextBoxBackgroundColor = true;

        private readonly RichTextBox textBox = new();

        private ObjectUniqueId buttonIdNew;
        private ObjectUniqueId buttonIdOpen;
        private ObjectUniqueId buttonIdSave;
        private ObjectUniqueId buttonIdUndo;
        private ObjectUniqueId buttonIdRedo;
        private ObjectUniqueId buttonIdBold;
        private ObjectUniqueId buttonIdItalic;
        private ObjectUniqueId buttonIdUnderline;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelRichTextBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PanelRichTextBox(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelRichTextBox"/> class.
        /// </summary>
        public PanelRichTextBox()
        {
            textBox.HasBorder = false;
            textBox.VerticalAlignment = VerticalAlignment.Fill;
            textBox.Margin = DefaultTextBoxMargin;
            textBox.Parent = this;

            if (UseTextBoxBackgroundColor)
            {
                ParentBackColor = false;
                BackColor = textBox.RealBackgroundColor;
            }
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
        /// Gets id of the 'Bold' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdBold => buttonIdBold;

        /// <summary>
        /// Gets id of the 'Italic' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdItalic => buttonIdItalic;

        /// <summary>
        /// Gets id of the 'Underline' toolbar item.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdUnderline => buttonIdUnderline;

        /// <summary>
        /// Gets <see cref="RichTextBox"/> control used in this panel.
        /// </summary>
        [Browsable(false)]
        public RichTextBox TextBox => textBox;

        /// <inheritdoc/>
        protected override void CreateToolbarItems()
        {
            buttonIdNew = ToolBar.AddSpeedBtn(KnownButton.New, OnFileNewClick);
            buttonIdOpen = ToolBar.AddSpeedBtn(KnownButton.Open, OnFileOpenClick);
            buttonIdSave = ToolBar.AddSpeedBtn(KnownButton.Save, OnFileSaveClick);
            buttonIdUndo = ToolBar.AddSpeedBtn(KnownButton.Undo, OnUndoClick);
            buttonIdRedo = ToolBar.AddSpeedBtn(KnownButton.Redo, OnRedoClick);
            buttonIdBold = ToolBar.AddSpeedBtn(KnownButton.Bold, OnBoldClick);
            buttonIdItalic = ToolBar.AddSpeedBtn(KnownButton.Italic, OnItalicClick);
            buttonIdUnderline = ToolBar.AddSpeedBtn(KnownButton.Underline, OnUnderlineClick);
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);

            if (UseTextBoxBackgroundColor)
            {
                ParentBackColor = false;
                BackColor = textBox.RealBackgroundColor;
            }
        }

        /// <summary>
        /// Called when 'Undo' button is clicked on the toolbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnUndoClick(object? sender, EventArgs e)
        {
            TextBox.Undo();
        }

        /// <summary>
        /// Called when 'Redo' button is clicked on the toolbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnRedoClick(object? sender, EventArgs e)
        {
            TextBox.Redo();
        }

        /// <summary>
        /// Called when 'Bold' button is clicked on the toolbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnBoldClick(object? sender, EventArgs e)
        {
            TextBox.SelectionToggleBold();
        }

        /// <summary>
        /// Called when 'Italic' button is clicked on the toolbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnItalicClick(object? sender, EventArgs e)
        {
            TextBox.SelectionToggleItalic();
        }

        /// <summary>
        /// Called when 'Underline' button is clicked on the toolbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnUnderlineClick(object? sender, EventArgs e)
        {
            TextBox.SelectionToggleUnderlined();
        }

        /// <summary>
        /// Called when 'New' button is clicked on the toolbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileNewClick(object? sender, EventArgs e)
        {
            FileNewClick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when 'Open' button is clicked on the toolbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileOpenClick(object? sender, EventArgs e)
        {
            FileOpenClick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when 'Save' button is clicked on the toolbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileSaveClick(object? sender, EventArgs e)
        {
            FileSaveClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
