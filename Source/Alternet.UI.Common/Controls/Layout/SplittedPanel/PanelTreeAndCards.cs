using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements panel with tree control on the left and
    /// <see cref="CardPanel"/> on the right separated with splitter.
    /// </summary>
    [ControlCategory("Panels")]
    public partial class PanelTreeAndCards : SplittedControlsPanel
    {
        private readonly CardPanel cardPanel = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelTreeAndCards"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PanelTreeAndCards(AbstractControl parent)
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
            LeftListBox.Required();
            cardPanel.Parent = FillPanel;
            LeftListBox.SelectionChanged += OnLeftTreeViewSelectionChanged;
        }

        /// <summary>
        /// Gets used <see cref="CardPanel"/> control.
        /// </summary>
        public CardPanel CardPanel => cardPanel;

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
            var item = LeftListBox.Add(title);
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
            var item = LeftListBox.Add(title);
            item.Tag = index;
            return index;
        }

        /// <summary>
        /// Handles the selection change event for the left tree view.
        /// </summary>
        /// <remarks>This method is invoked when the selection in the left tree view changes.
        /// It updates the card panel based on the selected item's tag, if applicable.
        /// Derived classes can override this method to
        /// provide custom behavior.</remarks>
        /// <param name="sender">The source of the event, typically the control that
        /// triggered the selection change.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnLeftTreeViewSelectionChanged(object? sender, System.EventArgs e)
        {
            var tag = LeftListBox.SelectedItem?.Tag;
            cardPanel.SelectCard(tag as int?);
        }
    }
}
