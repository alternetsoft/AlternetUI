using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static NullableProps DefaultWithValues = new(true);

        SampleClassWithProps classProp3 = new();
        SampleClassWithProps? classProp2;
        SampleClassWithProps classProp1 = new();
        byte? asByte;
        bool? asBool;
        char? asChar;
        sbyte? asSByte;
        short? asInt16;
        ushort? asUInt16;
        int? asInt32;
        uint? asUInt32;
        long? asInt64;
        ulong? asUInt64;
        float? asSingle;
        double? asDouble;
        decimal? asDecimal;
        DateTime? asDateTime;
        string? asString;
        Color? asColor;
        Font? asFont;
        Brush? asBrush;
        Pen? asPen;
        Size? asSize;
        Thickness? asThickness;
        Rect? asRect;
        Point? asPoint;

        static NullableProps()
        {
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsByteN), "byte");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsBoolN), "bool");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsCharN), "char");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsSByteN), "sbyte");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsInt16N), "short");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsUInt16N), "ushort");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsInt32N), "int");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsUInt32N), "uint");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsInt64N), "long");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsUInt64N), "ulong");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsSingleN), "float");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsDoubleN), "double");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsDecimalN), "decimal");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsDateTimeN), "DateTime");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsStringN), "string");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsColorN), "Color");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsFontN), "Font");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsBrushN), "Brush");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsPenN), "Pen");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsSizeN), "Size");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsThicknessN), "Thickness");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsRectN), "Rect");
            PropertyGrid.SetCustomLabel<NullableProps>(nameof(AsPointN), "Point");
        }

        public NullableProps(bool setDefaults = false)
        {
            if (setDefaults)
                SetDefaults();
        }

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

        [Browsable(false)]
        public SampleClassWithProps ClassProp1
        {
            get
            {
                return classProp1;
            }

            set
            {
                classProp1 = value;
            }
        }

        [Browsable(false)]
        public SampleClassWithProps? ClassProp2
        {
            get
            {
                return classProp2;
            }

            set
            {
                classProp2 = value;
            }
        }

        [Browsable(false)]
        public SampleClassWithProps ClassProp3
        {
            get
            {
                return classProp3;
            }
        }

        internal void SetDefaults()
        {
            asByte = 15;
            asBool = true;
            asChar = 'A';
            asSByte = 25;
            asInt16 = -150;
            asUInt16 = 215;
            asInt32 = 81;
            asUInt32 = 105;
            asInt64 = 12;
            asUInt64 = 315;
            asSingle = 15.26F;
            asDouble = 25.27D;
            asDecimal = 34.66M;
            asDateTime = DateTime.Now;
            asString = "hello";
            asColor = Color.Red;
            asFont = Font.Default;
            asBrush = Brush.Default;
            asPen = Pen.Default;
            asSize = new Size(26, 30);
            asThickness = new Thickness(126, 50, 80, 24);
            asRect = new Rect(51, 82, 354, 40);
            asPoint = new Point(50, 85);
        }
    }
}