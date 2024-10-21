using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of objects for different control states.
    /// </summary>
    public class ControlStateObjects<T> : ImmutableObject
    {
        private T? normal;
        private T? hovered;
        private T? pressed;
        private T? disabled;
        private T? focused;
        private T? selected;

        /// <summary>
        /// Occurs when <see cref="Normal"/> property changes.
        /// </summary>
        public event EventHandler? NormalChanged;

        /// <summary>
        /// Occurs when <see cref="Focused"/> property changes.
        /// </summary>
        public event EventHandler? FocusedChanged;

        /// <summary>
        /// Occurs when <see cref="Hovered"/> property changes.
        /// </summary>
        public event EventHandler? HoveredChanged;

        /// <summary>
        /// Occurs when <see cref="Pressed"/> property changes.
        /// </summary>
        public event EventHandler? PressedChanged;

        /// <summary>
        /// Occurs when <see cref="Disabled"/> property changes.
        /// </summary>
        public event EventHandler? DisabledChanged;

        /// <summary>
        /// Occurs when <see cref="Disabled"/> property changes.
        /// </summary>
        public event EventHandler? SelectedChanged;

        /// <summary>
        /// Gets or sets <see cref="IControlStateObjectChanged"/> object
        /// which is used for the notifications.
        /// </summary>
        public IControlStateObjectChanged? ChangedHandler { get; set; }

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
        /// Gets whether <see cref="Selected"/> property is assigned.
        /// </summary>
        public bool HasSelected => selected != null;

        /// <summary>
        /// Gets or sets an object for normal control state.
        /// </summary>
        public virtual T? Normal
        {
            get => normal;
            set
            {
                SetProperty(ref normal, value, nameof(Normal), RaiseNormalChanged);
            }
        }

        /// <summary>
        /// Gets or sets an object for normal control state.
        /// </summary>
        public virtual T? Focused
        {
            get => focused;
            set => SetProperty(ref focused, value, nameof(Focused), RaiseFocusedChanged);
        }

        /// <summary>
        /// Gets or sets an object for selected control state.
        /// </summary>
        public virtual T? Selected
        {
            get => selected;
            set => SetProperty(ref selected, value, nameof(Selected), RaiseSelectedChanged);
        }

        /// <summary>
        /// Gets or sets an object for hovered control state.
        /// </summary>
        public virtual T? Hovered
        {
            get => hovered;
            set => SetProperty(ref hovered, value, nameof(Hovered), RaiseHoveredChanged);
        }

        /// <summary>
        /// Gets or sets an object for pressed control state.
        /// </summary>
        public virtual T? Pressed
        {
            get => pressed;
            set => SetProperty(ref pressed, value, nameof(Pressed), RaisePressedChanged);
        }

        /// <summary>
        /// Gets or sets an object for disabled control state.
        /// </summary>
        public virtual T? Disabled
        {
            get => disabled;
            set => SetProperty(ref disabled, value, nameof(Disabled), RaiseDisabledChanged);
        }

        /// <summary>
        /// Gets whether there are other data for states except <see cref="Normal"/>.
        /// </summary>
        public virtual bool HasOtherStates
        {
            get
            {
                return (disabled is not null) || (hovered is not null) || (pressed is not null)
                    || (focused is not null) || (selected is not null);
            }
        }

        /// <summary>
        /// Gets whether a value is assigned for the specified state.
        /// </summary>
        /// <param name="state">Control state.</param>
        /// <returns></returns>
        public virtual bool HasObject(VisualControlState state) => GetObjectOrNormal(state) != null;

        /// <summary>
        /// Gets an object for the specified state or <see cref="Normal"/> if
        /// object for that state is not specified.
        /// </summary>
        /// <param name="state">Control state.</param>
        public virtual T? GetObjectOrNormal(VisualControlState state)
        {
            return GetObjectOrNull(state) ?? normal;
        }

        /// <summary>
        /// Sets value from one state to another.
        /// </summary>
        /// <param name="stateToChange">The state to change.</param>
        /// <param name="assignFromState">The state to get value from.</param>
        public virtual void SetStateFromState(
            VisualControlState stateToChange,
            VisualControlState assignFromState)
        {
            var value = GetObjectOrNull(assignFromState);
            SetObject(value, stateToChange);
        }

        /// <summary>
        /// Sets an object for the specified state.
        /// </summary>
        /// <param name="state">Control state.</param>
        /// <param name="value">Object value.</param>
        public virtual void SetObject(T? value, VisualControlState state = VisualControlState.Normal)
        {
            switch (state)
            {
                case VisualControlState.Normal:
                    Normal = value;
                    return;
                case VisualControlState.Hovered:
                    Hovered = value;
                    return;
                case VisualControlState.Pressed:
                    Pressed = value;
                    return;
                case VisualControlState.Disabled:
                    Disabled = value;
                    return;
                case VisualControlState.Focused:
                    Focused = value;
                    return;
                case VisualControlState.Selected:
                    Selected = value;
                    return;
                default:
                    return;
            }
        }

        /// <summary>
        /// Assigns data for all states from the <paramref name="source"/>.
        /// </summary>
        /// <param name="source"></param>
        public virtual void Assign(ControlStateObjects<T>? source)
        {
            if (source is null)
            {
                Normal = default;
                Hovered = default;
                Pressed = default;
                Disabled = default;
                Focused = default;
                Selected = default;
            }
            else
            {
                Normal = source.Normal;
                Hovered = source.Hovered;
                Pressed = source.Pressed;
                Disabled = source.Disabled;
                Focused = source.Focused;
                Selected = source.Selected;
            }
        }

        /// <summary>
        /// Sets data for all the states with the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Object value.</param>
        public virtual void SetAll(T? value)
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
        public virtual T GetObjectOrAction(VisualControlState state, Func<T> action)
        {
            return GetObjectOrNull(state) ?? action();
        }

        /// <summary>
        /// Gets an object for the specified state or <paramref name="defaultValue"/> if
        /// object for that state is not specified.
        /// </summary>
        /// <param name="state">Control state.</param>
        /// <param name="defaultValue">Default result value.</param>
        public virtual T GetObjectOrDefault(VisualControlState state, T defaultValue)
        {
            return GetObjectOrNull(state) ?? defaultValue;
        }

        /// <summary>
        /// Gets an object for the specified state or <c>null</c> if image for that state
        /// is not specified.
        /// </summary>
        /// <param name="state">Control state.</param>
        public virtual T? GetObjectOrNull(VisualControlState state)
        {
            return state switch
            {
                VisualControlState.Hovered => hovered,
                VisualControlState.Pressed => pressed,
                VisualControlState.Disabled => disabled,
                VisualControlState.Focused => focused,
                VisualControlState.Selected => selected,
                _ => normal,
            };
        }

        /// <summary>
        /// Raises <see cref="NormalChanged"/> event.
        /// </summary>
        public void RaiseNormalChanged()
        {
            NormalChanged?.Invoke(this, EventArgs.Empty);
            ChangedHandler?.NormalChanged(this);
        }

        /// <summary>
        /// Raises <see cref="FocusedChanged"/> event.
        /// </summary>
        public void RaiseFocusedChanged()
        {
            FocusedChanged?.Invoke(this, EventArgs.Empty);
            ChangedHandler?.FocusedChanged(this);
        }

        /// <summary>
        /// Raises <see cref="HoveredChanged"/> event.
        /// </summary>
        public void RaiseHoveredChanged()
        {
            HoveredChanged?.Invoke(this, EventArgs.Empty);
            ChangedHandler?.HoveredChanged(this);
        }

        /// <summary>
        /// Raises <see cref="PressedChanged"/> event.
        /// </summary>
        public void RaisePressedChanged()
        {
            PressedChanged?.Invoke(this, EventArgs.Empty);
            ChangedHandler?.PressedChanged(this);
        }

        /// <summary>
        /// Raises <see cref="SelectedChanged"/> event.
        /// </summary>
        public void RaiseSelectedChanged()
        {
            SelectedChanged?.Invoke(this, EventArgs.Empty);
            ChangedHandler?.SelectedChanged(this);
        }

        /// <summary>
        /// Raises <see cref="DisabledChanged"/> event.
        /// </summary>
        public void RaiseDisabledChanged()
        {
            DisabledChanged?.Invoke(this, EventArgs.Empty);
            ChangedHandler?.DisabledChanged(this);
        }
    }
}