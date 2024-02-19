﻿using System;
using System.Diagnostics;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements splitter panel with <see cref="TreeView"/> and <see cref="CardPanel"/>.
    /// </summary>
    public partial class SplittedTreeAndCards : LayoutPanel
    {
        private readonly TreeView treeView = new()
        {
            HasBorder = false,
        };

        private readonly CardPanel cardPanel = new()
        {
        };

        private readonly Splitter splitter = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SplittedTreeAndCards"/> class.
        /// </summary>
        public SplittedTreeAndCards()
        {
            cardPanel.Dock = DockStyle.Fill;
            cardPanel.Parent = this;

            splitter.Dock = DockStyle.Left;
            splitter.Parent = this;

            treeView.SelectionChanged += PagesListBox_SelectionChanged;
            treeView.Dock = DockStyle.Left;
            treeView.Width = DefaultSashPosition;
            treeView.Parent = this;
        }

        /// <summary>
        /// Gets <see cref="TreeView"/> attached to the control.
        /// </summary>
        public TreeView TreeView => treeView;

        /// <summary>
        /// Gets <see cref="CardPanel"/> attached to the control.
        /// </summary>
        public CardPanel Cards => cardPanel;

        /// <summary>
        /// Gets or sets index of the selected item in the <see cref="TreeView"/>.
        /// </summary>
        public int? SelectedIndex
        {
            get => treeView?.SelectedItem?.Index;
            set
            {
                treeView.SelectedItem = treeView.Items[(int)value!];
            }
        }

        /// <summary>
        /// Gets default sash position.
        /// </summary>
        protected virtual double DefaultSashPosition => 140;

        /*/// <summary>
        /// Splits controls vertically.
        /// </summary>
        /// <param name="sashPosition">Sash position in dips.</param>
        public void SplitVerticalDip(double sashPosition = 0)
        {
            SplitVertical(TreeView, Cards, PixelFromDip(sashPosition));
        }*/

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
            var item = new TreeViewItem(title)
            {
                Tag = index,
            };
            treeView.Add(item);
        }

        private void PagesListBox_SelectionChanged(object? sender, System.EventArgs e)
        {
            SetActiveCard();
        }

        private void SetActiveCard()
        {
            var pageIndex = treeView.SelectedItem?.Tag;
            if (pageIndex == null)
                return;
            cardPanel.SelectedCardIndex = (int)pageIndex;
        }
    }
}