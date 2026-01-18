using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements preview control which splits its view into two docked panels
    /// and uses <see cref="First"/> and <see cref="Second"/> preview sub-controls there.
    /// When <see cref="FileName"/> is changed, preview sub-controls are also updated.
    /// </summary>
    public partial class PreviewFileSplitted : HiddenBorder, IFilePreview
    {
        private readonly SplittedPanel panel = new()
        {
        };

        private readonly IFilePreview first;
        private readonly IFilePreview second;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewFileSplitted"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PreviewFileSplitted(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewFileSplitted"/> class.
        /// </summary>
        public PreviewFileSplitted()
            : this(new PreviewFile(), new PreviewFileDummy(), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewFileSplitted"/> class.
        /// </summary>
        /// <param name="centerPanel">Preview control which is docked in the center.</param>
        /// <param name="secondPanel">Second preview control.</param>
        /// <param name="isRight">Whether second panel is shown in the right side bar.</param>
        public PreviewFileSplitted(IFilePreview centerPanel, IFilePreview secondPanel, bool isRight)
        {
            this.first = centerPanel;
            this.second = secondPanel;
            panel.Parent = this;
            panel.DoInsideLayout(() =>
            {
                panel.TopVisible = false;
                panel.BottomVisible = false;
                panel.LeftVisible = false;
                panel.RightPanel.Width = 300;
                panel.BottomPanel.Height = 300;
            });
            first.Control.Parent = panel.CenterPanel;
            if(isRight)
                SecondPanelToRight();
            else
                SecondPanelToBottom();
        }

        /// <summary>
        /// First preview sub-control which default docking position is at the center.
        /// </summary>
        public virtual IFilePreview First
        {
            get => first;
        }

        /// <summary>
        /// First preview sub-control which default docking position is at the right.
        /// </summary>
        public virtual IFilePreview Second
        {
            get => second;
        }

        /// <summary>
        /// <inheritdoc cref="IFilePreview.FileName"/>
        /// </summary>
        public virtual string? FileName
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

        AbstractControl IFilePreview.Control => this;

        /// <summary>
        /// Moves second preview panel to the right side bar.
        /// </summary>
        public virtual void SecondPanelToRight()
        {
            panel.BottomVisible = false;
            second.Control.Parent = panel.RightPanel;
            panel.RightVisible = true;
        }

        /// <summary>
        /// Moves second preview panel to the bottom bar.
        /// </summary>
        public virtual void SecondPanelToBottom()
        {
            panel.RightVisible = false;
            second.Control.Parent = panel.BottomPanel;
            panel.BottomVisible = true;
        }
    }
}
