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
    /// Extends <see cref="DisposableObject"/> with immutable feature.
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
        public virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            OnPropertyChanged(propertyName);
            PropertyChanged?.Invoke(this, EventArgsUtils.GetPropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets field value and calls <see cref="OnPropertyChanged"/> method.
        /// </summary>
        /// <param name="storage">Field where property is stored.</param>
        /// <param name="value">New property value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="changedAction">This action is called when property changes</param>
        /// <returns></returns>
        protected virtual bool SetProperty<T>(
            ref T? storage,
            T? value,
            [CallerMemberName] string? propertyName = null,
            Action? changedAction = null)
        {
            if (immutable)
                return false;

            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            changedAction?.Invoke();
            return true;
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
