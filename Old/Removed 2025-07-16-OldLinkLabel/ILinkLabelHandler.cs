using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with link label control.
    /// </summary>
    public interface ILinkLabelHandler
    {
        /// <inheritdoc cref="LinkLabel.Url"/>
        string Url { get; set; }

        /// <inheritdoc cref="LinkLabel.HoverColor"/>
        Color HoverColor { get; set; }

        /// <inheritdoc cref="LinkLabel.NormalColor"/>
        Color NormalColor { get; set; }

        /// <inheritdoc cref="LinkLabel.VisitedColor"/>
        Color VisitedColor { get; set; }

        /// <inheritdoc cref="LinkLabel.Visited"/>
        bool Visited { get; set; }
    }
}
