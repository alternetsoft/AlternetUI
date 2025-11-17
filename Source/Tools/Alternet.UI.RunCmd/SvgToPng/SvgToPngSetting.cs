using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class SvgToPngSetting
    {
        public string? Filename { get; set; }

        public Color? LightColor { get; set; }

        public Color? DarkColor { get; set; }

        [XmlIgnore]
        public Image? Light16 { get; set; }

        [XmlIgnore]
        public Image? Light32 { get; set; }

        [XmlIgnore]
        public Image? Dark16 { get; set; }

        [XmlIgnore]
        public Image? Dark32 { get; set; }
    }
}
