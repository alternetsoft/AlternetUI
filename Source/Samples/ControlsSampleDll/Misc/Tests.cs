using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace ControlsSample
{
    public static class Tests
    {
        public static void TestWindowTextInput()
        {
            var window = new WindowTextInput();
            window.ShowDialogAsync();
        }
    }
}
