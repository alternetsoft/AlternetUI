using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements multiple <see cref="GenericToolBar"/> controls.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    public class GenericToolBarSet : VerticalStackPanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericToolBarSet"/> class.
        /// </summary>
        public GenericToolBarSet()
        {
            ToolBarCount = 1;
        }

        /// <summary>
        /// Gets or sets the number of toolbars.
        /// </summary>
        /// <remarks>
        /// Minimal and default toolbar count is 1.
        /// </remarks>
        [Browsable(false)]
        public virtual int ToolBarCount
        {
            get
            {
                return Children.Count;
            }

            set
            {
                value = Math.Max(value, 1);
                if (value == ToolBarCount)
                    return;
                Children.SetCount(value, CreateToolBar);
            }
        }

        /// <summary>
        /// Gets toolbar with the specified index.
        /// </summary>
        /// <param name="index">Toolbar index.</param>
        /// <returns></returns>
        public virtual GenericToolBar GetToolBar(int index)
        {
            if (ToolBarCount <= index)
                ToolBarCount = index + 1;
            return (GenericToolBar)Children[index];
        }

        /// <summary>
        /// Creates toolbar.
        /// </summary>
        /// <remarks>
        /// Used when <see cref="ToolBarCount"/> property is changed
        /// and new toolbars need to be created.
        /// </remarks>
        /// <returns></returns>
        protected virtual GenericToolBar CreateToolBar()
        {
            GenericToolBar result = new();
            if(Children.Count > 0)
            {
                result.Margin = (0, 4, 0, 0);
            }

            return result;
        }
    }
}
