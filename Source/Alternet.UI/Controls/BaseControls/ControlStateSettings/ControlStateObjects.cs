using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of objects for different control states.
    /// </summary>
    public class ControlStateObjects<T> : INotifyPropertyChanged
    {
        private bool immutable;
        private T? normal;
        private T? hovered;
        private T? pressed;
        private T? disabled;
        private T? focused;

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
                immutable = true;
            }
        }

        /// <summary>
        /// Gets whether <see cref="Normal"/> property is assigned.
        /// </summary>
        public bool HasNormal => normal != null;

        /// <summary>
        /// Gets whether <see cref="Hovered"/> property is assigned.
        /// </summary>
        public bool HasHovered => hovered != null;

        /// <summary>
        /// Gets whether <see cref="Pressed"/> property is assigned.
        /// </summary>
        public bool HasPressed => pressed != null;

        /// <summary>
        /// Gets whether <see cref="Disabled"/> property is assigned.
        /// </summary>
        public bool HasDisabled => disabled != null;

        /// <summary>
        /// Gets whether <see cref="Focused"/> property is assigned.
        /// </summary>
        public bool HasFocused => focused != null;

        /// <summary>
        /// Gets or sets an object for normal control state.
        /// </summary>
        public virtual T? Normal
        {
            get => normal;
            set => SetProperty(ref normal, value);
        }

        /// <summary>
        /// Gets or sets an object for normal control state.
        /// </summary>
        public virtual T? Focused
        {
            get => focused;
            set => SetProperty(ref focused, value);
        }

        /// <summary>
        /// Gets or sets an object for hovered control state.
        /// </summary>
        public virtual T? Hovered
        {
            get => hovered;
            set => SetProperty(ref hovered, value);
        }

        /// <summary>
        /// Gets or sets an object for pressed control state.
        /// </summary>
        public virtual T? Pressed
        {
            get => pressed;
            set => SetProperty(ref pressed, value);
        }

        /// <summary>
        /// Gets or sets an object for disabled control state.
        /// </summary>
        public virtual T? Disabled
        {
            get => disabled;
            set => SetProperty(ref disabled, value);
        }

        /// <summary>
        /// Gets whether there are other data for states except <see cref="Normal"/>.
        /// </summary>
        public virtual bool HasOtherStates
        {
            get
            {
                return (disabled is not null) || (hovered is not null) || (pressed is not null)
                    || (focused is not null);
            }
        }

        /// <summary>
        /// Gets whether a value is assigned for the specified state.
        /// </summary>
        /// <param name="state">Control state.</param>
        /// <returns></returns>
        public bool HasObject(GenericControlState state) => GetObjectOrNormal(state) != null;

        /// <summary>
        /// Gets an object for the specified state or <see cref="Normal"/> if
        /// object for that state is not specified.
        /// </summary>
        /// <param name="state">Control state.</param>
        public T? GetObjectOrNormal(GenericControlState state)
        {
            return GetObjectOrNull(state) ?? normal;
        }

        /// <summary>
        /// Sets an object for the specified state.
        /// </summary>
        /// <param name="state">Control state.</param>
        /// <param name="value">Object value.</param>
        public void SetObject(T? value, GenericControlState state = GenericControlState.Normal)
        {
            switch (state)
            {
                case GenericControlState.Normal:
                    Normal = value;
                    return;
                case GenericControlState.Hovered:
                    Hovered = value;
                    return;
                case GenericControlState.Pressed:
                    Pressed = value;
                    return;
                case GenericControlState.Disabled:
                    Disabled = value;
                    return;
                case GenericControlState.Focused:
                    Focused = value;
                    return;
                default:
                    return;
            }
        }

        /// <summary>
        /// Assigns data for all states from the <paramref name="source"/>.
        /// </summary>
        /// <param name="source"></param>
        public void Assign(ControlStateObjects<T> source)
        {
            Normal = source.Normal;
            Hovered = source.Hovered;
            Pressed = source.Pressed;
            Disabled = source.Disabled;
            Focused = source.Focused;
        }

        /// <summary>
        /// Sets data for all the states with the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Object value.</param>
        public void SetAll(T? value)
        {
            Normal = value;
            Hovered = value;
            Pressed = value;
            Disabled = value;
            Focused = value;
        }

        /// <summary>
        /// Gets an object for the specified state or calls an <paramref name="action"/>
        /// if object for that state is not specified.
        /// </summary>
        /// <param name="state">Control state.</param>
        /// <param name="action">Action to call if no object is assigned for the state.</param>
        /// <returns></returns>
        public T GetObjectOrAction(GenericControlState state, Func<T> action)
        {
            return GetObjectOrNull(state) ?? action();
        }

        /// <summary>
        /// Gets an object for the specified state or <paramref name="defaultValue"/> if
        /// object for that state is not specified.
        /// </summary>
        /// <param name="state">Control state.</param>
        /// <param name="defaultValue">Default result value.</param>
        public T GetObjectOrDefault(GenericControlState state, T defaultValue)
        {
            return GetObjectOrNull(state) ?? defaultValue;
        }

        /// <summary>
        /// Gets an object for the specified state or <c>null</c> if image for that state
        /// is not specified.
        /// </summary>
        /// <param name="state">Control state.</param>
        public T? GetObjectOrNull(GenericControlState state)
        {
            return state switch
            {
                GenericControlState.Hovered => hovered,
                GenericControlState.Pressed => pressed,
                GenericControlState.Disabled => disabled,
                GenericControlState.Focused => focused,
                _ => normal,
            };
        }

        /// <summary>
        /// Sets field value and calls <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty(
            ref T? storage,
            T? value,
            [CallerMemberName] string? propertyName = null)
        {
            if (immutable)
                return false;

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