using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public class FileDialogFilterItem : BaseObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDialogFilterItem"/> struct.
        /// </summary>
        /// <param name="title">The Item title.</param>
        /// <param name="extensions">The array of file extensions (with or without ".").</param>
        public FileDialogFilterItem(string title, params string[] extensions)
        {
            Title = title;
            Extensions = extensions;
            ItemKind = Kind.Custom;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDialogFilterItem"/> class.
        /// </summary>
        /// <param name="kind">The kind of item.</param>
        public FileDialogFilterItem(Kind kind)
        {
            ItemKind = kind;
            Title = string.Empty;
            Extensions = [];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDialogFilterItem"/> class.
        /// </summary>
        public FileDialogFilterItem()
        {
            ItemKind = Kind.Custom;
            Title = string.Empty;
            Extensions = [];
        }

        public enum Kind
        {
            Custom,

            AllFiles,

            LibraryFiles,

            ImageOpen,

            ImageSave,
        }

        public string Title { get; set; }

        public string[] Extensions { get; set; }

        public Kind ItemKind { get; set; }

        /// <inheritdoc/>
        public override string? ToString()
        {
            switch (ItemKind)
            {
                case Kind.Custom:
                    if (Extensions.Length == 1)
                        return FileMaskUtils.GetFileDialogFilter(Title, Extensions[0]);
                    else
                    {
                        return FileMaskUtils.GetFileDialogFilter(Title, Extensions);
                    }

                case Kind.AllFiles:
                default:
                    return FileMaskUtils.FileDialogFilterAllFiles;
                case Kind.LibraryFiles:
                    return FileMaskUtils.FileDialogFilterLibraryFiles;
                case Kind.ImageOpen:
                    return FileMaskUtils.GetFileDialogFilterForImageOpen(addAllFiles: false);
                case Kind.ImageSave:
                    return FileMaskUtils.GetFileDialogFilterForImageSave();
            }
        }
    }
}
