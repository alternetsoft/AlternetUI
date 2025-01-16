using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements panel with <see cref="TreeView"/> on the left and
    /// <see cref="CardPanel"/> on the right separated with splitter.
    /// </summary>
    [ControlCategory("Panels")]
    [Obsolete("Please use PanelListBoxAndCards instead of this control.")]
    public partial class PanelTreeAndCards : SplittedControlsPanel
    {
        private readonly CardPanel cardPanel = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelTreeAndCards"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PanelTreeAndCards(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelTreeAndCards"/> class.
        /// </summary>
        public PanelTreeAndCards()
        {
            RightVisible = false;
            LeftTreeView.Required();
            cardPanel.Parent = FillPanel;
            LeftTreeView.SelectionChanged += LeftTreeView_SelectionChanged;
        }

        /// <summary>
        /// Gets used <see cref="CardPanel"/> control.
        /// </summary>
        public CardPanel CardPanel => cardPanel;

        internal new VirtualListBox LeftListBox
        {
            get => base.LeftListBox;
        }

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="title">Page title.</param>
        /// <param name="fnCreate">Function which creates the control.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public virtual int Add(string title, Func<AbstractControl> fnCreate)
        {
            var index = cardPanel.Add(title, fnCreate);
            var item = LeftTreeView.Add(title);
            item.Tag = index;
            return index;
        }

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="title">Page title.</param>
        /// <param name="control">Control.</param>
        /// <returns>
        /// Created page index.
        /// </returns>
        public virtual int Add(string title, AbstractControl control)
        {
            var index = cardPanel.Add(title, control);
            var item = LeftTreeView.Add(title);
            item.Tag = index;
            return index;
        }

        private void LeftTreeView_SelectionChanged(object? sender, System.EventArgs e)
        {
            var tag = LeftTreeView.SelectedItem?.Tag;
            cardPanel.SelectCard(tag as int?);
        }
    }
}
