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
    internal class WelcomeProps
    {
        internal const string CategoryInt = "Int values";
        internal const string CategoryFloat = "Float values";
        internal const string CategoryOther = "Other values";
        internal const string CategoryString = "String values";
        internal const string CategoryStruct = "Struct values";
        internal const string CategoryObject = "Object values";

        public static WelcomeProps Default = new();

        byte asByte = 15;
        bool asBool = true;
        char asChar = 'A';
        sbyte asSByte = 25;
        short asInt16 = -150;
        ushort asUInt16 = 215;
        int asInt32 = 81;
        uint asUInt32 = 105;
        long asInt64 = 12;
        ulong asUInt64 = 315;
        float asSingle = 15.26F;
        double asDouble = 25.27D;
        decimal asDecimal = 34.66M;
        DateTime asDateTime = DateTime.Now;
        string asString = "hello";
        Color asColor = Color.Red;
        Font asFont = Font.Default;
        Brush asBrush = Brush.Default;
        Pen asPen = Pen.Default;
        SizeD asSize = new(26, 30);
        Thickness asThickness = new(126, 50, 80, 24);
        RectD asRect = new(51, 82, 354, 40);
        PointD asPoint = new(50, 85);

        static WelcomeProps()
        {
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsByte), "byte");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsBool), "bool");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsChar), "char");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsSByte), "sbyte");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsInt16), "short");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsUInt16), "ushort");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsInt32), "int");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsUInt32), "uint");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsInt64), "long");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsUInt64), "ulong");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsSingle), "float");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsDouble), "double");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsDecimal), "decimal");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsDateTime), "DateTime");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsString), "string");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsColor), "Color");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsFont), "Font");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsBrush), "Brush");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsPen), "Pen");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsSize), "Size");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsThickness), "Thickness");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsRect), "Rect");
            PropertyGrid.SetCustomLabel<WelcomeProps>(nameof(AsPoint), "Point");
        }

        [Category(CategoryInt)]
        public byte AsByte
        {
            get => asByte;
            set => asByte = value;
        }

        [Category(CategoryInt)]
        public sbyte AsSByte
        {
            get => asSByte;
            set => asSByte = value;
        }

        [Category(CategoryInt)]
        public short AsInt16
        {
            get => asInt16;
            set => asInt16 = value;
        }

        [Category(CategoryInt)]
        public ushort AsUInt16
        {
            get => asUInt16;
            set => asUInt16 = value;
        }

        [Category(CategoryInt)]
        public int AsInt32
        {
            get => asInt32;
            set => asInt32 = value;
        }

        [Category(CategoryInt)]
        public uint AsUInt32
        {
            get => asUInt32;
            set => asUInt32 = value;
        }

        [Category(CategoryInt)]
        public long AsInt64
        {
            get => asInt64;
            set => asInt64 = value;
        }

        [Category(CategoryInt)]
        public ulong AsUInt64
        {
            get => asUInt64;
            set => asUInt64 = value;
        }

        [Category(CategoryFloat)]        
        public float AsSingle
        {
            get => asSingle;
            set => asSingle = value;
        }

        [Category(CategoryFloat)]
        public double AsDouble
        {
            get => asDouble;
            set => asDouble = value;
        }

        [Category(CategoryFloat)]
        public decimal AsDecimal
        {
            get => asDecimal;
            set => asDecimal = value;
        }

        [Category(CategoryOther)]
        public bool AsBool
        {
            get => asBool;
            set => asBool = value;
        }

        [Category(CategoryString)]
        public char AsChar
        {
            get => asChar;
            set => asChar = value;
        }

        [Category(CategoryString)]
        public string AsString
        {
            get => asString;
            set => asString = value;
        }

        [Category(CategoryOther)]
        public DateTime AsDateTime
        {
            get => asDateTime;
            set => asDateTime = value;
        }

        [Category(CategoryOther)]
        public Color AsColor
        {
            get => asColor;
            set => asColor = value;
        }

        [Category(CategoryObject)]
        public Font AsFont
        {
            get => asFont;
            set => asFont = value;
        }

        [Category(CategoryObject)]
        public Brush AsBrush
        {
            get => asBrush;
            set => asBrush = value;
        }

        [Category(CategoryObject)]
        public Pen AsPen
        {
            get => asPen;
            set => asPen = value;
        }

        [Category(CategoryStruct)]
        public SizeD AsSize
        {
            get => asSize;
            set => asSize = value;
        }

        [Category(CategoryStruct)]
        public Thickness AsThickness
        {
            get => asThickness;
            set => asThickness = value;
        }

        [Category(CategoryStruct)]
        public RectD AsRect
        {
            get => asRect;
            set => asRect = value;
        }

        [Category(CategoryStruct)]
        public PointD AsPoint
        {
            get => asPoint;
            set => asPoint = value;
        }
    }
}

