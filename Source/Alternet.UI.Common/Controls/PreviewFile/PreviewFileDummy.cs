using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PreviewFileDummy : Control, IFilePreview
    {
        public string? FileName { get; set; }

        public AbstractControl Control => this;
    }
}
