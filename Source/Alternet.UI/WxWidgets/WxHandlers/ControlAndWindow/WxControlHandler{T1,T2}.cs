namespace Alternet.UI
{
    internal class WxControlHandler<T1, T2> : WxControlHandler
        where T1 : Control
        where T2 : Native.Control, new()
    {
        public new T1? Control => (T1?)base.Control;

        public new T2 NativeControl => (T2)base.NativeControl!;

        internal override Native.Control CreateNativeControl()
        {
            return new T2();
        }
    }
}