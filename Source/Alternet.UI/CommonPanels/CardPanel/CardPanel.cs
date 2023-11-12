using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to switch child controls, so only one of them is visible and other are hidden.
    /// </summary>
    /// <remarks>
    /// It behaves like <see cref="TabControl"/> but has no tab titles.
    /// </remarks>
    public class CardPanel : Control
    {
        private readonly VerticalStackPanel waitLabelContainer = new()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            SuggestedSize = new Size(400, 400),
        };

        private readonly Label waitLabel = new()
        {
            Text = "Loading. Please wait...",
            Margin = new Thickness(100, 100, 0, 0),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanel"/> class.
        /// </summary>
        public CardPanel()
        {
            waitLabel.Parent = waitLabelContainer;
        }

        /// <summary>
        /// Gets or sets whether to call <see cref="Application.BeginBusyCursor"/> when
        /// page is created. By default is <c>true</c>.
        /// </summary>
        public bool UseBusyCursor { get; set; } = true;

        /// <summary>
        /// Gets pages with child controls.
        /// </summary>
        public Collection<CardPanelItem> Cards { get; } = new Collection<CardPanelItem>();

        /// <summary>
        /// Sets active page.
        /// </summary>
        /// <param name="index">Card index.</param>
        public void SetActiveCard(int? index)
        {
            if (index == null || index < 0 || index >= Cards.Count)
                return;

            var busyCursor = false;
            SuspendLayout();
            try
            {
                GetVisibleChildOrNull()?.Hide();
                var page = Cards[index.Value];
                var loaded = page.ControlCreated;

                if (!loaded)
                {
                    if (UseBusyCursor)
                    {
                        Application.BeginBusyCursor();
                        busyCursor = true;
                    }

                    waitLabelContainer.Parent = this;
                    waitLabelContainer.Visible = true;
                    waitLabelContainer.Refresh();
                    Application.DoEvents();
                }

                var control = page.Control;

                 /*control.HorizontalAlignment = HorizontalAlignment.Stretch;
                 control.VerticalAlignment = VerticalAlignment.Stretch;*/

                control.Parent = this;
                control.Visible = true;
                control.PerformLayout();
                waitLabelContainer.Visible = false;
            }
            finally
            {
                if (busyCursor)
                    Application.EndBusyCursor();
                ResumeLayout();
            }
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
            Cards.Add(new CardPanelItem(title, control));
            return Cards.Count - 1;
        }

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
            Cards.Add(new CardPanelItem(title, fnCreate));
            return Cards.Count - 1;
        }
    }
}
