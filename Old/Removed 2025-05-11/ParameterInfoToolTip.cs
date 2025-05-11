#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Data;

using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    public class ParameterInfoToolTip : IParameterInfoToolTip
    {
        private TemplateControl controlTemplate = new()
        {
            Visible = false,
        };

        private ParameterInfoToolTipConfiguration configuration;

        public ParameterInfoToolTip(ParameterInfoToolTipConfiguration configuration)
            : base()
        {
            this.configuration = configuration;
        }

        public void ShowPopup(ParameterInfoSymbol data, PointD position)
        {
            //if (string.IsNullOrEmpty(data.ParameterDescription))
            //{
            //    controlTemplate = TemplateUtils.CreateTemplateWithBoldText(
            //        data.DisplayText,
            //        string.Empty,
            //        string.Empty,
            //        new FontAndColor(Color.Black, Color.LightGoldenrodYellow, Font.Default.WithSize(12)));
            //}
            //else
            //{
            //    controlTemplate = TemplateUtils.CreateTemplateWithBoldText(
            //        data.DisplayText,
            //        data.ParameterDescription,
            //        string.Empty,
            //        new FontAndColor(Color.Black, Color.LightGoldenrodYellow, Font.Default.WithSize(12)));
            //}
            //controlTemplate.SetSizeToContent(WindowSizeToContentMode.WidthAndHeight);
            var toolTip = configuration.Provider.Get(configuration.OwnerControl);
            //toolTip.ShowToolTipFromTemplate(controlTemplate, null, position);
            var image = DrawingUtils.ImageFromTextWithBoldTag(
    data.DisplayText,
    Display.MaxScaleFactor,
    Control.DefaultFont,
    Color.Black);
            ImageSet imageSet = new(image);
            toolTip?.OnlyImage(imageSet);
            toolTip?.ShowToolTip(position);
        }

        public bool Visible { get; set; }

        public SizeD GetPreferredSize()
        {
            // ???
            return SizeD.Empty;// content.GetPreferredSize(Size.Empty);
        }
        public void Close()
        {
        }

        private void OnApplicationDeactivated()
        {
            if (Visible)
                Close();
        }
    }
}