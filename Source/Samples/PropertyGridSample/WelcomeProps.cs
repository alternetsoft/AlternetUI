﻿using System;
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
        Size asSize = new(26, 30);
        Thickness asThickness = new(126, 50, 80, 24);
        Rect asRect = new(51, 82, 354, 40);
        Point asPoint = new(50, 85);

        public byte AsByte
        {
            get => asByte;
            set => asByte = value;
        }

        public bool AsBool
        {
            get => asBool;
            set => asBool = value;
        }

        public char AsChar
        {
            get => asChar;
            set => asChar = value;
        }
        public sbyte AsSByte
        {
            get => asSByte;
            set => asSByte = value;
        }

        public short AsInt16
        {
            get => asInt16;
            set => asInt16 = value;
        }

        public ushort AsUInt16
        {
            get => asUInt16;
            set => asUInt16 = value;
        }

        public int AsInt32
        {
            get => asInt32;
            set => asInt32 = value;
        }

        public uint AsUInt32
        {
            get => asUInt32;
            set => asUInt32 = value;
        }

        public long AsInt64
        {
            get => asInt64;
            set => asInt64 = value;
        }

        public ulong AsUInt64
        {
            get => asUInt64;
            set => asUInt64 = value;
        }
        
        public float AsSingle
        {
            get => asSingle;
            set => asSingle = value;
        }
        
        public double AsDouble
        {
            get => asDouble;
            set => asDouble = value;
        }
        
        public decimal AsDecimal
        {
            get => asDecimal;
            set => asDecimal = value;
        }
        
        public DateTime AsDateTime
        {
            get => asDateTime;
            set => asDateTime = value;
        }

        public string AsString
        {
            get => asString;
            set => asString = value;
        }

        public Color AsColor
        {
            get => asColor;
            set => asColor = value;
        }
        
        public Font AsFont
        {
            get => asFont;
            set => asFont = value;
        }
        
        public Brush AsBrush
        {
            get => asBrush;
            set => asBrush = value;
        }

        public Pen AsPen
        {
            get => asPen;
            set => asPen = value;
        }

        public Size AsSize
        {
            get => asSize;
            set => asSize = value;
        }
        
        public Thickness AsThickness
        {
            get => asThickness;
            set => asThickness = value;
        }
        
        public Rect AsRect
        {
            get => asRect;
            set => asRect = value;
        }
        
        public Point AsPoint
        {
            get => asPoint;
            set => asPoint = value;
        }
    }
}

