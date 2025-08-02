using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements panel with <see cref="VirtualListBox"/> on the left and
    /// <see cref="CardPanel"/> on the right separated with splitter.
    /// </summary>
    [ControlCategory("Panels")]
    public partial class PanelListBoxAndCards : SplittedControlsPanel
    {
        private readonly CardPanel cardPanel = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelListBoxAndCards"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PanelListBoxAndCards(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelListBoxAndCards"/> class.
        /// </summary>
        public PanelListBoxAndCards()
        {
            RightVisible = false;
            LeftListBox.Required();
            cardPanel.Parent = FillPanel;
            LeftListBox.SelectionChanged += LeftListBox_SelectionChanged;
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
            var id = cardPanel.Cards[index].UniqueId;
            TreeViewItem item = new(title);
            item.Tag = id;
            LeftListBox.Add(item);
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
            var id = cardPanel.Cards[index].UniqueId;
            TreeViewItem item = new(title);
            item.Tag = id;
            LeftListBox.Add(item);
            return index;
        }

        private void LeftListBox_SelectionChanged(object? sender, System.EventArgs e)
        {
            var tag = LeftListBox.SelectedItem?.Tag;
            if(tag is not null)
                cardPanel.SelectCard((ObjectUniqueId)tag);
        }
    }
}
