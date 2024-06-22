﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements panel with top, bottom, left, right sub-panels and splitters.
    /// </summary>
    public partial class SplittedPanel : LayoutPanel
    {
        private readonly Control rightPanel;
        private readonly Control leftPanel;
        private readonly Control topPanel;
        private readonly Control bottomPanel;
        private readonly Control fillPanel;

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
            var panelSize = DefaultPanelSize;

            rightPanel = CreateRightPanel();
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Width = panelSize.Right;

            leftPanel = CreateLeftPanel();
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Width = panelSize.Left;

            topPanel = CreateTopPanel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = panelSize.Top;

            bottomPanel = CreateBottomPanel();
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = panelSize.Bottom;

            fillPanel = CreateCenterPanel();
            fillPanel.Dock = DockStyle.Fill;

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
        /// Gets <see cref="ControlSet"/> with all splitters.
        /// </summary>
        public ControlSet Splitters =>
            ControlSet.New(leftSplitter, topSplitter, rightSplitter, bottomSplitter);

        /// <summary>
        /// Gets right sub-panel.
        /// </summary>
        [Browsable(false)]
        public Control RightPanel => rightPanel;

        /// <summary>
        /// Gets left sub-panel.
        /// </summary>
        [Browsable(false)]
        public Control LeftPanel => leftPanel;

        /// <summary>
        /// Gets top sub-panel.
        /// </summary>
        [Browsable(false)]
        public Control TopPanel => topPanel;

        /// <summary>
        /// Gets bottom sub-panel.
        /// </summary>
        [Browsable(false)]
        public Control BottomPanel => bottomPanel;

        /// <summary>
        /// Gets center sub-panel. Same as <see cref="CenterPanel"/>.
        /// </summary>
        [Browsable(false)]
        public Control FillPanel => fillPanel;

        /// <summary>
        /// Gets center sub-panel. Same as <see cref="FillPanel"/>.
        /// </summary>
        [Browsable(false)]
        public Control CenterPanel => fillPanel;

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
        /// Gets or sets width of the left panel.
        /// </summary>
        public virtual double LeftPanelWidth
        {
            get => LeftPanel.Width;
            set => LeftPanel.Width = value;
        }

        /// <summary>
        /// Gets or sets width of the right panel.
        /// </summary>
        public virtual double RightPanelWidth
        {
            get => RightPanel.Width;
            set => RightPanel.Width = value;
        }

        /// <summary>
        /// Gets or sets height of the top panel.
        /// </summary>
        public virtual double TopPanelHeight
        {
            get => TopPanel.Height;
            set => TopPanel.Height = value;
        }

        /// <summary>
        /// Gets or sets height of the bottom panel.
        /// </summary>
        public virtual double BottomPanelHeight
        {
            get => BottomPanel.Height;
            set => BottomPanel.Height = value;
        }

        /// <summary>
        /// Gets or sets whether top panel is visible.
        /// </summary>
        public virtual bool TopVisible
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
        /// Gets or sets whether both top and bottom panels are visible.
        /// </summary>
        public bool TopBottomVisible
        {
            get
            {
                return TopVisible && BottomVisible;
            }

            set
            {
                TopVisible = value;
                BottomVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether both left and right panels are visible.
        /// </summary>
        public bool LeftRightVisible
        {
            get
            {
                return LeftVisible && RightVisible;
            }

            set
            {
                LeftVisible = value;
                RightVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether bottom panel is visible.
        /// </summary>
        public virtual bool BottomVisible
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
        public virtual bool RightVisible
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
        public virtual bool LeftVisible
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

        /// <summary>
        /// Gets default size of the left, top, right and bottom panels.
        /// </summary>
        public virtual Thickness DefaultPanelSize => (50, 30, 50, 30);

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Creates right panel.
        /// </summary>
        /// <returns></returns>
        protected virtual Control CreateRightPanel() => CreateAnyPanel();

        /// <summary>
        /// Creates left panel.
        /// </summary>
        /// <returns></returns>
        protected virtual Control CreateLeftPanel() => CreateAnyPanel();

        /// <summary>
        /// Creates top panel.
        /// </summary>
        /// <returns></returns>
        protected virtual Control CreateTopPanel() => CreateAnyPanel();

        /// <summary>
        /// Creates bottom panel.
        /// </summary>
        /// <returns></returns>
        protected virtual Control CreateBottomPanel() => CreateAnyPanel();

        /// <summary>
        /// Creates center panel.
        /// </summary>
        /// <returns></returns>
        protected virtual Control CreateCenterPanel() => CreateAnyPanel();

        /// <summary>
        /// Creates panel.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// By default used in all create panel methods.
        /// </remarks>
        protected virtual Control CreateAnyPanel()
        {
            var result = new Panel();
            return result;
        }
    }
}