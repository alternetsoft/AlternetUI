using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Globalization;
using System.Text;

namespace Alternet.Drawing
{
	/// <summary>Represents an ARGB (alpha, red, green, blue) color.</summary>
	//[TypeConverter(typeof(ColorConverter))]
	[DebuggerDisplay("{NameAndARGBValue}")]
	//[Editor("System.Drawing.Design.ColorEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Serializable]
	public struct Color
	{
		/// <summary>Gets a system-defined color.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Transparent
		{
			get
			{
				return new Color(KnownColor.Transparent);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF0F8FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color AliceBlue
		{
			get
			{
				return new Color(KnownColor.AliceBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFAEBD7.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color AntiqueWhite
		{
			get
			{
				return new Color(KnownColor.AntiqueWhite);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00FFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Aqua
		{
			get
			{
				return new Color(KnownColor.Aqua);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF7FFFD4.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Aquamarine
		{
			get
			{
				return new Color(KnownColor.Aquamarine);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF0FFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Azure
		{
			get
			{
				return new Color(KnownColor.Azure);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF5F5DC.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Beige
		{
			get
			{
				return new Color(KnownColor.Beige);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFE4C4.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Bisque
		{
			get
			{
				return new Color(KnownColor.Bisque);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF000000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Black
		{
			get
			{
				return new Color(KnownColor.Black);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFEBCD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color BlanchedAlmond
		{
			get
			{
				return new Color(KnownColor.BlanchedAlmond);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF0000FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Blue
		{
			get
			{
				return new Color(KnownColor.Blue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF8A2BE2.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color BlueViolet
		{
			get
			{
				return new Color(KnownColor.BlueViolet);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFA52A2A.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Brown
		{
			get
			{
				return new Color(KnownColor.Brown);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDEB887.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color BurlyWood
		{
			get
			{
				return new Color(KnownColor.BurlyWood);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF5F9EA0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color CadetBlue
		{
			get
			{
				return new Color(KnownColor.CadetBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF7FFF00.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Chartreuse
		{
			get
			{
				return new Color(KnownColor.Chartreuse);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFD2691E.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Chocolate
		{
			get
			{
				return new Color(KnownColor.Chocolate);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF7F50.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Coral
		{
			get
			{
				return new Color(KnownColor.Coral);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF6495ED.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color CornflowerBlue
		{
			get
			{
				return new Color(KnownColor.CornflowerBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFF8DC.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Cornsilk
		{
			get
			{
				return new Color(KnownColor.Cornsilk);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDC143C.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Crimson
		{
			get
			{
				return new Color(KnownColor.Crimson);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00FFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Cyan
		{
			get
			{
				return new Color(KnownColor.Cyan);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00008B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkBlue
		{
			get
			{
				return new Color(KnownColor.DarkBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF008B8B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkCyan
		{
			get
			{
				return new Color(KnownColor.DarkCyan);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFB8860B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkGoldenrod
		{
			get
			{
				return new Color(KnownColor.DarkGoldenrod);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFA9A9A9.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkGray
		{
			get
			{
				return new Color(KnownColor.DarkGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF006400.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkGreen
		{
			get
			{
				return new Color(KnownColor.DarkGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFBDB76B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkKhaki
		{
			get
			{
				return new Color(KnownColor.DarkKhaki);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF8B008B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkMagenta
		{
			get
			{
				return new Color(KnownColor.DarkMagenta);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF556B2F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkOliveGreen
		{
			get
			{
				return new Color(KnownColor.DarkOliveGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF8C00.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkOrange
		{
			get
			{
				return new Color(KnownColor.DarkOrange);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF9932CC.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkOrchid
		{
			get
			{
				return new Color(KnownColor.DarkOrchid);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF8B0000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkRed
		{
			get
			{
				return new Color(KnownColor.DarkRed);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFE9967A.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkSalmon
		{
			get
			{
				return new Color(KnownColor.DarkSalmon);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF8FBC8F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkSeaGreen
		{
			get
			{
				return new Color(KnownColor.DarkSeaGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF483D8B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkSlateBlue
		{
			get
			{
				return new Color(KnownColor.DarkSlateBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF2F4F4F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkSlateGray
		{
			get
			{
				return new Color(KnownColor.DarkSlateGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00CED1.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkTurquoise
		{
			get
			{
				return new Color(KnownColor.DarkTurquoise);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF9400D3.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DarkViolet
		{
			get
			{
				return new Color(KnownColor.DarkViolet);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF1493.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DeepPink
		{
			get
			{
				return new Color(KnownColor.DeepPink);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00BFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DeepSkyBlue
		{
			get
			{
				return new Color(KnownColor.DeepSkyBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF696969.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DimGray
		{
			get
			{
				return new Color(KnownColor.DimGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF1E90FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color DodgerBlue
		{
			get
			{
				return new Color(KnownColor.DodgerBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFB22222.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Firebrick
		{
			get
			{
				return new Color(KnownColor.Firebrick);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFAF0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color FloralWhite
		{
			get
			{
				return new Color(KnownColor.FloralWhite);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF228B22.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color ForestGreen
		{
			get
			{
				return new Color(KnownColor.ForestGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF00FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Fuchsia
		{
			get
			{
				return new Color(KnownColor.Fuchsia);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDCDCDC.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Gainsboro
		{
			get
			{
				return new Color(KnownColor.Gainsboro);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF8F8FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color GhostWhite
		{
			get
			{
				return new Color(KnownColor.GhostWhite);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFD700.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Gold
		{
			get
			{
				return new Color(KnownColor.Gold);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDAA520.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Goldenrod
		{
			get
			{
				return new Color(KnownColor.Goldenrod);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF808080.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> strcture representing a system-defined color.</returns>
		public static Color Gray
		{
			get
			{
				return new Color(KnownColor.Gray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF008000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Green
		{
			get
			{
				return new Color(KnownColor.Green);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFADFF2F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color GreenYellow
		{
			get
			{
				return new Color(KnownColor.GreenYellow);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF0FFF0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Honeydew
		{
			get
			{
				return new Color(KnownColor.Honeydew);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF69B4.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color HotPink
		{
			get
			{
				return new Color(KnownColor.HotPink);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFCD5C5C.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color IndianRed
		{
			get
			{
				return new Color(KnownColor.IndianRed);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF4B0082.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Indigo
		{
			get
			{
				return new Color(KnownColor.Indigo);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFFF0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Ivory
		{
			get
			{
				return new Color(KnownColor.Ivory);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF0E68C.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Khaki
		{
			get
			{
				return new Color(KnownColor.Khaki);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFE6E6FA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Lavender
		{
			get
			{
				return new Color(KnownColor.Lavender);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFF0F5.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LavenderBlush
		{
			get
			{
				return new Color(KnownColor.LavenderBlush);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF7CFC00.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LawnGreen
		{
			get
			{
				return new Color(KnownColor.LawnGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFACD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LemonChiffon
		{
			get
			{
				return new Color(KnownColor.LemonChiffon);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFADD8E6.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightBlue
		{
			get
			{
				return new Color(KnownColor.LightBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF08080.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightCoral
		{
			get
			{
				return new Color(KnownColor.LightCoral);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFE0FFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightCyan
		{
			get
			{
				return new Color(KnownColor.LightCyan);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFAFAD2.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightGoldenrodYellow
		{
			get
			{
				return new Color(KnownColor.LightGoldenrodYellow);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF90EE90.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightGreen
		{
			get
			{
				return new Color(KnownColor.LightGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFD3D3D3.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightGray
		{
			get
			{
				return new Color(KnownColor.LightGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFB6C1.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightPink
		{
			get
			{
				return new Color(KnownColor.LightPink);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFA07A.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightSalmon
		{
			get
			{
				return new Color(KnownColor.LightSalmon);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF20B2AA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightSeaGreen
		{
			get
			{
				return new Color(KnownColor.LightSeaGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF87CEFA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightSkyBlue
		{
			get
			{
				return new Color(KnownColor.LightSkyBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF778899.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightSlateGray
		{
			get
			{
				return new Color(KnownColor.LightSlateGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFB0C4DE.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightSteelBlue
		{
			get
			{
				return new Color(KnownColor.LightSteelBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFFE0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LightYellow
		{
			get
			{
				return new Color(KnownColor.LightYellow);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00FF00.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Lime
		{
			get
			{
				return new Color(KnownColor.Lime);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF32CD32.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color LimeGreen
		{
			get
			{
				return new Color(KnownColor.LimeGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFAF0E6.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Linen
		{
			get
			{
				return new Color(KnownColor.Linen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF00FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Magenta
		{
			get
			{
				return new Color(KnownColor.Magenta);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF800000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Maroon
		{
			get
			{
				return new Color(KnownColor.Maroon);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF66CDAA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MediumAquamarine
		{
			get
			{
				return new Color(KnownColor.MediumAquamarine);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF0000CD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MediumBlue
		{
			get
			{
				return new Color(KnownColor.MediumBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFBA55D3.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MediumOrchid
		{
			get
			{
				return new Color(KnownColor.MediumOrchid);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF9370DB.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MediumPurple
		{
			get
			{
				return new Color(KnownColor.MediumPurple);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF3CB371.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MediumSeaGreen
		{
			get
			{
				return new Color(KnownColor.MediumSeaGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF7B68EE.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MediumSlateBlue
		{
			get
			{
				return new Color(KnownColor.MediumSlateBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00FA9A.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MediumSpringGreen
		{
			get
			{
				return new Color(KnownColor.MediumSpringGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF48D1CC.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MediumTurquoise
		{
			get
			{
				return new Color(KnownColor.MediumTurquoise);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFC71585.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MediumVioletRed
		{
			get
			{
				return new Color(KnownColor.MediumVioletRed);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF191970.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MidnightBlue
		{
			get
			{
				return new Color(KnownColor.MidnightBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF5FFFA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MintCream
		{
			get
			{
				return new Color(KnownColor.MintCream);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFE4E1.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color MistyRose
		{
			get
			{
				return new Color(KnownColor.MistyRose);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFE4B5.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Moccasin
		{
			get
			{
				return new Color(KnownColor.Moccasin);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFDEAD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color NavajoWhite
		{
			get
			{
				return new Color(KnownColor.NavajoWhite);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF000080.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Navy
		{
			get
			{
				return new Color(KnownColor.Navy);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFDF5E6.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color OldLace
		{
			get
			{
				return new Color(KnownColor.OldLace);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF808000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Olive
		{
			get
			{
				return new Color(KnownColor.Olive);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF6B8E23.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color OliveDrab
		{
			get
			{
				return new Color(KnownColor.OliveDrab);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFA500.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Orange
		{
			get
			{
				return new Color(KnownColor.Orange);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF4500.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color OrangeRed
		{
			get
			{
				return new Color(KnownColor.OrangeRed);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDA70D6.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Orchid
		{
			get
			{
				return new Color(KnownColor.Orchid);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFEEE8AA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color PaleGoldenrod
		{
			get
			{
				return new Color(KnownColor.PaleGoldenrod);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF98FB98.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color PaleGreen
		{
			get
			{
				return new Color(KnownColor.PaleGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFAFEEEE.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color PaleTurquoise
		{
			get
			{
				return new Color(KnownColor.PaleTurquoise);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDB7093.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color PaleVioletRed
		{
			get
			{
				return new Color(KnownColor.PaleVioletRed);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFEFD5.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color PapayaWhip
		{
			get
			{
				return new Color(KnownColor.PapayaWhip);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFDAB9.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color PeachPuff
		{
			get
			{
				return new Color(KnownColor.PeachPuff);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFCD853F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Peru
		{
			get
			{
				return new Color(KnownColor.Peru);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFC0CB.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Pink
		{
			get
			{
				return new Color(KnownColor.Pink);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDDA0DD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Plum
		{
			get
			{
				return new Color(KnownColor.Plum);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFB0E0E6.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color PowderBlue
		{
			get
			{
				return new Color(KnownColor.PowderBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF800080.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Purple
		{
			get
			{
				return new Color(KnownColor.Purple);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF0000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Red
		{
			get
			{
				return new Color(KnownColor.Red);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFBC8F8F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color RosyBrown
		{
			get
			{
				return new Color(KnownColor.RosyBrown);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF4169E1.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color RoyalBlue
		{
			get
			{
				return new Color(KnownColor.RoyalBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF8B4513.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color SaddleBrown
		{
			get
			{
				return new Color(KnownColor.SaddleBrown);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFA8072.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Salmon
		{
			get
			{
				return new Color(KnownColor.Salmon);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF4A460.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color SandyBrown
		{
			get
			{
				return new Color(KnownColor.SandyBrown);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF2E8B57.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color SeaGreen
		{
			get
			{
				return new Color(KnownColor.SeaGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFF5EE.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color SeaShell
		{
			get
			{
				return new Color(KnownColor.SeaShell);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFA0522D.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Sienna
		{
			get
			{
				return new Color(KnownColor.Sienna);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFC0C0C0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Silver
		{
			get
			{
				return new Color(KnownColor.Silver);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF87CEEB.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color SkyBlue
		{
			get
			{
				return new Color(KnownColor.SkyBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF6A5ACD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color SlateBlue
		{
			get
			{
				return new Color(KnownColor.SlateBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF708090.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color SlateGray
		{
			get
			{
				return new Color(KnownColor.SlateGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFAFA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Snow
		{
			get
			{
				return new Color(KnownColor.Snow);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00FF7F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color SpringGreen
		{
			get
			{
				return new Color(KnownColor.SpringGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF4682B4.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color SteelBlue
		{
			get
			{
				return new Color(KnownColor.SteelBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFD2B48C.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Tan
		{
			get
			{
				return new Color(KnownColor.Tan);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF008080.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Teal
		{
			get
			{
				return new Color(KnownColor.Teal);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFD8BFD8.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Thistle
		{
			get
			{
				return new Color(KnownColor.Thistle);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF6347.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Tomato
		{
			get
			{
				return new Color(KnownColor.Tomato);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF40E0D0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Turquoise
		{
			get
			{
				return new Color(KnownColor.Turquoise);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFEE82EE.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Violet
		{
			get
			{
				return new Color(KnownColor.Violet);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF5DEB3.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Wheat
		{
			get
			{
				return new Color(KnownColor.Wheat);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color White
		{
			get
			{
				return new Color(KnownColor.White);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF5F5F5.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color WhiteSmoke
		{
			get
			{
				return new Color(KnownColor.WhiteSmoke);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFF00.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color Yellow
		{
			get
			{
				return new Color(KnownColor.Yellow);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF9ACD32.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		public static Color YellowGreen
		{
			get
			{
				return new Color(KnownColor.YellowGreen);
			}
		}

		internal Color(KnownColor knownColor)
		{
			this.value = 0L;
			this.state = Color.StateKnownColorValid;
			this.name = null;
			this.knownColor = (short)knownColor;
		}

		private Color(long value, short state, string? name, KnownColor knownColor)
		{
			this.value = value;
			this.state = state;
			this.name = name;
			this.knownColor = (short)knownColor;
		}

		/// <summary>Gets the red component value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The red component value of this <see cref="T:System.Drawing.Color" />.</returns>
		public byte R
		{
			get
			{
				return (byte)(this.Value >> 16 & 255L);
			}
		}

		/// <summary>Gets the green component value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The green component value of this <see cref="T:System.Drawing.Color" />.</returns>
		public byte G
		{
			get
			{
				return (byte)(this.Value >> 8 & 255L);
			}
		}

		/// <summary>Gets the blue component value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The blue component value of this <see cref="T:System.Drawing.Color" />.</returns>
		public byte B
		{
			get
			{
				return (byte)(this.Value & 255L);
			}
		}

		/// <summary>Gets the alpha component value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The alpha component value of this <see cref="T:System.Drawing.Color" />.</returns>
		public byte A
		{
			get
			{
				return (byte)(this.Value >> 24 & 255L);
			}
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Color" /> structure is a predefined color. Predefined colors are represented by the elements of the <see cref="T:System.Drawing.KnownColor" /> enumeration.</summary>
		/// <returns>
		///     <see langword="true" /> if this <see cref="T:System.Drawing.Color" /> was created from a predefined color by using either the <see cref="M:System.Drawing.Color.FromName(System.String)" /> method or the <see cref="M:System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor)" /> method; otherwise, <see langword="false" />.</returns>
		public bool IsKnownColor
		{
			get
			{
				return (this.state & Color.StateKnownColorValid) != 0;
			}
		}

		/// <summary>Specifies whether this <see cref="T:System.Drawing.Color" /> structure is uninitialized.</summary>
		/// <returns>This property returns <see langword="true" /> if this color is uninitialized; otherwise, <see langword="false" />.</returns>
		public bool IsEmpty
		{
			get
			{
				return this.state == 0;
			}
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Color" /> structure is a named color or a member of the <see cref="T:System.Drawing.KnownColor" /> enumeration.</summary>
		/// <returns>
		///     <see langword="true" /> if this <see cref="T:System.Drawing.Color" /> was created by using either the <see cref="M:System.Drawing.Color.FromName(System.String)" /> method or the <see cref="M:System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor)" /> method; otherwise, <see langword="false" />.</returns>
		public bool IsNamedColor
		{
			get
			{
				return (this.state & Color.StateNameValid) != 0 || this.IsKnownColor;
			}
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Color" /> structure is a system color. A system color is a color that is used in a Windows display element. System colors are represented by elements of the <see cref="T:System.Drawing.KnownColor" /> enumeration.</summary>
		/// <returns>
		///     <see langword="true" /> if this <see cref="T:System.Drawing.Color" /> was created from a system color by using either the <see cref="M:System.Drawing.Color.FromName(System.String)" /> method or the <see cref="M:System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor)" /> method; otherwise, <see langword="false" />.</returns>
		public bool IsSystemColor
		{
			get
			{
				return this.IsKnownColor && (this.knownColor <= 26 || this.knownColor > 167);
			}
		}

		private string NameAndARGBValue
		{
			get
			{
				return string.Format(CultureInfo.CurrentCulture, "{{Name={0}, ARGB=({1}, {2}, {3}, {4})}}", new object[]
				{
					this.Name,
					this.A,
					this.R,
					this.G,
					this.B
				});
			}
		}

		/// <summary>Gets the name of this <see cref="T:System.Drawing.Color" />.</summary>
		/// <returns>The name of this <see cref="T:System.Drawing.Color" />.</returns>
		public string Name
		{
			get
			{
				if ((this.state & Color.StateNameValid) != 0)
				{
					return this.name ?? throw new InvalidOperationException();
				}
				if (!this.IsKnownColor)
				{
					return Convert.ToString(this.value, 16);
				}
				string text = KnownColorTable.KnownColorToName((KnownColor)this.knownColor) ?? throw new InvalidOperationException();
				if (text != null)
				{
					return text;
				}
				return ((KnownColor)this.knownColor).ToString();
			}
		}

		private long Value
		{
			get
			{
				if ((this.state & Color.StateValueMask) != 0)
				{
					return this.value;
				}
				if (this.IsKnownColor)
				{
					return (long)KnownColorTable.KnownColorToArgb((KnownColor)this.knownColor);
				}
				return Color.NotDefinedValue;
			}
		}

		private static void CheckByte(int value, string name)
		{
			if (value < 0 || value > 255)
			{
				throw new ArgumentException(string.Format("Variable {0} has invalid value {1}. Minimum allowed value is {2}, maximum is {3}.", new object[]
				{
					name,
					value,
					0,
					255
				}));
			}
		}

		private static long MakeArgb(byte alpha, byte red, byte green, byte blue)
		{
			return (long)((ulong)((int)red << 16 | (int)green << 8 | (int)blue | (int)alpha << 24) & unchecked((ulong)-1));
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from a 32-bit ARGB value.</summary>
		/// <param name="argb">A value specifying the 32-bit ARGB value. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> structure that this method creates.</returns>
		public static Color FromArgb(int argb)
		{
			return new Color((long)argb & unchecked((long)((ulong)-1)), Color.StateARGBValueValid, null, (KnownColor)0);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from the four ARGB component (alpha, red, green, and blue) values. Although this method allows a 32-bit value to be passed for each component, the value of each component is limited to 8 bits.</summary>
		/// <param name="alpha">The alpha component. Valid values are 0 through 255. </param>
		/// <param name="red">The red component. Valid values are 0 through 255. </param>
		/// <param name="green">The green component. Valid values are 0 through 255. </param>
		/// <param name="blue">The blue component. Valid values are 0 through 255. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="alpha" />, <paramref name="red" />, <paramref name="green" />, or <paramref name="blue" /> is less than 0 or greater than 255.</exception>
		public static Color FromArgb(int alpha, int red, int green, int blue)
		{
			Color.CheckByte(alpha, "alpha");
			Color.CheckByte(red, "red");
			Color.CheckByte(green, "green");
			Color.CheckByte(blue, "blue");
			return new Color(Color.MakeArgb((byte)alpha, (byte)red, (byte)green, (byte)blue), Color.StateARGBValueValid, null, (KnownColor)0);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from the specified <see cref="T:System.Drawing.Color" /> structure, but with the new specified alpha value. Although this method allows a 32-bit value to be passed for the alpha value, the value is limited to 8 bits.</summary>
		/// <param name="alpha">The alpha value for the new <see cref="T:System.Drawing.Color" />. Valid values are 0 through 255. </param>
		/// <param name="baseColor">The <see cref="T:System.Drawing.Color" /> from which to create the new <see cref="T:System.Drawing.Color" />. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="alpha" /> is less than 0 or greater than 255.</exception>
		public static Color FromArgb(int alpha, Color baseColor)
		{
			Color.CheckByte(alpha, "alpha");
			return new Color(Color.MakeArgb((byte)alpha, baseColor.R, baseColor.G, baseColor.B), Color.StateARGBValueValid, null, (KnownColor)0);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from the specified 8-bit color values (red, green, and blue). The alpha value is implicitly 255 (fully opaque). Although this method allows a 32-bit value to be passed for each color component, the value of each component is limited to 8 bits.</summary>
		/// <param name="red">The red component value for the new <see cref="T:System.Drawing.Color" />. Valid values are 0 through 255. </param>
		/// <param name="green">The green component value for the new <see cref="T:System.Drawing.Color" />. Valid values are 0 through 255. </param>
		/// <param name="blue">The blue component value for the new <see cref="T:System.Drawing.Color" />. Valid values are 0 through 255. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="red" />, <paramref name="green" />, or <paramref name="blue" /> is less than 0 or greater than 255.</exception>
		public static Color FromArgb(int red, int green, int blue)
		{
			return Color.FromArgb(255, red, green, blue);
		}

		static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue)
		{
			return value >= minValue && value <= maxValue;
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from the specified predefined color.</summary>
		/// <param name="color">An element of the <see cref="T:System.Drawing.KnownColor" /> enumeration. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that this method creates.</returns>
		public static Color FromKnownColor(KnownColor color)
		{
			if (!IsEnumValid(color, (int)color, 1, 174))
			{
				return Color.FromName(color.ToString());
			}
			return new Color(color);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from the specified name of a predefined color.</summary>
		/// <param name="name">A string that is the name of a predefined color. Valid names are the same as the names of the elements of the <see cref="T:System.Drawing.KnownColor" /> enumeration. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that this method creates.</returns>
		public static Color FromName(string name)
		{
			object? namedColor = ColorConverter.GetNamedColor(name);
			if (namedColor != null)
			{
				return (Color)namedColor;
			}
			return new Color(Color.NotDefinedValue, Color.StateNameValid, name, (KnownColor)0);
		}

		/// <summary>Gets the hue-saturation-brightness (HSB) brightness value for this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The brightness of this <see cref="T:System.Drawing.Color" />. The brightness ranges from 0.0 through 1.0, where 0.0 represents black and 1.0 represents white.</returns>
		public float GetBrightness()
		{
			float num = (float)this.R / 255f;
			float num2 = (float)this.G / 255f;
			float num3 = (float)this.B / 255f;
			float num4 = num;
			float num5 = num;
			if (num2 > num4)
			{
				num4 = num2;
			}
			if (num3 > num4)
			{
				num4 = num3;
			}
			if (num2 < num5)
			{
				num5 = num2;
			}
			if (num3 < num5)
			{
				num5 = num3;
			}
			return (num4 + num5) / 2f;
		}

		/// <summary>Gets the hue-saturation-brightness (HSB) hue value, in degrees, for this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The hue, in degrees, of this <see cref="T:System.Drawing.Color" />. The hue is measured in degrees, ranging from 0.0 through 360.0, in HSB color space.</returns>
		public float GetHue()
		{
			if (this.R == this.G && this.G == this.B)
			{
				return 0f;
			}
			float num = (float)this.R / 255f;
			float num2 = (float)this.G / 255f;
			float num3 = (float)this.B / 255f;
			float num4 = 0f;
			float num5 = num;
			float num6 = num;
			if (num2 > num5)
			{
				num5 = num2;
			}
			if (num3 > num5)
			{
				num5 = num3;
			}
			if (num2 < num6)
			{
				num6 = num2;
			}
			if (num3 < num6)
			{
				num6 = num3;
			}
			float num7 = num5 - num6;
			if (num == num5)
			{
				num4 = (num2 - num3) / num7;
			}
			else if (num2 == num5)
			{
				num4 = 2f + (num3 - num) / num7;
			}
			else if (num3 == num5)
			{
				num4 = 4f + (num - num2) / num7;
			}
			num4 *= 60f;
			if (num4 < 0f)
			{
				num4 += 360f;
			}
			return num4;
		}

		/// <summary>Gets the hue-saturation-brightness (HSB) saturation value for this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The saturation of this <see cref="T:System.Drawing.Color" />. The saturation ranges from 0.0 through 1.0, where 0.0 is grayscale and 1.0 is the most saturated.</returns>
		public float GetSaturation()
		{
			float num = (float)this.R / 255f;
			float num2 = (float)this.G / 255f;
			float num3 = (float)this.B / 255f;
			float result = 0f;
			float num4 = num;
			float num5 = num;
			if (num2 > num4)
			{
				num4 = num2;
			}
			if (num3 > num4)
			{
				num4 = num3;
			}
			if (num2 < num5)
			{
				num5 = num2;
			}
			if (num3 < num5)
			{
				num5 = num3;
			}
			if (num4 != num5)
			{
				float num6 = (num4 + num5) / 2f;
				if ((double)num6 <= 0.5)
				{
					result = (num4 - num5) / (num4 + num5);
				}
				else
				{
					result = (num4 - num5) / (2f - num4 - num5);
				}
			}
			return result;
		}

		/// <summary>Gets the 32-bit ARGB value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The 32-bit ARGB value of this <see cref="T:System.Drawing.Color" />.</returns>
		public int ToArgb()
		{
			return (int)this.Value;
		}

		/// <summary>Gets the <see cref="T:System.Drawing.KnownColor" /> value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>An element of the <see cref="T:System.Drawing.KnownColor" /> enumeration, if the <see cref="T:System.Drawing.Color" /> is created from a predefined color by using either the <see cref="M:System.Drawing.Color.FromName(System.String)" /> method or the <see cref="M:System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor)" /> method; otherwise, 0.</returns>
		public KnownColor ToKnownColor()
		{
			return (KnownColor)this.knownColor;
		}

		/// <summary>Converts this <see cref="T:System.Drawing.Color" /> structure to a human-readable string.</summary>
		/// <returns>A string that is the name of this <see cref="T:System.Drawing.Color" />, if the <see cref="T:System.Drawing.Color" /> is created from a predefined color by using either the <see cref="M:System.Drawing.Color.FromName(System.String)" /> method or the <see cref="M:System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor)" /> method; otherwise, a string that consists of the ARGB component names and their values.</returns>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			stringBuilder.Append(base.GetType().Name);
			stringBuilder.Append(" [");
			if ((this.state & Color.StateNameValid) != 0)
			{
				stringBuilder.Append(this.Name);
			}
			else if ((this.state & Color.StateKnownColorValid) != 0)
			{
				stringBuilder.Append(this.Name);
			}
			else if ((this.state & Color.StateValueMask) != 0)
			{
				stringBuilder.Append("A=");
				stringBuilder.Append(this.A);
				stringBuilder.Append(", R=");
				stringBuilder.Append(this.R);
				stringBuilder.Append(", G=");
				stringBuilder.Append(this.G);
				stringBuilder.Append(", B=");
				stringBuilder.Append(this.B);
			}
			else
			{
				stringBuilder.Append("Empty");
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		/// <summary>Tests whether two specified <see cref="T:System.Drawing.Color" /> structures are equivalent.</summary>
		/// <param name="left">The <see cref="T:System.Drawing.Color" /> that is to the left of the equality operator. </param>
		/// <param name="right">The <see cref="T:System.Drawing.Color" /> that is to the right of the equality operator. </param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Drawing.Color" /> structures are equal; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Color left, Color right)
		{
			return left.value == right.value && left.state == right.state && left.knownColor == right.knownColor && (left.name == right.name || (left.name != null && right.name != null && left.name.Equals(right.name)));
		}

		/// <summary>Tests whether two specified <see cref="T:System.Drawing.Color" /> structures are different.</summary>
		/// <param name="left">The <see cref="T:System.Drawing.Color" /> that is to the left of the inequality operator. </param>
		/// <param name="right">The <see cref="T:System.Drawing.Color" /> that is to the right of the inequality operator. </param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Drawing.Color" /> structures are different; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Color left, Color right)
		{
			return !(left == right);
		}

		/// <summary>Tests whether the specified object is a <see cref="T:System.Drawing.Color" /> structure and is equivalent to this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <param name="obj">The object to test. </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Drawing.Color" /> structure equivalent to this <see cref="T:System.Drawing.Color" /> structure; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object? obj)
		{
			if (obj is Color)
			{
				Color color = (Color)obj;
				if (this.value == color.value && this.state == color.state && this.knownColor == color.knownColor)
				{
					return this.name == color.name || (this.name != null && color.name != null && this.name.Equals(this.name));
				}
			}
			return false;
		}

		/// <summary>Returns a hash code for this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>An integer value that specifies the hash code for this <see cref="T:System.Drawing.Color" />.</returns>
		public override int GetHashCode()
		{
			return this.value.GetHashCode() ^ this.state.GetHashCode() ^ this.knownColor.GetHashCode();
		}

		/// <summary>Represents a color that is <see langword="null" />.</summary>
		public static readonly Color Empty = default(Color);

		private static short StateKnownColorValid = 1;

		private static short StateARGBValueValid = 2;

		private static short StateValueMask = Color.StateARGBValueValid;

		private static short StateNameValid = 8;

		private static long NotDefinedValue = 0L;

		private const int ARGBAlphaShift = 24;

		private const int ARGBRedShift = 16;

		private const int ARGBGreenShift = 8;

		private const int ARGBBlueShift = 0;

		private readonly string? name;

		private readonly long value;

		private readonly short knownColor;

		private readonly short state;
	}
}
