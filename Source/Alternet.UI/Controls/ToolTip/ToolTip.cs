using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class ToolTip : DisposableObject, IToolTip
    {
        public ToolTip(string message)
            : base(Native.WxOtherFactory.CreateToolTip(message), true)
        {
        }

        public ToolTip()
            : base(Native.WxOtherFactory.CreateToolTip(string.Empty), true)
        {
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanagedResources()
        {
            Native.WxOtherFactory.DeleteToolTip(Handle);
        }
    }
}
