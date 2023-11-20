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
    public class PanelTreeAndCards : PanelAuiManager
    {
        private readonly CardPanel cardPanel = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelTreeAndCards"/> class.
        /// </summary>
        public PanelTreeAndCards(Action<PanelAuiManager>? initAction = null)
            : base(initAction)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelTreeAndCards"/> class.
        /// </summary>
        public PanelTreeAndCards()
            : base()
        {
            Initialize();
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
        public int Add(string title, Func<Control> fnCreate)
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
        public int Add(string title, Control control)
        {
            var index = cardPanel.Add(title, control);
            var item = LeftTreeView.Add(title);
            item.Tag = index;
            return index;
        }

        private void Initialize()
        {
            LeftTreeView.Required();
            Manager.AddPane(cardPanel, CenterPane);

            LeftTreeView.SelectionChanged += LeftTreeView_SelectionChanged;
            Manager.Update();
        }

        private void LeftTreeView_SelectionChanged(object? sender, System.EventArgs e)
        {
            var tag = LeftTreeView.SelectedItem?.Tag;
            cardPanel.SelectCard(tag as int?);
        }
    }
}
