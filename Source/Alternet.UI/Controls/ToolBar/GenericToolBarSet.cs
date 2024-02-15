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
    public partial class GenericToolBarSet : Control
    {
        /// <summary>
        /// Gets or sets default distance (in dips) between toolbars.
        /// </summary>
        public static double DefaultToolBarDistance = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericToolBarSet"/> class.
        /// </summary>
        public GenericToolBarSet()
        {
            Layout = LayoutStyle.Vertical;
            ToolBarCount = 1;
        }

        /// <summary>
        /// Gets or sets distance (in dips) between toolbars.
        /// </summary>
        public virtual double? ToolBarDistance { get; set; }

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
        /// Gets or sets <see cref="GenericToolBar.ItemSize"/> for the child toolbars.
        /// </summary>
        public virtual double ItemSize
        {
            get
            {
                return this[0].ItemSize;
            }

            set
            {
                if (ItemSize == value)
                    return;

                DoInsideLayout(() =>
                {
                    for (int i = 0; i < ToolBarCount; i++)
                    {
                        this[i].ItemSize = value;
                    }
                });
            }
        }

        /// <summary>
        /// Gets toolbar at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the toolbar
        /// to get.</param>
        /// <returns>The toolbar at the specified index.</returns>
        public GenericToolBar this[int index]
        {
            get => GetToolBar(index);
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
                result.Margin = (0, ToolBarDistance ?? DefaultToolBarDistance, 0, 0);
            }

            return result;
        }
    }
}
