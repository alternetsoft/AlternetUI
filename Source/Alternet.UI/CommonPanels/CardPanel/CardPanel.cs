using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [ControlCategory("Containers")]
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

        private CardPanelItem? selectedCard;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanel"/> class.
        /// </summary>
        public CardPanel()
        {
            Cards.ThrowOnNullAdd = true;
            Cards.ItemInserted += Cards_ItemInserted;
            Cards.ItemRemoved += Cards_ItemRemoved;
            waitLabel.Parent = waitLabelContainer;
        }

        /// <summary>
        /// Gets wait control container which is shown when card is loaded
        /// and <see cref="UseWaitControl"/> is <c>true</c>.
        /// </summary>
        [Browsable(false)]
        public Control WaitControlContainer => waitLabelContainer;

        /// <summary>
        /// Gets wait control which is shown when card is loaded
        /// and <see cref="UseWaitControl"/> is <c>true</c>.
        /// </summary>
        [Browsable(false)]
        public Control WaitControl => waitLabel;

        /// <summary>
        /// Gets or sets whether to call <see cref="Application.BeginBusyCursor"/> when
        /// page is created. By default is <c>true</c>.
        /// </summary>
        public bool UseBusyCursor { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to show wait control when
        /// page is created. By default is <c>true</c>.
        /// </summary>
        public bool UseWaitControl { get; set; } = true;

        /// <summary>
        /// Gets pages with child controls.
        /// </summary>
        [Browsable(false)]
        public Collection<CardPanelItem> Cards { get; } = [];

        /// <summary>
        /// Gets or sets selected card.
        /// </summary>
        [Browsable(false)]
        public CardPanelItem? SelectedCard
        {
            get
            {
                return selectedCard;
            }

            set
            {
                if (value is null)
                    return;
                SelectCard(value);
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override bool IsBold
        {
            get => false;

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets selected card index.
        /// </summary>
        public int? SelectedCardIndex
        {
            get
            {
                return IndexOf(SelectedCard);
            }

            set
            {
                if (value is null || value < 0 || value >= Cards.Count)
                    return;
                SelectCard(value);
            }
        }

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
        /// Gets index of the specified card.
        /// </summary>
        /// <param name="item">Card.</param>
        public int? IndexOf(CardPanelItem? item)
        {
            if (item == null)
                return null;
            for(int i = 0; i < Cards.Count - 1; i++)
            {
                if (item == Cards[i])
                    return i;
            }

            return null;
        }

        /// <summary>
        /// Gets card with the specified control.
        /// </summary>
        /// <param name="control">Control attached to the card.</param>
        public CardPanelItem? Find(Control? control)
        {
            if (control is null)
                return null;
            foreach (var item in Cards)
            {
                if (item.ControlCreated && item.Control == control)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Sets active card by id.
        /// </summary>
        /// <param name="id">Card id.</param>
        public CardPanel SelectCard(ObjectUniqueId? id)
        {
            var item = Find(id);
            SelectCard(item);
            return this;
        }

        /// <summary>
        /// Sets active card.
        /// </summary>
        /// <param name="card">Card.</param>
        public CardPanel SelectCard(CardPanelItem? card)
        {
            if (card is null)
                return this;
            if (card == selectedCard)
                return this;
            selectedCard = card;
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

                    if (UseWaitControl)
                    {
                        waitLabelContainer.Parent = this;
                        waitLabelContainer.Visible = true;
                        waitLabelContainer.Refresh();
                        Application.DoEvents();
                    }
                }

                var control = card.Control;
                control.Parent = this;
                control.Visible = true;
                if (UseWaitControl)
                    waitLabelContainer.Visible = false;
                PerformLayout();
            }
            finally
            {
                if (busyCursor)
                    Application.EndBusyCursor();
                ResumeLayout();
            }

            return this;
        }

        /// <summary>
        /// Sets active card by index.
        /// </summary>
        /// <param name="index">Card index.</param>
        public CardPanel SelectCard(int? index)
        {
            if (index == null || index < 0 || index >= Cards.Count)
                return this;
            return SelectCard(Cards[index.Value]);
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

        private void Cards_ItemRemoved(object? sender, int index, CardPanelItem item)
        {
            if (item == selectedCard)
            {
                selectedCard = null;
            }
        }

        private void Cards_ItemInserted(object? sender, int index, CardPanelItem item)
        {
        }
    }
}
