using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class SvgToPngSettings
    {
        public BaseCollection<SvgToPngSetting> Items { get; } = new();

        public string? DarkStripName16 { get; set; }

        public string? DarkStripName32 { get; set; }

        public string? LightStripName16 { get; set; }

        public string? LightStripName32 { get; set; }
    }
}


