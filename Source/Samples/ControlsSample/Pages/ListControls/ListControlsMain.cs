using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class ListControlsMain : VerticalStackPanel
    {
        private readonly CardPanelHeader panelHeader = new();
        private readonly CardPanel cardPanel = new();

        public ListControlsMain()
            : base()
        {
            Padding = 5;
            panelHeader.Parent = this;
            cardPanel.Parent = this;
            cardPanel.VerticalAlignment = VerticalAlignment.Stretch;
            var listBoxCardIndex = cardPanel.Add(null, CreateListBoxPage);
            var checkListBoxCardIndex = cardPanel.Add(null, CreateCheckListBoxPage);
            var comboBoxCardIndex = cardPanel.Add(null, CreateComboBoxPage);
            var popupsCardIndex = cardPanel.Add(null, CreateListControlsPopups);

            panelHeader.CardPanel = cardPanel;
            panelHeader.Add("ListBox", cardPanel[listBoxCardIndex].UniqueId);
            panelHeader.Add("CheckListBox", cardPanel[checkListBoxCardIndex].UniqueId);
            panelHeader.Add("ComboBox", cardPanel[comboBoxCardIndex].UniqueId);
            panelHeader.Add("Popups", cardPanel[popupsCardIndex].UniqueId);
            cardPanel.SetActiveCard(0);
            panelHeader.SelectFirstTab();
        }

        private Control CreateListBoxPage()
        {
            return new ListBoxPage();
        }
        private Control CreateListControlsPopups()
        {
            return new ListControlsPopups();
        }

        private Control CreateCheckListBoxPage()
        {
            return new CheckListBoxPage();
        }

        private Control CreateComboBoxPage()
        {
            return new ComboBoxPage();
        }


    }
}
