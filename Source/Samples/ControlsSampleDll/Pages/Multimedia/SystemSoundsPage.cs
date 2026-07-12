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

            var beepButton = new XButton("Play Beep", () => SystemSounds.Beep.Play());
            var asteriskButton = new XButton("Play Asterisk", () => SystemSounds.Asterisk.Play());
            var exclamationButton = new XButton("Play Exclamation", () => SystemSounds.Exclamation.Play());
            var handButton = new XButton("Play Hand", () => SystemSounds.Hand.Play());
            var questionButton = new XButton("Play Question", () => SystemSounds.Question.Play());

            new ControlSet(beepButton, asteriskButton, exclamationButton, handButton, questionButton)
            .Margin(5).HorizontalAlignment(HorizontalAlignment.Left)
            .Parent(this).MinWidthToMaxPreferred();
        }
    }
}
