using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a filter item for file dialogs, allowing to specify a title and file extensions.
    /// </summary>
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

        /// <summary>
        /// Enumerates the kinds of file dialog filter items.
        /// </summary>
        public enum Kind
        {
            /// <summary>
            /// Item is a custom file dialog filter.
            /// </summary>
            Custom,

            /// <summary>
            /// Item is 'All Files'.
            /// </summary>
            AllFiles,

            /// <summary>
            /// Item is 'Library Files'.
            /// </summary>
            LibraryFiles,

            /// <summary>
            /// Item is for opening images.
            /// </summary>
            ImageOpen,

            /// <summary>
            /// Item is for saving images.
            /// </summary>
            ImageSave,
        }

        /// <summary>
        /// Gets or sets the title of the item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the file extensions associated with the current operation.
        /// </summary>
        public string[] Extensions { get; set; }

        /// <summary>
        /// Gets or sets the kind of item represented by this instance.
        /// </summary>
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
