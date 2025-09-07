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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

using Alternet.Common;
using Alternet.UI;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    //[ToolboxItem(false)]
    //public class FormattedLabel : Label
    //{
    //    private TextRegion[] textRegions;

    //    private Font boldFont;

    //    public override Size GetPreferredSize(Size proposedSize)
    //    {
    //        if (textRegions == null || boldFont == null)
    //            return Size.Empty;
    //        int width = 0;
    //        foreach (var region in textRegions)
    //            width += GetRegionWidth(region);
    //        int height = Math.Max(Font.Height, boldFont.Height);
    //        var textSize = new Size(width, height);

    //        return textSize + Padding.Size + DisplayScaling.AutoScale(new Size(4, 0));
    //    }

    //    protected override void OnFontChanged(EventArgs e)
    //    {
    //        base.OnFontChanged(e);

    //        Utilities.Dispose(ref boldFont);
    //        boldFont = new Font(Font, FontStyle.Bold);
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        base.Dispose(disposing);

    //        if (disposing)
    //            Utilities.Dispose(ref boldFont);
    //    }

    //    protected override void OnTextChanged(EventArgs e)
    //    {
    //        base.OnTextChanged(e);

    //        textRegions = ParseText(Text).ToArray();
    //        Invalidate();
    //    }

    //    protected override void OnPaint(PaintEventArgs e)
    //    {
    //        if (textRegions == null || boldFont == null)
    //            return;

    //        int x = Padding.Left;
    //        foreach (var region in textRegions)
    //        {
    //            TextRenderer.DrawText(
    //                e.Graphics,
    //                region.Text,
    //                GetRegionFont(region),
    //                new Point(x, Padding.Top),
    //                ForeColor);

    //            x += GetRegionWidth(region);
    //        }
    //    }

    //    private static IEnumerable<TextRegion> ParseText(string text)
    //    {
    //        const string Separator = "|";
    //        var processedText = text.Replace("<b>", Separator).Replace("</b>", Separator);
    //        var parts = processedText.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);

    //        bool bold = processedText.StartsWith(Separator);

    //        foreach (var part in parts)
    //        {
    //            yield return new TextRegion(part, bold);
    //            bold = !bold;
    //        }
    //    }

    //    private static Size MeasureText(string text, Font font)
    //    {
    //        var flags = TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix;
    //        var proposedSize = new Size(int.MaxValue, int.MaxValue);
    //        var size1 = TextRenderer.MeasureText(".", font, proposedSize, flags);
    //        var size2 = TextRenderer.MeasureText(text + ".", font, proposedSize, flags);
    //        return new Size(size2.Width - size1.Width, size2.Height);
    //    }

    //    private int GetRegionWidth(TextRegion region)
    //    {
    //        return MeasureText(region.Text, GetRegionFont(region)).Width;
    //    }

    //    private Font GetRegionFont(TextRegion region)
    //    {
    //        return region.IsBold ? boldFont : Font;
    //    }

    //    private class TextRegion
    //    {
    //        public TextRegion(string text, bool isBold)
    //        {
    //            Text = text;
    //            IsBold = isBold;
    //        }

    //        public string Text { get; private set; }

    //        public bool IsBold { get; private set; }
    //    }
    //}
}