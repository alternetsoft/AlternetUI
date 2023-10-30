using System.ComponentModel;
using System.Runtime.CompilerServices;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of images for different control states.
    /// </summary>
    public class ControlStateImages : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets <see cref="ControlStateImages"/> with empty state images.
        /// </summary>
        public static readonly ControlStateImages Empty = new()
        {
            Immutable = true,
        };

        private Image? normalImage;
        private Image? hoveredImage;
        private Image? pressedImage;
        private Image? disabledImage;
        private Image? focusedImage;
        private bool immutable;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets whether object is immutable (properties can not be changed).
        /// </summary>
        public bool Immutable
        {
            get
            {
                return immutable;
            }

            set
            {
                if (immutable == value || immutable)
                    return;
                immutable = true;
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for normal control state.
        /// </summary>
        public virtual Image? NormalImage
        {
            get => normalImage;
            set => SetProperty(ref normalImage, value);
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for normal control state.
        /// </summary>
        public virtual Image? FocusedImage
        {
            get => focusedImage;
            set => SetProperty(ref focusedImage, value);
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for hovered control state.
        /// </summary>
        public virtual Image? HoveredImage
        {
            get => hoveredImage;
            set => SetProperty(ref hoveredImage, value);
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for pressed control state.
        /// </summary>
        public virtual Image? PressedImage
        {
            get => pressedImage;
            set => SetProperty(ref pressedImage, value);
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for disabled control state.
        /// </summary>
        public virtual Image? DisabledImage
        {
            get => disabledImage;
            set => SetProperty(ref disabledImage, value);
        }

        /// <summary>
        /// Gets <see cref="Image"/> for the specified state or <see cref="NormalImage"/> if
        /// image for that state is not specified.
        /// </summary>
        /// <param name="state">Control state.</param>
        public Image? GetImage(GenericControlState state)
        {
            return GetImageOrNull(state) ?? NormalImage;
        }

        /// <summary>
        /// Gets <see cref="Image"/> for the specified state or <c>null</c> if image for that state
        /// is not specified.
        /// </summary>
        /// <param name="state">Control state.</param>
        public Image? GetImageOrNull(GenericControlState state)
        {
            return state switch
            {
                GenericControlState.Hovered => HoveredImage,
                GenericControlState.Pressed => PressedImage,
                GenericControlState.Disabled => DisabledImage,
                GenericControlState.Focused => FocusedImage,
                _ => NormalImage,
            };
        }

        private bool SetProperty<T>(
            ref T storage,
            T value,
            [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}