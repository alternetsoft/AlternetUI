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
        Font asFont = Control.DefaultFont;
        Brush asBrush = Brush.Default;
        Pen asPen = Pen.Default;
        SizeD asSize = new(26, 30);
        Thickness asThickness = new(126, 50, 80, 24);
        RectD asRect = new(51, 82, 354, 40);
        PointD asPoint = new(50, 85);

        static WelcomeProps()
        {
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsByte), "byte");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsBool), "bool");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsChar), "char");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsSByte), "sbyte");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsInt16), "short");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsUInt16), "ushort");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsInt32), "int");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsUInt32), "uint");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsInt64), "long");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsUInt64), "ulong");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsSingle), "float");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsDouble), "double");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsDecimal), "decimal");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsDateTime), "DateTime");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsString), "string");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsColor), "Color");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsFont), "Font");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsBrush), "Brush");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsPen), "Pen");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsSize), "Size");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsThickness), "Thickness");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsRect), "Rect");
            PropertyGridUtils.SetCustomLabel<WelcomeProps>(nameof(AsPoint), "Point");
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

