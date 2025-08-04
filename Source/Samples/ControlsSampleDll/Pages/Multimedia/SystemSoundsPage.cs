using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class SystemSoundsPage : Panel
    {
        public SystemSoundsPage()
        {
            Layout = LayoutStyle.Vertical;

            var beepButton = new Button("Play Beep", () => SystemSounds.Beep.Play());
            var asteriskButton = new Button("Play Asterisk", () => SystemSounds.Asterisk.Play());
            var exclamationButton = new Button("Play Exclamation", () => SystemSounds.Exclamation.Play());
            var handButton = new Button("Play Hand", () => SystemSounds.Hand.Play());
            var questionButton = new Button("Play Question", () => SystemSounds.Question.Play());

            new ControlSet(beepButton, asteriskButton, exclamationButton, handButton, questionButton)
            .Margin(5).HorizontalAlignment(HorizontalAlignment.Left)
            .Parent(this).SuggestedWidthToMax();
        }
    }
}
