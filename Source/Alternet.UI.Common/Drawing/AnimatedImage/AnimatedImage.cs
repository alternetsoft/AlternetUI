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
/// Animations can be loaded from a GIF or WEBP image, or created from an array of SVG images.
/// The class provides methods to retrieve frame information, durations, and scaled frames.
/// <see cref="AnimationPlayer"/> is a control that can be used to play an animated image.
/// If <see cref="IsDark"/> is set, <see cref="AnimationPlayer"/> will automatically
/// select the appropriate variant based on the current theme preference, choosing from
/// this animated image and image specified in <see cref="OtherThemeVariant"/> property.
/// </summary>
public partial class AnimatedImage : DisposableObject
{
    private readonly Func<Stream>? streamProvider;

    private AnimatedImageFrameInfo[]? frames;
    private string? sourceUrl;
    private ImageSet? inactiveBitmap;
    private int inactiveFrameIndex;

    /// <summary>
    /// Initializes a new instance of the AnimatedImage class with no frames.
    /// </summary>
    public AnimatedImage()
    {
        frames = Array.Empty<AnimatedImageFrameInfo>();
    }

    /// <summary>
    /// Initializes a new instance of the AnimatedImage class using an array of SVG images,
    /// a specified size, and a duration for each frame.
    /// </summary>
    /// <param name="svg">An array of SVG images to be used as frames for the animated image.</param>
    /// <param name="size">The size of each frame in pixels.</param>
    /// <param name="duration">The duration of each frame in milliseconds.</param>
    /// <param name="color">An optional color to apply to the SVG images.</param>
    /// <param name="isDark">A value indicating whether to use the dark color variant for the SVG images.</param>
    public AnimatedImage(SvgImage[] svg, int size, int duration = 100, Color? color = null, bool? isDark = null)
    {
        IsDark = isDark;

        frames = new AnimatedImageFrameInfo[svg.Length];

        for (int i = 0; i < svg.Length; i++)
        {
            var frame = new AnimatedImageFrameInfo(svg[i], size, color, isDark);
            frame.Duration = duration;
            frames[i] = frame;
        }
    }

    /// <summary>
    /// Initializes a new instance of the AnimatedImage class using the specified array of frame information.
    /// </summary>
    /// <param name="frames">An array of AnimatedImageFrameInfo objects
    /// representing the frames of the animated image.</param>
    public AnimatedImage(AnimatedImageFrameInfo[] frames)
    {
        this.frames = frames;
    }

    /// <summary>
    /// Initializes a new instance of the AnimatedImage class using
    /// the specified image source URL and an option to
    /// delay loading of animation frames.
    /// </summary>
    /// <remarks>If delayLoading is set to false, all animation frames are loaded during construction, which
    /// may impact performance for large images or slow network connections.</remarks>
    /// <param name="sourceUrl">The URL of the image to be used for the animated image.
    /// Must be a valid URL pointing to an image resource.</param>
    /// <param name="delayLoading">A value indicating whether to delay loading the animation frames.
    /// If set to false, frames are loaded immediately
    /// upon initialization; otherwise, loading is deferred. Default is <see langword="true"/>.</param>
    public AnimatedImage(string sourceUrl, bool delayLoading = true)
    {
        this.sourceUrl = sourceUrl;
        if (!delayLoading)
            RequireFrames();
    }

    /// <summary>
    /// Initializes a new instance of the AnimatedImage class using the specified
    /// stream provider and an option to delay
    /// frame loading.
    /// </summary>
    /// <remarks>If <paramref name="delayLoading"/> is set to <see langword="false"/>, all frames are loaded
    /// during construction, which may impact performance for large images.</remarks>
    /// <param name="streamProvider">A function that returns a stream containing the image data
    /// to be used for loading the animated image. This
    /// parameter cannot be null.</param>
    /// <param name="delayLoading">A value indicating whether to delay loading of
    /// image frames until they are needed. If set to <see
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
    /// <param name="stream">The stream that provides the image data.
    /// The stream must be readable and positioned at the beginning of the
    /// image data.</param>
    public AnimatedImage(Stream stream)
    {
        this.streamProvider = () => stream;
        RequireFrames();
        this.streamProvider = null;
    }

    /// <summary>
    /// Occurs when the inactive bitmap is updated, allowing subscribers
    /// to respond to changes in the bitmap's state.
    /// </summary>
    /// <remarks>Subscribers can use this event to refresh the display or perform actions when the inactive
    /// bitmap changes. Handling this event efficiently is recommended to avoid performance issues, especially in
    /// scenarios where frequent updates may occur.</remarks>
    public event EventHandler? InactiveBitmapChanged;

    /// <summary>
    /// Occurs when the index of the inactive frame changes.
    /// </summary>
    /// <remarks>This event is typically used to respond to changes in the inactive frame index, allowing
    /// subscribers to update their state or UI accordingly.</remarks>
    public event EventHandler? InactiveFrameIndexChanged;

    /// <summary>
    /// Gets or sets a value indicating whether the animated image is intended for use in a dark theme.
    /// This property must be specified in order for the <see cref="OtherThemeVariant"/> property to function correctly.
    /// </summary>
    [Browsable(false)]
    public virtual bool? IsDark { get; set; }

    /// <summary>
    /// Gets or sets the other theme variant of the animated image,
    /// allowing for easy switching between light and dark themes.
    /// <see cref="AnimationPlayer"/> will automatically select the appropriate
    /// variant based on the current theme preference.
    /// </summary>
    [Browsable(false)]
    public virtual AnimatedImage? OtherThemeVariant { get; set; }

    /// <summary>
    /// Gets the array of frames that make up the animated image.
    /// </summary>
    public AnimatedImageFrameInfo[] Frames
    {
        get
        {
            RequireFrames();
            return frames ?? Array.Empty<AnimatedImageFrameInfo>();
        }
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
    /// Gets or sets the bitmap to show in the animation player when it's not playing this animation.
    /// </summary>
    /// <remarks>
    /// If you set the inactive bitmap to <c>null</c> (default value), then the first
    /// frame of the animation is shown when the animation player is inactive. 
    /// </remarks>
    /// <remarks>
    /// If the animation player is not playing the animation, the given bitmap will be
    /// immediately shown; otherwise, it will be shown as soon as the animation is stopped.
    /// </remarks>
    /// <remarks>
    /// Note that the inactive bitmap, if smaller than the control's size, will be
    /// centered in the control; if bigger, it will be stretched to fit it.
    /// </remarks>
    public virtual ImageSet? InactiveBitmap
    {
        get
        {
            return inactiveBitmap;
        }

        set
        {
            if (SetProperty(ref inactiveBitmap, value, nameof(InactiveBitmap)))
            {
                InactiveBitmapChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets or sets the index of the frame which is shown when animation is stopped or disabled.
    /// Default is 0, which means that the first frame of the animation
    /// is shown when the animation player is inactive.
    /// </summary>
    /// <remarks>The index must be a valid frame index within the range of available frames.</remarks>
    public virtual int InactiveFrameIndex
    {
        get
        {
            return inactiveFrameIndex;
        }

        set
        {
            if (SetProperty(ref inactiveFrameIndex, value, nameof(InactiveFrameIndex)))
            {
                InactiveFrameIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the animated image contains valid frame data and is ready for processing.
    /// </summary>
    /// <remarks>Use this property to determine if the object is in a usable state before performing
    /// operations that require valid frames. Returns <see langword="true"/>
    /// if the frames are initialized and the first
    /// frame's bitmap is available; otherwise, returns <see langword="false"/>.</remarks>
    [Browsable(false)]
    public virtual bool IsOk
    {
        get
        {
            if (DisposingOrDisposed)
                return false;
            RequireFrames();
            return frames != null && frames.Length > 0
                && (frames[0].CombinedBitmap != null || frames[0].Image != null);
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
    /// Creates a pair of blinking animated images based on the provided SVG image, size, and optional colors
    /// for light and dark themes. First frame is the image, second frame is empty.
    /// </summary>
    /// <param name="svg">The SVG image to be used for creating the animated images.</param>
    /// <param name="size">The size of the animated images.</param>
    /// <param name="lightColor">The optional color for the light theme.</param>
    /// <param name="duration">The duration of each frame in milliseconds. Default is 600ms.</param>
    /// <param name="darkColor">The optional color for the dark theme.</param>
    /// <returns>A tuple containing the light and dark themed animated images.</returns>
    public static (AnimatedImage Light, AnimatedImage Dark) CreateBlinking(
        SvgImage svg,
        int size,
        int duration = 600,
        Color? lightColor = null,
        Color? darkColor = null)
    {
        var emptyFrameLight = new AnimatedImageFrameInfo(KnownSvgImages.ImgEmpty, size, null, isDark: false);
        var emptyFrameDark = new AnimatedImageFrameInfo(KnownSvgImages.ImgEmpty, size, null, isDark: true);
        var imageFrameLight = new AnimatedImageFrameInfo(svg, size, lightColor, isDark: false);
        var imageFrameDark = new AnimatedImageFrameInfo(svg, size, darkColor, isDark: true);

        emptyFrameLight.Duration = duration;
        emptyFrameDark.Duration = duration;
        imageFrameLight.Duration = duration;
        imageFrameDark.Duration = duration;

        AnimatedImageFrameInfo[] lightFrames = new[] { imageFrameLight, emptyFrameLight };
        AnimatedImageFrameInfo[] darkFrames = new[] { imageFrameDark, emptyFrameDark };

        var lightImage = new AnimatedImage(lightFrames) { IsDark = false };
        var darkImage = new AnimatedImage(darkFrames) { IsDark = true };

        lightImage.OtherThemeVariant = darkImage;
        darkImage.OtherThemeVariant = lightImage;
        
        return (lightImage, darkImage);
    }

    /// <summary>
    /// Creates a pair of blinking animated images based on the provided SVG image, size, and optional colors
    /// for light and dark themes. First frame is the image with first color, second frame is the image with second color.
    /// This method creates a blinking effect by alternating between two colored frames of the same SVG image.
    /// </summary>
    /// <param name="svg">The SVG image to be used for the animated frames.</param>
    /// <param name="size">The size of the animated images.</param>
    /// <param name="firstFrameLightColor">The optional color for the first frame in the light theme.</param>
    /// <param name="firstFrameDarkColor">The optional color for the first frame in the dark theme.</param>
    /// <param name="secondFrameColor">The color for the second frame.</param>
    /// <param name="duration">The duration of each frame in milliseconds.</param>
    /// <returns>A tuple containing the light and dark themed animated images.</returns>
    public static (AnimatedImage Light, AnimatedImage Dark) CreateBlinkingColor(
        SvgImage svg,
        int size,
        ThemedColor secondFrameColor,
        int duration = 600,
        Color? firstFrameLightColor = null,
        Color? firstFrameDarkColor = null)
    {
        var firstFrameLight = new AnimatedImageFrameInfo(svg, size, firstFrameLightColor, isDark: false);
        var firstFrameDark = new AnimatedImageFrameInfo(svg, size, firstFrameDarkColor, isDark: true);
        var secondFrameLight = new AnimatedImageFrameInfo(svg, size, secondFrameColor.Light, isDark: false);
        var secondFrameDark = new AnimatedImageFrameInfo(svg, size, secondFrameColor.Dark, isDark: true);

        firstFrameLight.Duration = duration;
        firstFrameDark.Duration = duration;
        secondFrameLight.Duration = duration;
        secondFrameDark.Duration = duration;

        AnimatedImageFrameInfo[] lightFrames = new[] { firstFrameLight, secondFrameLight };
        AnimatedImageFrameInfo[] darkFrames = new[] { firstFrameDark, secondFrameDark };

        var lightImage = new AnimatedImage(lightFrames) { IsDark = false };
        var darkImage = new AnimatedImage(darkFrames) { IsDark = true };

        lightImage.OtherThemeVariant = darkImage;
        darkImage.OtherThemeVariant = lightImage;

        return (lightImage, darkImage);
    }

    /// <summary>
    /// Gets a value indicating whether the image frames have been successfully loaded.
    /// </summary>
    /// <remarks>This property returns <see langword="true"/> if the image frames are available; otherwise, it
    /// returns <see langword="false"/>. Use this property to determine whether
    /// the animated image is ready for display
    /// or processing.</remarks>
    public virtual bool IsLoaded => frames != null;

    /// <summary>
    /// Retrieves information about a specific frame in the animated image.
    /// </summary>
    /// <remarks>Returns <see langword="null"/> if the object is disposing or disposed, if the frames have not
    /// been initialized, or if the specified index is out of bounds.</remarks>
    /// <param name="i">The zero-based index of the frame to retrieve. Must be within
    /// the range of available frames.</param>
    /// <returns>An instance of <see cref="AnimatedImageFrameInfo"/> containing the
    /// frame information if the index is valid;
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
    /// <param name="i">The zero-based index of the frame for which to retrieve the duration.
    /// Must be within the range of available
    /// frames.</param>
    /// <returns>The duration of the specified frame in milliseconds.
    /// Returns 0 if the frame does not exist or the index is
    /// invalid.</returns>
    public virtual int GetDuration(int i)
    {
        var frame = GetFrameInfo(i);
        return frame?.Duration ?? 0;
    }

    /// <summary>
    /// Retrieves the appropriate animated image variant based on the current app theme preference.
    /// </summary>
    /// <returns>An instance of <see cref="AnimatedImage"/> that matches the current app theme preference.</returns>
    public virtual AnimatedImage GetImage()
    {
        return GetImage(SystemSettings.AppearanceIsDark);
    }

    /// <summary>
    /// Retrieves the appropriate animated image variant based on the color mode of the specified control.
    /// </summary>
    /// <param name="control">The control whose color mode is used to determine
    /// the appropriate animated image variant.</param>
    /// <returns>An instance of <see cref="AnimatedImage"/> that matches the color
    /// mode of the specified control.</returns>
    public virtual AnimatedImage GetImage(AbstractControl control)
    {
        var colorMode = control.ColorMode;

        if (colorMode is null)
            return GetImage();

        return GetImage(colorMode.Value == ControlColorMode.Dark);
    }

    /// <summary>
    /// Retrieves the appropriate animated image variant based on the specified theme preference.
    /// </summary>
    /// <param name="isDark">A boolean value indicating whether the dark theme is preferred.</param>
    /// <returns>An instance of <see cref="AnimatedImage"/> that matches the specified theme preference.</returns>
    public virtual AnimatedImage GetImage(bool isDark)
    {
        if (IsDark.HasValue)
        {
            if (IsDark == isDark)
            {
                return this;
            }
            else
            {
                return OtherThemeVariant ?? this;
            }
        }
        else
        {
            return this;
        }
    }

    /// <summary>
    /// Retrieves a scaled image of the specified animation frame using the provided scale factor.
    /// </summary>
    /// <remarks>If the frame index is invalid, this method returns null. The scaling operation is applied to
    /// the frame image after retrieval.</remarks>
    /// <param name="i">The zero-based index of the frame to retrieve.
    /// Must be within the range of available frames.</param>
    /// <param name="scaleFactor">The factor by which to scale the frame image.
    /// Values greater than 1 enlarge the image; values less than 1 reduce
    /// its size.</param>
    /// <returns>An image representing the scaled frame, or null if the specified frame does not exist.</returns>
    public virtual Image? GetScaledFrame(int i, float scaleFactor)
    {
        var frame = GetFrameInfo(i);
        return frame?.GetScaledImage(scaleFactor);
    }

    /// <summary>
    /// Returns the specified frame as a <see cref="Image"/>.
    /// </summary>
    /// <param name="i">The zero-based index of the frame to retrieve.
    /// Must be within the range of available frames.</param>
    /// <returns>An instance of <see cref="Image"/> representing
    /// the specified frame if the index is valid; otherwise, <see langword="null"/>.</returns>
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
    /// Ensures that the required frames are available for processing.
    /// If frames are already present, the method exits
    /// without performing any action.
    /// </summary>
    /// <remarks>This method is typically called before operations that depend on the availability of frames.
    /// It is important to ensure that frames are initialized to avoid runtime errors.</remarks>
    public virtual bool RequireFrames()
    {
        if (DisposingOrDisposed)
            return false;
        if (frames != null)
            return frames.Length > 0;

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
            return frames.Length > 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading animated image frames: {ex.Message}");
            frames = Array.Empty<AnimatedImageFrameInfo>();
            return false;
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
