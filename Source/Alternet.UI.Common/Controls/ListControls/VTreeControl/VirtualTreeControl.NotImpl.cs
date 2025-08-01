using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class VirtualTreeControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether lines are drawn between the
        /// tree items that are at the root of the tree view. Default is <see langword="false"/>.
        /// Currently, setter of this property does nothing.
        /// </summary>
        /// <value><see langword="true"/> if lines are drawn between the tree
        /// items that are at the root of the tree
        /// view; otherwise, <see langword="false"/>. The default is
        /// <see langword="true"/>.</value>
        [Browsable(false)]
        public virtual bool ShowRootLines
        {
            get
            {
                return false;
            }

            internal set
            {
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw a contrasting
        /// border between displayed rows.
        /// Default is <see langword="false"/>.
        /// Currently, setter of this property does nothing.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if row lines are shown;
        /// <see langword="false" />, if row lines are hidden.
        /// The default is <see langword="false" />.
        /// </returns>
        [Browsable(false)]
        public virtual bool RowLines
        {
            get
            {
                return false;
            }

            internal set
            {
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the selection highlight spans
        /// the width of the tree control. Default is <see langword="true"/>.
        /// Currently, setter of this property does nothing.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the selection highlight spans the width of
        /// the tree view control; otherwise, <see langword="false"/>.
        /// The default is <see langword="false"/>.
        /// </value>
        [Browsable(false)]
        public virtual bool FullRowSelect
        {
            get
            {
                return true;
            }

            internal set
            {
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the label text of the tree
        /// items can be edited. Default is <see langword="false"/>.
        /// Currently, setter of this property does nothing.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the label text of the tree items can be
        /// edited; otherwise, <see
        /// langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        [Browsable(false)]
        public virtual bool AllowLabelEdit
        {
            get
            {
                return false;
            }

            internal set
            {
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether lines are drawn between tree
        /// items in the tree view control.
        /// Default is <see langword="false"/>.
        /// Currently, setter of this property does nothing.
        /// </summary>
        /// <value><see langword="true"/> if lines are drawn between tree items in
        /// the tree view control; otherwise,
        /// <see langword="false"/>. The default is <see langword="true"/>.</value>
        [Browsable(false)]
        public virtual bool ShowLines
        {
            get
            {
                return false;
            }

            internal set
            {
            }
        }
    }
}
