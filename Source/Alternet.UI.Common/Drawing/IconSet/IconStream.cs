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
    ========================
    16×16: Context menus, title bar, system tray.
    24×24: Toolbar buttons, taskbar (at 100% scaling).
    32×32: Desktop icons (classic size).
    48×48: Larger desktop icons, taskbar at higher DPI.
    256×256: Explorer large icon view, high DPI scaling.

    Best practice: include 16, 24, 32, 48, and 256px in one ICO file.
    This ensures Windows always has a pixel‑perfect match and avoids blurry scaling.
   
    Meaning of planes
    =================
    Planes = number of color planes in the image.

    Historically, this was used in very old graphics hardware that could split colors into separate planes.
    In practice, for Windows icons and bitmaps, planes is always 1.

    Relation to bitCount
    bitCount = bits per pixel (e.g., 1, 4, 8, 24, 32).
    Together, planes × bitCount gives the total bits per pixel.
    Example: planes = 1, bitCount = 32 → 32‑bit RGBA icon.

    Cursor Sizes
    ============
    16×16 (legacy, very small cursors)
    32×32 (standard Windows cursor size)
    48×48 or 64×64 (supported in newer Windows versions)

    */

    /// <summary>
    /// Represents helper class for reading icon data from a stream.
    /// </summary>
    public class IconStream : DisposableObject
    {
        /// <summary>
        /// The default minimum width for the preview image.
        /// </summary>
        public static int DefaultPreviewImageMinWidth = 300;

        private readonly List<IconEntry> list = new();
        private readonly bool isIcon;
        private readonly bool isCursor;

        /// <summary>
        /// Represents an entry in the icon stream.
        /// </summary>
        public class IconEntry
        {
            /// <summary>
            /// Gets the horizontal hotspot for cursor icons.
            /// </summary>
            public int? HotSpotX { get; set; }

            /// <summary>
            /// Gets the vertical hotspot for cursor icons.
            /// </summary>
            public int? HotSpotY { get; set; }

            /// <summary>
            /// Gets the number of color planes for the icon entry.
            /// </summary>
            public int? Planes { get; set; }

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
            public int? BitCount { get; set; }

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
                App.Log($"Width: {Width}, Height: {Height}, Size: {Size}, Offset: {Offset}");
                App.Log("Image: " + (Image != null ? "Loaded" : "Not Loaded"));
                App.Log("Image size: " + (Image != null ? $"{Image.Width}x{Image.Height}" : "N/A"));
                App.Log("Is PNG: " + (IsPng ? "Yes" : "No"));
                App.Log("HotSpotX: " + (HotSpotX.HasValue ? HotSpotX.Value.ToString() : "N/A"));
                App.Log("HotSpotY: " + (HotSpotY.HasValue ? HotSpotY.Value.ToString() : "N/A"));
                App.Log("Planes: " + (Planes.HasValue ? Planes.Value.ToString() : "N/A"));
                App.Log("BitCount: " + (BitCount.HasValue ? BitCount.Value.ToString() : "N/A"));
                App.LogEndSection();
            }
        }

        /// <summary>
        /// Gets whether the stream contains an icon.
        /// </summary>
        public bool IsIcon => isIcon;

        /// <summary>
        /// Gets whether the stream contains a cursor.
        /// </summary>
        public bool IsCursor => isCursor;

        /// <summary>
        /// Gets whether the stream contains unknown data.
        /// </summary>
        public bool IsUnknown => !isIcon && !isCursor;

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
            try
            {
                using var reader = new BinaryReader(stream);

                ushort reserved = reader.ReadUInt16();

                if (reserved != 0)
                    return;

                ushort type = reader.ReadUInt16();

                isIcon = type == 1;
                isCursor = type == 2;

                if (IsUnknown)
                    return;

                ushort count = reader.ReadUInt16();

                for (int i = 0; i < count; i++)
                {
                    byte width = reader.ReadByte();
                    byte height = reader.ReadByte();
                    byte colorCount = reader.ReadByte();
                    byte reservedByte = reader.ReadByte();

                    ushort? hotspotX = null;
                    ushort? hotspotY = null;
                    ushort? planes = null;
                    ushort? bitCount = null;

                    if (isCursor)
                    {
                        hotspotX = reader.ReadUInt16();
                        hotspotY = reader.ReadUInt16();
                    }
                    else
                    {
                        planes = reader.ReadUInt16();
                        bitCount = reader.ReadUInt16();
                    }

                    int size = reader.ReadInt32();
                    int offset = reader.ReadInt32();

                    long currentPos = reader.BaseStream.Position;

                    reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                    byte[] imageData = reader.ReadBytes(size);

                    var isPng = FileFormatDetector.IsPng(imageData);

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
                        HotSpotX = hotspotX,
                        HotSpotY = hotspotY,
                        Planes = planes,
                        Image = bmp,
                    });

                    reader.BaseStream.Seek(currentPos, SeekOrigin.Begin);
                }
            }
            catch
            {
                isIcon = false;
                isCursor = false;
                list.Clear();
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
                        var hotspotInfo = entry.HotSpotX.HasValue && entry.HotSpotY.HasValue
                            ? $"HotSpot: ({entry.HotSpotX.Value}, {entry.HotSpotY.Value})"
                            : null;
                        return $"{entry.Width}x{entry.Height}, {entry.BitCount} bit, {(entry.IsPng ? "PNG" : "BMP")}{hotspotInfo}";
                    }
                }

                return string.Empty;
            }

            foreach (var entry in entries)
            {
                if (entry.HotSpotX.HasValue && entry.HotSpotY.HasValue)
                {
                    var x = entry.HotSpotX.Value;
                    var y = entry.HotSpotY.Value;
                    entry.Image?.SetPixel(x, y, SKColors.Red);
                }
            }

            var combinedBitmap = SkiaUtils.CombineIconsVertically(
                images,
                LabelProviderFunc,
                minWidth ?? DefaultPreviewImageMinWidth);
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

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            foreach (var entry in Entries)
            {
                entry.Image?.Dispose();
                entry.Image = null;
            }

            list.Clear();

            base.DisposeManaged();
        }

        private static SKBitmap LoadBmpFromIco(byte[] dibData)
        {
            int infoHeaderSize = BitConverter.ToInt32(dibData, 0);
            int width = BitConverter.ToInt32(dibData, 4);
            int height = BitConverter.ToInt32(dibData, 8) / 2; // actual height
            ushort _ = BitConverter.ToUInt16(dibData, 12); // planes
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
