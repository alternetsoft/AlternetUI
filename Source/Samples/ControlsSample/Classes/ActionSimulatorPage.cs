using System;
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
        private readonly TextBox editor2;
        private int counter;

        public ActionSimulatorPage()
        {
            Layout = LayoutStyle.Vertical;
            MinChildMargin = 5;
            Size = (800, 600);

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

            editor2 = new TextBox();
            editor2.Parent = this;

            AddLabels(
                "Simulator moves mouse to 'Button1' and clicks it.",
                "After that, it moves mouse to the editor, clicks it",
                "and enters 'Hello' text.");

            AddLabels(
                "Simulator currently doesn't work when using Wayland on Linux.");
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(e.Key == Key.F5)
            {
                Application.AddIdleTask(() =>
                {
                    if (Fn())
                        Application.Log("Simulation is ok.");
                    else
                        Application.Log("Simulation not working.");

                    bool Fn()
                    {
                        var condition = true;
                        simulator.SendMouseMoveIf(ref condition, button1, (5, 5));
                        simulator.SendMouseClickIf(ref condition);
                        simulator.SendMouseMoveIf(ref condition, editor, (5, 5));
                        simulator.SendMouseClickIf(ref condition);
                        simulator.SendTextIf(ref condition, "Hello");
                        simulator.SendMouseMoveIf(ref condition, button2, (5, 5));
                        simulator.SendMouseClickIf(ref condition);

                        Clipboard.SetText($"({++counter}) Text from clipboard using Ctrl+V");
                        editor2.Focus();
                        editor2.Text = string.Empty;
                        simulator.SendKeyIf(ref condition, WxWidgetsKeyCode.V, UIActionSimulator.KeyModifier.Control);
                        return condition;
                    }
                });
            }
        }
    }
}
