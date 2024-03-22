using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class PreviewFileSplitted : Control, IFilePreview 
    {
        private readonly SplittedPanel panel = new()
        {
        };
        
        private readonly IFilePreview first;
        private readonly IFilePreview second;

        public PreviewFileSplitted(IFilePreview centerPanel, IFilePreview rightPanel)
        {
            this.first = centerPanel;
            this.second = rightPanel;
            panel.Parent = this;
            panel.DoInsideLayout(() =>
            {
                panel.TopVisible = false;
                panel.BottomVisible = false;
                panel.LeftVisible = false;
                panel.RightPanel.Width = 300;
            });
            first.Control.Parent = panel.CenterPanel;
            second.Control.Parent = panel.RightPanel;
        }

        public IFilePreview First => first;
        
        public IFilePreview Second => second;

        public string? FileName
        {
            get => first.FileName;

            set
            {
                try
                {
                    first.FileName = value;
                }
                finally
                {
                    second.FileName = value;
                }
            }
        }

        Control IFilePreview.Control => this;
    }
}
