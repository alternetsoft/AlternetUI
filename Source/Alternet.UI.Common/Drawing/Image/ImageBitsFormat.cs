using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public struct ImageBitsFormat
    {
        public int BitsPerPixel;

        public bool HasAlpha;

        public int SizePixel;

        public int Red;

        public int Green;

        public int Blue;

        public int Alpha;

        public readonly void Log(string? sectionName = null)
        {
            var self = this;

            App.LogSection(() =>
            {
                App.LogNameValue("BitsPerPixel", self.BitsPerPixel);
                App.LogNameValue("HasAlpha", self.HasAlpha);
                App.LogNameValue("SizePixel", self.SizePixel);
                App.LogNameValue("Red", self.Red);
                App.LogNameValue("Green", self.Green);
                App.LogNameValue("Blue", self.Blue);
                App.LogNameValue("Alpha", self.Alpha);
            }, sectionName);
        }
    }
}