using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a base object with notification capabilities.
    /// </summary>
    public partial class BaseObjectWithNotify : BaseObjectWithAttr, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int suspendCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseObjectWithNotify"/>
        /// class with the specified unique id.
        /// </summary>
        /// <param name="uniqueId">The unique identifier for the new instance.</param>
        public BaseObjectWithNotify(ObjectUniqueId uniqueId)
            : base(uniqueId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseObjectWithNotify"/> class.
        /// </summary>
        public BaseObjectWithNotify()
        {
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Occurs before a property value changes.
        /// </summary>
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <summary>
        /// Occurs when a property value change is requested. In the event handler,
        /// you can cancel the property change by setting 'Cancel' to <c>true</c>.
        /// </summary>
        public event EventHandler<PropertyChangeRequestedEventArgs>? PropertyChangeRequested;

        /// <summary>
        /// Gets or sets an action which is called when a property value changes.
        /// </summary>
        [Browsable(false)]
        public Action<string?>? PropertyChangedAction { get; set; }

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

            if (suspendCounter == 0)
            {
                if (callChangedOnResume)
                    RaiseNullPropertyChanged();
            }
        }

        /// <summary>
        /// Calls the specified function inside block with suspended <see cref="PropertyChanged"/> event
        /// and returns the result of the function.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <param name="callChangedOnResume">Whether to call <see cref="PropertyChanged"/> event
        /// after the function is executed and events are resumed. Optional. Default is True.</param>
        /// <returns>The result of the function.</returns>
        public TResult DoInsideSuspendedPropertyChanged<TResult>(Func<TResult> func, bool callChangedOnResume = true)
        {
            SuspendPropertyChanged();
            try
            {
                return func();
            }
            finally
            {
                ResumePropertyChanged(callChangedOnResume);
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
        /// Creates <see cref="PropertyChangedEventArgs"/> for the specified property name
        /// and calls <see cref="RaisePropertyChangedEx(PropertyChangedEventArgs)"/>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public virtual void RaisePropertyChanged(string? propertyName)
        {
            if (DisposingOrDisposed || suspendCounter > 0)
                return;
            OnPropertyChanged(propertyName);
            PropertyChanged?.Invoke(this, EventArgsUtils.GetPropertyChangedEventArgs(propertyName));
            PropertyChangedAction?.Invoke(propertyName);
        }

        /// <summary>
        /// Calls <see cref="PropertyChanged"/> event. If events are suspended
        /// with previous call to <see cref="SuspendPropertyChanged"/>, does nothing.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public virtual void RaisePropertyChangedEx(PropertyChangedEventArgs e)
        {
            if (DisposingOrDisposed || suspendCounter > 0)
                return;
            var propName = e.PropertyName;
            OnPropertyChanged(propName);
            PropertyChanged?.Invoke(this, e);
            PropertyChangedAction?.Invoke(propName);
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> event with <see langword="null"/> property name.
        /// </summary>        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RaiseNullPropertyChanged()
        {
            RaisePropertyChanged(null);
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> event with empty property name.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RaiseEmptyPropertyChanged()
        {
            RaisePropertyChanged(string.Empty);
        }

        /// <summary>
        /// Raises <see cref="PropertyChanging"/> event and calls <see cref="OnPropertyChanging"/> method.
        /// </summary>
        public void RaisePropertyChanging(string propertyName)
        {
            if (DisposingOrDisposed)
                return;
            OnPropertyChanging(propertyName);
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Determines whether the specified property can be changed.
        /// Override this method to implement custom logic for property change validation.
        /// By default, this method always returns true, allowing all property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the property can be changed; otherwise, <c>false</c>.</returns>
        public virtual bool CanChangeProperty(string propertyName)
        {
            if (PropertyChangeRequested != null && !string.IsNullOrEmpty(propertyName))
            {
                if (DisposingOrDisposed)
                    return false;

                var args = new PropertyChangeRequestedEventArgs(propertyName);
                PropertyChangeRequested(this, args);
                if (args.Cancel)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Called when object property is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
        }

        /// <summary>
        /// Sets field value and raises property changed events and methods.
        /// </summary>
        /// <param name="storage">Field where property is stored.</param>
        /// <param name="value">New property value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="changedAction">This action is called when property changes</param>
        /// <returns>True if the property value was changed; otherwise, false.</returns>
        protected virtual bool SetProperty<T>(
            ref T? storage,
            T? value,
            string propertyName,
            Action? changedAction = null)
        {
            if (Immutable || Equals(storage, value))
                return false;
            if (!CanChangeProperty(propertyName))
                return false;
            RaisePropertyChanging(propertyName);
            storage = value;
            RaisePropertyChanged(propertyName);
            changedAction?.Invoke();
            return true;
        }

        /// <summary>
        /// Sets field value and raises property changed events and methods.
        /// </summary>
        /// <param name="storage">Field where property is stored.</param>
        /// <param name="value">New property value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>True if the property value was changed; otherwise, false.</returns>
        protected virtual bool SetProperty<T>(ref T? storage, T? value, string propertyName)
        {
            if (Immutable || Equals(storage, value))
                return false;
            if (!CanChangeProperty(propertyName))
                return false;
            RaisePropertyChanging(propertyName);
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Sets field value and raises property changed events and methods.
        /// This is the same as <see cref="SetProperty{T}(ref T, T, string)"/> but parameters are not nullable.
        /// </summary>
        /// <typeparam name="T">Type of the field value.</typeparam>
        /// <param name="storage">Field where property is stored.</param>
        /// <param name="value">New property value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>True if the property value was changed; otherwise, false.</returns>
        protected virtual bool SetNotNullProperty<T>(ref T storage, T value, string propertyName)
        {
            if (Immutable || Equals(storage, value))
                return false;
            if (!CanChangeProperty(propertyName))
                return false;
            RaisePropertyChanging(propertyName);
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Called before a property value changes.
        /// Override this method to perform custom actions before a property change occurs.
        /// </summary>
        protected virtual void OnPropertyChanging(string propertyName)
        {
        }
    }
}
