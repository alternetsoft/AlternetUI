using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class KeyInfo
    {
        public KeyInfo(Key key, ModifierKeys modifiers = ModifierKeys.None)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public Key Key { get; set; }

        public ModifierKeys Modifiers { get; set; }

        public bool IsPressed(KeyEventArgs e) => e.Key == Key && e.Modifiers == Modifiers;

        public bool Run(KeyEventArgs e, Action? action = null, bool setHandled = true)
        {
            var result = IsPressed(e);
            if (result)
            {
                action?.Invoke();
                if (setHandled)
                    e.Handled = true;
            }

            return result;
        }
    }
}
