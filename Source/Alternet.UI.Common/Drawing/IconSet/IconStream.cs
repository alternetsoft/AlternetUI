using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using SkiaSharp;

using Alternet.UI;
using System.Linq;

namespace Alternet.Drawing
{
    /*

    wxWidgets: In wxMSW, icons must contain a 16x16 or 32x32 icon, preferably both.

    Container format: ICO files can hold multiple images of different sizes and color depths.
    Transparency: Icons should have transparent backgrounds for best results.
    Compression: Modern ICOs use PNG compression for the 256×256 layer.
    Color depth: 32‑bit with alpha transparency is recommended.

    Minimum Sizes to Include
    16×16: Context menus, title bar, system tray.
    24×24: Toolbar buttons, taskbar (at 100% scaling).
    32×32: Desktop icons (classic size).
    48×48: Larger desktop icons, taskbar at higher DPI.
    256×256: Explorer large icon view, high DPI scaling.

    Best practice: include 16, 24, 32, 48, and 256px in one ICO file.
    This ensures Windows always has a pixel‑perfect match and avoids blurry scaling.
    */

    /// <summary>
    /// Represents helper class for reading icon data from a stream.
    /// </summary>
    public class IconStream
    {
        /// <summary>
        /// The default minimum width for the preview image.
        /// </summary>
        public static int DefaultPreviewImageMinWidth = 150;

        private readonly List<IconEntry> list = new ();

        /// <summary>
        /// Represents an entry in the icon stream.
        /// </summary>
        public class IconEntry
        {
            /// <summary>
            /// Gets or sets the width of the icon entry.
            /// </summary>
            public int Width { get; set; }

            /// <summary>
            /// Gets or sets the height of the icon entry.
            /// </summary>
            public int Height { get; set; }

            /// <summary>
            /// Gets or sets the color count of the icon entry.
            /// </summary>
            public int BitCount { get; set; }

            /// <summary>
            /// Gets or sets the size of the icon entry data.
            /// </summary>
            public int Size { get; set; }

            /// <summary>
            /// Gets or sets the offset of the icon entry data in the stream.
            /// </summary>
            public int Offset { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the icon entry is a PNG image.
            /// </summary>
            public bool IsPng { get; set; }

            /// <summary>
            /// Gets or sets the icon entry image.
            /// </summary>
            public SKBitmap? Image { get; set; }

            /// <summary>
            /// Logs the details of the icon entry for debugging purposes.
            /// </summary>
            public void Log()
            {
                App.LogBeginSection("IconEntry");
                App.Log($"Width: {Width}, Height: {Height}, BitCount: {BitCount}, Size: {Size}, Offset: {Offset}");
                App.Log("Image: " + (Image != null ? "Loaded" : "Not Loaded"));
                App.Log("Image size: " + (Image != null ? $"{Image.Width}x{Image.Height}" : "N/A"));
                App.Log("Is PNG: " + (IsPng ? "Yes" : "No"));
                App.LogEndSection();
            }
        }

        /// <summary>
        /// Gets the list of icon entries contained in the stream.
        /// </summary>
        public IReadOnlyList<IconEntry> Entries { get => list; }

        /// <summary>
        /// Gets the bitmaps of the icon entries contained in the stream.
        /// </summary>
        public IEnumerable<SKBitmap> Bitmaps
        {
            get
            {
                foreach (var entry in Entries)
                {
                    if (entry.Image != null)
                        yield return entry.Image;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconStream"/> class by reading icon entries from the specified stream.
        /// </summary>
        /// <param name="stream">The stream containing the icon data.</param>
        public IconStream(Stream stream)
        {
            using var reader = new BinaryReader(stream);

            ushort reserved = reader.ReadUInt16(); // always 0
            ushort type = reader.ReadUInt16();     // 1 = icon, 2 = cursor
            ushort count = reader.ReadUInt16();    // number of images

            if (type != 1)
                count = 0;

            for (int i = 0; i < count; i++)
            {
                byte width = reader.ReadByte();
                byte height = reader.ReadByte();
                byte colorCount = reader.ReadByte();
                byte reservedByte = reader.ReadByte();
                ushort planes = reader.ReadUInt16();
                ushort bitCount = reader.ReadUInt16();
                int size = reader.ReadInt32();
                int offset = reader.ReadInt32();

                long currentPos = reader.BaseStream.Position;

                reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                byte[] imageData = reader.ReadBytes(size);

                var isPng = IsPng(imageData);

                SKBitmap bmp = isPng
                    ? SKBitmap.Decode(imageData)
                    : LoadBmpFromIco(imageData);

                list.Add(new IconEntry
                {
                    Width = width == 0 ? 256 : width,
                    Height = height == 0 ? 256 : height,
                    BitCount = bitCount,
                    Size = size,
                    Offset = offset,
                    IsPng = isPng,
                    Image = bmp,
                });

                reader.BaseStream.Seek(currentPos, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Creates temporary preview image for the specified icon.
        /// </summary>
        /// <param name="iconPath">The path to the icon file.</param>
        /// <param name="minWidth">The minimum width of the resulting bitmap.</param>
        /// <returns>The path to the temporary preview image.</returns>
        public static string CreateTempPreviewImage(string iconPath, int? minWidth = null)
        {
            var tempPath = PathUtils.GenTempFileName(".png");
            var image = CreatePreviewImage(iconPath, minWidth);
            SkiaUtils.SaveBitmapToPng(image, tempPath);
            return tempPath;
        }

        /// <summary>
        /// Creates a preview bitmap of the icon.
        /// </summary>
        /// <param name="filePath">The path to the icon file.</param>
        /// <param name="minWidth">The minimum width of the resulting bitmap.</param>
        /// <returns>A <see cref="SKBitmap"/> representing the preview image.</returns>
        public static SKBitmap CreatePreviewImage(string filePath, int? minWidth = null)
        {
            using var stream = File.OpenRead(filePath);
            return CreatePreviewImage(stream, minWidth);
        }

        /// <summary>
        /// Creates a preview image for the specified icon.
        /// </summary>
        /// <param name="stream">The stream containing the icon data.</param>
        /// <param name="minWidth">The minimum width of the resulting bitmap.</param>
        /// <returns>A <see cref="SKBitmap"/> representing the preview image.</returns>
        public static SKBitmap CreatePreviewImage(Stream stream, int? minWidth = null)
        {
            var entries = LoadEntriesFromStream(stream);
            IEnumerable<SKBitmap> images = entries.Select(e => e.Image).Where(img => img != null).ToList()!;

            string LabelProviderFunc(SKBitmap bitmap)
            {
                foreach (var entry in entries)
                {
                    if (entry.Image == bitmap)
                    {
                        return $"{entry.Width}x{entry.Height}, {entry.BitCount} bit, {(entry.IsPng ? "PNG" : "BMP")}";
                    }
                }

                return string.Empty;
            }

            var combinedBitmap = SkiaUtils.CombineIconsVertically(images, LabelProviderFunc, minWidth ?? DefaultPreviewImageMinWidth);
            return combinedBitmap;
        }

        /// <summary>
        /// Loads icon images from the specified stream.
        /// </summary>
        /// <param name="stream">The stream containing the icon data.</param>
        /// <returns>An enumerable of <see cref="SKBitmap"/> objects representing the icon images.</returns>
        public static IEnumerable<SKBitmap> LoadFromStream(Stream stream)
        {
            IEnumerable<SKBitmap> LoadBitmaps()
            {
                var iconStream = new IconStream(stream);
                foreach (var entry in iconStream.Entries)
                {
                    if (entry.Image != null)
                        yield return entry.Image;
                }
            }

            try
            {
                return LoadBitmaps().ToList();
            }
            catch
            {
                return Array.Empty<SKBitmap>();
            }
        }

        /// <summary>
        /// Loads icon entries from the specified stream.
        /// </summary>
        /// <param name="stream">The stream containing the icon data.</param>
        /// <returns>An enumerable of <see cref="IconEntry"/> objects representing the icon entries.</returns>
        public static IEnumerable<IconEntry> LoadEntriesFromStream(Stream stream)
        {
            IEnumerable<IconEntry> LoadEntries()
            {
                var iconStream = new IconStream(stream);
                foreach (var entry in iconStream.Entries)
                {
                    if (entry.Image != null)
                        yield return entry;
                }
            }

            try
            {
                return LoadEntries().ToList();
            }
            catch
            {
                return Array.Empty<IconEntry>();
            }
        }

        private static bool IsPng(byte[] data)
        {
            // PNG signature: 89 50 4E 47 0D 0A 1A 0A
            return data.Length > 8 &&
                   data[0] == 0x89 &&
                   data[1] == 0x50 &&
                   data[2] == 0x4E &&
                   data[3] == 0x47 &&
                   data[4] == 0x0D &&
                   data[5] == 0x0A &&
                   data[6] == 0x1A &&
                   data[7] == 0x0A;
        }

        private static SKBitmap LoadBmpFromIco(byte[] dibData)
        {
            int infoHeaderSize = BitConverter.ToInt32(dibData, 0);
            int width = BitConverter.ToInt32(dibData, 4);
            int height = BitConverter.ToInt32(dibData, 8) / 2; // actual height
            ushort planes = BitConverter.ToUInt16(dibData, 12);
            ushort bitCount = BitConverter.ToUInt16(dibData, 14);

            // Palette size
            int paletteEntries = (bitCount <= 8) ? (1 << bitCount) : 0;
            int paletteSize = paletteEntries * 4;

            // Pixel data offset
            int xorOffset = infoHeaderSize + paletteSize;

            // Stride (rows padded to 4 bytes)
            int bytesPerPixel = (bitCount >= 24) ? (bitCount / 8) : 0;
            int stride = (bitCount >= 24)
                ? ((width * bytesPerPixel + 3) / 4) * 4
                : ((width * bitCount + 31) / 32) * 4;

            var bmp = new SKBitmap(width, height);

            // --- Decode palette if needed ---
            SKColor[]? palette = null;
            if (bitCount <= 8)
            {
                palette = new SKColor[paletteEntries];
                for (int i = 0; i < paletteEntries; i++)
                {
                    int idx = infoHeaderSize + i * 4;
                    byte b = dibData[idx + 0];
                    byte g = dibData[idx + 1];
                    byte r = dibData[idx + 2];
                    palette[i] = new SKColor(r, g, b);
                }
            }

            // --- Decode XOR bitmap (color image) ---
            for (int y = 0; y < height; y++)
            {
                int srcRow = xorOffset + (height - 1 - y) * stride;

                for (int x = 0; x < width; x++)
                {
                    SKColor color;

                    if (bitCount == 32)
                    {
                        int pixelIndex = srcRow + x * 4;
                        byte b = dibData[pixelIndex + 0];
                        byte g = dibData[pixelIndex + 1];
                        byte r = dibData[pixelIndex + 2];
                        byte a = dibData[pixelIndex + 3];
                        color = new SKColor(r, g, b, a);
                    }
                    else if (bitCount == 24)
                    {
                        int pixelIndex = srcRow + x * 3;
                        byte b = dibData[pixelIndex + 0];
                        byte g = dibData[pixelIndex + 1];
                        byte r = dibData[pixelIndex + 2];
                        color = new SKColor(r, g, b, 255);
                    }
                    else if (bitCount == 8)
                    {
                        byte index = dibData[srcRow + x];
                        color = palette![index];
                    }
                    else if (bitCount == 4)
                    {
                        byte bVal = dibData[srcRow + (x / 2)];
                        byte index = (x % 2 == 0) ? (byte)(bVal >> 4) : (byte)(bVal & 0x0F);
                        color = palette![index];
                    }
                    else if (bitCount == 1)
                    {
                        byte bVal = dibData[srcRow + (x / 8)];
                        int bitIndex = 7 - (x % 8);
                        byte index = (byte)((bVal >> bitIndex) & 1);
                        color = palette![index];
                    }
                    else
                    {
                        throw new NotSupportedException($"Unsupported bit depth: {bitCount}");
                    }

                    bmp.SetPixel(x, y, color);
                }
            }

            // --- Apply AND mask for transparency (if no alpha channel) ---
            if (bitCount < 32)
            {
                int xorSize = stride * height;
                int maskOffset = xorOffset + xorSize;
                int maskStride = ((width + 31) / 32) * 4;

                for (int y = 0; y < height; y++)
                {
                    int rowStart = maskOffset + (height - 1 - y) * maskStride;
                    for (int x = 0; x < width; x++)
                    {
                        int byteIndex = rowStart + (x / 8);
                        int bitIndex = 7 - (x % 8);
                        bool transparent = ((dibData[byteIndex] >> bitIndex) & 1) == 1;

                        if (transparent)
                        {
                            var c = bmp.GetPixel(x, y);
                            bmp.SetPixel(x, y, new SKColor(c.Red, c.Green, c.Blue, 0));
                        }
                    }
                }
            }

            return bmp;
        }
    }
}
