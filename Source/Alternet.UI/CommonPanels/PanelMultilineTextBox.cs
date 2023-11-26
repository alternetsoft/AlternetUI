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
    /// Implements panel with <see cref="MultilineTextBox"/> and <see cref="AuiToolbar"/> with
    /// text edit buttons.
    /// </summary>
    public class PanelMultilineTextBox : PanelAuiManager
    {
        private MultilineTextBox? textBox;
        private int buttonIdNew;
        private int buttonIdOpen;
        private int buttonIdSave;
        private int buttonIdUndo;
        private int buttonIdRedo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelMultilineTextBox"/> class.
        /// </summary>
        public PanelMultilineTextBox()
        {
            Initialize();
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
        public int ButtonIdNew => buttonIdNew;

        /// <summary>
        /// Gets id of the 'Open' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdOpen => buttonIdOpen;

        /// <summary>
        /// Gets id of the 'Save' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdSave => buttonIdSave;

        /// <summary>
        /// Gets id of the 'Undo' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdUndo => buttonIdUndo;

        /// <summary>
        /// Gets id of the 'Redo' toolbar item.
        /// </summary>
        [Browsable(false)]
        public int ButtonIdRedo => buttonIdRedo;

        /// <summary>
        /// Gets <see cref="RichTextBox"/> control used in this panel.
        /// </summary>
        [Browsable(false)]
        public MultilineTextBox TextBox
        {
            get
            {
                textBox ??= new MultilineTextBox();
                return textBox;
            }
        }

        /// <inheritdoc/>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent == null || StateFlags.HasFlag(ControlFlags.ParentAssigned))
                return;

            Manager.AddPane(Control.SubstituteControl(TextBox), CenterPane);
            Manager.AddPane(Toolbar, ToolbarPane);

            Manager.Update();
        }

        /// <summary>
        /// Creates toolbar items.
        /// </summary>
        protected virtual void CreateToolbarItems()
        {
            var toolbar = Toolbar;
            var imageSize = GetToolBitmapSize();

            var images = KnownSvgImages.GetForSize(
                toolbar.GetSvgColor(KnownSvgColor.Normal),
                imageSize);

            buttonIdNew = toolbar.AddTool(
                CommonStrings.Default.ButtonNew,
                images.ImgFileNew,
                CommonStrings.Default.ButtonNew);

            buttonIdOpen = toolbar.AddTool(
                CommonStrings.Default.ButtonOpen,
                images.ImgFileOpen,
                CommonStrings.Default.ButtonOpen);

            buttonIdSave = toolbar.AddTool(
                CommonStrings.Default.ButtonSave,
                images.ImgFileSave,
                CommonStrings.Default.ButtonSave);

            buttonIdUndo = toolbar.AddTool(
                CommonStrings.Default.ButtonUndo,
                images.ImgUndo,
                CommonStrings.Default.ButtonUndo);

            buttonIdRedo = toolbar.AddTool(
                CommonStrings.Default.ButtonRedo,
                images.ImgRedo,
                CommonStrings.Default.ButtonRedo);

            toolbar.AddToolOnClick(buttonIdNew, FileNew_Click);
            toolbar.AddToolOnClick(buttonIdOpen, FileOpen_Click);
            toolbar.AddToolOnClick(buttonIdSave, FileSave_Click);
            toolbar.AddToolOnClick(buttonIdUndo, Undo_Click);
            toolbar.AddToolOnClick(buttonIdRedo, Redo_Click);
            toolbar.Realize();
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

        private void Initialize()
        {
            DefaultRightPaneBestSize = new(150, 200);
            DefaultRightPaneMinSize = new(150, 200);
            RightNotebookDefaultCreateStyle = AuiNotebookCreateStyle.Top;
            TextBox.HasBorder = false;
            DefaultToolbarStyle &=
                ~(AuiToolbarCreateStyle.Text | AuiToolbarCreateStyle.HorzLayout);
            Toolbar.Required();
            CreateToolbarItems();
        }
    }
}
