using ApiCommon;

namespace NativeApi.Api
{
    [ManagedExternName("Alternet.UI.DragAction")]
    [ManagedName("Alternet.UI.DragAction")]
    public enum DragAction
    {
        Continue = 0,
        Drop = 1,
        Cancel = 2,
    }
}