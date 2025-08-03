using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements item of the <see cref="FileListBox"/> control.
    /// </summary>
    public class FileListBoxItem : TreeViewItem
    {
        private bool? isFolder;
        private bool? isFile;
        private FileSystemInfo? info;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileListBoxItem"/> class.
        /// </summary>
        /// <param name="title">Title of the item.</param>
        /// <param name="path">Path to file or folder.</param>
        /// <param name="doubleClick">Double click action.</param>
        public FileListBoxItem(string? title, string? path, Action? doubleClick = null)
        {
            Text = title ?? System.IO.Path.GetFileName(path) ?? string.Empty;
            Path = path;
            DoubleClickAction = doubleClick;
        }

        /// <summary>
        /// Gets or sets path to the file.
        /// </summary>
        public virtual string? Path { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current path represents a folder.
        /// </summary>
        public virtual bool IsFolder
        {
            get
            {
                try
                {
                    return isFolder ??= Directory.Exists(Path);
                }
                catch (Exception e)
                {
                    Nop(e);
                    isFolder = false;
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current item represents a file.
        /// </summary>
        public virtual bool IsFile
        {
            get
            {
                if (isFolder is not null && isFolder.Value)
                    return false;
                try
                {
                    return isFile ??= File.Exists(Path);
                }
                catch (Exception e)
                {
                    Nop(e);
                    isFile = false;
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="FileSystemInfo"/> object representing the
        /// file or directory associated with the current instance.
        /// </summary>
        /// <remarks>The returned object is either a <see cref="FileInfo"/> if the path represents
        /// a file, or a <see cref="DirectoryInfo"/> if the path represents a directory.
        /// If the path is invalid or
        /// an exception occurs during initialization,
        /// the property returns <see langword="null"/>.</remarks>
        public virtual FileSystemInfo? Info
        {
            get
            {
                try
                {
                    if (IsFile)
                    {
                        info ??= new FileInfo(Path);
                    }
                    else
                    if (IsFolder)
                    {
                        info ??= new DirectoryInfo(Path);
                    }
                }
                catch (Exception e)
                {
                    Nop(e);
                    info = null;
                }

                return info;
            }
        }

        /// <summary>
        /// Gets the size of the file in bytes, or <see langword="null"/>
        /// if the size cannot be determined.
        /// </summary>
        /// <remarks>This property returns <see langword="null"/> if the instance does not
        /// represent a file or if an error occurs
        /// while attempting to retrieve the file size.</remarks>
        public long? Size
        {
            get
            {
                if (IsFile)
                {
                    try
                    {
                        return Info is FileInfo fileInfo ? fileInfo.Length : null;
                    }
                    catch (Exception e)
                    {
                        Nop(e);
                        return null;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets extension in the lower case and without "." character.
        /// </summary>
        [Browsable(false)]
        public virtual string ExtensionLower => PathUtils.GetExtensionLower(Path);

        /// <summary>
        /// Compares two <see cref="FileListBoxItem"/> objects based on whether
        /// they represent folders.
        /// </summary>
        /// <remarks>This method prioritizes folder items over non-folder items.
        /// If both items are of the same type
        /// (either both folders or both non-folders), they are considered equal
        /// in this comparison.</remarks>
        /// <param name="x">The first <see cref="FileListBoxItem"/> to compare.
        /// Can not be <see langword="null"/>.</param>
        /// <param name="y">The second <see cref="FileListBoxItem"/> to compare.
        /// Can not be <see langword="null"/>.</param>
        /// <returns>A signed integer that indicates the relative order of the objects:
        /// <list type="bullet">
        /// <item><description>-1 if <paramref name="x"/> is a folder and <paramref name="y"/>
        /// is not.</description></item>
        /// <item><description>1 if <paramref name="y"/> is a folder and
        /// <paramref name="x"/> is not.</description></item>
        /// <item><description>0 if both objects are either folders or
        /// non-folders.</description></item> </list></returns>
        public static int CompareByIsFolder(FileListBoxItem x, FileListBoxItem y)
        {
            var xIsFolder = x.IsFolder;
            var yIsFolder = y.IsFolder;

            if (xIsFolder && !yIsFolder)
                return -1;
            if (!xIsFolder && yIsFolder)
                return 1;
            return 0;
        }

        /// <summary>
        /// Compares two <see cref="FileListBoxItem"/> objects based on their
        /// folder status and text values.
        /// </summary>
        /// <remarks>The comparison prioritizes folder status, ensuring folders are ordered before
        /// non-folders.  If both items have the same folder status, their
        /// <see cref="ListControlItem.Text"/> values
        /// are compared using a case-insensitive, ordinal string comparison.</remarks>
        /// <param name="x">The first <see cref="FileListBoxItem"/> to compare.
        /// Cannot be <c>null</c>.</param>
        /// <param name="y">The second <see cref="FileListBoxItem"/> to compare.
        /// Cannot be <c>null</c>.</param>
        /// <returns>A signed integer that indicates the relative order of the objects being
        /// compared: <list type="bullet">
        /// <item><description>Less than zero if <paramref name="x"/> precedes <paramref
        /// name="y"/>.</description></item> <item><description>Zero if <paramref name="x"/>
        /// and <paramref name="y"/> are equal.</description></item>
        /// <item><description>Greater than zero if <paramref name="x"/>
        /// follows <paramref name="y"/>.</description></item> </list></returns>
        public static int ComparisonByText(FileListBoxItem x, FileListBoxItem y)
        {
            var result = CompareUtils.CompareByPriority(
                x,
                y,
                CompareTextRank,
                CompareByIsFolder,
                CompareTextOnly);
            return result;
        }

        /// <summary>
        /// Same as <see cref="ComparisonByText"/>, but compares in descending order.
        /// </summary>
        /// <param name="x">The first <see cref="FileListBoxItem"/> to compare.</param>
        /// <param name="y">The second <see cref="FileListBoxItem"/> to compare.</param>
        /// <returns>A signed integer that indicates the relative order
        /// of the objects being compared.</returns>
        public static int ComparisonByTextDescending(FileListBoxItem x, FileListBoxItem y)
        {
            var result = CompareUtils.CompareByPriority(
                x,
                y,
                CompareTextRank,
                CompareByIsFolder,
                CompareTextOnlyDescending);
            return result;
        }

        /// <summary>
        /// Compares two <see cref="FileListBoxItem"/> objects based on their folder status,
        /// size, and text.
        /// </summary>
        /// <remarks>The comparison is performed in the following order:
        /// <list type="number">
        /// <item><description>Folders are prioritized over non-folders.</description></item>
        /// <item><description>If
        /// both items are folders or both are non-folders, their sizes are compared. If the size
        /// is <c>null</c>, it
        /// is treated as 0.</description></item> <item><description>If the sizes are equal,
        /// their text values are
        /// compared using a case-insensitive ordinal comparison.</description></item> </list>
        /// </remarks>
        /// <param name="x">The first <see cref="FileListBoxItem"/> to compare.
        /// Can not be <c>null</c>.</param>
        /// <param name="y">The second <see cref="FileListBoxItem"/> to compare.
        /// Can not be <c>null</c>.</param>
        /// <returns>A signed integer that indicates the relative order of the objects:
        /// <list type="bullet">
        /// <item><description>Less than zero if <paramref name="x"/> is less than
        /// <paramref
        /// name="y"/>.</description></item>
        /// <item><description>Zero if <paramref name="x"/> is equal to
        /// <paramref name="y"/>.</description></item>
        /// <item><description>Greater than zero if <paramref name="x"/> is greater
        /// than <paramref name="y"/>.</description></item> </list></returns>
        public static int ComparisonBySize(FileListBoxItem x, FileListBoxItem y)
        {
            var result = CompareUtils.CompareByPriority(
                x,
                y,
                CompareTextRank,
                CompareByIsFolder,
                CompareSizeOnly,
                CompareTextOnly);
            return result;
        }

        /// <summary>
        /// Compares two <see cref="FileListBoxItem"/> objects based on their date modified
        /// and other criteria.
        /// </summary>
        /// <remarks>The comparison considers multiple criteria, including whether the items are folders,
        /// their text rank, and their date modified. The exact priority of these criteria
        /// is determined by the
        /// internal comparison logic.</remarks>
        /// <param name="x">The first <see cref="FileListBoxItem"/> to compare.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="y">The second <see cref="FileListBoxItem"/> to compare.
        /// Cannot be <see langword="null"/>.</param>
        /// <returns>A signed integer that indicates the relative order of the two objects:
        /// <list type="bullet">
        /// <item><description>Less than zero if <paramref name="x"/> is less than
        /// <paramref name="y"/>.</description></item> <item><description>Zero if <paramref name="x"/>
        /// is equal to <paramref name="y"/>.</description></item>
        /// <item><description>Greater than zero if <paramref name="x"/> is greater
        /// than <paramref name="y"/>.</description></item> </list></returns>
        public static int ComparisonByDateModified(FileListBoxItem x, FileListBoxItem y)
        {
            var result = CompareUtils.CompareByPriority(
                x,
                y,
                CompareTextRank,
                CompareByIsFolder,
                CompareDateModifiedOnly,
                CompareTextOnly);
            return result;
        }

        /// <summary>
        /// Same as <see cref="ComparisonByDateModified"/>, but compares in descending order.
        /// </summary>
        /// <param name="x">The first <see cref="FileListBoxItem"/> to compare.</param>
        /// <param name="y">The second <see cref="FileListBoxItem"/> to compare.</param>
        /// <returns>A signed integer that indicates the relative order
        /// of the objects being compared.</returns>
        public static int ComparisonByDateModifiedDescending(FileListBoxItem x, FileListBoxItem y)
        {
            var result = CompareUtils.CompareByPriority(
                x,
                y,
                CompareTextRank,
                CompareByIsFolder,
                CompareDateModifiedOnlyDescending,
                CompareTextOnly);
            return result;
        }

        /// <summary>
        /// Same as <see cref="ComparisonBySize"/>, but compares in descending order.
        /// </summary>
        /// <param name="x">The first <see cref="FileListBoxItem"/> to compare.</param>
        /// <param name="y">The second <see cref="FileListBoxItem"/> to compare.</param>
        /// <returns>A signed integer that indicates the relative order
        /// of the objects being compared.</returns>
        public static int ComparisonBySizeDescending(FileListBoxItem x, FileListBoxItem y)
        {
            var result = CompareUtils.CompareByPriority(
                x,
                y,
                CompareTextRank,
                CompareByIsFolder,
                CompareSizeOnlyDescending,
                CompareTextOnly);
            return result;
        }

        private static int GetTextRank(FileListBoxItem item)
        {
            if (item.Text == "/")
            {
                return 0;
            }

            if (item.Text == "..")
            {
                return 1;
            }

            return 2;
        }

        private static int CompareTextRank(FileListBoxItem x, FileListBoxItem y)
        {
            var xRank = GetTextRank(x);
            var yRank = GetTextRank(y);

            return xRank.CompareTo(yRank);
        }

        private static int CompareTextOnly(FileListBoxItem x, FileListBoxItem y)
        {
            return string.Compare(x.Text, y.Text, StringComparison.OrdinalIgnoreCase);
        }

        private static int CompareTextOnlyDescending(FileListBoxItem x, FileListBoxItem y)
        {
            return -CompareTextOnly(x, y);
        }

        private static int CompareSizeOnlyDescending(FileListBoxItem x, FileListBoxItem y)
        {
            var result = CompareSizeOnly(x, y);
            return -result;
        }

        private static int CompareDateModifiedOnly(FileListBoxItem x, FileListBoxItem y)
        {
            var xDate = x.Info?.LastWriteTime ?? DateTime.MinValue;
            var yDate = y.Info?.LastWriteTime ?? DateTime.MinValue;
            return xDate.CompareTo(yDate);
        }

        private static int CompareDateModifiedOnlyDescending(FileListBoxItem x, FileListBoxItem y)
        {
            var result = CompareDateModifiedOnly(x, y);
            return -result;
        }

        private static int CompareSizeOnly(FileListBoxItem x, FileListBoxItem y)
        {
            var xSize = x.Size ?? 0;
            var ySize = y.Size ?? 0;

            var result = xSize.CompareTo(ySize);
            return result;
        }
    }
}
