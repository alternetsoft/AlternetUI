﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal partial class ObjectInit
    {
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
            control.SuggestedSize = defaultListSize;
            var panel = CreatePanelWithButtons("Card 1");
            panel.AddLabel("Use SelectedCardIndex").Margin = 5;
            panel.AddLabel("to change active card").Margin = 5;
            control.Add("card 1", panel);
            control.Add("card 2", CreatePanelWithButtons("Card 2"));
            control.SelectCard(0);
        }

        public static void InitCardPanelHeader(object c)
        {
            if (c is not CardPanelHeader control)
                return;
            control.SuggestedSize = defaultListSize;
            control.UseTabBackgroundColor = true;
            control.Add("tab 1");
            control.Add("tab 2");
            control.SelectFirstTab();

            control.TabClick += Control_TabClick;

            static void Control_TabClick(object? sender, EventArgs e)
            {
                if (sender is not CardPanelHeader control)
                    return;
                var text = control.SelectedTab?.HeaderButton?.Text;
                Application.Log($"TabClick: {text}");
            }
        }
    }
}
