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
        /// <summary>
        /// Specifies the default alignment for the content of the second preview element.
        /// </summary>
        /// <remarks>The default value is set to right alignment. This field can be used to ensure
        /// consistent alignment behavior for the second element's content across the application.</remarks>
        public static ElementContentAlign DefaultSecondAlignment = ElementContentAlign.Right;

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
            : this(new PreviewFile(), new PreviewFileDummy(), DefaultSecondAlignment)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewFileSplitted"/> class.
        /// </summary>
        /// <param name="centerPanel">Preview control which is docked in the center.</param>
        /// <param name="secondPanel">Second preview control.</param>
        /// <param name="alignment">Specifies the alignment of the second panel.</param>
        public PreviewFileSplitted(IFilePreview centerPanel, IFilePreview secondPanel, ElementContentAlign alignment)
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
                panel.LeftPanel.Width = 300;
                panel.BottomPanel.Height = 300;
                panel.TopPanel.Height = 300;
            });
            first.Control.Parent = panel.CenterPanel;

            AlignSecondPanel(alignment);
        }

        /// <summary>
        /// Gets the main panel used for arranging and displaying first and second preview panels.
        /// </summary>
        public SplittedPanel MainPanel => panel;

        /// <summary>
        /// First preview sub-control which default docking position is at the center.
        /// </summary>
        public IFilePreview First
        {
            get => first;
        }

        /// <summary>
        /// Second preview sub-control which default docking position is at the right or bottom.
        /// </summary>
        public IFilePreview Second
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
        /// Aligns the second panel to the specified edge of the container.
        /// </summary>
        /// <remarks>Use this method to reposition the second panel within the container according to the
        /// desired alignment. Supplying an invalid alignment value may result in no action or undefined
        /// behavior.</remarks>
        /// <param name="alignment">One of the <see cref="ElementContentAlign"/>
        /// values that specifies the edge (Left, Right, Top, or Bottom) to
        /// which the second panel will be aligned.</param>
        public virtual void AlignSecondPanel(ElementContentAlign alignment)
        {
            switch (alignment)
            {
                case ElementContentAlign.Left:
                    SecondPanelToLeft();
                    break;
                case ElementContentAlign.Right:
                    SecondPanelToRight();
                    break;
                case ElementContentAlign.Top:
                    SecondPanelToTop();
                    break;
                case ElementContentAlign.Bottom:
                    SecondPanelToBottom();
                    break;
            }
        }

        /// <summary>
        /// Moves the second panel to the top position within the main panel and updates the visibility of other panel
        /// sides accordingly.
        /// </summary>
        /// <remarks>Call this method to display the second panel at the top of the main panel while
        /// hiding the bottom, left, and right panels. Ensure that the second panel is properly initialized and assigned
        /// before invoking this method.</remarks>
        public virtual void SecondPanelToTop()
        {
            panel.BottomVisible = false;
            panel.LeftVisible = false;
            panel.RightVisible = false;
            second.Control.Parent = panel.TopPanel;
            panel.TopVisible = true;
        }

        /// <summary>
        /// Displays the second panel on the left side of the main panel by adjusting visibility settings.
        /// </summary>
        /// <remarks>This method hides the bottom, top, and right panels, and makes the left panel visible
        /// to accommodate the second panel. Ensure that the second panel is properly initialized before calling this
        /// method.</remarks>
        public virtual void SecondPanelToLeft()
        {
            panel.BottomVisible = false;
            panel.TopVisible = false;
            panel.RightVisible = false;
            second.Control.Parent = panel.LeftPanel;
            panel.LeftVisible = true;
        }

        /// <summary>
        /// Moves second preview panel to the right side bar.
        /// </summary>
        public virtual void SecondPanelToRight()
        {
            panel.BottomVisible = false;
            panel.TopVisible = false;
            panel.LeftVisible = false;
            second.Control.Parent = panel.RightPanel;
            panel.RightVisible = true;
        }

        /// <summary>
        /// Moves second preview panel to the bottom bar.
        /// </summary>
        public virtual void SecondPanelToBottom()
        {
            panel.RightVisible = false;
            panel.TopVisible = false;
            panel.LeftVisible = false;
            second.Control.Parent = panel.BottomPanel;
            panel.BottomVisible = true;
        }
    }
}
