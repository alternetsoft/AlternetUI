using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for components that can be loaded from uixml.
    /// </summary>
    public partial class FrameworkElement : DisposableObject, IComponent
    {
        private FrameworkElement? logicalParent;
        private object? dataContext;
        private object? dataContextProperty;
        private string? name;
        private ISite? site;

        /// <summary>
        /// Occurs when the <see cref="DataContextProperty"/>, <see cref="DataContext"/>
        /// or other properties related to data context change.
        /// </summary>
        public event EventHandler? DataContextChanged;

        /// <summary>
        /// Gets or sets the identifying name of the object.
        /// The name provides a reference so that code-behind, such as event handler code,
        /// can refer to a markup object after it is constructed during processing by a
        /// UIXML processor.
        /// </summary>
        /// <value>The name of the object. The default is <c>null</c>.</value>
        public virtual string? Name
        {
            get => name;

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ISite"/> associated with the object.
        /// </summary>
        /// <returns>
        /// The <see cref="ISite" /> associated with the object, if any.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Browsable(false)]
        public virtual ISite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        /// <summary>
        /// Gets or sets property of the <see cref="DataContext"/>
        /// to use in the element. Only some elements use this property.
        /// </summary>
        [Browsable(false)]
        public virtual object? DataContextProperty
        {
            get => dataContextProperty;

            set
            {
                if (dataContextProperty == value)
                    return;
                dataContextProperty = value;
                RaiseDataContextChanged();
            }
        }

        /// <summary>
        /// Gets or sets data context to use in the element.
        /// Only some elements use this property.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object? DataContext
        {
            get => dataContext;

            set
            {
                if (dataContext == value)
                    return;
                dataContext = value;
                RaiseDataContextChanged();
            }
        }

        /// <summary>
        /// Returns a collection of content elements which is used by the UIXML loader to
        /// find content items by index.
        /// </summary>
        [Browsable(false)]
        public virtual IReadOnlyList<FrameworkElement> ContentElements
        {
            get
            {
                return LogicalChildrenCollection.ToArray();
            }
        }

        /// <summary>
        /// Returns a collection of elements which can be treated as "logical children" of
        /// this element.
        /// </summary>
        [Browsable(false)]
        public virtual IEnumerable<FrameworkElement> LogicalChildrenCollection
        {
            get
            {
                return Array.Empty<FrameworkElement>();
            }
        }

        /// <summary>
        /// Gets logical parent.
        /// </summary>
        [Browsable(false)]
        internal virtual FrameworkElement? LogicalParent
        {
            get => logicalParent;

            set
            {
                logicalParent = value;
            }
        }

        /// <summary>
        /// Sets the data context and associated property for the current instance.
        /// </summary>
        /// <remarks>If either the <paramref name="dataContext"/> or
        /// <paramref name="dataContextProperty"/> is different from the current values,
        /// the method updates the respective fields
        /// and raises the <c>DataContextChanged</c> event.
        /// This method ensures that changes to the data context or its
        /// property are properly propagated.</remarks>
        /// <param name="dataContext">The new data context to associate with the instance.
        /// Can be <see langword="null"/>.</param>
        /// <param name="dataContextProperty">The property of the data context to associate
        /// with the instance. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the data context or data context
        /// property was changed; otherwise, <see langword="false"/>.</returns>
        public virtual bool SetDataContext(object? dataContext, object? dataContextProperty)
        {
            bool changed = false;

            if (this.dataContext != dataContext)
            {
                this.dataContext = dataContext;
                changed = true;
            }

            if (this.dataContextProperty != dataContextProperty)
            {
                this.dataContextProperty = dataContextProperty;
                changed = true;
            }

            if (changed)
                RaiseDataContextChanged();

            return changed;
        }

        /// <summary>
        /// Recursively searches all <see cref="LogicalChildrenCollection"/> for a control with
        /// the specified name, and returns that control if found.
        /// </summary>
        /// <param name="name">The name of the control to be found.</param>
        /// <returns>The found control, or <c>null</c> if no control with the provided name
        /// is found.</returns>
        public virtual FrameworkElement? TryFindElement(string? name)
        {
            if (name is null)
                return null;

            if (Name == name)
                return this;

            foreach (var child in LogicalChildrenCollection)
            {
                var result = child.TryFindElement(name);
                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// Recursively searches all <see cref="LogicalChildrenCollection"/> for a control
        /// with the specified name,
        /// and throws an exception if the requested control is not found.
        /// </summary>
        /// <param name="name">The name of the control to be found.</param>
        /// <returns>The requested resource. If no control with the provided name was found,
        /// an exception is thrown.</returns>
        /// <exception cref="InvalidOperationException">A control with the provided name was
        /// found.</exception>
        public FrameworkElement FindElement(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return TryFindElement(name) ?? throw new InvalidOperationException(
                $"Element with name '{name}' was not found.");
        }

        /// <summary>
        /// Raises the <see cref="OnDataContextChanged"/> method and
        /// <see cref="DataContextChanged"/> event.
        /// </summary>
        /// <remarks>This method is called to notify subscribers that the data context has changed.
        /// Derived classes can override this method to provide additional behavior
        /// when the event is raised.</remarks>
        protected virtual void RaiseDataContextChanged()
        {
            OnDataContextChanged();
            DataContextChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the <see cref="DataContextProperty"/> or <see cref="DataContext"/>
        /// property changes.
        /// </summary>
        protected virtual void OnDataContextChanged()
        {
        }
    }
}