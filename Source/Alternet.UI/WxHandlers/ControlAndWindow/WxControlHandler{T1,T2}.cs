namespace Alternet.UI
{
    internal abstract class WxControlHandler<T1, T2> : WxControlHandler
        where T1 : Control
        where T2 : Native.Control
    {
        public new T1 Control => (T1)base.Control;

        public new T2 NativeControl => (T2)base.NativeControl!;
    }
}