#pragma warning disable
using Alternet.UI;
using System;
using Alternet.Drawing;
using ApiCommon;

namespace NativeApi.Api
{
    public class GLControl : Control
    {
        public static bool IsOpenGLAvailable() => default;

        public static void CreateDummyOpenGlCanvas() { }

        public SizeI ViewportSize { get; }

        public IntPtr GetGLContext() => default;

        public bool DefaultPaintUsed { get; set; }
    }
}
