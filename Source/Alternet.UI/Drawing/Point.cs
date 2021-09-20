using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Alternet.Drawing
{
	/// <summary>Represents an ordered pair of integer x- and y-coordinates that defines a point in a two-dimensional plane.</summary>
	//[TypeConverter(typeof(PointConverter))]
	[ComVisible(true)]
	[Serializable]
	public struct Point
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Point" /> class with the specified coordinates.</summary>
		/// <param name="x">The horizontal position of the point. </param>
		/// <param name="y">The vertical position of the point. </param>
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Point" /> class from a <see cref="T:System.Drawing.Size" />.</summary>
		/// <param name="sz">A <see cref="T:System.Drawing.Size" /> that specifies the coordinates for the new <see cref="T:System.Drawing.Point" />. </param>
		public Point(Size sz)
		{
			this.x = sz.Width;
			this.y = sz.Height;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Point" /> class using coordinates specified by an integer value.</summary>
		/// <param name="dw">A 32-bit integer that specifies the coordinates for the new <see cref="T:System.Drawing.Point" />. </param>
		public Point(int dw)
		{
			this.x = (int)((short)Point.LOWORD(dw));
			this.y = (int)((short)Point.HIWORD(dw));
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Point" /> is empty.</summary>
		/// <returns>
		///     <see langword="true" /> if both <see cref="P:System.Drawing.Point.X" /> and <see cref="P:System.Drawing.Point.Y" /> are 0; otherwise, <see langword="false" />.</returns>
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.x == 0 && this.y == 0;
			}
		}

		/// <summary>Gets or sets the x-coordinate of this <see cref="T:System.Drawing.Point" />.</summary>
		/// <returns>The x-coordinate of this <see cref="T:System.Drawing.Point" />.</returns>
		public int X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		/// <summary>Gets or sets the y-coordinate of this <see cref="T:System.Drawing.Point" />.</summary>
		/// <returns>The y-coordinate of this <see cref="T:System.Drawing.Point" />.</returns>
		public int Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.Point" /> structure to a <see cref="T:System.Drawing.PointF" /> structure.</summary>
		/// <param name="p">The <see cref="T:System.Drawing.Point" /> to be converted.</param>
		/// <returns>The <see cref="T:System.Drawing.PointF" /> that results from the conversion.</returns>
		public static implicit operator PointF(Point p)
		{
			return new PointF((float)p.X, (float)p.Y);
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.Point" /> structure to a <see cref="T:System.Drawing.Size" /> structure.</summary>
		/// <param name="p">The <see cref="T:System.Drawing.Point" /> to be converted.</param>
		/// <returns>The <see cref="T:System.Drawing.Size" /> that results from the conversion.</returns>
		public static explicit operator Size(Point p)
		{
			return new Size(p.X, p.Y);
		}

		/// <summary>Translates a <see cref="T:System.Drawing.Point" /> by a given <see cref="T:System.Drawing.Size" />.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> to translate. </param>
		/// <param name="sz">A <see cref="T:System.Drawing.Size" /> that specifies the pair of numbers to add to the coordinates of <paramref name="pt" />. </param>
		/// <returns>The translated <see cref="T:System.Drawing.Point" />.</returns>
		public static Point operator +(Point pt, Size sz)
		{
			return Point.Add(pt, sz);
		}

		/// <summary>Translates a <see cref="T:System.Drawing.Point" /> by the negative of a given <see cref="T:System.Drawing.Size" />.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> to translate. </param>
		/// <param name="sz">A <see cref="T:System.Drawing.Size" /> that specifies the pair of numbers to subtract from the coordinates of <paramref name="pt" />. </param>
		/// <returns>A <see cref="T:System.Drawing.Point" /> structure that is translated by the negative of a given <see cref="T:System.Drawing.Size" /> structure.</returns>
		public static Point operator -(Point pt, Size sz)
		{
			return Point.Subtract(pt, sz);
		}

		/// <summary>Compares two <see cref="T:System.Drawing.Point" /> objects. The result specifies whether the values of the <see cref="P:System.Drawing.Point.X" /> and <see cref="P:System.Drawing.Point.Y" /> properties of the two <see cref="T:System.Drawing.Point" /> objects are equal.</summary>
		/// <param name="left">A <see cref="T:System.Drawing.Point" /> to compare. </param>
		/// <param name="right">A <see cref="T:System.Drawing.Point" /> to compare. </param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Drawing.Point.X" /> and <see cref="P:System.Drawing.Point.Y" /> values of <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Point left, Point right)
		{
			return left.X == right.X && left.Y == right.Y;
		}

		/// <summary>Compares two <see cref="T:System.Drawing.Point" /> objects. The result specifies whether the values of the <see cref="P:System.Drawing.Point.X" /> or <see cref="P:System.Drawing.Point.Y" /> properties of the two <see cref="T:System.Drawing.Point" /> objects are unequal.</summary>
		/// <param name="left">A <see cref="T:System.Drawing.Point" /> to compare. </param>
		/// <param name="right">A <see cref="T:System.Drawing.Point" /> to compare. </param>
		/// <returns>
		///     <see langword="true" /> if the values of either the <see cref="P:System.Drawing.Point.X" /> properties or the <see cref="P:System.Drawing.Point.Y" /> properties of <paramref name="left" /> and <paramref name="right" /> differ; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Point left, Point right)
		{
			return !(left == right);
		}

		/// <summary>Adds the specified <see cref="T:System.Drawing.Size" /> to the specified <see cref="T:System.Drawing.Point" />.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> to add.</param>
		/// <param name="sz">The <see cref="T:System.Drawing.Size" /> to add</param>
		/// <returns>The <see cref="T:System.Drawing.Point" /> that is the result of the addition operation.</returns>
		public static Point Add(Point pt, Size sz)
		{
			return new Point(pt.X + sz.Width, pt.Y + sz.Height);
		}

		/// <summary>Returns the result of subtracting specified <see cref="T:System.Drawing.Size" /> from the specified <see cref="T:System.Drawing.Point" />.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> to be subtracted from. </param>
		/// <param name="sz">The <see cref="T:System.Drawing.Size" /> to subtract from the <see cref="T:System.Drawing.Point" />.</param>
		/// <returns>The <see cref="T:System.Drawing.Point" /> that is the result of the subtraction operation.</returns>
		public static Point Subtract(Point pt, Size sz)
		{
			return new Point(pt.X - sz.Width, pt.Y - sz.Height);
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.PointF" /> to a <see cref="T:System.Drawing.Point" /> by rounding the values of the <see cref="T:System.Drawing.PointF" /> to the next higher integer values.</summary>
		/// <param name="value">The <see cref="T:System.Drawing.PointF" /> to convert. </param>
		/// <returns>The <see cref="T:System.Drawing.Point" /> this method converts to.</returns>
		public static Point Ceiling(PointF value)
		{
			return new Point((int)Math.Ceiling((double)value.X), (int)Math.Ceiling((double)value.Y));
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.PointF" /> to a <see cref="T:System.Drawing.Point" /> by truncating the values of the <see cref="T:System.Drawing.Point" />.</summary>
		/// <param name="value">The <see cref="T:System.Drawing.PointF" /> to convert. </param>
		/// <returns>The <see cref="T:System.Drawing.Point" /> this method converts to.</returns>
		public static Point Truncate(PointF value)
		{
			return new Point((int)value.X, (int)value.Y);
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.PointF" /> to a <see cref="T:System.Drawing.Point" /> object by rounding the <see cref="T:System.Drawing.Point" /> values to the nearest integer.</summary>
		/// <param name="value">The <see cref="T:System.Drawing.PointF" /> to convert. </param>
		/// <returns>The <see cref="T:System.Drawing.Point" /> this method converts to.</returns>
		public static Point Round(PointF value)
		{
			return new Point((int)Math.Round((double)value.X), (int)Math.Round((double)value.Y));
		}

		/// <summary>Specifies whether this <see cref="T:System.Drawing.Point" /> contains the same coordinates as the specified <see cref="T:System.Object" />.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to test. </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Drawing.Point" /> and has the same coordinates as this <see cref="T:System.Drawing.Point" />.</returns>
		public override bool Equals(object? obj)
		{
			if (!(obj is Point))
			{
				return false;
			}
			Point point = (Point)obj;
			return point.X == this.X && point.Y == this.Y;
		}

		/// <summary>Returns a hash code for this <see cref="T:System.Drawing.Point" />.</summary>
		/// <returns>An integer value that specifies a hash value for this <see cref="T:System.Drawing.Point" />.</returns>
		public override int GetHashCode()
		{
			return this.x ^ this.y;
		}

		/// <summary>Translates this <see cref="T:System.Drawing.Point" /> by the specified amount.</summary>
		/// <param name="dx">The amount to offset the x-coordinate. </param>
		/// <param name="dy">The amount to offset the y-coordinate. </param>
		public void Offset(int dx, int dy)
		{
			this.X += dx;
			this.Y += dy;
		}

		/// <summary>Translates this <see cref="T:System.Drawing.Point" /> by the specified <see cref="T:System.Drawing.Point" />.</summary>
		/// <param name="p">The <see cref="T:System.Drawing.Point" /> used offset this <see cref="T:System.Drawing.Point" />.</param>
		public void Offset(Point p)
		{
			this.Offset(p.X, p.Y);
		}

		/// <summary>Converts this <see cref="T:System.Drawing.Point" /> to a human-readable string.</summary>
		/// <returns>A string that represents this <see cref="T:System.Drawing.Point" />.</returns>
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{X=",
				this.X.ToString(CultureInfo.CurrentCulture),
				",Y=",
				this.Y.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		private static int HIWORD(int n)
		{
			return n >> 16 & 65535;
		}

		private static int LOWORD(int n)
		{
			return n & 65535;
		}

		/// <summary>Represents a <see cref="T:System.Drawing.Point" /> that has <see cref="P:System.Drawing.Point.X" /> and <see cref="P:System.Drawing.Point.Y" /> values set to zero. </summary>
		public static readonly Point Empty;

		private int x;

		private int y;
	}
}
