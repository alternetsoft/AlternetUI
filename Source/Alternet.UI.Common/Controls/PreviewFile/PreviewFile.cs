using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to preview the file using one of the registered preview controls.
    /// </summary>
    public partial class PreviewFile : HiddenBorder, IFilePreview
    {
        private readonly Label label = new()
        {
            Visible = false,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        private readonly CardPanel cardPanel = new()
        {
        };

        private readonly List<PreviewFileRegisterItem> register = new();
        private string? fileName;

        static PreviewFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewFile"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PreviewFile(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewFile"/> class.
        /// </summary>
        public PreviewFile()
        {
            BackColor = SystemColors.Window;
            ForeColor = SystemColors.WindowText;
            cardPanel.Parent = this;
            label.Text = CommonStrings.Default.SelectFileToPreview;
            cardPanel.Add(null, label);
            ShowNoFile();
        }

        AbstractControl IFilePreview.Control { get => this; }

        /// <summary>
        /// Gets list of registered preview controls. One of these
        /// controls will be used to preview the file.
        /// </summary>
        public virtual IList<PreviewFileRegisterItem> RegisteredItems => register;

        /// <summary>
        /// Gets or sets path to the file which will be previewed in the control.
        /// </summary>
        public virtual string? FileName
        {
            get => fileName;

            set
            {
                if (fileName == value)
                    return;
                fileName = value;
                if (register.Count == 0)
                    RegisterDefaultPreviewControls();
                Reload();
            }
        }

        /// <summary>
        /// Shows text "No preview available".
        /// </summary>
        public virtual void ShowNoPreview()
        {
            string noPreviewText = CommonStrings.Default.NoPreviewAvailable;
            ShowLabel(noPreviewText);
        }

        /// <summary>
        /// Shows custom text.
        /// </summary>
        public virtual void ShowLabel(string text)
        {
            label.Text = text;
            cardPanel.SelectCard(0);
        }

        /// <summary>
        /// Resets preview control unloading any data.
        /// </summary>
        public virtual void Reset()
        {
            fileName = null;
            ShowNoFile();

            foreach (var item in register)
            {
                if (!item.ControlCreated)
                    continue;
                if (item.Control is not IFilePreview preview)
                    continue;
                preview.FileName = null;
            }
        }

        /// <summary>
        /// Shows text "Select a file to preview".
        /// </summary>
        public virtual void ShowNoFile()
        {
            string selectFileToPreviewText = CommonStrings.Default.SelectFileToPreview;
            ShowLabel(selectFileToPreviewText);
        }

        /// <summary>
        /// Reloads current previewed file.
        /// </summary>
        public virtual void Reload()
        {
            if(fileName is null || !File.Exists(fileName))
            {
                ShowNoFile();
                return;
            }

            var item = GetItem(fileName);

            if(item is null)
            {
                ShowNoPreview();
                return;
            }

            if(item.Card is null)
            {
                var index = cardPanel.Add(null, item.Control);
                item.Card = cardPanel.Cards[index];
            }

            if (item.Control is not IFilePreview preview)
            {
                ShowNoPreview();
                return;
            }

            preview.FileName = fileName;
            cardPanel.SelectCard(item.Card);
        }

        /// <summary>
        /// Gets registered item for the specified file.
        /// </summary>
        /// <param name="fileName">Path to file.</param>
        /// <returns></returns>
        public virtual PreviewFileRegisterItem? GetItem(string? fileName)
        {
            if (fileName is null)
                return null;

            foreach(var item in register)
            {
                if (item.IsSupportedFile(fileName))
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Registers default preview controls for use with this object.
        /// </summary>
        public virtual void RegisterDefaultPreviewControls()
        {
            RegisterPreview(
                new(PreviewUixmlSplitted.IsSupportedFile, PreviewUixmlSplitted.CreatePreviewControl));
            RegisterPreview(new(PreviewTextFile.IsSupportedFile, PreviewTextFile.CreatePreviewControl));
            RegisterPreview(new(PreviewInBrowser.IsSupportedFile, PreviewInBrowser.CreatePreviewControl));
        }

        /// <summary>
        /// Registers preview control for use with this object.
        /// </summary>
        /// <param name="item">Preview control information.</param>
        public virtual void RegisterPreview(PreviewFileRegisterItem item)
        {
            register.Add(item);
        }

        /// <summary>
        /// Implements registered item for the preview control.
        /// </summary>
        public class PreviewFileRegisterItem
        {
            private AbstractControl? control;

            /// <summary>
            /// Initializes a new instance of the <see cref="PreviewFileRegisterItem"/> class.
            /// </summary>
            /// <param name="isSupported">Function which is called when preview control
            /// needs to be queried whether is supports preview of the specified file.</param>
            /// <param name="createControl">Function which is called when preview control
            /// needs to be created.</param>
            public PreviewFileRegisterItem(
                Func<string, bool> isSupported,
                Func<IFilePreview> createControl)
            {
                this.IsSupportedFn = isSupported;
                this.CreateControlFn = createControl;
            }

            /// <summary>
            /// Gets whether control is created.
            /// </summary>
            public virtual bool ControlCreated
            {
                get => control is not null;
            }

            /// <summary>
            /// Gets preview control.
            /// </summary>
            public virtual AbstractControl Control
            {
                get
                {
                    control ??= CreateControlFn().Control;
                    return control;
                }
            }

            internal Func<string, bool> IsSupportedFn { get; }

            internal Func<IFilePreview> CreateControlFn { get; }

            internal CardPanelItem? Card { get; set; }

            /// <summary>
            /// Gets whether specified file is supported for preview by this item.
            /// </summary>
            /// <param name="fileName">Path to file.</param>
            /// <returns></returns>
            public virtual bool IsSupportedFile(string? fileName)
            {
                if (fileName is null)
                    return false;

                return IsSupportedFn(fileName);
            }
        }
    }
}
