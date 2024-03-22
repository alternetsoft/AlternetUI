using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements preview control which splits it's view into two docked panels
    /// and uses <see cref="First"/> and <see cref="Second"/> preview sub-controls there.
    /// When <see cref="FileName"/> is changed, preview sub-controls are also updated.
    /// </summary>
    public class PreviewFileSplitted : Control, IFilePreview
    {
        private readonly SplittedPanel panel = new()
        {
        };

        private readonly IFilePreview first;
        private readonly IFilePreview second;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewFileSplitted"/> class.
        /// </summary>
        /// <param name="centerPanel">Preview control which is docked in the center.</param>
        /// <param name="rightPanel">Preview control which is docked to the right.</param>
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

        /// <summary>
        /// First preview sub-control which default docking position is at the center.
        /// </summary>
        public IFilePreview First => first;

        /// <summary>
        /// First preview sub-control which default docking position is at the right.
        /// </summary>
        public IFilePreview Second => second;

        /// <summary>
        /// <inheritdoc cref="IFilePreview.FileName"/>
        /// </summary>
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
