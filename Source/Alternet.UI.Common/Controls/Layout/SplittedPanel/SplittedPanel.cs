using System;
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
    [ControlCategory("Containers")]
    public partial class SplittedPanel : LayoutPanel
    {
        private readonly AbstractControl rightPanel;
        private readonly AbstractControl leftPanel;
        private readonly AbstractControl topPanel;
        private readonly AbstractControl bottomPanel;
        private readonly AbstractControl fillPanel;

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
        /// <param name="parent">Parent of the control.</param>
        public SplittedPanel(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplittedPanel"/> class.
        /// </summary>
        public SplittedPanel()
        {
            CanSelect = false;
            TabStop = false;
            ParentBackColor = true;
            ParentForeColor = true;

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
            try
            {
                FillPanel.Parent = this;
                RightSplitter.Parent = this;
                RightPanel.Parent = this;
                LeftSplitter.Parent = this;
                LeftPanel.Parent = this;
                TopSplitter.Parent = this;
                TopPanel.Parent = this;
                BottomSplitter.Parent = this;
                BottomPanel.Parent = this;
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Gets <see cref="ControlSet"/> with all splitters.
        /// </summary>
        [Browsable(false)]
        public ControlSet Splitters =>
            ControlSet.New(leftSplitter, topSplitter, rightSplitter, bottomSplitter);

        /// <summary>
        /// Gets right sub-panel.
        /// </summary>
        [Browsable(false)]
        public AbstractControl RightPanel => rightPanel;

        /// <summary>
        /// Gets left sub-panel.
        /// </summary>
        [Browsable(false)]
        public AbstractControl LeftPanel => leftPanel;

        /// <summary>
        /// Gets top sub-panel.
        /// </summary>
        [Browsable(false)]
        public AbstractControl TopPanel => topPanel;

        /// <summary>
        /// Gets bottom sub-panel.
        /// </summary>
        [Browsable(false)]
        public AbstractControl BottomPanel => bottomPanel;

        /// <summary>
        /// Gets center sub-panel. Same as <see cref="CenterPanel"/>.
        /// </summary>
        [Browsable(false)]
        public AbstractControl FillPanel => fillPanel;

        /// <summary>
        /// Gets center sub-panel. Same as <see cref="FillPanel"/>.
        /// </summary>
        [Browsable(false)]
        public AbstractControl CenterPanel => fillPanel;

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
        public virtual Coord LeftPanelWidth
        {
            get => LeftPanel.Width;
            set => LeftPanel.Width = value;
        }

        /// <summary>
        /// Gets or sets width of the right panel.
        /// </summary>
        public virtual Coord RightPanelWidth
        {
            get => RightPanel.Width;
            set => RightPanel.Width = value;
        }

        /// <summary>
        /// Gets or sets height of the top panel.
        /// </summary>
        public virtual Coord TopPanelHeight
        {
            get => TopPanel.Height;
            set => TopPanel.Height = value;
        }

        /// <summary>
        /// Gets or sets height of the bottom panel.
        /// </summary>
        public virtual Coord BottomPanelHeight
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
        [Browsable(false)]
        public virtual Thickness DefaultPanelSize => (50, 30, 50, 30);

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Ensures that the specified panel position contains a child of the specified type.
        /// </summary>
        /// <remarks>This method checks if the panel at the given position already
        /// has a child of the specified type. If not, it creates a new instance of the
        /// type <typeparamref name="T"/> and assigns it as the
        /// child. If a child of a different type is present, an exception is thrown.</remarks>
        /// <typeparam name="T">The type of the child control to ensure.
        /// Must be a subclass of <see cref="AbstractControl"/> and have a
        /// parameterless constructor.</typeparam>
        /// <param name="position">The position of the panel where the child control
        /// should be ensured.</param>
        /// <returns>An instance of the specified child control type <typeparamref name="T"/>.
        /// If the panel already contains a
        /// child of the correct type, that instance is returned; otherwise, a new instance
        /// is created and returned.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the panel at the
        /// specified position already contains a child of a different type than
        /// <typeparamref name="T"/>.</exception>
        public virtual T EnsureChild<T>(SplitPanelPosition position)
            where T : AbstractControl, new()
        {
            var panel = GetPanel(position);
            var child = panel.FirstChild;

            if (child == null)
            {
                child = new T();
                child.Parent = panel;
            }
            else
            {
                if (child is not T)
                {
                    throw new InvalidOperationException(
                        $"Panel at position '{position}' already has a child of type '{child.GetType().Name}', " +
                        $"but expected type is '{typeof(T).Name}'.");
                }
            }

            return (T)child;
        }

        /// <summary>
        /// Retrieves the panel associated with the specified position.
        /// </summary>
        /// <param name="position">The position of the panel to retrieve.
        /// Must be one of the defined <see cref="SplitPanelPosition"/> values.</param>
        /// <returns>The <see cref="AbstractControl"/> corresponding to the specified
        /// <paramref name="position"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="position"/>
        /// is not a valid <see cref="SplitPanelPosition"/>.</exception>
        public virtual AbstractControl GetPanel(SplitPanelPosition position)
        {
            return position switch
            {
                SplitPanelPosition.Left => LeftPanel,
                SplitPanelPosition.Right => RightPanel,
                SplitPanelPosition.Top => TopPanel,
                SplitPanelPosition.Bottom => BottomPanel,
                SplitPanelPosition.Center => FillPanel,
                _ => throw new ArgumentOutOfRangeException(nameof(position), position, null),
            };
        }

        /// <summary>
        /// Gets visibility state of the panel by its position.
        /// </summary>
        /// <param name="position">The position of the panel</param>
        /// <returns><c>true</c> if the panel is visible; otherwise, <c>false</c>.</returns>
        public virtual bool GetPanelVisible(SplitPanelPosition position)
        {
            return position switch
            {
                SplitPanelPosition.Left => LeftVisible,
                SplitPanelPosition.Right => RightVisible,
                SplitPanelPosition.Top => TopVisible,
                SplitPanelPosition.Bottom => BottomVisible,
                SplitPanelPosition.Center => FillPanel.Visible,
                _ => false,
            };
        }

        /// <summary>
        /// Retrieves the width of the specified panel based on its position.
        /// </summary>
        /// <param name="position">The position of the panel whose width is to be retrieved.
        /// Must be one of the defined <see
        /// cref="SplitPanelPosition"/> values.</param>
        /// <returns>The width of the panel at the specified position.
        /// Returns -1 if the position is not recognized.</returns>
        public virtual Coord GetPanelWidth(SplitPanelPosition position)
        {
            return position switch
            {
                SplitPanelPosition.Left => LeftPanelWidth,
                SplitPanelPosition.Right => RightPanelWidth,
                SplitPanelPosition.Top => TopPanel.Width,
                SplitPanelPosition.Bottom => BottomPanel.Width,
                SplitPanelPosition.Center => FillPanel.Width,
                _ => -1,
            };
        }

        /// <summary>
        /// Sets the width of the specified panel.
        /// </summary>
        /// <param name="position">The position of the panel to set the width for.
        /// Must be either <see cref="SplitPanelPosition.Left"/> or <see
        /// cref="SplitPanelPosition.Right"/>.</param>
        /// <param name="width">The new width to set for the specified panel.</param>
        /// <returns><see langword="true"/> if the width was successfully set;
        /// otherwise, <see langword="false"/> if the position
        /// is not valid.</returns>
        public virtual bool SetPanelWidth(SplitPanelPosition position, Coord width)
        {
            switch (position)
            {
                case SplitPanelPosition.Left:
                    LeftPanelWidth = width;
                    return true;
                case SplitPanelPosition.Right:
                    RightPanelWidth = width;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Retrieves the height of the specified panel based on its position.
        /// </summary>
        /// <param name="position">The position of the panel whose height is to be retrieved.
        /// Must be one of the defined <see cref="SplitPanelPosition"/> values.</param>
        /// <returns>The height of the panel at the specified position. Returns -1 if the
        /// position is not recognized.</returns>
        public virtual Coord GetPanelHeight(SplitPanelPosition position)
        {
            return position switch
            {
                SplitPanelPosition.Top => TopPanelHeight,
                SplitPanelPosition.Bottom => BottomPanelHeight,
                SplitPanelPosition.Left => LeftPanel.Height,
                SplitPanelPosition.Right => RightPanel.Height,
                SplitPanelPosition.Center => FillPanel.Height,
                _ => -1,
            };
        }

        /// <summary>
        /// Ensures that the height of the specified panel is at least the given value.
        /// </summary>
        /// <remarks>If the current height of the panel is less than the specified value,
        /// the panel's height is increased to meet the minimum requirement.</remarks>
        /// <param name="position">The position of the panel whose height is to be adjusted.</param>
        /// <param name="value">The minimum height to set for the panel,
        /// if the current height is less.</param>
        /// <param name="onlyOnce">Whether to set the height only once.</param>
        public virtual bool SetPanelHeightAtLeast(
            SplitPanelPosition position,
            Coord value,
            bool onlyOnce = false)
        {
            var panel = GetPanel(position);

            if (onlyOnce)
            {
                var hasFlag = panel.FlagsAndAttributes.HasFlag("SetPanelHeightAtLeastCalled");
                if (hasFlag)
                    return false;
            }

            panel.FlagsAndAttributes.SetFlag("SetPanelHeightAtLeastCalled", true);

            var currentHeight = GetPanelHeight(position);
            if (currentHeight < value)
            {
                SetPanelHeight(position, value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ensures that the width of the specified panel is at least the given value.
        /// </summary>
        /// <remarks>If the current width of the panel is less than the specified value,
        /// the panel's width is increased to meet the minimum requirement.</remarks>
        /// <param name="position">The position of the panel whose width is to be set.</param>
        /// <param name="value">The minimum width to set for the panel.</param>
        /// <param name="onlyOnce">Whether to set the width only once.</param>
        public virtual bool SetPanelWidthAtLeast(
            SplitPanelPosition position,
            Coord value,
            bool onlyOnce = false)
        {
            var panel = GetPanel(position);

            if (onlyOnce)
            {
                var hasFlag = panel.FlagsAndAttributes.HasFlag("SetPanelWidthAtLeastCalled");
                if (hasFlag)
                    return false;
            }

            panel.FlagsAndAttributes.SetFlag("SetPanelWidthAtLeastCalled", true);

            var currentWidth = GetPanelWidth(position);
            if (currentWidth < value)
            {
                SetPanelWidth(position, value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the height of the specified panel in a split view layout.
        /// </summary>
        /// <param name="position">The position of the panel to set the height for.
        /// Must be either <see cref="SplitPanelPosition.Top"/> or
        /// <see cref="SplitPanelPosition.Bottom"/>.</param>
        /// <param name="height">The new height to assign to the specified panel.</param>
        /// <returns><see langword="true"/> if the panel height was successfully set;
        /// otherwise, <see langword="false"/> if the
        /// position is invalid.</returns>
        public virtual bool SetPanelHeight(SplitPanelPosition position, Coord height)
        {
            switch (position)
            {
                case SplitPanelPosition.Top:
                    TopPanelHeight = height;
                    return true;
                case SplitPanelPosition.Bottom:
                    BottomPanelHeight = height;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Ensures that a child control of type <typeparamref name="T"/> exists
        /// within the sidebar panel at the
        /// specified position.
        /// </summary>
        /// <typeparam name="T">The type of the control to ensure, which must
        /// derive from <see cref="AbstractControl"/> and have a
        /// parameterless constructor.</typeparam>
        /// <param name="position">The position of the sidebar panel where the control
        /// should be ensured.</param>
        /// <param name="title">The title of the child control.</param>
        /// <param name="onCreate">The action to execute when creating the child control.</param>
        /// <param name="onUpdate">The action to execute when updating the child control.</param>
        /// <param name="flags"></param>
        /// <returns>The ensured child control of type <typeparamref name="T"/> within the specified
        /// sidebar panel.</returns>
        public virtual T EnsureSideBarChild<T>(
            SplitPanelPosition position,
            string? title,
            Action<T>? onCreate = null,
            Action<T>? onUpdate = null,
            TabControl.EnsureSideBarChildFlags flags = TabControl.EnsureSideBarChildFlags.None)
            where T : AbstractControl, new()
        {
            var sideBar = EnsureChild<SideBarPanel>(position);
            var makeVisible = flags.HasFlag(TabControl.EnsureSideBarChildFlags.MakeVisible);
            var child = sideBar.EnsureChild<T>(title, onCreate, onUpdate, flags);
            if (makeVisible)
            {
                SetPanelVisible(position, makeVisible);
            }

            return child;
        }

        /// <summary>
        /// Sets the visibility of the specified panel.
        /// </summary>
        /// <remarks>This method updates the visibility state of the panel at the specified position.
        /// The visibility of the panel is determined by the <paramref name="visible"/> parameter.</remarks>
        /// <param name="position">The position of the panel to modify. Must be one of the defined
        /// <see cref="SplitPanelPosition"/> values.</param>
        /// <param name="visible"><see langword="true"/> to make the panel visible;
        /// otherwise, <see langword="false"/>.</param>
        public virtual void SetPanelVisible(SplitPanelPosition position, bool visible = true)
        {
            switch (position)
            {
                case SplitPanelPosition.Left:
                    LeftVisible = visible;
                    break;
                case SplitPanelPosition.Right:
                    RightVisible = visible;
                    break;
                case SplitPanelPosition.Top:
                    TopVisible = visible;
                    break;
                case SplitPanelPosition.Bottom:
                    BottomVisible = visible;
                    break;
                case SplitPanelPosition.Center:
                    FillPanel.Visible = visible;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Creates right panel.
        /// </summary>
        /// <returns></returns>
        protected virtual AbstractControl CreateRightPanel() => CreateAnyPanel(SplitPanelPosition.Right);

        /// <summary>
        /// Creates left panel.
        /// </summary>
        /// <returns></returns>
        protected virtual AbstractControl CreateLeftPanel() => CreateAnyPanel(SplitPanelPosition.Left);

        /// <summary>
        /// Creates top panel.
        /// </summary>
        /// <returns></returns>
        protected virtual AbstractControl CreateTopPanel() => CreateAnyPanel(SplitPanelPosition.Top);

        /// <summary>
        /// Creates bottom panel.
        /// </summary>
        /// <returns></returns>
        protected virtual AbstractControl CreateBottomPanel() => CreateAnyPanel(SplitPanelPosition.Bottom);

        /// <summary>
        /// Creates center panel.
        /// </summary>
        /// <returns></returns>
        protected virtual AbstractControl CreateCenterPanel() => CreateAnyPanel(SplitPanelPosition.Center);

        /// <summary>
        /// Creates panel.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// By default used in all create panel methods.
        /// </remarks>
        protected virtual Control CreateAnyPanel(SplitPanelPosition position)
        {
            var result = new Panel();
            return result;
        }
    }
}