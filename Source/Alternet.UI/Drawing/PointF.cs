using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Alternet.Drawing
{
	/// <summary>Represents an ordered pair of floating-point x- and y-coordinates that defines a point in a two-dimensional plane.</summary>
	[ComVisible(true)]
	[Serializable]
	public struct PointF
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.PointF" /> class with the specified coordinates.</summary>
		/// <param name="x">The horizontal position of the point. </param>
		/// <param name="y">The vertical position of the point. </param>
		public PointF(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.PointF" /> is empty.</summary>
		/// <returns>
		///     <see langword="true" /> if both <see cref="P:System.Drawing.PointF.X" /> and <see cref="P:System.Drawing.PointF.Y" /> are 0; otherwise, <see langword="false" />.</returns>
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.x == 0f && this.y == 0f;
			}
		}

		/// <summary>Gets or sets the x-coordinate of this <see cref="T:System.Drawing.PointF" />.</summary>
		/// <returns>The x-coordinate of this <see cref="T:System.Drawing.PointF" />.</returns>
		public float X
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

		/// <summary>Gets or sets the y-coordinate of this <see cref="T:System.Drawing.PointF" />.</summary>
		/// <returns>The y-coordinate of this <see cref="T:System.Drawing.PointF" />.</returns>
		public float Y
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

		/// <summary>Translates a <see cref="T:System.Drawing.PointF" /> by a given <see cref="T:System.Drawing.Size" />.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.PointF" /> to translate. </param>
		/// <param name="sz">A <see cref="T:System.Drawing.Size" /> that specifies the pair of numbers to add to the coordinates of <paramref name="pt" />. </param>
		/// <returns>Returns the translated <see cref="T:System.Drawing.PointF" />.</returns>
		public static PointF operator +(PointF pt, Size sz)
		{
			return PointF.Add(pt, sz);
		}

		/// <summary>Translates a <see cref="T:System.Drawing.PointF" /> by the negative of a given <see cref="T:System.Drawing.Size" />.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.PointF" /> to translate.</param>
		/// <param name="sz">The <see cref="T:System.Drawing.Size" /> that specifies the numbers to subtract from the coordinates of <paramref name="pt" />.</param>
		/// <returns>The translated <see cref="T:System.Drawing.PointF" />.</returns>
		public static PointF operator -(PointF pt, Size sz)
		{
			return PointF.Subtract(pt, sz);
		}

		/// <summary>Translates the <see cref="T:System.Drawing.PointF" /> by the specified <see cref="T:System.Drawing.SizeF" />.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.PointF" /> to translate.</param>
		/// <param name="sz">The <see cref="T:System.Drawing.SizeF" /> that specifies the numbers to add to the x- and y-coordinates of the <see cref="T:System.Drawing.PointF" />.</param>
		/// <returns>The translated <see cref="T:System.Drawing.PointF" />.</returns>
		public static PointF operator +(PointF pt, SizeF sz)
		{
			return PointF.Add(pt, sz);
		}

		/// <summary>Translates a <see cref="T:System.Drawing.PointF" /> by the negative of a specified <see cref="T:System.Drawing.SizeF" />. </summary>
		/// <param name="pt">The <see cref="T:System.Drawing.PointF" /> to translate.</param>
		/// <param name="sz">The <see cref="T:System.Drawing.SizeF" /> that specifies the numbers to subtract from the coordinates of <paramref name="pt" />.</param>
		/// <returns>The translated <see cref="T:System.Drawing.PointF" />.</returns>
		public static PointF operator -(PointF pt, SizeF sz)
		{
			return PointF.Subtract(pt, sz);
		}

		/// <summary>Compares two <see cref="T:System.Drawing.PointF" /> structures. The result specifies whether the values of the <see cref="P:System.Drawing.PointF.X" /> and <see cref="P:System.Drawing.PointF.Y" /> properties of the two <see cref="T:System.Drawing.PointF" /> structures are equal.</summary>
		/// <param name="left">A <see cref="T:System.Drawing.PointF" /> to compare. </param>
		/// <param name="right">A <see cref="T:System.Drawing.PointF" /> to compare. </param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Drawing.PointF.X" /> and <see cref="P:System.Drawing.PointF.Y" /> values of the left and right <see cref="T:System.Drawing.PointF" /> structures are equal; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(PointF left, PointF right)
		{
			return left.X == right.X && left.Y == right.Y;
		}

		/// <summary>Determines whether the coordinates of the specified points are not equal.</summary>
		/// <param name="left">A <see cref="T:System.Drawing.PointF" /> to compare.</param>
		/// <param name="right">A <see cref="T:System.Drawing.PointF" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> to indicate the <see cref="P:System.Drawing.PointF.X" /> and <see cref="P:System.Drawing.PointF.Y" /> values of <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />. </returns>
		public static bool operator !=(PointF left, PointF right)
		{
			return !(left == right);
		}

		/// <summary>Translates a given <see cref="T:System.Drawing.PointF" /> by the specified <see cref="T:System.Drawing.Size" />.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.PointF" /> to translate.</param>
		/// <param name="sz">The <see cref="T:System.Drawing.Size" /> that specifies the numbers to add to the coordinates of <paramref name="pt" />.</param>
		/// <returns>The translated <see cref="T:System.Drawing.PointF" />.</returns>
		public static PointF Add(PointF pt, Size sz)
		{
			return new PointF(pt.X + (float)sz.Width, pt.Y + (float)sz.Height);
		}

		/// <summary>Translates a <see cref="T:System.Drawing.PointF" /> by the negative of a specified size.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.PointF" /> to translate.</param>
		/// <param name="sz">The <see cref="T:System.Drawing.Size" /> that specifies the numbers to subtract from the coordinates of <paramref name="pt" />.</param>
		/// <returns>The translated <see cref="T:System.Drawing.PointF" />.</returns>
		public static PointF Subtract(PointF pt, Size sz)
		{
			return new PointF(pt.X - (float)sz.Width, pt.Y - (float)sz.Height);
		}

		/// <summary>Translates a given <see cref="T:System.Drawing.PointF" /> by a specified <see cref="T:System.Drawing.SizeF" />.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.PointF" /> to translate.</param>
		/// <param name="sz">The <see cref="T:System.Drawing.SizeF" /> that specifies the numbers to add to the coordinates of <paramref name="pt" />.</param>
		/// <returns>The translated <see cref="T:System.Drawing.PointF" />.</returns>
		public static PointF Add(PointF pt, SizeF sz)
		{
			return new PointF(pt.X + sz.Width, pt.Y + sz.Height);
		}

		/// <summary>Translates a <see cref="T:System.Drawing.PointF" /> by the negative of a specified size.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.PointF" /> to translate.</param>
		/// <param name="sz">The <see cref="T:System.Drawing.SizeF" /> that specifies the numbers to subtract from the coordinates of <paramref name="pt" />.</param>
		/// <returns>The translated <see cref="T:System.Drawing.PointF" />.</returns>
		public static PointF Subtract(PointF pt, SizeF sz)
		{
			return new PointF(pt.X - sz.Width, pt.Y - sz.Height);
		}

		/// <summary>Specifies whether this <see cref="T:System.Drawing.PointF" /> contains the same coordinates as the specified <see cref="T:System.Object" />.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Drawing.PointF" /> and has the same coordinates as this <see cref="T:System.Drawing.Point" />.</returns>
		public override bool Equals(object? obj)
		{
			if (!(obj is PointF))
			{
				return false;
			}
			PointF pointF = (PointF)obj;
			return pointF.X == this.X && pointF.Y == this.Y && pointF.GetType().Equals(base.GetType());
		}

		/// <summary>Returns a hash code for this <see cref="T:System.Drawing.PointF" /> structure.</summary>
		/// <returns>An integer value that specifies a hash value for this <see cref="T:System.Drawing.PointF" /> structure.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Converts this <see cref="T:System.Drawing.PointF" /> to a human readable string.</summary>
		/// <returns>A string that represents this <see cref="T:System.Drawing.PointF" />.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{X={0}, Y={1}}}", new object[]
			{
				this.x,
				this.y
			});
		}

		/// <summary>Represents a new instance of the <see cref="T:System.Drawing.PointF" /> class with member data left uninitialized.</summary>
		public static readonly PointF Empty;

		private float x;

		private float y;
	}
}
