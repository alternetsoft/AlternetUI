using Alternet.Drawing;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of images for different control states.
    /// </summary>
    public class ControlStateImages : INotifyPropertyChanged
    {
        private Image? normalImage;
        private Image? hoveredImage;
        private Image? pressedImage;
        private Image? disabledImage;

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for normal control state.
        /// </summary>
        public Image? NormalImage { get => normalImage; set => SetProperty(ref normalImage, value); }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for hovered control state.
        /// </summary>
        public Image? HoveredImage { get => hoveredImage; set => SetProperty(ref hoveredImage, value); }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for pressed control state.
        /// </summary>
        public Image? PressedImage { get => pressedImage; set => SetProperty(ref pressedImage, value); }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for disabled control state.
        /// </summary>
        public Image? DisabledImage { get => disabledImage; set => SetProperty(ref disabledImage, value); }

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}