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
        public byte? AsByteNull { get; set; }

        public bool AsBool { get; set; } = true;
        public bool? AsBoolNull { get; set; }

        public char AsChar { get; set; } = 'A';
        public char? AsCharNull { get; set; }

        public sbyte AsSByte { get; set; } = 25;
        public sbyte? AsSByteNull { get; set; }

        public short AsInt16 { get; set; } = 15;
        public short? AsInt16Null { get; set; }

        public ushort AsUInt16 { get; set; } = 15;
        public ushort? AsUInt16Null { get; set; }

        public int AsInt32 { get; set; } = 15;
        public int? AsInt32Null { get; set; }

        public uint AsUInt32 { get; set; } = 15;
        public uint? AsUInt32Null { get; set; }

        public long AsInt64 { get; set; } = 15;
        public long? AsInt64Null { get; set; }

        public ulong AsUInt64 { get; set; } = 15;
        public ulong? AsUInt64Null { get; set; }

        public float AsSingle { get; set; } = 15.26F;
        public float? AsSingleNull { get; set; }

        public double AsDouble { get; set; } = 15.27D;
        public double? AsDoubleNull { get; set; }

        public decimal AsDecimal { get; set; } = 15.66M;
        public decimal? AsDecimalNull { get; set; }

        public DateTime AsDateTime { get; set; } = DateTime.Now;
        public DateTime? AsDateTimeNull { get; set; }

        public string AsString { get; set; } = "hello";
        public string? AsStringNull { get; set; }

        public Color AsColor { get; set; }
        public Color? AsColorNull { get; set; }

        public Font AsFont { get; set; } = Font.Default;
        public Font? AsFontNull { get; set; }

        public Brush AsBrush { get; set; } = Brush.Default;
        public Brush? AsBrushNull { get; set; }

        public Pen AsPen { get; set; } = Pen.Default;
        public Pen? AsPenNull { get; set; }

        public Size AsSize { get; set; }
        public Size? AsSizeNull { get; set; }

        public Thickness AsThickness { get; set; }
        public Rect AsRect { get; set; }
        public Point AsPoint { get; set; }
    }
}

