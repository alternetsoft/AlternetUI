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
        private int suspendCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableObject"/> class.
        /// </summary>
        public ImmutableObject()
        {
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets an action which is called when a property value changes.
        /// </summary>
        [Browsable(false)]
        public Action<PropertyChangedEventArgs>? PropertyChangedAction { get; set; }

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
        /// Gets whether <see cref="PropertyChanged"/> event is suspended.
        /// </summary>
        [Browsable(false)]
        public bool IsPropertyChangedSuspended
        {
            get
            {
                return suspendCounter > 0;
            }
        }

        /// <summary>
        /// Marks the object as immutable.
        /// </summary>
        /// <remarks>
        /// Marks this object as immutable, meaning that the contents of its properties
        /// will not change
        /// for the lifetime of the object. This state can be set, but it cannot be cleared once
        /// it is set.
        /// </remarks>
        [Browsable(false)]
        public virtual void SetImmutable()
        {
            immutable = true;
        }

        /// <summary>
        /// Suspends <see cref="PropertyChanged"/> event and <see cref="OnPropertyChanged"/>
        /// method so they will not be called when properties are changed.
        /// </summary>
        public virtual void SuspendPropertyChanged()
        {
            suspendCounter++;
        }

        /// <summary>
        /// Resumes <see cref="PropertyChanged"/> event and <see cref="OnPropertyChanged"/>
        /// method so they will be called when properties are changed.
        /// </summary>
        /// <param name="callChangedOnResume">Whether to call <see cref="PropertyChanged"/> event
        /// after action is executed and events are resumed.
        /// Optional. Default is True.</param>
        public virtual void ResumePropertyChanged(bool callChangedOnResume = true)
        {
            if (suspendCounter <= 0)
            {
                throw new InvalidOperationException(
                    "ResumePropertyChanged is called without previous call to SuspendPropertyChanged");
            }

            suspendCounter--;

            if(suspendCounter == 0)
            {
                if(callChangedOnResume)
                    RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Calls the specified action inside block with suspended
        /// <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="action">Action to call.</param>
        /// <param name="callChangedOnResume">Whether to call <see cref="PropertyChanged"/> event
        /// after action is executed and events are resumed.
        /// Optional. Default is True.</param>
        public void DoInsideSuspendedPropertyChanged(Action action, bool callChangedOnResume = true)
        {
            SuspendPropertyChanged();
            try
            {
                action();
            }
            finally
            {
                ResumePropertyChanged(callChangedOnResume);
            }
        }

        /// <summary>
        /// Calls <see cref="PropertyChanged"/> event. If events are suspended
        /// with previous call to <see cref="SuspendPropertyChanged"/>, does nothing.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (DisposingOrDisposed || suspendCounter > 0)
                return;
            OnPropertyChanged(propertyName);
            var e = EventArgsUtils.GetPropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
            PropertyChangedAction?.Invoke(e);
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
