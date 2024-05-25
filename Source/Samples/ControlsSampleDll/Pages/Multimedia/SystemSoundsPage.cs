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

            AddButtons(
                ("Play Beep", SystemSounds.Beep.Play),
                ("Play Asterisk", SystemSounds.Asterisk.Play),
                ("Play Exclamation", SystemSounds.Exclamation.Play),
                ("Play Hand", SystemSounds.Hand.Play),
                ("Play Question", SystemSounds.Question.Play))
            .Margin(5).HorizontalAlignment(HorizontalAlignment.Left).SuggestedWidthToMax();
        }
    }
}
