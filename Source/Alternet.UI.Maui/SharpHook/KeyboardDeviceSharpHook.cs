using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.UI.Maui
{
    public class KeyboardDeviceSharpHook : KeyboardDevice
    {
        public static KeyboardDevice Default = new KeyboardDeviceSharpHook();

        public static KeyStates KeyStates;

        protected override KeyStates GetKeyStatesFromSystem(Key key)
        {
            return KeyStates;
        }
    }
}
