using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        private List<IControlOverlay>? overlays;

        /// <summary>
        /// Gets or sets the collection of overlays associated with the control.
        /// </summary>
        /// <remarks>Setting this property updates the internal overlay collection
        /// and invalidates the control,
        /// prompting a redraw to reflect the changes.</remarks>
        [Browsable(false)]
        public virtual IReadOnlyList<IControlOverlay>? Overlays
        {
            get => overlays;
            set
            {
                if (overlays is null)
                {
                    if (value is null || value.Count == 0)
                        return;
                }

                overlays = value?.ToList();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets whether this control has overlays.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasOverlays()
        {
            return overlays?.Count > 0;
        }

        /// <summary>
        /// Gets overlays attached to this control.
        /// </summary>
        /// <returns></returns>
        public virtual IReadOnlyList<IControlOverlay>? GetOverlays()
        {
            return overlays;
        }

        /// <summary>
        /// Removes the overlay with the specified identifier from the collection.
        /// After removal, sets the identifier to null.
        /// </summary>
        /// <param name="overlayId">A reference to the unique identifier of the overlay to remove.
        /// The value is set to null if the overlay is
        /// successfully removed.</param>
        /// <param name="invalidate">true to invalidate the display after removing the overlay;
        /// otherwise, false. The default is true.</param>
        /// <returns>true if the overlay was found and removed; otherwise, false.</returns>
        public bool RemoveOverlay(ref ObjectUniqueId? overlayId, bool invalidate = true)
        {
            if (overlays is null || overlayId is null)
                return false;
            var result = RemoveOverlay(overlayId, invalidate);
            overlayId = null;
            return result;
        }

        /// <summary>
        /// Removes an overlay with the specified unique identifier from the overlays collection.
        /// </summary>
        /// <param name="overlayId">The unique identifier of the overlay to remove.</param>
        /// <param name="invalidate">A value indicating whether to invalidate the control
        /// after removing the overlay. If <see langword="true"/>,
        /// the control is invalidated; otherwise, it is not.</param>
        /// <returns><see langword="true"/> if the overlay was successfully removed;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool RemoveOverlay(ObjectUniqueId? overlayId, bool invalidate = true)
        {
            if (overlays is null || overlayId is null)
                return false;

            for (int i = 0; i < overlays.Count; i++)
            {
                if (overlays[i].UniqueId == overlayId)
                {
                    overlays.RemoveAt(i);
                    if (overlays.Count == 0)
                        overlays = null;
                    if (invalidate)
                        Invalidate();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes overlays from the control that match the specified flags.
        /// </summary>
        /// <remarks>This method iterates through the collection of overlays and removes
        /// those that match the specified flags. If <paramref name="invalidate"/> is
        /// <see langword="true"/>, the control is invalidated
        /// to reflect the changes.</remarks>
        /// <param name="flags">The flags that determine which overlays to remove.</param>
        /// <param name="invalidate">A value indicating whether the control should
        /// be invalidated after removing the overlays. The default is
        /// <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if any overlays were removed;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool RemoveOverlays(ControlOverlayFlags flags, bool invalidate = true)
        {
            if (overlays is null)
                return false;

            for (int i = overlays.Count - 1; i >= 0; i--)
            {
                if ((overlays[i].Flags & flags) != 0)
                {
                    overlays.RemoveAt(i);
                }
            }

            if (invalidate)
                Invalidate();

            return true;
        }

        /// <summary>
        /// Removes the specified overlay from the control.
        /// </summary>
        /// <param name="overlay">The overlay to remove.</param>
        /// <param name="invalidate">The <see langword="bool"/> indicating whether
        /// to invalidate the control.</param>
        /// <returns><see langword="true"/> if the overlay was successfully removed;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool RemoveOverlay(IControlOverlay overlay, bool invalidate = true)
        {
            if (overlays is null)
                return false;
            var removed = overlays.Remove(overlay);
            if (!removed)
                return false;
            if (overlays.Count == 0)
                overlays = null;
            if (invalidate)
                Invalidate();
            return true;
        }

        /// <summary>
        /// Adds the specified overlay to the control.
        /// </summary>
        /// <param name="overlay">The overlay to add.</param>
        /// <param name="invalidate">The <see langword="bool"/> indicating whether
        /// to invalidate the control. Optional. Default is <see langword="true"/>.</param>
        public virtual void AddOverlay(IControlOverlay overlay, bool invalidate = true)
        {
            overlays ??= new();
            overlays.Add(overlay);
            if (invalidate)
                Invalidate();
        }

        /// <summary>
        /// Gets rectangle to which the overlay should be fitted.
        /// </summary>
        /// <returns>A <see cref="RectD"/> representing the overlay rectangle.
        /// This typically corresponds to the client
        /// rectangle of the object.</returns>
        public virtual RectD GetOverlayRectangle()
        {
            return ClientRectangle;
        }

        /// <summary>
        /// Displays an overlay tooltip with an error message based on the provided exception.
        /// </summary>
        /// <remarks>This method is a convenience wrapper for displaying error messages
        /// in an overlay tooltip. It uses the exception's message as the tooltip content
        /// and provides default values for optional
        /// parameters when not specified.</remarks>
        /// <param name="title">The title of the tooltip. If <see langword="null"/>,
        /// a default error title is used.</param>
        /// <param name="e">The exception message will be displayed in the tooltip.
        /// This parameter cannot be <see langword="null"/>.</param>
        /// <param name="alignment">The horizontal and vertical alignment of the tooltip
        /// relative to its target. If <see langword="null"/>, a
        /// default alignment is used.</param>
        /// <param name="options">Flags that control the behavior of the tooltip,
        /// such as whether it dismisses automatically after a set
        /// interval. The default value is
        /// <see cref="OverlayToolTipFlags.DismissAfterInterval"/>.</param>
        /// <param name="dismissIntervalMilliseconds">The time, in milliseconds, after
        /// which the tooltip will automatically dismiss if the <paramref name="options"/> include
        /// <see cref="OverlayToolTipFlags.DismissAfterInterval"/>.
        /// If <see langword="null"/>, a default interval is used.</param>
        /// <returns>An <see cref="ObjectUniqueId"/> that uniquely identifies the
        /// displayed tooltip.</returns>
        public virtual ObjectUniqueId ShowOverlayToolTipWithError(
            object? title,
            object e,
            HVAlignment? alignment = null,
            OverlayToolTipFlags options = OverlayToolTipFlags.DismissAfterInterval,
            int? dismissIntervalMilliseconds = null)
        {
            string? msg;

            if (e is Exception exception)
            {
                msg = exception.Message;
            }
            else
            {
                msg = e.ToString();
            }

            return ShowOverlayToolTip(
                title ?? ErrorMessages.Default.ErrorTitle,
                msg,
                MessageBoxIcon.Error,
                alignment,
                options,
                dismissIntervalMilliseconds);
        }

        /// <summary>
        /// Displays a simple overlay tooltip with the specified message, alignment, and options.
        /// </summary>
        /// <remarks>This method provides a simplified way to display an overlay tooltip. For more
        /// advanced customization, consider using an overload of this method with additional
        /// parameters.</remarks>
        /// <param name="message">The content to display in the tooltip. Can be a string
        /// or another object representing the message.
        /// If <see langword="null"/>, the tooltip will not display any content.</param>
        /// <param name="alignment">Specifies the horizontal and vertical alignment
        /// of the tooltip relative to its target.</param>
        /// <param name="options">A combination of flags from <see cref="OverlayToolTipFlags"/>
        /// that determine the behavior and appearance and behavior of the tooltip.</param>
        /// <returns>An <see cref="ObjectUniqueId"/> representing the unique identifier
        /// of the displayed tooltip. This identifier
        /// can be used to manage or dismiss the tooltip.</returns>
        public virtual ObjectUniqueId ShowOverlayToolTipSimple(
            object? message,
            HVAlignment alignment,
            OverlayToolTipFlags options)
        {
            return ShowOverlayToolTipSimple(
                message,
                null,
                alignment,
                options,
                null);
        }

        /// <summary>
        /// Displays a simple overlay tooltip with the specified
        /// message and optional customization parameters.
        /// </summary>
        /// <remarks>If both <paramref name="location"/> and <paramref name="alignment"/>
        /// are <see langword="null"/>, the tooltip defaults to being
        /// centered on the screen.</remarks>
        /// <param name="message">The content to display in the tooltip.
        /// If <see langword="null"/>, an empty string is displayed.</param>
        /// <param name="location">The optional screen location where the tooltip should appear.
        /// If <see langword="null"/>, the tooltip is
        /// aligned based on the specified <paramref name="alignment"/>.</param>
        /// <param name="alignment">The optional alignment of the tooltip
        /// when <paramref name="location"/> is <see langword="null"/>.
        /// Defaults to centered alignment if not specified.</param>
        /// <param name="options">Flags that control the behavior of the tooltip,
        /// such as whether it dismisses automatically or requires
        /// manual dismissal. The default value is
        /// <see cref="OverlayToolTipFlags.DismissAfterInterval"/>.</param>
        /// <param name="dismissIntervalMilliseconds">The optional time, in milliseconds,
        /// after which the tooltip is automatically dismissed. This parameter is
        /// only applicable if <paramref name="options"/> includes
        /// <see cref="OverlayToolTipFlags.DismissAfterInterval"/>.
        /// If <see langword="null"/>, a default interval is used.</param>
        /// <returns>An <see cref="ObjectUniqueId"/> representing the unique
        /// identifier of the displayed tooltip. This can be
        /// used to manage or dismiss the tooltip programmatically.</returns>
        public virtual ObjectUniqueId ShowOverlayToolTipSimple(
            object? message,
            PointD? location = null,
            HVAlignment? alignment = null,
            OverlayToolTipFlags options = OverlayToolTipFlags.DismissAfterInterval,
            int? dismissIntervalMilliseconds = null)
        {
            OverlayToolTipParams data = new()
            {
                Text = message?.ToString() ?? string.Empty,
                Options = options,
                DismissInterval = dismissIntervalMilliseconds,
            };

            if (location is null)
            {
                data.HorizontalAlignment = alignment?.Horizontal ?? HorizontalAlignment.Center;
                data.VerticalAlignment = alignment?.Vertical ?? VerticalAlignment.Center;
            }
            else
            {
                data.LocationWithoutOffset = location.Value;
            }

            return ShowOverlayToolTip(data);
        }

        /// <summary>
        /// Displays an overlay tooltip with the specified title, message, and options.
        /// </summary>
        /// <remarks>This method provides a convenient way to display
        /// an overlay tooltip with customizable
        /// content, appearance, and behavior. If more advanced customization is required,
        /// consider using the overload
        /// that accepts an <see cref="OverlayToolTipParams"/> object.</remarks>
        /// <param name="title">The title of the tooltip. If <see langword="null"/>,
        /// an empty string is used.</param>
        /// <param name="message">The message content of the tooltip.
        /// If <see langword="null"/>, an empty string is used.</param>
        /// <param name="icon">An optional icon to display in the tooltip.
        /// If <see langword="null"/>, no icon is shown.</param>
        /// <param name="alignment">An optional alignment specification for the tooltip.
        /// If <see langword="null"/>, the tooltip is centered
        /// horizontally and vertically.</param>
        /// <param name="options">A set of flags that control the behavior of the tooltip.
        /// The default is <see cref="OverlayToolTipFlags.DismissAfterInterval"/>.</param>
        /// <param name="dismissIntervalMilliseconds">An optional duration, in milliseconds,
        /// after which the tooltip is automatically dismissed. If <see langword="null"/>,
        /// the default interval is used.</param>
        /// <returns>An <see cref="ObjectUniqueId"/> that uniquely identifies
        /// the displayed tooltip. This can be used to manage
        /// or dismiss the tooltip programmatically.</returns>
        public virtual ObjectUniqueId ShowOverlayToolTip(
            object? title,
            object? message,
            MessageBoxIcon? icon = null,
            HVAlignment? alignment = null,
            OverlayToolTipFlags options = OverlayToolTipFlags.DismissAfterInterval,
            int? dismissIntervalMilliseconds = null)
        {
            OverlayToolTipParams data = new()
            {
                Title = title?.ToString() ?? string.Empty,
                Text = message?.ToString() ?? string.Empty,
                Icon = icon,
                HorizontalAlignment = alignment?.Horizontal ?? HorizontalAlignment.Center,
                VerticalAlignment = alignment?.Vertical ?? VerticalAlignment.Center,
                Options = options,
                DismissInterval = dismissIntervalMilliseconds,
            };

            return ShowOverlayToolTip(data);
        }

        /// <summary>
        /// Displays an overlay tooltip at the specified location with the provided
        /// content and removal interval.
        /// </summary>
        /// <param name="data">The content, styling and behavior parameters for the tooltip.</param>
        /// <returns>A unique identifier for the created overlay tooltip, which
        /// can be used to manage or remove it later.</returns>
        public virtual ObjectUniqueId ShowOverlayToolTip(OverlayToolTipParams data)
        {
            if (data.Options.HasFlag(OverlayToolTipFlags.Clear))
                Overlays = [];

            data.Font ??= RealFont;
            data.ContainerBounds ??= GetOverlayRectangle();
            var containerBounds = data.ContainerBounds.Value;

            data.MaxWidth ??= Math.Max(RichToolTip.DefaultMaxWidth ?? 0, containerBounds.Width);
            data.ScaleFactor = ScaleFactor;

            if (data.Options.HasFlag(OverlayToolTipFlags.UseSystemColors))
            {
                data.UsesSystemColors();
            }

            var overlay = new ControlOverlayWithToolTip()
            {
                ToolTip = data,
            };

            overlay.UpdateImage();

            var imageSize = overlay.Image?.SizeDip(this) ?? SizeD.Empty;

            var notAlignedRect = new RectD(data.LocationWithOffset ?? PointD.Empty, imageSize);

            if (imageSize.IsPositive && data.HasAlignment)
            {
                var alignedRect = AlignUtils.AlignRectInRect(
                    notAlignedRect,
                    containerBounds,
                    data.HorizontalAlignment,
                    data.VerticalAlignment);
                overlay.Location = alignedRect.Location;
            }
            else
            {
                overlay.Location = notAlignedRect.Location;
            }

            if (data.Options.HasFlag(OverlayToolTipFlags.FitIntoContainer))
            {
                if (containerBounds.Contains(overlay.Bounds))
                {
                }
                else
                {
                    var patchLocation = data.LocationWithoutOffset ?? overlay.Location;
                    RectD patchRect = RectD.GetEllipseBoundingBox(
                        patchLocation,
                        Math.Max(data.LocationOffset.X, 1),
                        Math.Max(data.LocationOffset.Y, 1));

                    var patchContainer = patchRect.Inflated(overlay.ImageSizeInDips);

                    NineRects nineRects = new(patchContainer, patchRect, ScaleFactor);

                    var containerRectI = containerBounds.PixelFromDip(ScaleFactor);

                    var bestRect = nineRects.OuterRectInsideContainer(containerRectI);

                    if (bestRect is not null)
                    {
                        overlay.Location = bestRect.Value.PixelToDip(ScaleFactor).Location;
                    }
                }
            }

            AddOverlay(overlay);

            if (data.DismissAfterInterval)
                overlay.SetRemovalTimer(data.DismissInterval, this);
            return overlay.UniqueId;
        }

        /// <summary>
        /// Paints all overlays associated with the control.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        protected virtual void PaintOverlays(PaintEventArgs e)
        {
            if (overlays is null)
                return;
            foreach (var overlay in overlays)
            {
                overlay.OnPaint(this, e);
            }
        }
    }
}
