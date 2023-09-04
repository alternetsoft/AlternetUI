using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

namespace PropertyGridSample
{
    internal class NullableProps
    {
        public static NullableProps Default = new();

        byte? asByte = 15;
        bool? asBool = true;
        char? asChar = 'A';
        sbyte? asSByte = 25;
        short? asInt16 = -150;
        ushort? asUInt16 = 215;
        int? asInt32 = 81;
        uint? asUInt32 = 105;
        long? asInt64 = 12;
        ulong? asUInt64 = 315;
        float? asSingle = 15.26F;
        double? asDouble = 25.27D;
        decimal? asDecimal = 34.66M;
        DateTime? asDateTime = DateTime.Now;
        string? asString = "hello";
        Color? asColor = Color.Red;
        Font? asFont = Font.Default;
        Brush? asBrush = Brush.Default;
        Pen? asPen = Pen.Default;
        Size? asSize = new Size(26, 30);
        Thickness? asThickness = new Thickness(126, 50, 80, 24);
        Rect? asRect = new Rect(51, 82, 354, 40);
        Point? asPoint = new Point(50, 85);

        public byte? AsByteN
        {
            get => asByte;
            set => asByte = value;
        }

        public bool? AsBoolN
        {
            get => asBool;
            set => asBool = value;
        }

        public char? AsCharN
        {
            get => asChar;
            set => asChar = value;
        }
        public sbyte? AsSByteN
        {
            get => asSByte;
            set => asSByte = value;
        }

        public short? AsInt16N
        {
            get => asInt16;
            set => asInt16 = value;
        }

        public ushort? AsUInt16N
        {
            get => asUInt16;
            set => asUInt16 = value;
        }

        public int? AsInt32N
        {
            get => asInt32;
            set => asInt32 = value;
        }

        public uint? AsUInt32N
        {
            get => asUInt32;
            set => asUInt32 = value;
        }

        public long? AsInt64N
        {
            get => asInt64;
            set => asInt64 = value;
        }

        public ulong? AsUInt64N
        {
            get => asUInt64;
            set => asUInt64 = value;
        }

        public float? AsSingleN
        {
            get => asSingle;
            set => asSingle = value;
        }

        public double? AsDoubleN
        {
            get => asDouble;
            set => asDouble = value;
        }

        public decimal? AsDecimalN
        {
            get => asDecimal;
            set => asDecimal = value;
        }

        public DateTime? AsDateTimeN
        {
            get => asDateTime;
            set => asDateTime = value;
        }

        public string? AsStringN
        {
            get => asString;
            set => asString = value;
        }

        public Color? AsColorN
        {
            get => asColor;
            set => asColor = value;
        }

        public Font? AsFontN
        {
            get => asFont;
            set => asFont = value;
        }

        public Brush? AsBrushN
        {
            get => asBrush;
            set => asBrush = value;
        }

        public Pen? AsPenN
        {
            get => asPen;
            set => asPen = value;
        }

        public Size? AsSizeN
        {
            get => asSize;
            set => asSize = value;
        }

        public Thickness? AsThicknessN
        {
            get => asThickness;
            set => asThickness = value;
        }

        public Rect? AsRectN
        {
            get => asRect;
            set => asRect = value;
        }

        public Point? AsPointN
        {
            get => asPoint;
            set => asPoint = value;
        }
    }
}
