using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Alternet.Drawing
{
	/// <summary>Stores an ordered pair of floating-point numbers, typically the width and height of a rectangle.</summary>
	[ComVisible(true)]
	//[TypeConverter(typeof(SizeFConverter))]
	[Serializable]
	public struct SizeF
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.SizeF" /> structure from the specified existing <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="size">The <see cref="T:System.Drawing.SizeF" /> structure from which to create the new <see cref="T:System.Drawing.SizeF" /> structure. </param>
		public SizeF(SizeF size)
		{
			this.width = size.width;
			this.height = size.height;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.SizeF" /> structure from the specified <see cref="T:System.Drawing.PointF" /> structure.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.PointF" /> structure from which to initialize this <see cref="T:System.Drawing.SizeF" /> structure. </param>
		public SizeF(PointF pt)
		{
			this.width = pt.X;
			this.height = pt.Y;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.SizeF" /> structure from the specified dimensions.</summary>
		/// <param name="width">The width component of the new <see cref="T:System.Drawing.SizeF" /> structure. </param>
		/// <param name="height">The height component of the new <see cref="T:System.Drawing.SizeF" /> structure. </param>
		public SizeF(float width, float height)
		{
			this.width = width;
			this.height = height;
		}

		/// <summary>Adds the width and height of one <see cref="T:System.Drawing.SizeF" /> structure to the width and height of another <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="sz1">The first <see cref="T:System.Drawing.SizeF" /> structure to add. </param>
		/// <param name="sz2">The second <see cref="T:System.Drawing.SizeF" /> structure to add. </param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> structure that is the result of the addition operation.</returns>
		public static SizeF operator +(SizeF sz1, SizeF sz2)
		{
			return SizeF.Add(sz1, sz2);
		}

		/// <summary>Subtracts the width and height of one <see cref="T:System.Drawing.SizeF" /> structure from the width and height of another <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="sz1">The <see cref="T:System.Drawing.SizeF" /> structure on the left side of the subtraction operator. </param>
		/// <param name="sz2">The <see cref="T:System.Drawing.SizeF" /> structure on the right side of the subtraction operator. </param>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> that is the result of the subtraction operation.</returns>
		public static SizeF operator -(SizeF sz1, SizeF sz2)
		{
			return SizeF.Subtract(sz1, sz2);
		}

		/// <summary>Tests whether two <see cref="T:System.Drawing.SizeF" /> structures are equal.</summary>
		/// <param name="sz1">The <see cref="T:System.Drawing.SizeF" /> structure on the left side of the equality operator. </param>
		/// <param name="sz2">The <see cref="T:System.Drawing.SizeF" /> structure on the right of the equality operator. </param>
		/// <returns>This operator returns <see langword="true" /> if <paramref name="sz1" /> and <paramref name="sz2" /> have equal width and height; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(SizeF sz1, SizeF sz2)
		{
			return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
		}

		/// <summary>Tests whether two <see cref="T:System.Drawing.SizeF" /> structures are different.</summary>
		/// <param name="sz1">The <see cref="T:System.Drawing.SizeF" /> structure on the left of the inequality operator. </param>
		/// <param name="sz2">The <see cref="T:System.Drawing.SizeF" /> structure on the right of the inequality operator. </param>
		/// <returns>This operator returns <see langword="true" /> if <paramref name="sz1" /> and <paramref name="sz2" /> differ either in width or height; <see langword="false" /> if <paramref name="sz1" /> and <paramref name="sz2" /> are equal.</returns>
		public static bool operator !=(SizeF sz1, SizeF sz2)
		{
			return !(sz1 == sz2);
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.SizeF" /> structure to a <see cref="T:System.Drawing.PointF" /> structure.</summary>
		/// <param name="size">The <see cref="T:System.Drawing.SizeF" /> structure to be converted</param>
		/// <returns>The <see cref="T:System.Drawing.PointF" /> structure to which this operator converts.</returns>
		public static explicit operator PointF(SizeF size)
		{
			return new PointF(size.Width, size.Height);
		}

		/// <summary>Gets a value that indicates whether this <see cref="T:System.Drawing.SizeF" /> structure has zero width and height.</summary>
		/// <returns>This property returns <see langword="true" /> when this <see cref="T:System.Drawing.SizeF" /> structure has both a width and height of zero; otherwise, <see langword="false" />.</returns>
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.width == 0f && this.height == 0f;
			}
		}

		/// <summary>Gets or sets the horizontal component of this <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <returns>The horizontal component of this <see cref="T:System.Drawing.SizeF" /> structure, typically measured in pixels.</returns>
		public float Width
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

		/// <summary>Gets or sets the vertical component of this <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <returns>The vertical component of this <see cref="T:System.Drawing.SizeF" /> structure, typically measured in pixels.</returns>
		public float Height
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

		/// <summary>Adds the width and height of one <see cref="T:System.Drawing.SizeF" /> structure to the width and height of another <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="sz1">The first <see cref="T:System.Drawing.SizeF" /> structure to add.</param>
		/// <param name="sz2">The second <see cref="T:System.Drawing.SizeF" /> structure to add.</param>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> structure that is the result of the addition operation.</returns>
		public static SizeF Add(SizeF sz1, SizeF sz2)
		{
			return new SizeF(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
		}

		/// <summary>Subtracts the width and height of one <see cref="T:System.Drawing.SizeF" /> structure from the width and height of another <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="sz1">The <see cref="T:System.Drawing.SizeF" /> structure on the left side of the subtraction operator. </param>
		/// <param name="sz2">The <see cref="T:System.Drawing.SizeF" /> structure on the right side of the subtraction operator. </param>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> structure that is a result of the subtraction operation.</returns>
		public static SizeF Subtract(SizeF sz1, SizeF sz2)
		{
			return new SizeF(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
		}

		/// <summary>Tests to see whether the specified object is a <see cref="T:System.Drawing.SizeF" /> structure with the same dimensions as this <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Drawing.SizeF" /> and has the same width and height as this <see cref="T:System.Drawing.SizeF" />; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object? obj)
		{
			if (!(obj is SizeF))
			{
				return false;
			}
			SizeF sizeF = (SizeF)obj;
			return sizeF.Width == this.Width && sizeF.Height == this.Height && sizeF.GetType().Equals(base.GetType());
		}

		/// <summary>Returns a hash code for this <see cref="T:System.Drawing.Size" /> structure.</summary>
		/// <returns>An integer value that specifies a hash value for this <see cref="T:System.Drawing.Size" /> structure.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Converts a <see cref="T:System.Drawing.SizeF" /> structure to a <see cref="T:System.Drawing.PointF" /> structure.</summary>
		/// <returns>Returns a <see cref="T:System.Drawing.PointF" /> structure.</returns>
		public PointF ToPointF()
		{
			return (PointF)this;
		}

		/// <summary>Converts a <see cref="T:System.Drawing.SizeF" /> structure to a <see cref="T:System.Drawing.Size" /> structure.</summary>
		/// <returns>Returns a <see cref="T:System.Drawing.Size" /> structure.</returns>
		public Size ToSize()
		{
			return Size.Truncate(this);
		}

		/// <summary>Creates a human-readable string that represents this <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <returns>A string that represents this <see cref="T:System.Drawing.SizeF" /> structure.</returns>
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{Width=",
				this.width.ToString(CultureInfo.CurrentCulture),
				", Height=",
				this.height.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		/// <summary>Gets a <see cref="T:System.Drawing.SizeF" /> structure that has a <see cref="P:System.Drawing.SizeF.Height" /> and <see cref="P:System.Drawing.SizeF.Width" /> value of 0. </summary>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> structure that has a <see cref="P:System.Drawing.SizeF.Height" /> and <see cref="P:System.Drawing.SizeF.Width" /> value of 0.</returns>
		public static readonly SizeF Empty;

		private float width;

		private float height;
	}
}
