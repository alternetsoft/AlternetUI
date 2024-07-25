using Alternet.UI;
using Alternet.Drawing;
using System;
using System.Reflection;

namespace DrawingSample
{
    internal static class Resources
    {
        public static readonly Image LogoImage;
        public static readonly Image LeavesImage;

        static Resources()
        {
            var asm = typeof(Resources).Assembly;
            LogoImage = new Bitmap(AssemblyUtils.GetImageUrlInAssembly(asm, "Resources.Logo.png"));
            LeavesImage = new Bitmap(AssemblyUtils.GetImageUrlInAssembly(asm, "Resources.Leaves.jpg"));
        }
    }
}