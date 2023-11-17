using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to switch child controls, so only one of them is visible and other are hidden.
    /// </summary>
    /// <remarks>
    /// It behaves like <see cref="TabControl"/> but has no tab titles.
    /// </remarks>
    [ControlCategory("Hidden")]
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
            Text = CommonStrings.Default.LoadingPleaseWait,
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
        /// Gets the card at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the card to get.</param>
        /// <returns>The card at the specified index.</returns>
        public CardPanelItem this[int index] => Cards[index];

        /// <summary>
        /// Gets card with the specified id.
        /// </summary>
        /// <param name="id">Card id.</param>
        public CardPanelItem? Find(ObjectUniqueId? id)
        {
            if (id is null)
                return null;
            foreach(var item in Cards)
            {
                if (item.UniqueId == id)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Sets active card by id.
        /// </summary>
        /// <param name="id">Card id.</param>
        public void SetActiveCard(ObjectUniqueId? id)
        {
            var item = Find(id);
            SetActiveCard(item);
        }

        /// <summary>
        /// Sets active card.
        /// </summary>
        /// <param name="card">Card.</param>
        public void SetActiveCard(CardPanelItem? card)
        {
            if (card is null)
                return;
            var busyCursor = false;
            SuspendLayout();
            try
            {
                GetVisibleChildOrNull()?.Hide();
                var loaded = card.ControlCreated;

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

                var control = card.Control;
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
        /// Sets active card by index.
        /// </summary>
        /// <param name="index">Card index.</param>
        public void SetActiveCard(int? index)
        {
            if (index == null || index < 0 || index >= Cards.Count)
                return;
            SetActiveCard(Cards[index.Value]);
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
            var result = new CardPanelItem(title, control);
            Cards.Add(result);
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
        public int Add(string? title, Func<Control> fnCreate)
        {
            var result = new CardPanelItem(title, fnCreate);
            Cards.Add(result);
            return Cards.Count - 1;
        }
    }
}
