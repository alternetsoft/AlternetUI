using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial interface IControlHandler
    {
        /// <inheritdoc cref="Control.VertScrollBarInfo"/>
        ScrollBarInfo VertScrollBarInfo { get; set; }

        /// <inheritdoc cref="Control.HorzScrollBarInfo"/>
        ScrollBarInfo HorzScrollBarInfo { get; set; }

        /// <inheritdoc cref="Control.BindScrollEvents"/>
        bool BindScrollEvents { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'VerticalScrollBarValueChanged' event is raised.
        /// </summary>
        Action? VerticalScrollBarValueChanged { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'HorizontalScrollBarValueChanged' event is raised.
        /// </summary>
        Action? HorizontalScrollBarValueChanged { get; set; }

        /// <inheritdoc cref="Control.IsScrollable"/>
        bool IsScrollable { get; set; }

        /// <summary>
        /// Gets scrollbar position. Available only in the event handler.
        /// </summary>
        /// <returns></returns>
        int GetScrollBarEvtPosition();

        /// <summary>
        /// Gets scrollbar event type. Available only in the event handler.
        /// </summary>
        /// <returns></returns>
        ScrollEventType GetScrollBarEvtKind();
    }
}
