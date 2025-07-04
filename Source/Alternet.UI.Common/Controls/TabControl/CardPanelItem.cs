﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Individual page of the <see cref="CardPanel"/>
    /// </summary>
    public class CardPanelItem : BaseControlItem
    {
        /// <summary>
        /// Gets or sets default value for the <see cref="SuppressException"/>. It is used
        /// if <see cref="SuppressException"/> is Null.
        /// </summary>
        public static bool DefaultSuppressException = true;

        private readonly Func<AbstractControl>? action;
        private AbstractControl? control;

        internal CardPanelItem(string? title, AbstractControl control)
        {
            Title = title;
            this.control = control;
        }

        internal CardPanelItem(string? title, Func<AbstractControl> action)
        {
            Title = title;
            this.action = action;
        }

        /// <summary>
        /// Gets page title.
        /// </summary>
        public string? Title
        {
            get;
        }

        /// <summary>
        /// Gets whether child control was created.
        /// </summary>
        [Browsable(false)]
        public bool ControlCreated => control != null;

        /// <summary>
        /// Gets or sets whether to suppress exception when card is created.
        /// In case when exception is suppressed, a card with error is created and shown
        /// instead of the card. Default is Null and
        /// <see cref="DefaultSuppressException"/> is used.
        /// </summary>
        public bool? SuppressException { get; set; }

        /// <summary>
        /// Child control.
        /// </summary>
        [Browsable(false)]
        public AbstractControl Control
        {
            get
            {
                var oldControl = control;

                try
                {
                    control ??= action?.Invoke() ?? new Control();
                }
                catch (Exception e)
                {
                    if (SuppressException ?? DefaultSuppressException)
                        control = CreateErrorCard(e);
                    else
                        throw;
                }

                if (oldControl != control)
                    RaisePropertyChanged(nameof(Control));

                return control;
            }
        }

        /// <summary>
        /// Creates card with the error to show instead of not loaded card.
        /// </summary>
        /// <param name="e">Exception.</param>
        /// <returns></returns>
        public static AbstractControl CreateErrorCard(Exception e)
        {
            RichToolTip tooltip = new();
            tooltip.Visible = true;
            tooltip.ShowToolTipWithError(null, e, 0);
            return tooltip;
        }
    }
}
