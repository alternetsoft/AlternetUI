using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements multiple <see cref="ToolBar"/> controls.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    public partial class ToolBarSet : HiddenBorderedGraphicControl
    {
        /// <summary>
        /// Gets or sets default distance (in dips) between toolbars.
        /// </summary>
        public static Coord DefaultToolBarDistance = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBarSet"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ToolBarSet(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBarSet"/> class.
        /// </summary>
        public ToolBarSet()
        {
            ParentBackColor = true;
            ParentForeColor = true;
            Layout = LayoutStyle.Vertical;
            ToolBarCount = 1;
            IsGraphicControl = true;
        }

        /// <summary>
        /// Gets or sets distance (in dips) between toolbars.
        /// </summary>
        public virtual Coord? ToolBarDistance { get; set; }

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
        /// Gets or sets <see cref="ToolBar.ItemSize"/> for the child toolbars.
        /// </summary>
        public virtual Coord ItemSize
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

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        [Browsable(false)]
        internal new bool IsBold
        {
            get => base.IsBold;
            set => base.IsBold = value;
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Gets toolbar at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the toolbar
        /// to get.</param>
        /// <returns>The toolbar at the specified index.</returns>
        public ToolBar this[int index]
        {
            get => GetToolBar(index);
        }

        /// <summary>
        /// Gets toolbar with the specified index.
        /// </summary>
        /// <param name="index">Toolbar index.</param>
        /// <returns></returns>
        public virtual ToolBar GetToolBar(int index)
        {
            if (ToolBarCount <= index)
                ToolBarCount = index + 1;
            return (ToolBar)Children[index];
        }

        /// <summary>
        /// Creates toolbar.
        /// </summary>
        /// <remarks>
        /// Used when <see cref="ToolBarCount"/> property is changed
        /// and new toolbars need to be created.
        /// </remarks>
        /// <returns></returns>
        protected virtual ToolBar CreateToolBar()
        {
            ToolBar result = new();
            result.ParentBackColor = true;
            result.ParentFont = true;
            result.ParentForeColor = true;

            var distance = ToolBarDistance ?? DefaultToolBarDistance;
            result.Margin = distance;

            if (Children.Count > 0)
            {
            }

            return result;
        }
    }
}
