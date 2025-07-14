#pragma warning disable
using System;
using Alternet.UI;

namespace NativeApi.Api
{
    public class PageSetupDialog
    {
        public ModalResult ShowModal(Window? owner) => throw new Exception();
        public PrintDocument? Document { get => throw new Exception(); set => throw new Exception(); }

        public double MinMarginLeft { get => throw new Exception(); set => throw new Exception(); }
        public double MinMarginTop { get => throw new Exception(); set => throw new Exception(); }

        public double MinMarginRight { get => throw new Exception(); set => throw new Exception(); }

        public double MinMarginBottom { get => throw new Exception(); set => throw new Exception(); }

        public bool MinMarginsValueSet { get => throw new Exception(); set => throw new Exception(); }


        public bool AllowMargins { get => throw new Exception(); set => throw new Exception(); }
        public bool AllowOrientation { get => throw new Exception(); set => throw new Exception(); }
        public bool AllowPaper { get => throw new Exception(); set => throw new Exception(); }
        public bool AllowPrinter { get => throw new Exception(); set => throw new Exception(); }
    }
}