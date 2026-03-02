using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// This is a static control which displays an animation.
    /// This control is useful to display a (small) animation while doing
    /// a long task (e.g. a "throbber").
    /// </summary>
    /// <remarks>
    /// <see cref="AnimationPlayer"/> API is as simple as possible and won't give you full
    /// control on the animation; if you need it then use other controls.
    /// </remarks>
    /// <remarks>
    /// For the platforms where this control has a native implementation, it
    /// may have only limited support for the animation types. Set UseGeneric if you need to
    /// support all of them.
    /// </remarks>
    [ControlCategory("Other")]
    public partial class AnimationPlayer : HiddenGenericBorder
    {
        private AnimatedImage? animatedImage;
        private int currentFrame;
        private ImageSet? inactiveBitmap;
        private Timer? timer;
        private bool isPlaying;
        private HorizontalAlignment imageHorizontalAlignment = HorizontalAlignment.Center;
        private VerticalAlignment imageVerticalAlignment = VerticalAlignment.Center;
        private bool isTransparent = true;
        private bool disposeAnimatedImage;
        private bool isAnimationScaled;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationPlayer"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public AnimationPlayer(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationPlayer"/> class.
        /// </summary>
        public AnimationPlayer()
        {
            AutoPadding = false;
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether animations are scaled according to the current settings.
        /// Default is false.
        /// </summary>
        /// <remarks>When this property is set to <see langword="true"/>, animations are adjusted based on
        /// the defined scaling parameters. Changing the value triggers a layout update and invalidates the current
        /// layout, which may affect rendering performance.</remarks>
        public virtual bool IsAnimationScaled
        {
            get
            {
                return isAnimationScaled;
            }

            set
            {
                if (isAnimationScaled == value)
                    return;
                isAnimationScaled = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is transparent. Default is true.
        /// When true, the control's background is not drawn, allowing the parent control's
        /// background to show through. When false, the control's background
        /// is drawn normally. In both states, the control's border is drawn if applicable.
        /// </summary>
        /// <remarks>Setting this property to a new value will trigger a redraw of the object.</remarks>
        public virtual bool IsTransparent
        {
            get
            {
                return isTransparent;
            }

            set
            {
                if (isTransparent == value)
                    return;
                isTransparent = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="AnimatedImage"/> object associated with this control.
        /// </summary>
        [Browsable(false)]
        public virtual AnimatedImage? AnimatedImage
        {
            get => animatedImage;
            set
            {
                if (animatedImage == value)
                    return;
                var playing = IsPlaying();
                Stop();
                animatedImage = value;
                disposeAnimatedImage = false;
                currentFrame = 0;
                PerformLayoutAndInvalidate();
                if (playing)
                    Play();
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the animated image within the control.
        /// </summary>
        public virtual HorizontalAlignment ImageHorizontalAlignment
        {
            get => imageHorizontalAlignment;
            set
            {
                if (imageHorizontalAlignment == value)
                    return;
                imageHorizontalAlignment = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the animated image within the control.
        /// </summary>
        public virtual VerticalAlignment ImageVerticalAlignment
        {
            get => imageVerticalAlignment;
            set
            {
                if (imageVerticalAlignment == value)
                    return;
                imageVerticalAlignment = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets the URL of the animation currently loaded in the control, or <c>null</c> if no url is available.
        /// </summary>
        public virtual string? AnimationUrl
        {
            get
            {
                return animatedImage?.SourceUrl;
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
                return animatedImage?.FrameCount ?? 0;
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
                return animatedImage?.AnimationSize ?? SizeI.Empty;
            }
        }

        /// <summary>
        /// Returns true if animation data is present.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual bool IsOk
        {
            get
            {
                return animatedImage?.IsOk ?? false;
            }
        }

        /// <summary>
        /// Starts playing the animation.
        /// </summary>
        /// <returns><c>true</c> if the animation started to play; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// The animation is always played in loop mode (unless the last frame of the animation
        /// has an infinite delay time) and always start from the first frame even if
        /// you stopped it while some other frame was displayed.
        /// </remarks>
        public virtual bool Play()
        {
            if (DisposingOrDisposed || animatedImage is null || !animatedImage.IsOk || isPlaying || !Enabled)
                return default;

            timer ??= new Timer();

            if (currentFrame >= animatedImage.FrameCount)
            {
                currentFrame = 0;
            }

            isPlaying = true;
            timer.Interval = animatedImage.GetDuration(currentFrame);
            timer.TickAction = () =>
            {
                if (DisposingOrDisposed || animatedImage is null || !animatedImage.IsOk)
                    return;
                currentFrame++;

                if (currentFrame >= animatedImage.FrameCount)
                {
                    currentFrame = 0;
                }

                timer.Interval = animatedImage.GetDuration(currentFrame);
                Refresh();
                timer.StartOnce();
            };

            Refresh();

            timer.StartOnce();

            return true;
        }

        /// <summary>
        /// Stops playing the animation.
        /// </summary>
        /// <remarks>
        /// The control will show the first frame of the animation, a custom static
        /// image or the window's background color as specified by the last
        /// <see cref="SetInactiveBitmap"/> call.
        /// </remarks>
        public virtual void Stop()
        {
            if (DisposingOrDisposed || !isPlaying)
                return;
            timer?.Stop();
            isPlaying = false;
            currentFrame = 0;
            Invalidate();
        }

        /// <summary>
        /// Gets whether animation is being played.
        /// </summary>
        /// <returns><c>true</c> if the animation is being played; <c>false</c> otherwise.</returns>
        public virtual bool IsPlaying()
        {
            if (DisposingOrDisposed)
                return default;
            return isPlaying;
        }

        /// <summary>
        /// Loads an animation from a file.
        /// </summary>
        /// <param name="filename">The path to the animation file.</param>
        /// <param name="type">One of the <see cref="AnimationType"/> values;
        /// <see cref="AnimationType.Any"/> means that the function should try to autodetect the animation type. </param>
        /// <param name="delayLoading">Whether to delay loading of the animation until it's played for the first time. Default is false.</param>
        /// <returns></returns>
        public virtual bool LoadFile(string filename, AnimationType type = AnimationType.Any, bool delayLoading = false)
        {
            if (DisposingOrDisposed)
                return default;
            if (string.IsNullOrEmpty(filename))
                return false;
            if (!File.Exists(filename))
                return false;

            try
            {
                var url = CommonUtils.PrepareFileUrl(filename);

                AnimatedImage = new AnimatedImage(url, delayLoading: false);
                disposeAnimatedImage = true;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, $"Failed to load animation from file '{filename}'");
                animatedImage = null;
                return false;
            }
        }

        /// <summary>
        /// Loads the animation from the given stream.
        /// </summary>
        /// <param name="stream">The stream to use to load the animation.
        /// Under Linux may be any kind of stream; under other platforms this must be
        /// a seekable stream.
        /// </param>
        /// <param name="type">One of the <see cref="AnimationType"/> values;
        /// <see cref="AnimationType.Any"/>
        /// means that the function should try to autodetect the animation type.
        /// </param>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise.</returns>
        public virtual bool Load(Stream stream, AnimationType type = AnimationType.Any)
        {
            if (DisposingOrDisposed)
                return default;
            try
            {
                AnimatedImage = new AnimatedImage(stream);
                disposeAnimatedImage = true;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, $"Failed to load animation from stream");
                animatedImage = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the delay (in milliseconds) for the specified frame.
        /// </summary>
        /// <param name="i">Frame index.</param>
        /// <returns></returns>
        public virtual int GetDelay(int i)
        {
            if (DisposingOrDisposed)
                return default;
            return animatedImage?.GetDuration(i) ?? 0;
        }

        /// <summary>
        /// Returns the specified frame as a <see cref="Image"/>.
        /// </summary>
        /// <param name="i">Frame index.</param>
        /// <returns></returns>
        public virtual Image? GetFrame(int i)
        {
            if (DisposingOrDisposed)
                return null;
            return animatedImage?.GetFrame(i);
        }

        /// <summary>
        /// Retrieves a frame from the animated image at the specified index and returns a scaled version of that frame.
        /// </summary>
        /// <remarks>This method returns null if the object is currently disposing or has already been
        /// disposed. Ensure that the index is valid and the scale factor is appropriate for the intended use.</remarks>
        /// <param name="i">The zero-based index of the frame to retrieve from the animated image. Must be within the valid range of
        /// available frames.</param>
        /// <param name="scaleFactor">The factor by which to scale the dimensions of the retrieved frame. Must be greater than zero.</param>
        /// <returns>An Image object representing the scaled frame at the specified index, or null if the object is disposing or
        /// has been disposed.</returns>
        public virtual Image? GetScaledFrame(int i, float scaleFactor)
        {
            if (DisposingOrDisposed)
                return null;
            return animatedImage?.GetScaledFrame(i, scaleFactor);
        }

        /// <summary>
        /// Loads the animation from the given file or resource url.
        /// </summary>
        /// <param name="url">Url with the animation.</param>
        /// <param name="type">One of the <see cref="AnimationType"/> values;
        /// <see cref="AnimationType.Any"/>
        /// means that the function should try to autodetect the animation type.
        /// </param>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise.</returns>
        /// <example>
        /// <code>
        /// var resPrefix = $"embres:ControlsSample.Resources.Animation.";
        /// var animationPlant = $"{resPrefix}Plant.gif";
        /// animation.LoadFromUrl(animationPlant);
        /// animation.Play();
        /// </code>
        /// </example>
        public virtual bool LoadFromUrl(string url, AnimationType type = AnimationType.Any)
        {
            if (DisposingOrDisposed)
                return default;

            try
            {
                AnimatedImage = new AnimatedImage(url, delayLoading: false);
                disposeAnimatedImage = true;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, $"Failed to load animation from url: {url}");
                animatedImage = null;
                return false;
            }
        }

        /// <summary>
        /// Sets the bitmap to show on the control when it's not playing an animation.
        /// </summary>
        /// <remarks>
        /// If you set as inactive bitmap <c>null</c> (which is the default), then the first
        /// frame of the animation is instead shown when the control is inactive;
        /// in this case, if there's no valid animation associated with the control, then
        /// the background color of the control is shown.
        /// </remarks>
        /// <remarks>
        /// If the control is not playing the animation, the given bitmap will be
        /// immediately shown, otherwise it will be shown as soon as <see cref="Stop"/> is called.
        /// </remarks>
        /// <remarks>
        /// Note that the inactive bitmap, if smaller than the control's size, will be
        /// centered in the control; if bigger, it will be stretched to fit it.
        /// </remarks>
        /// <param name="bitmap"></param>
        public virtual void SetInactiveBitmap(ImageSet? bitmap)
        {
            if (DisposingOrDisposed || inactiveBitmap == bitmap)
                return;
            inactiveBitmap = bitmap;
            if (!IsPlaying() || !Enabled)
                PerformLayoutAndInvalidate();
        }

        /// <summary>
        /// Gets the effective scale factor applied to animations, reflecting whether animation scaling
        /// is currently enabled.
        /// </summary>
        /// <remarks>Use this method to determine the current scaling applied to animations, which may
        /// affect animation speed or appearance depending on the application's settings.</remarks>
        /// <returns>A floating-point value representing the effective animation scale factor. Returns the scale factor if
        /// animations are scaled; otherwise, returns 1.</returns>
        public virtual float GetEffectiveAnimationScaleFactor()
        {
            return IsAnimationScaled ? ScaleFactor : 1f;
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            var flags = IsTransparent ? DrawDefaultBackgroundFlags.DrawBorder
                : DrawDefaultBackgroundFlags.DrawBorderAndBackground;

            DrawBorderAndBackground(e, flags);

            var r = e.ClientRectangle;

            if (HasBorder)
            {
                var borderwidth = BorderWidth;
                r = r.DeflatedWithPadding(borderwidth);
            }

            r = r.DeflatedWithPadding(Padding);
            var dc = e.Graphics;

            Image? frameImage;

            var animationScaleFactor = GetEffectiveAnimationScaleFactor();
            var animationSizeScaled = SizeI.Multiply(AnimationSize, animationScaleFactor);
            var animationSizeScaledI = animationSizeScaled.ToSize();

            if (Enabled && IsPlaying())
            {
                frameImage = GetScaledFrame(currentFrame, animationScaleFactor);
            }
            else
            {
                frameImage = inactiveBitmap?.AsImage(AnimationSize);

                if (frameImage is null && animatedImage is not null && animatedImage.IsOk)
                {
                    frameImage ??= animatedImage.InactiveBitmap?.AsImage(animationSizeScaledI);
                    frameImage ??= GetScaledFrame(animatedImage.InactiveFrameIndex, animationScaleFactor);
                }

                if (!Enabled)
                    frameImage = frameImage?.ToGrayScaleCached();
            }

            var frameSizePixels = frameImage?.Size ?? SizeI.Empty;
            var frameSizeDips = PixelToDip(frameSizePixels);
            var frameRect = new RectD(r.Location, frameSizeDips);

            if (frameImage != null && frameSizeDips.IsPositive)
            {
                var alignedFrameRect = AlignUtils.AlignRectInRect(
                    frameRect,
                    r,
                    ImageHorizontalAlignment,
                    ImageVerticalAlignment,
                    shrinkSize: false);
                dc.DrawImage(frameImage, alignedFrameRect.Location);
            }

            DefaultPaintDebug(e);
        }

        /// <inheritdoc/>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Stop();
            Invalidate();
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            var specifiedWidth = SuggestedWidth;
            var specifiedHeight = SuggestedHeight;
            var validWidth = !Coord.IsNaN(specifiedWidth);
            var validHeight = !Coord.IsNaN(specifiedHeight);
            var validSize = validWidth && validHeight;

            if (validSize)
                return new SizeD(specifiedWidth, specifiedHeight);

            if (IsOk)
            {
                var animationScaleFactor = GetEffectiveAnimationScaleFactor();
                var animationSizeScaled = SizeI.Multiply(AnimationSize, animationScaleFactor);
                var animationSizeScaledI = animationSizeScaled.ToSize();

                var sizeDips = PixelToDip(animationSizeScaledI);
                return sizeDips + Padding.Size;
            }

            return base.GetPreferredSize(context);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDispose(ref timer);

            if (disposeAnimatedImage)
                SafeDispose(ref animatedImage);
            else
                animatedImage = null;

            SafeDispose(ref inactiveBitmap);

            base.DisposeManaged();
        }
    }
}