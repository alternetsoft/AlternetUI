using System;
using System.Collections.Generic;
using System.Text;

namespace ControlsSample
{
    public static class EnumValuesRu
    {
        private const string CommonNone = "Нет";
        private const string CommonTop = "Вверх";
        private const string CommonBottom = "Вниз";
        private const string CommonLeft = "Влево";
        private const string CommonRight = "Вправо";
        private const string CommonFill = "Заполнить";
        private const string CommonCenter = "Центр";
        private const string CommonStretch = "Растянуть";

        public static class DockStyleProperties
        {
            public static string None = CommonNone;
            public static string Top = CommonTop;
            public static string Bottom = CommonBottom;
            public static string Left = CommonLeft;
            public static string Right = CommonRight;
            public static string Fill = CommonFill;
        }

        public static class WindowSizeToContentModeProperties
        {
            public static string None = CommonNone;
            public static string Width = "Ширина";
            public static string Height = "Высота";
            public static string WidthAndHeight = "ШиринаВысота";
        }

        public static class VerticalAlignmentProperties
        {
            public static string Top = CommonTop;
            public static string Center = CommonCenter;
            public static string Bottom = CommonBottom;
            public static string Stretch = CommonStretch;
            public static string Fill = CommonFill;
        }

        public static class HorizontalAlignmentProperties
        {
            public static string Left = CommonLeft;
            public static string Center = CommonCenter;
            public static string Right = CommonRight;
            public static string Stretch = CommonStretch;
            public static string Fill = CommonFill;
        }

        public static class LayoutStyleProperties
        {
            public static string None = CommonNone;
            public static string Dock = "Докировать";
            public static string Basic = "Базовый";
            public static string Vertical = "Вертикальный";
            public static string Horizontal = "Горизонтальный";
        }
    }
}

