using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class PreviewFile : Control, IFilePreview
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

        private List<PreviewFileRegisterItem> register = new();
        private string? fileName;

        static PreviewFile()
        {
        }

        public PreviewFile()
        {
            cardPanel.Parent = this;
            label.Text = CommonStrings.Default.SelectFileToPreview;
            cardPanel.Add(null, label);
            ShowNoFile();
        }

        Control IFilePreview.Control { get => this; }

        public IList<PreviewFileRegisterItem> Register => register;

        public string? FileName
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

        public void ShowNoPreview()
        {
            string noPreviewText = CommonStrings.Default.NoPreviewAvailable;
            ShowLabel(noPreviewText);
        }

        public void ShowLabel(string text)
        {
            label.Text = text;
            cardPanel.SelectCard(0);
        }

        public void Reset()
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

        public void ShowNoFile()
        {
            string selectFileToPreviewText = CommonStrings.Default.SelectFileToPreview;
            ShowLabel(selectFileToPreviewText);
        }

        public void Reload()
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

        public PreviewFileRegisterItem? GetItem(string? fileName)
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

        public void RegisterDefaultPreviewControls()
        {
            RegisterPreview(new(PreviewUixml.IsSupportedFile, PreviewUixml.CreatePreviewControl));
            RegisterPreview(new(PreviewInBrowser.IsSupportedFile, PreviewInBrowser.CreatePreviewControl));
        }

        public void RegisterPreview(PreviewFileRegisterItem item)
        {
            register.Add(item);
        }

        public class PreviewFileRegisterItem
        {
            private Control? control;

            public PreviewFileRegisterItem(
                Func<string, bool> isSupported,
                Func<IFilePreview> createControl)
            {
                this.IsSupportedFn = isSupported;
                this.CreateControlFn = createControl;
            }

            public bool ControlCreated
            {
                get => control is not null;
            }

            public Control Control
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

            public bool IsSupportedFile(string? fileName)
            {
                if (fileName is null)
                    return false;

                return IsSupportedFn(fileName);
            }
        }
    }
}
