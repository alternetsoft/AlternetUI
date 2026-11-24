using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements panel with toolbar on top of it.
    /// </summary>
    public class PanelWithToolBar : Panel
    {
        private readonly ToolBar toolBar = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelWithToolBar"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PanelWithToolBar(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelWithToolBar"/> class.
        /// </summary>
        public PanelWithToolBar()
        {
            Layout = LayoutStyle.Vertical;
            toolBar.Parent = this;
            toolBar.SetBorderAndMargin(AnchorStyles.Bottom, null);
            CreateToolbarItems();
        }

        /// <summary>
        /// Gets <see cref="ToolBar"/> control used in this panel.
        /// </summary>
        [Browsable(false)]
        public ToolBar ToolBar => toolBar;

        /// <summary>
        /// Creates toolbar items.
        /// </summary>
        protected virtual void CreateToolbarItems()
        {
        }
    }
}
