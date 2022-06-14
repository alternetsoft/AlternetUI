using System;

namespace NativeApi.Api
{
    public class MenuItem : Control
    {
        public string ManagedCommandId { get => throw new Exception(); set => throw new Exception(); }

        public string Text { get => throw new Exception(); set => throw new Exception(); }

        public string Role { get => throw new Exception(); set => throw new Exception(); }

        public bool Checked { get => throw new Exception(); set => throw new Exception(); }

        public void SetShortcut(Key key, ModifierKeys modifierKeys) => throw new Exception();

        public event EventHandler? Click { add => throw new Exception(); remove => throw new Exception(); }

        public Menu? Submenu { get => throw new Exception(); set => throw new Exception(); }
    }
}