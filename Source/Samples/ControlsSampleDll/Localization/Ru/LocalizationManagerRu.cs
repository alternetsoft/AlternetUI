using System;
using System.Collections.Generic;
using System.Text;

namespace ControlsSample
{
    public static class LocalizationManagerRu
    {
        public static void Initialize()
        {
            InitializeControlProperties();
            PropNameStrings.RegisterPropNameLocalizations();
            PropNameStrings.RegisterPropNameLocalizations(typeof(EnumValuesRu));
            PropNameStrings.RegisterPropNameLocalizations(typeof(PropNamesRu));
        }

        private static void InitializeControlProperties()
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
            PropNameStrings.ControlProperties.SuggestedWidth = "Желаем ширина";
            PropNameStrings.ControlProperties.SuggestedHeight = "Желаем высота";
            PropNameStrings.ControlProperties.MinChildMargin = "Мин отступ детей";
            PropNameStrings.ControlProperties.Margin = "Внешн отступ";
            PropNameStrings.ControlProperties.Padding = "Внутр отступ";
            PropNameStrings.ControlProperties.MinWidth = "Мин ширина";
            PropNameStrings.ControlProperties.MinHeight = "Мин высота";
            PropNameStrings.ControlProperties.MaxWidth = "Макс ширина";
            PropNameStrings.ControlProperties.MaxHeight = "Макс высота";
            PropNameStrings.ControlProperties.BackgroundColor = "Цвет фона";
            PropNameStrings.ControlProperties.ParentBackColor = "Цвет фона родителя";
            PropNameStrings.ControlProperties.ParentForeColor = "Цвет текста родителя";
            PropNameStrings.ControlProperties.ParentFont = "Шрифт родителя";
            PropNameStrings.ControlProperties.ForegroundColor = "Цвет текста";
            PropNameStrings.ControlProperties.Font = "Шрифт";
            PropNameStrings.ControlProperties.IsBold = "Жирный шрифт";
            PropNameStrings.ControlProperties.VerticalAlignment = "Вертик полож";
            PropNameStrings.ControlProperties.HorizontalAlignment = "Горизонт полож";
            PropNameStrings.ControlProperties.CanFocus = "Могу фокус";
            PropNameStrings.ControlProperties.TabStop = "Таб остановка";
            PropNameStrings.ControlProperties.CanSelect = "Могу выбрать";
        }        
    }
}
