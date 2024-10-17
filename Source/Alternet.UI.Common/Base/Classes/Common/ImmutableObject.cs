using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="DisposableObject"/> with immutable feature,
    /// event <see cref="PropertyChanged"/> and other features.
    /// Allows to implement immutable objects with properties that can not be changed.
    /// </summary>
    public class ImmutableObject : DisposableObject, IImmutableObject, INotifyPropertyChanged
    {
        private bool immutable;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets whether object is immutable (properties can not be changed).
        /// </summary>
        [Browsable(false)]
        public virtual bool Immutable
        {
            get
            {
                return immutable;
            }

            protected set
            {
                immutable = value;
            }
        }

        /// <summary>
        /// Marks the object as immutable.
        /// </summary>
        /// <remarks>
        /// Marks this object as immutable, meaning that the contents of its properties will not change
        /// for the lifetime of the object. This state can be set, but it cannot be cleared once
        /// it is set.
        /// </remarks>
        public virtual void SetImmutable()
        {
            immutable = true;
        }

        /// <summary>
        /// Calls <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            OnPropertyChanged(propertyName);
            PropertyChanged?.Invoke(this, EventArgsUtils.GetPropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets field value and calls <see cref="RaisePropertyChanged"/> method.
        /// </summary>
        /// <param name="storage">Field where property is stored.</param>
        /// <param name="value">New property value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="changedAction">This action is called when property changes</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool SetProperty<T>(
            ref T? storage,
            T? value,
            [CallerMemberName] string? propertyName = null,
            Action? changedAction = null)
        {
            if (immutable || Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            changedAction?.Invoke();
            return true;
        }

        /// <summary>
        /// Sets field value and calls <see cref="RaisePropertyChanged"/> method.
        /// </summary>
        /// <param name="storage">Field where property is stored.</param>
        /// <param name="value">New property value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool SetProperty<T>(ref T? storage, T? value)
        {
            if (immutable || Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged();
            return true;
        }

        /// <summary>
        /// Gets new field value and calls <see cref="RaisePropertyChanged"/> method.
        /// </summary>
        /// <param name="storage">Field value.</param>
        /// <param name="value">New property value.</param>
        /// <returns></returns>
        protected T GetNewFieldValue<T>(T storage, T value)
        {
            if (immutable || Equals(storage, value))
                return storage;
            RaisePropertyChanged();
            return value;
        }

        /// <summary>
        /// Called when object property is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
        }
    }
}
