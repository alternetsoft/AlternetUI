#pragma warning disable
using System;

namespace NativeApi.Api
{
    public class ToolbarItem : Control
    {
        public string ManagedCommandId { get => throw new Exception(); set => throw new Exception(); }

        public string Text { get => throw new Exception(); set => throw new Exception(); }

        public bool Checked { get => throw new Exception(); set => throw new Exception(); }

        public event EventHandler? Click { add => throw new Exception(); remove => throw new Exception(); }

        public Menu? DropDownMenu { get => throw new Exception(); set => throw new Exception(); }

        public bool IsCheckable { get => throw new Exception(); set => throw new Exception(); }

        public ImageSet? DisabledImage { get => throw new Exception(); set => throw new Exception(); }

        public ImageSet? Image { get => throw new Exception(); set => throw new Exception(); }
    }
}