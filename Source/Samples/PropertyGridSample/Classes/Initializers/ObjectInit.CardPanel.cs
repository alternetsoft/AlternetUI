using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        public static void InitCardPanelHeader(CardPanelHeader control)
        {
            control.Add("Item 1");
            control.Add("Item 2");
            control.Add("Item 3");

            var item0 = control.Tabs[0];
            var item1 = control.Tabs[1];
            var item2 = control.Tabs[2];

            var size = 32;

            item0.Image = MessageBoxSvg.GetAsBitmap(MessageBoxIcon.Error, size, control);
            item1.Image = MessageBoxSvg.GetAsBitmap(MessageBoxIcon.Information, size, control);
            item2.Image = MessageBoxSvg.GetAsBitmap(MessageBoxIcon.Warning, size, control);

            control.TabClick += (s, e) =>
            {
                App.LogReplace(
                    $"CarPanelHeader.TabClick: {control.SelectedTabIndex}",
                    "CarPanelHeader.TabClick");
            };
        }

        public static void InitSideBarPanel(object c)
        {
            if (c is not SideBarPanel control)
                return;
            control.SuggestedSize = defaultListSize;
            var tab1 = CreatePanelWithButtons("Tab 1");
            var tab2 = CreatePanelWithButtons("Tab 2");
            control.Add("Tab 1", tab1);
            control.Add("Tab 2", tab2);
            control.SelectTab(0);
        }

        public static void InitCardPanel(object c)
        {
            if (c is not CardPanel control)
                return;
            control.HasBorder = true;
            control.SuggestedSize = defaultListSize;
            var panel = CreatePanelWithButtons("Card 1");

            new Label()
            {
                Text = "Use SelectedCardIndex",
                Margin = 5,
                Parent = panel,
            };

            new Label()
            {
                Text = "to change active card",
                Margin = 5,
                Parent = panel,
            };

            control.Add("card 1", panel);
            control.Add("card 2", CreatePanelWithButtons("Card 2"));
            control.SelectCard(0);
        }
    }
}
