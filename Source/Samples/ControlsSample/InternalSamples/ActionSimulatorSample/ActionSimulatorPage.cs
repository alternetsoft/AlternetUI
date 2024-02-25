﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class ActionSimulatorPage : Window
    {
        private readonly Button button1;
        private readonly Button button2;
        private readonly UIActionSimulator simulator = new();
        private readonly TextBox editor;

        public ActionSimulatorPage()
        {
            Layout = LayoutStyle.Vertical;
            MinChildMargin = 5;

            AddLabels(
                "This demo send mouse clicks and key presses to operating system",
                "in order to control user interface of the application.",
                "Press F5 to start.");

            button1 = AddButton("Button 1", () =>
            {
                Application.Log("Button 1 clicked");
            });

            button2 = AddButton("Button 2", () =>
            {
                Application.Log("Button 2 clicked");
            });

            editor = new TextBox();
            editor.Parent = this;

            AddLabels(
                "Simulator moves mouse to 'Button1' and clicks it.",
                "After that, it moves mouse to the editor, clicks it",
                "and enters 'Hello' text.");

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(e.Key == Key.F5)
            {
                Application.AddIdleTask(() =>
                {
                    simulator.SendMouseMove(button1, (5,5));
                    simulator.SendMouseClick();
                    simulator.SendMouseMove(editor, (5, 5));
                    simulator.SendMouseClick();
                    simulator.SendText("Hello");
                });
            }
        }
    }
}