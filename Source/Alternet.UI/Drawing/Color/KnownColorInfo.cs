using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    internal class KnownColorInfo : IKnownColorInfo
    {
        private readonly KnownColor knownColor;
        private readonly string label;
        private readonly Color value;
        private KnownColorCategory category;
        private string labelLocalized;

        public KnownColorInfo(KnownColor knownColor)
        {
            this.knownColor = knownColor;
            this.category = KnownColorCategory.Other;
            label = knownColor.ToString();
            labelLocalized = label;
            value = Color.FromKnownColor(knownColor);
        }

        public bool Visible { get; set; } = true;

        public KnownColor KnownColor { get => knownColor; }

        public KnownColorCategory Category { get => category; set => category = value; }

        public string Label { get => label; }

        public string LabelLocalized { get => labelLocalized; set => labelLocalized = value; }

        public Color Value { get => value; }
    }
}
