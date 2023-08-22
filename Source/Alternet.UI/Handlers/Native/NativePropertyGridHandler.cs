using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativePropertyGridHandler : ControlHandler<PropertyGrid>
    {
        public NativePropertyGridHandler()
            : base()
        {
        }

        public long CreateStyle
        {
            get
            {
                return NativeControl.CreateStyle;
            }

            set
            {
                NativeControl.CreateStyle = value;
            }
        }

        public override IEnumerable<Control> AllChildrenIncludedInLayout
            => Enumerable.Empty<Control>();

        public new Native.PropertyGrid NativeControl =>
            (Native.PropertyGrid)base.NativeControl!;

        public override void OnLayout()
        {
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativePropertyGrid(PropertyGrid.DefaultCreateStyle);
        }

        protected override void OnDetach()
        {
            base.OnDetach();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
        }

        public class NativePropertyGrid : Native.PropertyGrid
        {
            public NativePropertyGrid(PropertyGridCreateStyle style)
                : base()
            {
                SetNativePointer(NativeApi.PropertyGrid_CreateEx_((int)style));
            }
        }
    }
}