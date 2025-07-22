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
        [IsTextLocalized(true)]
        private readonly Button button1;

        [IsTextLocalized(true)]
        private readonly Button button2;

        private readonly UIActionSimulator simulator = new();
        private readonly TextBox editor;
        private readonly TextBox editor2;
        private int counter;

        private readonly Panel panel = new()
        {
            Layout = LayoutStyle.Vertical,
        };

        public ActionSimulatorPage()
        {
            panel.MinChildMargin = 5;
            Size = (800, 600);

            new Label(
                panel,
                [
                    "This demo send mouse clicks and key presses to operating system",
                    "in order to control user interface of the application.",
                    "Press F5 to start.",
                ]);

            button1 = new(panel, "Button 1", () => App.Log("Button 1 clicked"));
            button2 = new(panel, "Button 2", () => App.Log("Button 2 clicked"));

            editor = new TextBox(panel);
            editor2 = new TextBox(panel);

            new Label(
                panel,
                [
                    "Simulator moves mouse to 'Button1' and clicks it.",
                    "After that, it moves mouse to the editor, clicks it and enters 'Hello' text.",
                    "Simulator currently doesn't work when using Wayland on Linux.",
                ]);

            panel.Parent = this;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(e.Key == Key.F5)
            {
                App.AddIdleTask(() =>
                {
                    if (Fn())
                        App.Log("Simulation is ok.");
                    else
                        App.Log("Simulation not working.");

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
                        simulator.SendKeyIf(
                            ref condition,
                            Key.V,
                            Alternet.UI.ModifierKeys.Control);
                        return condition;
                    }
                });
            }
        }
    }
}
