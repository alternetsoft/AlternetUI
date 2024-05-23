using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Allows you to specify an icon to represent a control in a container,
    /// such as a form designer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ToolboxBitmapAttribute : Attribute
    {
        private readonly string? imageFile;
        private readonly Type? imageType;
        private readonly string? imageName;

        static ToolboxBitmapAttribute()
        {
        }

        /// <summary>Initializes a new <see cref="ToolboxBitmapAttribute" /> object
        /// with an image from a specified file.</summary>
        /// <param name="imageFile">The name of a file that contains a bitmap.</param>
        public ToolboxBitmapAttribute(string imageFile)
        {
            this.imageFile = imageFile;
        }

        /// <summary>
        /// Initializes a new <see cref="ToolboxBitmapAttribute" /> object based
        /// on a bitmap that is embedded as a resource in a specified
        /// assembly.
        /// </summary>
        /// <param name="t">A <see cref="Type" /> whose defining assembly is searched
        /// for the bitmap resource.</param>
        public ToolboxBitmapAttribute(Type t)
        {
            imageType = t;
        }

        /// <summary>
        /// Initializes a new <see cref="ToolboxBitmapAttribute" /> object based
        /// on a bitmap that is embedded as a resource in a
        /// specified assembly.</summary>
        /// <param name="t">A <see cref="Type" /> whose defining assembly is
        /// searched for the bitmap resource.</param>
        /// <param name="name">The name of the embedded bitmap resource.</param>
        public ToolboxBitmapAttribute(Type t, string name)
        {
            imageType = t;
            imageName = name;
        }

        /// <summary>
        /// Gets the name of a file that contains a bitmap.
        /// </summary>
        public string? ImageFile => imageFile;

        /// <summary>
        /// Gets a type whose defining assembly is searched for the bitmap resource.
        /// </summary>
        public Type? ImageType => imageType;

        /// <summary>
        /// Gets the name of the embedded bitmap resource.
        /// </summary>
        public string? ImageName => imageName;
    }
}
