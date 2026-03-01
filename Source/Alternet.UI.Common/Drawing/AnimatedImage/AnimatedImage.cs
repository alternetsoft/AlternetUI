using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing;

/// <summary>
/// Represents an animated image that can be displayed in a user interface.
/// </summary>
/// <remarks>This class inherits from DisposableObject, which means it should be properly disposed of to free
/// resources. It is designed to handle the loading and rendering of animated images efficiently.</remarks>
public partial class AnimatedImage : DisposableObject
{
    private readonly Func<Stream>? streamProvider;

    private AnimatedImageFrameInfo[]? frames;
    private string? sourceUrl;

    /// <summary>
    /// Initializes a new instance of the AnimatedImage class using the specified image source URL and an option to
    /// delay loading of animation frames.
    /// </summary>
    /// <remarks>If delayLoading is set to false, all animation frames are loaded during construction, which
    /// may impact performance for large images or slow network connections.</remarks>
    /// <param name="sourceUrl">The URL of the image to be used for the animated image. Must be a valid URL pointing to an image resource.</param>
    /// <param name="delayLoading">A value indicating whether to delay loading the animation frames. If set to false, frames are loaded immediately
    /// upon initialization; otherwise, loading is deferred. Default is <see langword="true"/>.</param>
    public AnimatedImage(string sourceUrl, bool delayLoading = true)
    {
        this.sourceUrl = sourceUrl;
        if (!delayLoading)
            RequireFrames();
    }

    /// <summary>
    /// Initializes a new instance of the AnimatedImage class using the specified stream provider and an option to delay
    /// frame loading.
    /// </summary>
    /// <remarks>If <paramref name="delayLoading"/> is set to <see langword="false"/>, all frames are loaded
    /// during construction, which may impact performance for large images.</remarks>
    /// <param name="streamProvider">A function that returns a stream containing the image data to be used for loading the animated image. This
    /// parameter cannot be null.</param>
    /// <param name="delayLoading">A value indicating whether to delay loading of image frames until they are needed. If set to <see
    /// langword="false"/>, all frames are loaded immediately upon initialization.</param>
    public AnimatedImage(Func<Stream> streamProvider, bool delayLoading = true)
    {
        this.streamProvider = streamProvider;
        if (!delayLoading)
            RequireFrames();
    }

    /// <summary>
    /// Initializes a new instance of the AnimatedImage class using the specified stream containing image data.
    /// </summary>
    /// <remarks>The constructor requires that the provided stream contains valid image frames. If the stream
    /// does not meet these requirements, an exception may be thrown during initialization.</remarks>
    /// <param name="stream">The stream that provides the image data. The stream must be readable and positioned at the beginning of the
    /// image data.</param>
    public AnimatedImage(Stream stream)
    {
        this.streamProvider = () => stream;
        RequireFrames();
        this.streamProvider = null;
    }

    /// <summary>
    /// Gets the number of frames for this animation.
    /// </summary>
    /// <returns></returns>
    [Browsable(false)]
    public virtual int FrameCount
    {
        get
        {
            if (DisposingOrDisposed)
                return default;
            RequireFrames();
            return frames != null ? frames.Length : 0;
        }
    }

    /// <summary>
    /// Gets the size of the animation in pixels.
    /// </summary>
    /// <returns></returns>
    [Browsable(false)]
    public virtual SizeI AnimationSize
    {
        get
        {
            if (DisposingOrDisposed)
                return default;
            RequireFrames();
            if (frames == null || frames.Length == 0)
                return default;
            var image = frames[0].Image;
            return image?.Size ?? default;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the animated image contains valid frame data and is ready for processing.
    /// </summary>
    /// <remarks>Use this property to determine if the object is in a usable state before performing
    /// operations that require valid frames. Returns <see langword="true"/> if the frames are initialized and the first
    /// frame's bitmap is available; otherwise, returns <see langword="false"/>.</remarks>
    [Browsable(false)]
    public virtual bool IsOk
    {
        get
        {
            if (DisposingOrDisposed)
                return false;
            RequireFrames();
            return frames != null && frames.Length > 0 && frames[0].CombinedBitmap != null;
        } 
    }

    /// <summary>
    /// Gets or sets the URL of the source resource associated with this instance.
    /// </summary>
    /// <remarks>Setting the SourceUrl property to a new value clears the cached frames associated with the
    /// previous URL.</remarks>
    public virtual string? SourceUrl
    {
        get => sourceUrl;
        set
        {
            if (DisposingOrDisposed)
                return;
            if (sourceUrl == value)
                return;
            sourceUrl = value;
            ResetFrames();
        }
    }

    /// <summary>
    /// Gets a value indicating whether the image frames have been successfully loaded.
    /// </summary>
    /// <remarks>This property returns <see langword="true"/> if the image frames are available; otherwise, it
    /// returns <see langword="false"/>. Use this property to determine whether the animated image is ready for display
    /// or processing.</remarks>
    public virtual bool IsLoaded => frames != null;

    /// <summary>
    /// Retrieves information about a specific frame in the animated image.
    /// </summary>
    /// <remarks>Returns <see langword="null"/> if the object is disposing or disposed, if the frames have not
    /// been initialized, or if the specified index is out of bounds.</remarks>
    /// <param name="i">The zero-based index of the frame to retrieve. Must be within the range of available frames.</param>
    /// <returns>An instance of <see cref="AnimatedImageFrameInfo"/> containing the frame information if the index is valid;
    /// otherwise, <see langword="null"/>.</returns>
    public virtual AnimatedImageFrameInfo? GetFrameInfo(int i)
    {
        if (DisposingOrDisposed)
            return null;
        RequireFrames();
        if (frames == null || i >= frames.Length || i < 0)
            return null;
        return frames[i];
    }

    /// <summary>
    /// Gets the duration, in milliseconds, of the frame at the specified index.
    /// </summary>
    /// <remarks>If the specified index is out of range or the frame information is unavailable, the method
    /// returns 0. Ensure that the index is valid to obtain the correct duration.</remarks>
    /// <param name="i">The zero-based index of the frame for which to retrieve the duration. Must be within the range of available
    /// frames.</param>
    /// <returns>The duration of the specified frame in milliseconds. Returns 0 if the frame does not exist or the index is
    /// invalid.</returns>
    public virtual int GetDuration(int i)
    {
        var frame = GetFrameInfo(i);
        return frame?.Duration ?? 0;
    }

    /// <summary>
    /// Returns the specified frame as a <see cref="Image"/>.
    /// </summary>
    /// <param name="i">The zero-based index of the frame to retrieve. Must be within the range of available frames.</param>
    /// <returns>An instance of <see cref="Image"/> representing the specified frame if the index is valid; otherwise, <see langword="null"/>.</returns>
    public virtual Image? GetFrame(int i)
    {
        var frame = GetFrameInfo(i);
        return frame?.Image;
    }

    /// <summary>
    /// Clears all frame data and returns the animated image to its initial state.
    /// </summary>
    /// <remarks>Call this method to discard any existing frames and prepare the animated image for new frame
    /// processing. This is useful when reloading or resetting the animation sequence.</remarks>
    public virtual void ResetFrames()
    {
        frames = null;
    }

    /// <summary>
    /// Ensures that the required frames are available for processing. If frames are already present, the method exits
    /// without performing any action.
    /// </summary>
    /// <remarks>This method is typically called before operations that depend on the availability of frames.
    /// It is important to ensure that frames are initialized to avoid runtime errors.</remarks>
    public virtual void RequireFrames()
    {
        if (DisposingOrDisposed)
            return;
        if (frames != null)
            return;

        try
        {
            if (streamProvider != null)
            {
                using var stream = streamProvider();
                frames = AnimatedImageExtractor.ExtractFramesFromGif(stream);

            }
            else
                if (sourceUrl != null)
                {
                    frames = AnimatedImageExtractor.ExtractFramesFromGif(sourceUrl);
                }
                else
                    frames = Array.Empty<AnimatedImageFrameInfo>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading animated image frames: {ex.Message}");
            frames = Array.Empty<AnimatedImageFrameInfo>();
        }
    }

    /// <inheritdoc/>
    protected override void DisposeManaged()
    {
        if (frames != null)
        {
            foreach (var frame in frames)
            {
                frame?.Dispose();
            }

            frames = null;
        }

        base.DisposeManaged();
    }
}
