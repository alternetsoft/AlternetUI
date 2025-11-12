using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI.WinForms
{
    internal partial class WinFormsDisplayHandler : DisposableObject, IDisplayHandler
    {
        static WinFormsDisplayHandler()
        {
        }

        public WinFormsDisplayHandler()
        {
        }

        public WinFormsDisplayHandler(int index)
        {
        }

        public bool IsOk => true;

        public static Coord GetDefaultScaleFactor()
        {
            return 2f;
        }

        public RectI GetClientArea()
        {
            return GetGeometry();
        }

        public RectI GetGeometry()
        {
            return (0, 0, (int)1280, (int)1024);
        }

        public string GetName()
        {
            return "MainDisplay";
        }

        public Coord GetScaleFactor()
        {
            var result = GetDefaultScaleFactor();
            return result;
        }

        public bool IsPrimary()
        {
            return true;
        }
    }
}
