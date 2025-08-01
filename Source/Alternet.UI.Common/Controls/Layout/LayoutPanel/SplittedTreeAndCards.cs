using System;
using System.Diagnostics;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements splitter panel with <see cref="TreeView"/> or <see cref="VirtualListBox"/>
    /// on the left and <see cref="CardPanel"/> on the right.
    /// </summary>
    public partial class SplittedTreeAndCards : LayoutPanel
    {
        /// <summary>
        /// Gets default sash position.
        /// </summary>
        public Coord DefaultSashPosition = 140;

        private readonly CardPanel cardPanel = new()
        {
        };

        private readonly Splitter splitter = new();

        private TreeKind kind = TreeKind.TreeView;
        private AbstractControl? leftControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplittedTreeAndCards"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public SplittedTreeAndCards(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplittedTreeAndCards"/> class.
        /// </summary>
        public SplittedTreeAndCards()
        {
            Initialize(TreeKind.TreeView);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplittedTreeAndCards"/> class.
        /// </summary>
        public SplittedTreeAndCards(TreeKind kind)
        {
            Initialize(kind);
        }

        /// <summary>
        /// Enumerates possible kinds of the left control.
        /// </summary>
        public enum TreeKind
        {
            /// <summary>
            /// Control is <see cref="TreeView"/>.
            /// </summary>
            TreeView,

            /// <summary>
            /// Control is <see cref="ListBox"/>.
            /// </summary>
            ListBox,
        }

        /// <summary>
        /// Gets left control.
        /// </summary>
        public AbstractControl? LeftControl
        {
            get
            {
                return leftControl;
            }
        }

        /// <summary>
        /// Gets 'Tag' property of the selected item.
        /// </summary>
        public object? SelectedItemTag
        {
            get
            {
                object? result;

                if (kind == TreeKind.TreeView)
                    result = TreeView?.SelectedItem?.Tag;
                else
                    result = ListBox?.SelectedItemTag;
                return result;
            }
        }

        /// <summary>
        /// Gets <see cref="CardPanel"/> attached to the control.
        /// </summary>
        public CardPanel Cards => cardPanel;

        /// <summary>
        /// Gets left control as <see cref="TreeView"/>.
        /// </summary>
        public VirtualTreeControl? TreeView => leftControl as VirtualTreeControl;

        /// <summary>
        /// Gets left control as <see cref="VirtualListBox"/>.
        /// </summary>
        public VirtualListBox? ListBox => leftControl as VirtualListBox;

        /// <summary>
        /// Gets or sets index of the selected item in the <see cref="TreeView"/>.
        /// </summary>
        public virtual int? SelectedIndex
        {
            get
            {
                if(kind == TreeKind.TreeView)
                    return TreeView?.ListBox.SelectedIndex;
                else
                    return ListBox?.SelectedIndex;
            }

            set
            {
                if (kind == TreeKind.TreeView)
                    TreeView?.ListBox.SelectItemAndScroll(value);
                else
                    ListBox?.SelectItemAndScroll(value);
                SetActiveCard();
            }
        }

        /// <summary>
        /// Gets kind of the left control.
        /// </summary>
        public virtual TreeKind LeftControlKind
        {
            get => kind;
        }

        /// <summary>
        /// Calls <see cref="VirtualTreeControl.MakeAsListBox"/> for the left control if
        /// it is tree view.
        /// </summary>
        public virtual void MakeAsListBox()
        {
        }

        /// <summary>
        /// Sets debug background colors for the different parts of the control.
        /// </summary>
        [Conditional("DEBUG")]
        public virtual void SetDebugColors()
        {
            DebugBackgroundColor(Color.Green, nameof(SplittedTreeAndCards));
            Cards.DebugBackgroundColor(Color.Yellow, "SplittedTreeAndCards.Cards");
            Cards.CardPropertyChanged += CardPropertyChanged;

            static void CardPropertyChanged(object? sender, ObjectPropertyChangedEventArgs e)
            {
                if (e.Instance is not CardPanelItem card)
                    return;
                if (card.ControlCreated)
                    card.Control.DebugBackgroundColor(Color.Navy, "SplittedTreeAndCards.Cards.card");
            }
        }

        /// <summary>
        /// Adds new item.
        /// </summary>
        /// <param name="title">Item title.</param>
        /// <param name="action">Card control create action.</param>
        public virtual void Add(string title, Func<AbstractControl> action)
        {
            var index = cardPanel.Add(title, action);

            if(LeftControlKind is TreeKind.TreeView)
            {
                var item = new TreeControlItem(title)
                {
                    Tag = index,
                };
                TreeView?.Add(item);
            }
            else
            {
                var item = new ListControlItem(title)
                {
                    Tag = index,
                };

                ListBox?.Add(item);
            }
        }

        /// <summary>
        /// Creates used <see cref="TreeView"/> control.
        /// </summary>
        /// <returns></returns>
        protected virtual AbstractControl CreateTreeView()
        {
            TreeView treeView = new()
            {
                HasBorder = false,
            };

            treeView.SelectionChanged += OnSelectionChanged;

            return treeView;
        }

        /// <summary>
        /// Creates used <see cref="VirtualListBox"/> control.
        /// </summary>
        /// <returns></returns>
        protected virtual AbstractControl CreateListBox()
        {
            VirtualListBox listBox = new()
            {
                HasBorder = false,
            };

            listBox.SelectionChanged += OnSelectionChanged;

            return listBox;
        }

        /// <summary>
        /// Called when selection changed in the left control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSelectionChanged(object? sender, System.EventArgs e)
        {
            SetActiveCard();
        }

        /// <summary>
        /// Initializes controls. Called from constructor.
        /// </summary>
        protected virtual void Initialize(TreeKind kind)
        {
            this.kind = kind;
            cardPanel.Dock = DockStyle.Fill;
            cardPanel.Parent = this;

            splitter.Dock = DockStyle.Left;
            splitter.Parent = this;

            if (kind == TreeKind.TreeView)
                leftControl = CreateTreeView();
            else
                leftControl = CreateListBox();

            leftControl.Dock = DockStyle.Left;
            leftControl.Width = DefaultSashPosition;
            leftControl.Parent = this;
        }

        private void SetActiveCard()
        {
            cardPanel.SelectedCardIndex = (int?)SelectedItemTag;
        }
    }
}