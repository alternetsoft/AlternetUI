using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

namespace PropertyGridSample
{
    internal class WelcomeProps
    {
        public static WelcomeProps Default = new();

        public byte AsByte { get; set; } = 15;
        public bool AsBool { get; set; } = true;
        public char AsChar { get; set; } = 'A';
        public sbyte AsSByte { get; set; } = 25;
        public short AsInt16 { get; set; } = -150;
        public ushort AsUInt16 { get; set; } = 215;
        public int AsInt32 { get; set; } = 81;
        public uint AsUInt32 { get; set; } = 105;
        public long AsInt64 { get; set; } = 12;
        public ulong AsUInt64 { get; set; } = 315;
        public float AsSingle { get; set; } = 15.26F;
        public double AsDouble { get; set; } = 25.27D;
        public decimal AsDecimal { get; set; } = 34.66M;
        public DateTime AsDateTime { get; set; } = DateTime.Now;
        public string AsString { get; set; } = "hello";
        public Color AsColor { get; set; } = Color.Red;
        public Font AsFont { get; set; } = Font.Default;
        public Brush AsBrush { get; set; } = Brush.Default;
        public Pen AsPen { get; set; } = Pen.Default;
        public Size AsSize { get; set; } = new Size(26, 30);
        public Thickness AsThickness { get; set; } = new Thickness(126, 50, 80, 24);
        public Rect AsRect { get; set; } = new Rect(51, 82, 354, 40);
        public Point AsPoint { get; set; } = new Point(50, 85);

        public float? AsSingleNull { get; set; }
        public double? AsDoubleNull { get; set; }
        public decimal? AsDecimalNull { get; set; }
        public byte? AsByteNull { get; set; }
        public bool? AsBoolNull { get; set; }
        public char? AsCharNull { get; set; }
        public sbyte? AsSByteNull { get; set; }
        public short? AsInt16Null { get; set; }
        public ushort? AsUInt16Null { get; set; }
        public int? AsInt32Null { get; set; }
        public uint? AsUInt32Null { get; set; }
        public long? AsInt64Null { get; set; }
        public ulong? AsUInt64Null { get; set; }
        public DateTime? AsDateTimeNull { get; set; }
        public string? AsStringNull { get; set; }
        public Color? AsColorNull { get; set; }
        public Font? AsFontNull { get; set; }
        public Brush? AsBrushNull { get; set; }
        public Pen? AsPenNull { get; set; }
        public Size? AsSizeNull { get; set; }
    }
}

