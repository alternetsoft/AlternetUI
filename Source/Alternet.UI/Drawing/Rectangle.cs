using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Alternet.Drawing
{
	/// <summary>Stores a set of four integers that represent the location and size of a rectangle</summary>
	//[TypeConverter(typeof(RectangleConverter))]
	[ComVisible(true)]
	[Serializable]
	public struct Rectangle
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Rectangle" /> class with the specified location and size.</summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle. </param>
		/// <param name="width">The width of the rectangle. </param>
		/// <param name="height">The height of the rectangle. </param>
		public Rectangle(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Rectangle" /> class with the specified location and size.</summary>
		/// <param name="location">A <see cref="T:System.Drawing.Point" /> that represents the upper-left corner of the rectangular region. </param>
		/// <param name="size">A <see cref="T:System.Drawing.Size" /> that represents the width and height of the rectangular region. </param>
		public Rectangle(Point location, Size size)
		{
			this.x = location.X;
			this.y = location.Y;
			this.width = size.Width;
			this.height = size.Height;
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Rectangle" /> structure with the specified edge locations.</summary>
		/// <param name="left">The x-coordinate of the upper-left corner of this <see cref="T:System.Drawing.Rectangle" /> structure. </param>
		/// <param name="top">The y-coordinate of the upper-left corner of this <see cref="T:System.Drawing.Rectangle" /> structure. </param>
		/// <param name="right">The x-coordinate of the lower-right corner of this <see cref="T:System.Drawing.Rectangle" /> structure. </param>
		/// <param name="bottom">The y-coordinate of the lower-right corner of this <see cref="T:System.Drawing.Rectangle" /> structure. </param>
		/// <returns>The new <see cref="T:System.Drawing.Rectangle" /> that this method creates.</returns>
		public static Rectangle FromLTRB(int left, int top, int right, int bottom)
		{
			return new Rectangle(left, top, right - left, bottom - top);
		}

		/// <summary>Gets or sets the coordinates of the upper-left corner of this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> that represents the upper-left corner of this <see cref="T:System.Drawing.Rectangle" /> structure.</returns>
		[Browsable(false)]
		public Point Location
		{
			get
			{
				return new Point(this.X, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		/// <summary>Gets or sets the size of this <see cref="T:System.Drawing.Rectangle" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the width and height of this <see cref="T:System.Drawing.Rectangle" /> structure.</returns>
		[Browsable(false)]
		public Size Size
		{
			get
			{
				return new Size(this.Width, this.Height);
			}
			set
			{
				this.Width = value.Width;
				this.Height = value.Height;
			}
		}

		/// <summary>Gets or sets the x-coordinate of the upper-left corner of this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <returns>The x-coordinate of the upper-left corner of this <see cref="T:System.Drawing.Rectangle" /> structure. The default is 0.</returns>
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

		/// <summary>Gets or sets the y-coordinate of the upper-left corner of this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <returns>The y-coordinate of the upper-left corner of this <see cref="T:System.Drawing.Rectangle" /> structure. The default is 0.</returns>
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

		/// <summary>Gets or sets the width of this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <returns>The width of this <see cref="T:System.Drawing.Rectangle" /> structure. The default is 0.</returns>
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		/// <summary>Gets or sets the height of this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <returns>The height of this <see cref="T:System.Drawing.Rectangle" /> structure. The default is 0.</returns>
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		/// <summary>Gets the x-coordinate of the left edge of this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <returns>The x-coordinate of the left edge of this <see cref="T:System.Drawing.Rectangle" /> structure.</returns>
		[Browsable(false)]
		public int Left
		{
			get
			{
				return this.X;
			}
		}

		/// <summary>Gets the y-coordinate of the top edge of this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <returns>The y-coordinate of the top edge of this <see cref="T:System.Drawing.Rectangle" /> structure.</returns>
		[Browsable(false)]
		public int Top
		{
			get
			{
				return this.Y;
			}
		}

		/// <summary>Gets the x-coordinate that is the sum of <see cref="P:System.Drawing.Rectangle.X" /> and <see cref="P:System.Drawing.Rectangle.Width" /> property values of this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <returns>The x-coordinate that is the sum of <see cref="P:System.Drawing.Rectangle.X" /> and <see cref="P:System.Drawing.Rectangle.Width" /> of this <see cref="T:System.Drawing.Rectangle" />.</returns>
		[Browsable(false)]
		public int Right
		{
			get
			{
				return this.X + this.Width;
			}
		}

		/// <summary>Gets the y-coordinate that is the sum of the <see cref="P:System.Drawing.Rectangle.Y" /> and <see cref="P:System.Drawing.Rectangle.Height" /> property values of this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <returns>The y-coordinate that is the sum of <see cref="P:System.Drawing.Rectangle.Y" /> and <see cref="P:System.Drawing.Rectangle.Height" /> of this <see cref="T:System.Drawing.Rectangle" />.</returns>
		[Browsable(false)]
		public int Bottom
		{
			get
			{
				return this.Y + this.Height;
			}
		}

		/// <summary>Tests whether all numeric properties of this <see cref="T:System.Drawing.Rectangle" /> have values of zero.</summary>
		/// <returns>This property returns <see langword="true" /> if the <see cref="P:System.Drawing.Rectangle.Width" />, <see cref="P:System.Drawing.Rectangle.Height" />, <see cref="P:System.Drawing.Rectangle.X" />, and <see cref="P:System.Drawing.Rectangle.Y" /> properties of this <see cref="T:System.Drawing.Rectangle" /> all have values of zero; otherwise, <see langword="false" />.</returns>
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.height == 0 && this.width == 0 && this.x == 0 && this.y == 0;
			}
		}

		/// <summary>Tests whether <paramref name="obj" /> is a <see cref="T:System.Drawing.Rectangle" /> structure with the same location and size of this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Drawing.Rectangle" /> structure and its <see cref="P:System.Drawing.Rectangle.X" />, <see cref="P:System.Drawing.Rectangle.Y" />, <see cref="P:System.Drawing.Rectangle.Width" />, and <see cref="P:System.Drawing.Rectangle.Height" /> properties are equal to the corresponding properties of this <see cref="T:System.Drawing.Rectangle" /> structure; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object? obj)
		{
			if (!(obj is Rectangle))
			{
				return false;
			}
			Rectangle rectangle = (Rectangle)obj;
			return rectangle.X == this.X && rectangle.Y == this.Y && rectangle.Width == this.Width && rectangle.Height == this.Height;
		}

		/// <summary>Tests whether two <see cref="T:System.Drawing.Rectangle" /> structures have equal location and size.</summary>
		/// <param name="left">The <see cref="T:System.Drawing.Rectangle" /> structure that is to the left of the equality operator. </param>
		/// <param name="right">The <see cref="T:System.Drawing.Rectangle" /> structure that is to the right of the equality operator. </param>
		/// <returns>This operator returns <see langword="true" /> if the two <see cref="T:System.Drawing.Rectangle" /> structures have equal <see cref="P:System.Drawing.Rectangle.X" />, <see cref="P:System.Drawing.Rectangle.Y" />, <see cref="P:System.Drawing.Rectangle.Width" />, and <see cref="P:System.Drawing.Rectangle.Height" /> properties.</returns>
		public static bool operator ==(Rectangle left, Rectangle right)
		{
			return left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;
		}

		/// <summary>Tests whether two <see cref="T:System.Drawing.Rectangle" /> structures differ in location or size.</summary>
		/// <param name="left">The <see cref="T:System.Drawing.Rectangle" /> structure that is to the left of the inequality operator. </param>
		/// <param name="right">The <see cref="T:System.Drawing.Rectangle" /> structure that is to the right of the inequality operator. </param>
		/// <returns>This operator returns <see langword="true" /> if any of the <see cref="P:System.Drawing.Rectangle.X" />, <see cref="P:System.Drawing.Rectangle.Y" />, <see cref="P:System.Drawing.Rectangle.Width" /> or <see cref="P:System.Drawing.Rectangle.Height" /> properties of the two <see cref="T:System.Drawing.Rectangle" /> structures are unequal; otherwise <see langword="false" />.</returns>
		public static bool operator !=(Rectangle left, Rectangle right)
		{
			return !(left == right);
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.RectangleF" /> structure to a <see cref="T:System.Drawing.Rectangle" /> structure by rounding the <see cref="T:System.Drawing.RectangleF" /> values to the next higher integer values.</summary>
		/// <param name="value">The <see cref="T:System.Drawing.RectangleF" /> structure to be converted. </param>
		/// <returns>Returns a <see cref="T:System.Drawing.Rectangle" />.</returns>
		public static Rectangle Ceiling(RectangleF value)
		{
			return new Rectangle((int)Math.Ceiling((double)value.X), (int)Math.Ceiling((double)value.Y), (int)Math.Ceiling((double)value.Width), (int)Math.Ceiling((double)value.Height));
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.RectangleF" /> to a <see cref="T:System.Drawing.Rectangle" /> by truncating the <see cref="T:System.Drawing.RectangleF" /> values.</summary>
		/// <param name="value">The <see cref="T:System.Drawing.RectangleF" /> to be converted. </param>
		/// <returns>The truncated value of the  <see cref="T:System.Drawing.Rectangle" />.</returns>
		public static Rectangle Truncate(RectangleF value)
		{
			return new Rectangle((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.RectangleF" /> to a <see cref="T:System.Drawing.Rectangle" /> by rounding the <see cref="T:System.Drawing.RectangleF" /> values to the nearest integer values.</summary>
		/// <param name="value">The <see cref="T:System.Drawing.RectangleF" /> to be converted. </param>
		/// <returns>The rounded interger value of the <see cref="T:System.Drawing.Rectangle" />.</returns>
		public static Rectangle Round(RectangleF value)
		{
			return new Rectangle((int)Math.Round((double)value.X), (int)Math.Round((double)value.Y), (int)Math.Round((double)value.Width), (int)Math.Round((double)value.Height));
		}

		/// <summary>Determines if the specified point is contained within this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="x">The x-coordinate of the point to test. </param>
		/// <param name="y">The y-coordinate of the point to test. </param>
		/// <returns>This method returns <see langword="true" /> if the point defined by <paramref name="x" /> and <paramref name="y" /> is contained within this <see cref="T:System.Drawing.Rectangle" /> structure; otherwise <see langword="false" />.</returns>
		public bool Contains(int x, int y)
		{
			return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
		}

		/// <summary>Determines if the specified point is contained within this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if the point represented by <paramref name="pt" /> is contained within this <see cref="T:System.Drawing.Rectangle" /> structure; otherwise <see langword="false" />.</returns>
		public bool Contains(Point pt)
		{
			return this.Contains(pt.X, pt.Y);
		}

		/// <summary>Determines if the rectangular region represented by <paramref name="rect" /> is entirely contained within this <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if the rectangular region represented by <paramref name="rect" /> is entirely contained within this <see cref="T:System.Drawing.Rectangle" /> structure; otherwise <see langword="false" />.</returns>
		public bool Contains(Rectangle rect)
		{
			return this.X <= rect.X && rect.X + rect.Width <= this.X + this.Width && this.Y <= rect.Y && rect.Y + rect.Height <= this.Y + this.Height;
		}

		/// <summary>Returns the hash code for this <see cref="T:System.Drawing.Rectangle" /> structure. For information about the use of hash codes, see <see cref="M:System.Object.GetHashCode" /> .</summary>
		/// <returns>An integer that represents the hash code for this rectangle.</returns>
		public override int GetHashCode()
		{
			return this.X ^ (this.Y << 13 | (int)((uint)this.Y >> 19)) ^ (this.Width << 26 | (int)((uint)this.Width >> 6)) ^ (this.Height << 7 | (int)((uint)this.Height >> 25));
		}

		/// <summary>Enlarges this <see cref="T:System.Drawing.Rectangle" /> by the specified amount.</summary>
		/// <param name="width">The amount to inflate this <see cref="T:System.Drawing.Rectangle" /> horizontally. </param>
		/// <param name="height">The amount to inflate this <see cref="T:System.Drawing.Rectangle" /> vertically. </param>
		public void Inflate(int width, int height)
		{
			this.X -= width;
			this.Y -= height;
			this.Width += 2 * width;
			this.Height += 2 * height;
		}

		/// <summary>Enlarges this <see cref="T:System.Drawing.Rectangle" /> by the specified amount.</summary>
		/// <param name="size">The amount to inflate this rectangle. </param>
		public void Inflate(Size size)
		{
			this.Inflate(size.Width, size.Height);
		}

		/// <summary>Creates and returns an enlarged copy of the specified <see cref="T:System.Drawing.Rectangle" /> structure. The copy is enlarged by the specified amount. The original <see cref="T:System.Drawing.Rectangle" /> structure remains unmodified.</summary>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle" /> with which to start. This rectangle is not modified. </param>
		/// <param name="x">The amount to inflate this <see cref="T:System.Drawing.Rectangle" /> horizontally. </param>
		/// <param name="y">The amount to inflate this <see cref="T:System.Drawing.Rectangle" /> vertically. </param>
		/// <returns>The enlarged <see cref="T:System.Drawing.Rectangle" />.</returns>
		public static Rectangle Inflate(Rectangle rect, int x, int y)
		{
			Rectangle result = rect;
			result.Inflate(x, y);
			return result;
		}

		/// <summary>Replaces this <see cref="T:System.Drawing.Rectangle" /> with the intersection of itself and the specified <see cref="T:System.Drawing.Rectangle" />.</summary>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle" /> with which to intersect. </param>
		public void Intersect(Rectangle rect)
		{
			Rectangle rectangle = Rectangle.Intersect(rect, this);
			this.X = rectangle.X;
			this.Y = rectangle.Y;
			this.Width = rectangle.Width;
			this.Height = rectangle.Height;
		}

		/// <summary>Returns a third <see cref="T:System.Drawing.Rectangle" /> structure that represents the intersection of two other <see cref="T:System.Drawing.Rectangle" /> structures. If there is no intersection, an empty <see cref="T:System.Drawing.Rectangle" /> is returned.</summary>
		/// <param name="a">A rectangle to intersect. </param>
		/// <param name="b">A rectangle to intersect. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the intersection of <paramref name="a" /> and <paramref name="b" />.</returns>
		public static Rectangle Intersect(Rectangle a, Rectangle b)
		{
			int num = Math.Max(a.X, b.X);
			int num2 = Math.Min(a.X + a.Width, b.X + b.Width);
			int num3 = Math.Max(a.Y, b.Y);
			int num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
			if (num2 >= num && num4 >= num3)
			{
				return new Rectangle(num, num3, num2 - num, num4 - num3);
			}
			return Rectangle.Empty;
		}

		/// <summary>Determines if this rectangle intersects with <paramref name="rect" />.</summary>
		/// <param name="rect">The rectangle to test. </param>
		/// <returns>This method returns <see langword="true" /> if there is any intersection, otherwise <see langword="false" />.</returns>
		public bool IntersectsWith(Rectangle rect)
		{
			return rect.X < this.X + this.Width && this.X < rect.X + rect.Width && rect.Y < this.Y + this.Height && this.Y < rect.Y + rect.Height;
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Rectangle" /> structure that contains the union of two <see cref="T:System.Drawing.Rectangle" /> structures.</summary>
		/// <param name="a">A rectangle to union. </param>
		/// <param name="b">A rectangle to union. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> structure that bounds the union of the two <see cref="T:System.Drawing.Rectangle" /> structures.</returns>
		public static Rectangle Union(Rectangle a, Rectangle b)
		{
			int num = Math.Min(a.X, b.X);
			int num2 = Math.Max(a.X + a.Width, b.X + b.Width);
			int num3 = Math.Min(a.Y, b.Y);
			int num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
			return new Rectangle(num, num3, num2 - num, num4 - num3);
		}

		/// <summary>Adjusts the location of this rectangle by the specified amount.</summary>
		/// <param name="pos">Amount to offset the location. </param>
		public void Offset(Point pos)
		{
			this.Offset(pos.X, pos.Y);
		}

		/// <summary>Adjusts the location of this rectangle by the specified amount.</summary>
		/// <param name="x">The horizontal offset. </param>
		/// <param name="y">The vertical offset. </param>
		public void Offset(int x, int y)
		{
			this.X += x;
			this.Y += y;
		}

		/// <summary>Converts the attributes of this <see cref="T:System.Drawing.Rectangle" /> to a human-readable string.</summary>
		/// <returns>A string that contains the position, width, and height of this <see cref="T:System.Drawing.Rectangle" /> structure ¾ for example, {X=20, Y=20, Width=100, Height=50} </returns>
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{X=",
				this.X.ToString(CultureInfo.CurrentCulture),
				",Y=",
				this.Y.ToString(CultureInfo.CurrentCulture),
				",Width=",
				this.Width.ToString(CultureInfo.CurrentCulture),
				",Height=",
				this.Height.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		/// <summary>Represents a <see cref="T:System.Drawing.Rectangle" /> structure with its properties left uninitialized.</summary>
		public static readonly Rectangle Empty;

		private int x;

		private int y;

		private int width;

		private int height;
	}
}
