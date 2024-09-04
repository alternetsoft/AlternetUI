using System;
using System.Collections.Generic;
using System.Text;

namespace ControlsSample
{
    public static class LocalizationManagerRu
    {
        public static void InitializeControlProperties()
        {
            PropNameStrings.ControlProperties.Layout = "Макет";
            PropNameStrings.ControlProperties.Title = "Заголовок";
            PropNameStrings.ControlProperties.Dock = "Докирование";
            PropNameStrings.ControlProperties.Text = "Текст";
            PropNameStrings.ControlProperties.ToolTip = "Подсказка";
            PropNameStrings.ControlProperties.Left = "Лево";
            PropNameStrings.ControlProperties.Top = "Верх";
            PropNameStrings.ControlProperties.Visible = "Видимый";
            PropNameStrings.ControlProperties.Enabled = "Включено";
            PropNameStrings.ControlProperties.Width = "Ширина";
            PropNameStrings.ControlProperties.Height = "Высота";
            PropNameStrings.ControlProperties.SuggestedWidth = "Желаемая Ширина";
            PropNameStrings.ControlProperties.SuggestedHeight = "Желаемая Высота";
            PropNameStrings.ControlProperties.MinChildMargin = "Мин Отступ Элементов";
            PropNameStrings.ControlProperties.Margin = "Внешний Отступ";
            PropNameStrings.ControlProperties.Padding = "Внутренний Отступ";
            PropNameStrings.ControlProperties.MinWidth = "Мин Ширина";
            PropNameStrings.ControlProperties.MinHeight = "Мин Высота";
            PropNameStrings.ControlProperties.MaxWidth = "Макс Ширина";
            PropNameStrings.ControlProperties.MaxHeight = "Макс Высота";
            PropNameStrings.ControlProperties.BackgroundColor = "Цвет Фона";
            PropNameStrings.ControlProperties.ParentBackColor = "Цвет Фона Родителя";
            PropNameStrings.ControlProperties.ParentForeColor = "Цвет Текста Родителя";
            PropNameStrings.ControlProperties.ParentFont = "Шрифт Родителя";
            PropNameStrings.ControlProperties.ForegroundColor = "Цвет Текста";
            PropNameStrings.ControlProperties.Font = "Шрифт";
            PropNameStrings.ControlProperties.IsBold = "Жирный шрифт";
            PropNameStrings.ControlProperties.VerticalAlignment = "Вертикальное положение";
            PropNameStrings.ControlProperties.HorizontalAlignment = "Горизонтальное положение";
        }

        public static void InitializeProperties()
        {
            InitializeControlProperties();
        }

        public static void Initialize()
        {
            InitializeProperties();
        }
    }
}
