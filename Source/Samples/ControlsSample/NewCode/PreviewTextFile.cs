using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class PreviewTextFile : Control, IFilePreview
    {
        public static event EventHandler<QueryEncodingEventArgs>? QueryEncoding;

        public static Encoding? DefaultTextEncoding;

        public static Encoding? DefaultOtherEncoding;
        
        public static List<string> SupportedExtensions = new()
        {
            "txt",
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
            "json",
            "yml",
            "proj",
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
        };

        private string? fileName;

        public PreviewTextFile()
        {
            textBox.Parent = this;
        }

        public string? FileName
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

        public static bool IsSupportedFile(string fileName)
        {
            var ext = PathUtils.GetExtensionLower(fileName);
            var equals = EnumerableUtils.EqualsRange(ext, SupportedExtensions);
            return equals;
        }

        public static IFilePreview CreatePreviewControl()
        {
            return new PreviewTextFile();
        }

        public void Reload()
        {
            textBox.Text = string.Empty;

            if (FileName is null || !File.Exists(FileName))
            {
                return;
            }

            using var stream = File.OpenRead(FileName);

            Encoding? encoding;

            if(QueryEncoding is not null)
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

        Control IFilePreview.Control => this;
    }
}
