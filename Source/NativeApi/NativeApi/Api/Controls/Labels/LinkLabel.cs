#pragma warning disable
using ApiCommon;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class LinkLabel : Control
    {
        public Color HoverColor { get; set; }
        public Color NormalColor { get; set; }
        public Color VisitedColor { get; set; }
        public bool Visited { get; set; }

        public string Url { get; set; }

        public static bool UseGenericControl { get; set; }

        [NativeEvent(cancellable: true)]
        public event EventHandler? HyperlinkClick;
    }
}


