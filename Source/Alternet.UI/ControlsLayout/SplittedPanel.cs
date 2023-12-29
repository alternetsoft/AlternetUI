using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements panel with top, bottom, left, right sub-panels and splitters.
    /// </summary>
    public class SplittedPanel : LayoutPanel
    {
        private readonly Panel rightPanel = new()
        {
            Dock = DockStyle.Right,
            Width = 50,
        };

        private readonly Panel leftPanel = new()
        {
            Dock = DockStyle.Left,
            Width = 50,
        };

        private readonly Panel topPanel = new()
        {
            Dock = DockStyle.Top,
            Height = 30,
        };

        private readonly Panel bottomPanel = new()
        {
            Dock = DockStyle.Bottom,
            Height = 30,
        };

        private readonly Panel fillPanel = new()
        {
            Dock = DockStyle.Fill,
        };

        private readonly Splitter leftSplitter = new()
        {
            Dock = DockStyle.Left,
        };

        private readonly Splitter rightSplitter = new()
        {
            Dock = DockStyle.Right,
        };

        private readonly Splitter topSplitter = new()
        {
            Dock = DockStyle.Top,
        };

        private readonly Splitter bottomSplitter = new()
        {
            Dock = DockStyle.Bottom,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SplittedPanel"/> class.
        /// </summary>
        public SplittedPanel()
        {
            SuspendLayout();
            FillPanel.Parent = this;
            RightSplitter.Parent = this;
            RightPanel.Parent = this;
            LeftSplitter.Parent = this;
            LeftPanel.Parent = this;
            TopSplitter.Parent = this;
            TopPanel.Parent = this;
            BottomSplitter.Parent = this;
            BottomPanel.Parent = this;
            ResumeLayout();
        }

        /// <summary>
        /// Gets right sub-panel.
        /// </summary>
        [Browsable(false)]
        public Panel RightPanel => rightPanel;

        /// <summary>
        /// Gets left sub-panel.
        /// </summary>
        [Browsable(false)]
        public Panel LeftPanel => leftPanel;

        /// <summary>
        /// Gets top sub-panel.
        /// </summary>
        [Browsable(false)]
        public Panel TopPanel => topPanel;

        /// <summary>
        /// Gets bottom sub-panel.
        /// </summary>
        [Browsable(false)]
        public Panel BottomPanel => bottomPanel;

        /// <summary>
        /// Gets center sub-panel.
        /// </summary>
        [Browsable(false)]
        public Panel FillPanel => fillPanel;

        /// <summary>
        /// Gets left splitter.
        /// </summary>
        [Browsable(false)]
        public Splitter LeftSplitter => leftSplitter;

        /// <summary>
        /// Gets right splitter.
        /// </summary>
        [Browsable(false)]
        public Splitter RightSplitter => rightSplitter;

        /// <summary>
        /// Gets top splitter.
        /// </summary>
        [Browsable(false)]
        public Splitter TopSplitter => topSplitter;

        /// <summary>
        /// Gets bottom splitter.
        /// </summary>
        [Browsable(false)]
        public Splitter BottomSplitter => bottomSplitter;

        /// <summary>
        /// Gets or sets whether top panel is visible.
        /// </summary>
        public bool TopVisible
        {
            get
            {
                return TopPanel.Visible;
            }

            set
            {
                TopPanel.Visible = value;
                TopSplitter.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether bottom panel is visible.
        /// </summary>
        public bool BottomVisible
        {
            get
            {
                return BottomPanel.Visible;
            }

            set
            {
                BottomPanel.Visible = value;
                BottomSplitter.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether right panel is visible.
        /// </summary>
        public bool RightVisible
        {
            get
            {
                return RightPanel.Visible;
            }

            set
            {
                RightPanel.Visible = value;
                RightSplitter.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether left panel is visible.
        /// </summary>
        public bool LeftVisible
        {
            get
            {
                return LeftPanel.Visible;
            }

            set
            {
                LeftPanel.Visible = value;
                LeftSplitter.Visible = value;
            }
        }
    }
}