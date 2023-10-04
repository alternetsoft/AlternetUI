using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI.Localization
{
    /// <summary>
    /// Defines localizations for system color names.
    /// </summary>
    public class KnownColorStrings
    {
        private string custom = "Custom";
        private string empty = "Empty";

        /// <summary>
        /// Current localizations for system color names.
        /// </summary>
        public static KnownColorStrings Default { get; set; } = new();

        /// <summary>
        /// Gets or sets localized system color name.
        /// </summary>
        public string ActiveBorder
        {
            get => GetLabel(KnownColor.ActiveBorder);
            set => SetLabel(KnownColor.ActiveBorder, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ActiveCaptionText
        {
            get => GetLabel(KnownColor.ActiveCaptionText);
            set => SetLabel(KnownColor.ActiveCaptionText, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ActiveCaption
        {
            get => GetLabel(KnownColor.ActiveCaption);
            set => SetLabel(KnownColor.ActiveCaption, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string AppWorkspace
        {
            get => GetLabel(KnownColor.AppWorkspace);
            set => SetLabel(KnownColor.AppWorkspace, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Control
        {
            get => GetLabel(KnownColor.Control);
            set => SetLabel(KnownColor.Control, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ControlDark
        {
            get => GetLabel(KnownColor.ControlDark);
            set => SetLabel(KnownColor.ControlDark, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ControlDarkDark
        {
            get => GetLabel(KnownColor.ControlDarkDark);
            set => SetLabel(KnownColor.ControlDarkDark, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ControlLight
        {
            get => GetLabel(KnownColor.ControlLight);
            set => SetLabel(KnownColor.ControlLight, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ControlLightLight
        {
            get => GetLabel(KnownColor.ControlLightLight);
            set => SetLabel(KnownColor.ControlLightLight, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ControlText
        {
            get => GetLabel(KnownColor.ControlText);
            set => SetLabel(KnownColor.ControlText, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Desktop
        {
            get => GetLabel(KnownColor.Desktop);
            set => SetLabel(KnownColor.Desktop, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string GrayText
        {
            get => GetLabel(KnownColor.GrayText);
            set => SetLabel(KnownColor.GrayText, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Highlight
        {
            get => GetLabel(KnownColor.Highlight);
            set => SetLabel(KnownColor.Highlight, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string HighlightText
        {
            get => GetLabel(KnownColor.HighlightText);
            set => SetLabel(KnownColor.HighlightText, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string HotTrack
        {
            get => GetLabel(KnownColor.HotTrack);
            set => SetLabel(KnownColor.HotTrack, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string InactiveBorder
        {
            get => GetLabel(KnownColor.InactiveBorder);
            set => SetLabel(KnownColor.InactiveBorder, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string InactiveCaption
        {
            get => GetLabel(KnownColor.InactiveCaption);
            set => SetLabel(KnownColor.InactiveCaption, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string InactiveCaptionText
        {
            get => GetLabel(KnownColor.InactiveCaptionText);
            set => SetLabel(KnownColor.InactiveCaptionText, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Info
        {
            get => GetLabel(KnownColor.Info);
            set => SetLabel(KnownColor.Info, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string InfoText
        {
            get => GetLabel(KnownColor.InfoText);
            set => SetLabel(KnownColor.InfoText, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Menu
        {
            get => GetLabel(KnownColor.Menu);
            set => SetLabel(KnownColor.Menu, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MenuText
        {
            get => GetLabel(KnownColor.MenuText);
            set => SetLabel(KnownColor.MenuText, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ScrollBar
        {
            get => GetLabel(KnownColor.ScrollBar);
            set => SetLabel(KnownColor.ScrollBar, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Window
        {
            get => GetLabel(KnownColor.Window);
            set => SetLabel(KnownColor.Window, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string WindowFrame
        {
            get => GetLabel(KnownColor.WindowFrame);
            set => SetLabel(KnownColor.WindowFrame, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string WindowText
        {
            get => GetLabel(KnownColor.WindowText);
            set => SetLabel(KnownColor.WindowText, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Transparent
        {
            get => GetLabel(KnownColor.Transparent);
            set => SetLabel(KnownColor.Transparent, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string AliceBlue
        {
            get => GetLabel(KnownColor.AliceBlue);
            set => SetLabel(KnownColor.AliceBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string AntiqueWhite
        {
            get => GetLabel(KnownColor.AntiqueWhite);
            set => SetLabel(KnownColor.AntiqueWhite, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Aqua
        {
            get => GetLabel(KnownColor.Aqua);
            set => SetLabel(KnownColor.Aqua, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Aquamarine
        {
            get => GetLabel(KnownColor.Aquamarine);
            set => SetLabel(KnownColor.Aquamarine, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Azure
        {
            get => GetLabel(KnownColor.Azure);
            set => SetLabel(KnownColor.Azure, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Beige
        {
            get => GetLabel(KnownColor.Beige);
            set => SetLabel(KnownColor.Beige, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Bisque
        {
            get => GetLabel(KnownColor.Bisque);
            set => SetLabel(KnownColor.Bisque, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Black
        {
            get => GetLabel(KnownColor.Black);
            set => SetLabel(KnownColor.Black, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string BlanchedAlmond
        {
            get => GetLabel(KnownColor.BlanchedAlmond);
            set => SetLabel(KnownColor.BlanchedAlmond, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Blue
        {
            get => GetLabel(KnownColor.Blue);
            set => SetLabel(KnownColor.Blue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string BlueViolet
        {
            get => GetLabel(KnownColor.BlueViolet);
            set => SetLabel(KnownColor.BlueViolet, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Brown
        {
            get => GetLabel(KnownColor.Brown);
            set => SetLabel(KnownColor.Brown, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string BurlyWood
        {
            get => GetLabel(KnownColor.BurlyWood);
            set => SetLabel(KnownColor.BurlyWood, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string CadetBlue
        {
            get => GetLabel(KnownColor.CadetBlue);
            set => SetLabel(KnownColor.CadetBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Chartreuse
        {
            get => GetLabel(KnownColor.Chartreuse);
            set => SetLabel(KnownColor.Chartreuse, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Chocolate
        {
            get => GetLabel(KnownColor.Chocolate);
            set => SetLabel(KnownColor.Chocolate, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Coral
        {
            get => GetLabel(KnownColor.Coral);
            set => SetLabel(KnownColor.Coral, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string CornflowerBlue
        {
            get => GetLabel(KnownColor.CornflowerBlue);
            set => SetLabel(KnownColor.CornflowerBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Cornsilk
        {
            get => GetLabel(KnownColor.Cornsilk);
            set => SetLabel(KnownColor.Cornsilk, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Crimson
        {
            get => GetLabel(KnownColor.Crimson);
            set => SetLabel(KnownColor.Crimson, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Cyan
        {
            get => GetLabel(KnownColor.Cyan);
            set => SetLabel(KnownColor.Cyan, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkBlue
        {
            get => GetLabel(KnownColor.DarkBlue);
            set => SetLabel(KnownColor.DarkBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkCyan
        {
            get => GetLabel(KnownColor.DarkCyan);
            set => SetLabel(KnownColor.DarkCyan, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkGoldenrod
        {
            get => GetLabel(KnownColor.DarkGoldenrod);
            set => SetLabel(KnownColor.DarkGoldenrod, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkGray
        {
            get => GetLabel(KnownColor.DarkGray);
            set => SetLabel(KnownColor.DarkGray, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkGreen
        {
            get => GetLabel(KnownColor.DarkGreen);
            set => SetLabel(KnownColor.DarkGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkKhaki
        {
            get => GetLabel(KnownColor.DarkKhaki);
            set => SetLabel(KnownColor.DarkKhaki, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkMagenta
        {
            get => GetLabel(KnownColor.DarkMagenta);
            set => SetLabel(KnownColor.DarkMagenta, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkOliveGreen
        {
            get => GetLabel(KnownColor.DarkOliveGreen);
            set => SetLabel(KnownColor.DarkOliveGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkOrange
        {
            get => GetLabel(KnownColor.DarkOrange);
            set => SetLabel(KnownColor.DarkOrange, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkOrchid
        {
            get => GetLabel(KnownColor.DarkOrchid);
            set => SetLabel(KnownColor.DarkOrchid, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkRed
        {
            get => GetLabel(KnownColor.DarkRed);
            set => SetLabel(KnownColor.DarkRed, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkSalmon
        {
            get => GetLabel(KnownColor.DarkSalmon);
            set => SetLabel(KnownColor.DarkSalmon, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkSeaGreen
        {
            get => GetLabel(KnownColor.DarkSeaGreen);
            set => SetLabel(KnownColor.DarkSeaGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkSlateBlue
        {
            get => GetLabel(KnownColor.DarkSlateBlue);
            set => SetLabel(KnownColor.DarkSlateBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkSlateGray
        {
            get => GetLabel(KnownColor.DarkSlateGray);
            set => SetLabel(KnownColor.DarkSlateGray, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkTurquoise
        {
            get => GetLabel(KnownColor.DarkTurquoise);
            set => SetLabel(KnownColor.DarkTurquoise, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DarkViolet
        {
            get => GetLabel(KnownColor.DarkViolet);
            set => SetLabel(KnownColor.DarkViolet, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DeepPink
        {
            get => GetLabel(KnownColor.DeepPink);
            set => SetLabel(KnownColor.DeepPink, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DeepSkyBlue
        {
            get => GetLabel(KnownColor.DeepSkyBlue);
            set => SetLabel(KnownColor.DeepSkyBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DimGray
        {
            get => GetLabel(KnownColor.DimGray);
            set => SetLabel(KnownColor.DimGray, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string DodgerBlue
        {
            get => GetLabel(KnownColor.DodgerBlue);
            set => SetLabel(KnownColor.DodgerBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Firebrick
        {
            get => GetLabel(KnownColor.Firebrick);
            set => SetLabel(KnownColor.Firebrick, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string FloralWhite
        {
            get => GetLabel(KnownColor.FloralWhite);
            set => SetLabel(KnownColor.FloralWhite, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ForestGreen
        {
            get => GetLabel(KnownColor.ForestGreen);
            set => SetLabel(KnownColor.ForestGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Fuchsia
        {
            get => GetLabel(KnownColor.Fuchsia);
            set => SetLabel(KnownColor.Fuchsia, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Gainsboro
        {
            get => GetLabel(KnownColor.Gainsboro);
            set => SetLabel(KnownColor.Gainsboro, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string GhostWhite
        {
            get => GetLabel(KnownColor.GhostWhite);
            set => SetLabel(KnownColor.GhostWhite, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Gold
        {
            get => GetLabel(KnownColor.Gold);
            set => SetLabel(KnownColor.Gold, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Goldenrod
        {
            get => GetLabel(KnownColor.Goldenrod);
            set => SetLabel(KnownColor.Goldenrod, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Gray
        {
            get => GetLabel(KnownColor.Gray);
            set => SetLabel(KnownColor.Gray, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Green
        {
            get => GetLabel(KnownColor.Green);
            set => SetLabel(KnownColor.Green, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string GreenYellow
        {
            get => GetLabel(KnownColor.GreenYellow);
            set => SetLabel(KnownColor.GreenYellow, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Honeydew
        {
            get => GetLabel(KnownColor.Honeydew);
            set => SetLabel(KnownColor.Honeydew, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string HotPink
        {
            get => GetLabel(KnownColor.HotPink);
            set => SetLabel(KnownColor.HotPink, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string IndianRed
        {
            get => GetLabel(KnownColor.IndianRed);
            set => SetLabel(KnownColor.IndianRed, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Indigo
        {
            get => GetLabel(KnownColor.Indigo);
            set => SetLabel(KnownColor.Indigo, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Ivory
        {
            get => GetLabel(KnownColor.Ivory);
            set => SetLabel(KnownColor.Ivory, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Khaki
        {
            get => GetLabel(KnownColor.Khaki);
            set => SetLabel(KnownColor.Khaki, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Lavender
        {
            get => GetLabel(KnownColor.Lavender);
            set => SetLabel(KnownColor.Lavender, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LavenderBlush
        {
            get => GetLabel(KnownColor.LavenderBlush);
            set => SetLabel(KnownColor.LavenderBlush, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LawnGreen
        {
            get => GetLabel(KnownColor.LawnGreen);
            set => SetLabel(KnownColor.LawnGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LemonChiffon
        {
            get => GetLabel(KnownColor.LemonChiffon);
            set => SetLabel(KnownColor.LemonChiffon, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightBlue
        {
            get => GetLabel(KnownColor.LightBlue);
            set => SetLabel(KnownColor.LightBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightCoral
        {
            get => GetLabel(KnownColor.LightCoral);
            set => SetLabel(KnownColor.LightCoral, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightCyan
        {
            get => GetLabel(KnownColor.LightCyan);
            set => SetLabel(KnownColor.LightCyan, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightGoldenrodYellow
        {
            get => GetLabel(KnownColor.LightGoldenrodYellow);
            set => SetLabel(KnownColor.LightGoldenrodYellow, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightGray
        {
            get => GetLabel(KnownColor.LightGray);
            set => SetLabel(KnownColor.LightGray, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightGreen
        {
            get => GetLabel(KnownColor.LightGreen);
            set => SetLabel(KnownColor.LightGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightPink
        {
            get => GetLabel(KnownColor.LightPink);
            set => SetLabel(KnownColor.LightPink, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightSalmon
        {
            get => GetLabel(KnownColor.LightSalmon);
            set => SetLabel(KnownColor.LightSalmon, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightSeaGreen
        {
            get => GetLabel(KnownColor.LightSeaGreen);
            set => SetLabel(KnownColor.LightSeaGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightSkyBlue
        {
            get => GetLabel(KnownColor.LightSkyBlue);
            set => SetLabel(KnownColor.LightSkyBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightSlateGray
        {
            get => GetLabel(KnownColor.LightSlateGray);
            set => SetLabel(KnownColor.LightSlateGray, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightSteelBlue
        {
            get => GetLabel(KnownColor.LightSteelBlue);
            set => SetLabel(KnownColor.LightSteelBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LightYellow
        {
            get => GetLabel(KnownColor.LightYellow);
            set => SetLabel(KnownColor.LightYellow, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Lime
        {
            get => GetLabel(KnownColor.Lime);
            set => SetLabel(KnownColor.Lime, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string LimeGreen
        {
            get => GetLabel(KnownColor.LimeGreen);
            set => SetLabel(KnownColor.LimeGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Linen
        {
            get => GetLabel(KnownColor.Linen);
            set => SetLabel(KnownColor.Linen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Magenta
        {
            get => GetLabel(KnownColor.Magenta);
            set => SetLabel(KnownColor.Magenta, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Maroon
        {
            get => GetLabel(KnownColor.Maroon);
            set => SetLabel(KnownColor.Maroon, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MediumAquamarine
        {
            get => GetLabel(KnownColor.MediumAquamarine);
            set => SetLabel(KnownColor.MediumAquamarine, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MediumBlue
        {
            get => GetLabel(KnownColor.MediumBlue);
            set => SetLabel(KnownColor.MediumBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MediumOrchid
        {
            get => GetLabel(KnownColor.MediumOrchid);
            set => SetLabel(KnownColor.MediumOrchid, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MediumPurple
        {
            get => GetLabel(KnownColor.MediumPurple);
            set => SetLabel(KnownColor.MediumPurple, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MediumSeaGreen
        {
            get => GetLabel(KnownColor.MediumSeaGreen);
            set => SetLabel(KnownColor.MediumSeaGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MediumSlateBlue
        {
            get => GetLabel(KnownColor.MediumSlateBlue);
            set => SetLabel(KnownColor.MediumSlateBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MediumSpringGreen
        {
            get => GetLabel(KnownColor.MediumSpringGreen);
            set => SetLabel(KnownColor.MediumSpringGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MediumTurquoise
        {
            get => GetLabel(KnownColor.MediumTurquoise);
            set => SetLabel(KnownColor.MediumTurquoise, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MediumVioletRed
        {
            get => GetLabel(KnownColor.MediumVioletRed);
            set => SetLabel(KnownColor.MediumVioletRed, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MidnightBlue
        {
            get => GetLabel(KnownColor.MidnightBlue);
            set => SetLabel(KnownColor.MidnightBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MintCream
        {
            get => GetLabel(KnownColor.MintCream);
            set => SetLabel(KnownColor.MintCream, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MistyRose
        {
            get => GetLabel(KnownColor.MistyRose);
            set => SetLabel(KnownColor.MistyRose, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Moccasin
        {
            get => GetLabel(KnownColor.Moccasin);
            set => SetLabel(KnownColor.Moccasin, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string NavajoWhite
        {
            get => GetLabel(KnownColor.NavajoWhite);
            set => SetLabel(KnownColor.NavajoWhite, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Navy
        {
            get => GetLabel(KnownColor.Navy);
            set => SetLabel(KnownColor.Navy, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string OldLace
        {
            get => GetLabel(KnownColor.OldLace);
            set => SetLabel(KnownColor.OldLace, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Olive
        {
            get => GetLabel(KnownColor.Olive);
            set => SetLabel(KnownColor.Olive, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string OliveDrab
        {
            get => GetLabel(KnownColor.OliveDrab);
            set => SetLabel(KnownColor.OliveDrab, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Orange
        {
            get => GetLabel(KnownColor.Orange);
            set => SetLabel(KnownColor.Orange, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string OrangeRed
        {
            get => GetLabel(KnownColor.OrangeRed);
            set => SetLabel(KnownColor.OrangeRed, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Orchid
        {
            get => GetLabel(KnownColor.Orchid);
            set => SetLabel(KnownColor.Orchid, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string PaleGoldenrod
        {
            get => GetLabel(KnownColor.PaleGoldenrod);
            set => SetLabel(KnownColor.PaleGoldenrod, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string PaleGreen
        {
            get => GetLabel(KnownColor.PaleGreen);
            set => SetLabel(KnownColor.PaleGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string PaleTurquoise
        {
            get => GetLabel(KnownColor.PaleTurquoise);
            set => SetLabel(KnownColor.PaleTurquoise, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string PaleVioletRed
        {
            get => GetLabel(KnownColor.PaleVioletRed);
            set => SetLabel(KnownColor.PaleVioletRed, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string PapayaWhip
        {
            get => GetLabel(KnownColor.PapayaWhip);
            set => SetLabel(KnownColor.PapayaWhip, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string PeachPuff
        {
            get => GetLabel(KnownColor.PeachPuff);
            set => SetLabel(KnownColor.PeachPuff, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Peru
        {
            get => GetLabel(KnownColor.Peru);
            set => SetLabel(KnownColor.Peru, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Pink
        {
            get => GetLabel(KnownColor.Pink);
            set => SetLabel(KnownColor.Pink, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Plum
        {
            get => GetLabel(KnownColor.Plum);
            set => SetLabel(KnownColor.Plum, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string PowderBlue
        {
            get => GetLabel(KnownColor.PowderBlue);
            set => SetLabel(KnownColor.PowderBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Purple
        {
            get => GetLabel(KnownColor.Purple);
            set => SetLabel(KnownColor.Purple, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Red
        {
            get => GetLabel(KnownColor.Red);
            set => SetLabel(KnownColor.Red, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string RosyBrown
        {
            get => GetLabel(KnownColor.RosyBrown);
            set => SetLabel(KnownColor.RosyBrown, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string RoyalBlue
        {
            get => GetLabel(KnownColor.RoyalBlue);
            set => SetLabel(KnownColor.RoyalBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string SaddleBrown
        {
            get => GetLabel(KnownColor.SaddleBrown);
            set => SetLabel(KnownColor.SaddleBrown, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Salmon
        {
            get => GetLabel(KnownColor.Salmon);
            set => SetLabel(KnownColor.Salmon, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string SandyBrown
        {
            get => GetLabel(KnownColor.SandyBrown);
            set => SetLabel(KnownColor.SandyBrown, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string SeaGreen
        {
            get => GetLabel(KnownColor.SeaGreen);
            set => SetLabel(KnownColor.SeaGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string SeaShell
        {
            get => GetLabel(KnownColor.SeaShell);
            set => SetLabel(KnownColor.SeaShell, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Sienna
        {
            get => GetLabel(KnownColor.Sienna);
            set => SetLabel(KnownColor.Sienna, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Silver
        {
            get => GetLabel(KnownColor.Silver);
            set => SetLabel(KnownColor.Silver, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string SkyBlue
        {
            get => GetLabel(KnownColor.SkyBlue);
            set => SetLabel(KnownColor.SkyBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string SlateBlue
        {
            get => GetLabel(KnownColor.SlateBlue);
            set => SetLabel(KnownColor.SlateBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string SlateGray
        {
            get => GetLabel(KnownColor.SlateGray);
            set => SetLabel(KnownColor.SlateGray, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Snow
        {
            get => GetLabel(KnownColor.Snow);
            set => SetLabel(KnownColor.Snow, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string SpringGreen
        {
            get => GetLabel(KnownColor.SpringGreen);
            set => SetLabel(KnownColor.SpringGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string SteelBlue
        {
            get => GetLabel(KnownColor.SteelBlue);
            set => SetLabel(KnownColor.SteelBlue, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Tan
        {
            get => GetLabel(KnownColor.Tan);
            set => SetLabel(KnownColor.Tan, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Teal
        {
            get => GetLabel(KnownColor.Teal);
            set => SetLabel(KnownColor.Teal, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Thistle
        {
            get => GetLabel(KnownColor.Thistle);
            set => SetLabel(KnownColor.Thistle, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Tomato
        {
            get => GetLabel(KnownColor.Tomato);
            set => SetLabel(KnownColor.Tomato, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Turquoise
        {
            get => GetLabel(KnownColor.Turquoise);
            set => SetLabel(KnownColor.Turquoise, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Violet
        {
            get => GetLabel(KnownColor.Violet);
            set => SetLabel(KnownColor.Violet, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Wheat
        {
            get => GetLabel(KnownColor.Wheat);
            set => SetLabel(KnownColor.Wheat, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string White
        {
            get => GetLabel(KnownColor.White);
            set => SetLabel(KnownColor.White, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string WhiteSmoke
        {
            get => GetLabel(KnownColor.WhiteSmoke);
            set => SetLabel(KnownColor.WhiteSmoke, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string Yellow
        {
            get => GetLabel(KnownColor.Yellow);
            set => SetLabel(KnownColor.Yellow, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string YellowGreen
        {
            get => GetLabel(KnownColor.YellowGreen);
            set => SetLabel(KnownColor.YellowGreen, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ButtonFace
        {
            get => GetLabel(KnownColor.ButtonFace);
            set => SetLabel(KnownColor.ButtonFace, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ButtonHighlight
        {
            get => GetLabel(KnownColor.ButtonHighlight);
            set => SetLabel(KnownColor.ButtonHighlight, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string ButtonShadow
        {
            get => GetLabel(KnownColor.ButtonShadow);
            set => SetLabel(KnownColor.ButtonShadow, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string GradientActiveCaption
        {
            get => GetLabel(KnownColor.GradientActiveCaption);
            set => SetLabel(KnownColor.GradientActiveCaption, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string GradientInactiveCaption
        {
            get => GetLabel(KnownColor.GradientInactiveCaption);
            set => SetLabel(KnownColor.GradientInactiveCaption, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MenuBar
        {
            get => GetLabel(KnownColor.MenuBar);
            set => SetLabel(KnownColor.MenuBar, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string MenuHighlight
        {
            get => GetLabel(KnownColor.MenuHighlight);
            set => SetLabel(KnownColor.MenuHighlight, value);
        }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>
        /// </summary>
        public string RebeccaPurple
        {
            get => GetLabel(KnownColor.RebeccaPurple);
            set => SetLabel(KnownColor.RebeccaPurple, value);
        }

        /// <summary>
        /// Gets or sets localized "Custom" color name.
        /// </summary>
        /// <remarks>
        /// When "Custom" color is selected in the color editor
        /// (for example in <see cref="PropertyGrid"/> control),
        /// <see cref="ColorDialog"/> is shown for the user.
        /// </remarks>
        public string Custom
        {
            get
            {
                return custom;
            }

            set
            {
                if (custom == value)
                    return;
                custom = value;
                Native.PropertyGrid.KnownColorsSetCustomColorTitle(custom);
            }
        }

        /// <summary>
        /// Gets or sets localized "Empty" color name.
        /// </summary>
        public string Empty
        {
            get
            {
                return empty;
            }

            set
            {
                if (empty == value)
                    return;
                empty = value;
            }
        }

        /// <summary>
        /// Gets localized color name for <see cref="KnownColor"/>.
        /// </summary>
        /// <param name="color">Known color.</param>
        public virtual string GetLabel(KnownColor color)
        {
            return ColorUtils.GetColorInfo(color).LabelLocalized;
        }

        /// <summary>
        /// Sets localized color name for <see cref="KnownColor"/>.
        /// </summary>
        /// <param name="color">Known color.</param>
        /// <param name="label">Localized color name.</param>
        public virtual void SetLabel(KnownColor color, string label)
        {
            ColorUtils.SetLabelLocalized(color, label);
        }
    }
}
