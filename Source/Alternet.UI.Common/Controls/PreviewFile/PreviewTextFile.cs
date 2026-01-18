using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements preview control which uses <see cref="MultilineTextBox"/> for the preview
    /// of the files.
    /// </summary>
    public partial class PreviewTextFile : HiddenBorder, IFilePreview
    {
        /// <summary>
        /// Gets or sets default encoding for the "txt" files.
        /// </summary>
        public static Encoding? DefaultTextEncoding;

        /// <summary>
        /// Gets or sets default encoding for the all files except "txt".
        /// </summary>
        public static Encoding? DefaultOtherEncoding;

        /// <summary>
        /// Gets or sets list of file extension which
        /// are supported in <see cref="PreviewTextFile"/>.
        /// </summary>
        /// <remarks>
        /// Extensions must be in the lower case and without "." character.
        /// </remarks>
        public static List<string> SupportedExtensions = new()
        {
            "txt",
            "ini",
            "editorconfig",
            "gitattributes",
            "gitmodules",
            "gitignore",
            "manifest",
            "md",
            "ps1",
            "vbs",
            "h",
            "cpp",
            "c",
            "csproj",
            "vbproj",
            "vb",
            "fb2",
            "vsixmanifest",
            "json",
            "yml",
            "proj",
            "config",
            "sln",
            "props",
            "targets",
            "bat",
            "sh",
            "py",
            "xml",
            "js",
            "cs",
            "css",
        };

        private readonly MultilineTextBox textBox = new()
        {
            HasBorder = false,
            ReadOnly = true,
        };

        private string? fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewTextFile"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PreviewTextFile(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewTextFile"/> class.
        /// </summary>
        public PreviewTextFile()
        {
            textBox.Parent = this;
        }

        /// <summary>
        /// Occurs when preview control needs to get encoding for the
        /// previewed file.
        /// </summary>
        public static event EventHandler<QueryEncodingEventArgs>? QueryEncoding;

        /// <summary>
        /// <inheritdoc cref="IFilePreview.Control"/>
        /// </summary>
        AbstractControl IFilePreview.Control => this;

        /// <summary>
        /// <inheritdoc cref="IFilePreview.FileName"/>
        /// </summary>
        public virtual string? FileName
        {
            get => fileName;
            set
            {
                if (fileName == value)
                    return;
                fileName = value;
                Reload();
            }
        }

        /// <summary>
        /// Gets whether specified file is supported in this preview control.
        /// </summary>
        /// <param name="fileName">Path to file.</param>
        /// <returns></returns>
        public static bool IsSupportedFile(string fileName)
        {
            var ext = PathUtils.GetExtensionLower(fileName);
            var equals = EnumerableUtils.EqualsRange(ext, SupportedExtensions);
            return equals;
        }

        /// <summary>
        /// Creates this preview control.
        /// </summary>
        /// <returns></returns>
        public static IFilePreview CreatePreviewControl()
        {
            return new PreviewTextFile();
        }

        /// <summary>
        /// Reloads the file which is currently previewed.
        /// </summary>
        public virtual void Reload()
        {
            textBox.Text = string.Empty;

            App.AddBackgroundAction(Internal);

            void Internal()
            {
                if (FileName is null || !GetFileSystem().FileExists(FileName))
                {
                    return;
                }

                using var stream = GetFileSystem().OpenRead(FileName);

                Encoding? encoding;

                if (QueryEncoding is not null)
                {
                    QueryEncodingEventArgs args = new(FileName);
                    QueryEncoding(this, args);
                    encoding = args.Result;
                }
                else
                {
                    var ext = PathUtils.GetExtensionLower(fileName);

                    if (ext == "txt")
                        encoding = DefaultTextEncoding ?? Encoding.Default;
                    else
                        encoding = DefaultOtherEncoding ?? Encoding.UTF8;
                }

                encoding ??= Encoding.UTF8;

                using var reader = new StreamReader(stream, encoding);

                var s = reader.ReadToEnd();

                textBox.Text = s;
            }
        }
    }
}
