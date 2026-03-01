using System;
using System.IO;
using SkiaSharp;

using Alternet.UI;

namespace Alternet.Drawing;

/// <summary>
/// Provides static methods for extracting individual frames from animated images.
/// </summary>
/// <remarks>The AnimatedImageExtractor class enables users to extract all frames from a GIF file, either by
/// specifying a file path or by providing a stream. Each extracted frame is returned as an AnimatedImageFrameInfo
/// object, which contains the frame's bitmap, duration, and related metadata. This class is useful for scenarios where
/// frame-by-frame access to animated images is required, such as custom animation playback or image analysis.</remarks>
public static class AnimatedImageExtractor
{
    /// <summary>
    /// Logs information about each frame extracted from a GIF located at the specified URL using the provided log
    /// writer.
    /// </summary>
    /// <remarks>If no frames are available for the specified URL, a message is logged indicating the absence
    /// of frames. Each frame is logged in a separate section for clarity.</remarks>
    /// <param name="url">The URL of the GIF from which to extract frames. This parameter can be null, in which case no frames will be
    /// logged.</param>
    /// <param name="logWriter">An optional log writer used to output frame information. If not specified,
    /// a default debug log writer is used.</param>
    /// <param name="maxFrames">The maximum number of frames to log. If the number of frames exceeds this value,
    /// only the specified number of frames will be logged.</param>
    public static void LogFrames(string? url, int maxFrames, ILogWriter? logWriter = null)
    {
        logWriter ??= LogWriter.Debug;

        var frames = ExtractFramesFromGif(url);
        if (frames == null || frames.Length == 0)
        {
            logWriter?.WriteLine($"No frames available for URL: {url}");
            return;
        }

        var effective = Math.Min(frames.Length, maxFrames);

        logWriter.BeginSection($"Logging {effective} of {frames.Length} frames for URL: {url}");

        for (int i = 0; i < effective; i++)
        {
            var frame = frames[i];

            logWriter.BeginSection($"Logging {i} frame");
            LogFrame(frame, logWriter);
            logWriter.EndSection();
        }

        logWriter.EndSection();
    }

    /// <summary>
    /// Logs detailed information about a specific animation frame identified by the specified URL and frame index.
    /// </summary>
    /// <remarks>If no frame information is available for the given URL and frame index, a message indicating
    /// the absence of information is logged instead.</remarks>
    /// <param name="logWriter">An optional log writer used to output the frame information. If not specified,
    /// the default debug log writer is used.</param>
    /// <param name="info">Frame information.</param>
    public static void LogFrame(AnimatedImageFrameInfo? info, ILogWriter? logWriter = null)
    {
        logWriter ??= LogWriter.Debug;

        if (info == null)
        {
            return;
        }

        logWriter.WriteLine($"Duration: {info.Duration} ms");
        logWriter.WriteLine($"Required Frame Index: {info.RequiredFrame}");
        logWriter.WriteLine($"Alpha Type: {info.AlphaType}");
        logWriter.WriteLine($"Original Bitmap Size: {(info.OriginalBitmap != null ? $"{info.OriginalBitmap.Width}x{info.OriginalBitmap.Height}" : "null")}");
        logWriter.WriteLine($"Combined Bitmap Size: {(info.CombinedBitmap != null ? $"{info.CombinedBitmap.Width}x{info.CombinedBitmap.Height}" : "null")}");
    }

    /// <summary>
    /// Retrieves a bitmap representing a specific frame from a GIF image located at the specified URL.
    /// </summary>
    /// <remarks>This method extracts all frames from the GIF and returns the bitmap for the specified frame
    /// index. Ensure that the URL points to a valid GIF image.</remarks>
    /// <param name="url">The URL of the GIF image from which to extract frames. Must reference a valid GIF file.</param>
    /// <param name="frameIndex">The zero-based index of the frame to retrieve.
    /// Must be within the range of available frames in the GIF.</param>
    /// <returns>An SKBitmap containing the image data for the specified frame, or null if the frame cannot be retrieved.</returns>
    public static SKBitmap? GetFrame(string? url, int frameIndex = 0)
    {
        var frame = GetFrameInfo(url, frameIndex);
        return frame?.CombinedBitmap;
    }

    /// <summary>
    /// Retrieves information about a specific frame from an animated image at the specified URL.
    /// </summary>
    /// <remarks>If the URL is null or the frame index is invalid, the method returns null. This method is
    /// useful for accessing individual frames in animated images, such as GIFs.</remarks>
    /// <param name="url">The URL of the animated image from which to extract frame information. This parameter cannot be null.</param>
    /// <param name="frameIndex">The zero-based index of the frame to retrieve. Must be within the range of available frames.</param>
    /// <returns>An instance of AnimatedImageFrameInfo containing details of the specified frame, or null if the URL is null or
    /// the frame index is out of range.</returns>
    public static AnimatedImageFrameInfo? GetFrameInfo(string? url, int frameIndex = 0)
    {
        if (url == null)
            return null;
        var frames = ExtractFramesFromGif(url);
        if (frameIndex < 0 || frameIndex >= frames.Length)
            return null;
        var frame = frames[frameIndex];
        return frame;
    }

    /// <summary>
    /// Extracts all frames from a GIF image file at the specified URL or path.
    /// In order to build file name into url, use <see cref="CommonUtils.PrepareFileUrl"/>.
    /// </summary>
    /// <remarks>This method opens the specified file for reading and extracts its frames. The file must be in
    /// a valid GIF format.</remarks>
    /// <param name="url">The resource URL or path to the GIF file to extract frames from. The file must exist and be accessible.</param>
    /// <returns>An array of AnimatedImageFrameInfo objects, each representing a frame extracted from the GIF image.</returns>
    public static AnimatedImageFrameInfo[] ExtractFramesFromGif(string? url)
    {
        using var stream = ResourceLoader.StreamFromUrlOrDefault(url);
        if (stream is null)
            return [];
        return ExtractFramesFromGif(stream);
    }

    /// <summary>
    /// Extracts all frames from a GIF image stream and returns an array containing information about each frame,
    /// including composited and original bitmaps, duration, and frame dependencies.
    /// </summary>
    /// <remarks>The method seeks to the beginning of the stream if it is seekable. Each
    /// AnimatedImageFrameInfo in the returned array contains both the original frame bitmap and the composited bitmap,
    /// which represents the full image as it should appear at that frame in the animation. The caller is responsible
    /// for disposing the bitmaps contained in the returned objects.</remarks>
    /// <param name="stream">The input stream containing GIF image data. The stream must support seeking and should be positioned at the
    /// beginning of the GIF data.</param>
    /// <returns>An array of AnimatedImageFrameInfo objects, each representing a frame extracted from the GIF, including both the
    /// original partial frame and the fully composited frame for playback.</returns>
    /// <exception cref="Exception">Thrown if the provided stream does not contain a valid or supported GIF image format.</exception>
    public static AnimatedImageFrameInfo[] ExtractFramesFromGif(Stream stream)
    {
        if (stream.CanSeek)
            stream.Seek(0, SeekOrigin.Begin);

        using var codec = SKCodec.Create(stream) ?? throw new Exception("Invalid or unsupported image format.");

        var frameCount = codec.FrameCount;
        var info = codec.Info;
        var frameInfos = codec.FrameInfo;

        var finalFrames = new AnimatedImageFrameInfo[frameCount];

        // Use an onscreen bitmap/canvas for compositing
        using var canvasBitmap = new SKBitmap(info.Width, info.Height, info.ColorType, SKAlphaType.Premul);
        using var canvas = new SKCanvas(canvasBitmap);

        for (int i = 0; i < frameCount; i++)
        {
            var frameInfo = frameInfos[i];

            // Restore required frame background
            if (frameInfo.RequiredFrame == -1)
            {
                canvas.Clear(SKColors.Transparent);
            }
            else
            {
                // Copy required frame's bitmap to canvas
                canvas.Clear(SKColors.Transparent);
                using var requiredFrameBmp = finalFrames[frameInfo.RequiredFrame].CombinedBitmap;
                canvas.DrawBitmap(requiredFrameBmp, 0, 0);
            }

            // Decode this frame as a partial/delta image (as stored in GIF)
            var partialFrameBmp = new SKBitmap(info.Width, info.Height, info.ColorType, SKAlphaType.Premul);
            var codecOpts = new SKCodecOptions(i);

            codec.GetPixels(partialFrameBmp.Info, partialFrameBmp.GetPixels(), codecOpts);

            // Compose this frame over current state
            canvas.DrawBitmap(partialFrameBmp, 0, 0);

            // Save a copy of the composed frame (full view for playback)
            var composedBitmap = new SKBitmap(info.Width, info.Height, info.ColorType, SKAlphaType.Premul);
            canvasBitmap.CopyTo(composedBitmap);

            // Store both the original partial frame and the composited one
            finalFrames[i] = new AnimatedImageFrameInfo
            {
                OriginalBitmap = partialFrameBmp,          // Dispose later: handled by user
                CombinedBitmap = composedBitmap,           // Dispose later: handled by user
                Index = i,
                Duration = frameInfo.Duration,
                RequiredFrame = frameInfo.RequiredFrame,
                AlphaType = frameInfo.AlphaType
            };
        }

        Array.Sort(finalFrames, (a, b) => a.Index.CompareTo(b.Index));

        return finalFrames;
    }
}