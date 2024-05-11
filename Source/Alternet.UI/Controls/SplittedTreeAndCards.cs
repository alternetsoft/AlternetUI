using System;
using System.Diagnostics;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements splitter panel with <see cref="TreeView"/> or <see cref="VListBox"/>
    /// on the left and <see cref="CardPanel"/> on the right.
    /// </summary>
    public partial class SplittedTreeAndCards : LayoutPanel
    {
        /// <summary>
        /// Gets default sash position.
        /// </summary>
        public double DefaultSashPosition = 140;

        private readonly CardPanel cardPanel = new()
        {
        };

        private readonly Splitter splitter = new();

        private TreeKind kind = TreeKind.TreeView;
        private Control? leftControl;

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
        public Control? LeftControl
        {
            get
            {
                return leftControl;
            }
        }

        /// <summary>
        /// Gets <see cref="CardPanel"/> attached to the control.
        /// </summary>
        public CardPanel Cards => cardPanel;

        /// <summary>
        /// Gets left control as <see cref="TreeView"/>.
        /// </summary>
        public TreeView? TreeView => leftControl as TreeView;

        /// <summary>
        /// Gets left control as <see cref="VListBox"/>.
        /// </summary>
        public VListBox? ListBox => leftControl as VListBox;

        /// <summary>
        /// Gets or sets index of the selected item in the <see cref="TreeView"/>.
        /// </summary>
        public int? SelectedIndex
        {
            get
            {
                if(kind == TreeKind.TreeView)
                    return TreeView?.SelectedItem?.Index;
                else
                    return ListBox?.SelectedIndex;
            }

            set
            {
                if (kind == TreeKind.TreeView)
                    TreeView?.SetSelectedIndex(value);
                else
                    ListBox?.SetSelectedIndex(value);
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
        /// Calls <see cref="TreeView.MakeAsListBox"/> for the left control if
        /// it is <see cref="TreeView"/>.
        /// </summary>
        public void MakeAsListBox()
        {
            TreeView?.MakeAsListBox();
        }

        /// <summary>
        /// Sets debug background colors for the different parts of the control.
        /// </summary>
        [Conditional("DEBUG")]
        public void SetDebugColors()
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
        public void Add(string title, Func<Control> action)
        {
            var index = cardPanel.Add(title, action);

            if(LeftControlKind is TreeKind.TreeView)
            {
                var item = new TreeViewItem(title)
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
        protected virtual Control CreateTreeView()
        {
            TreeView treeView = new()
            {
                HasBorder = false,
            };

            treeView.SelectionChanged += OnSelectionChanged;

            return treeView;
        }

        /// <summary>
        /// Creates used <see cref="VListBox"/> control.
        /// </summary>
        /// <returns></returns>
        protected virtual Control CreateListBox()
        {
            VListBox listBox = new()
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
            object? pageIndex;

            if (kind == TreeKind.TreeView)
                pageIndex = TreeView?.SelectedItem?.Tag;
            else
                pageIndex = (ListBox?.SelectedItem as ListControlItem)?.Tag;

            if (pageIndex == null)
                return;
            cardPanel.SelectedCardIndex = (int)pageIndex;
        }
    }
}