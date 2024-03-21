using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class PreviewUixml : Control
    {
        private readonly ScrollViewer scrollView = new()
        {
            BackgroundColor = SystemColors.Window,
        };

        private readonly Control control = new()
        {
            SuggestedSize = (400, 400),
            BackgroundColor = SystemColors.ButtonFace,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private string? fileName;

        public PreviewUixml()
        {
            scrollView.Parent = this;
            control.Parent = scrollView;
        }

        public string? FileName
        {
            get => fileName;

            set
            {
                if (fileName == value)
                    return;
                fileName = value;
            }
        }
    }
}
