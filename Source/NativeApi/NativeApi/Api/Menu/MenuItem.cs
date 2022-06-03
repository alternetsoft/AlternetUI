using System;

namespace NativeApi.Api
{
    public class MenuItem : Control
    {
        public string Text { get => throw new Exception(); set => throw new Exception(); }

        public bool Checked { get => throw new Exception(); set => throw new Exception(); }

        public Key ShortcutKey { get => throw new Exception(); }
        
        public ModifierKeys ShortcutModifierKeys { get => throw new Exception(); }

        public void SetShortcut(Key key, ModifierKeys modifierKeys) => throw new Exception();

        public event EventHandler? Click { add => throw new Exception(); remove => throw new Exception(); }

        public Menu? Submenu { get => throw new Exception(); set => throw new Exception(); }
    }
}