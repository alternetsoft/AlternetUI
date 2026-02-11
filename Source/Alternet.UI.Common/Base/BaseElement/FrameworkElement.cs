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
        private string? name;
        private ISite? site;

        /// <summary>
        /// Occurs when the <see cref="DataContext"/> property changes.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs>? DataContextChanged;

        /// <summary>
        /// Occurs when the <see cref="Name"/> property changes.
        /// </summary>
        public event EventHandler? NameChanged;

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
                if (name == value)
                    return;
                name = value;
                RaiseNameChanged();
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier that allows the automation
        /// framework to find and interact with this element.
        /// This value may only be set once on an element.
        /// </summary>
        public virtual string? AutomationId { get; set; }

        /// <summary>
        /// Gets or sets the debug identifier for the element.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Browsable(false)]
        public virtual string? DebugIdentifier { get; set; }

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
                var oldValue = dataContext;
                dataContext = value;
                RaiseDataContextChanged(oldValue, value);
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
                return [];
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
                return ContentElements;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is the root element.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsRoot => LogicalParent == null;

        /// <summary>
        /// Gets the logical root of the element tree to which this element belongs.
        /// If this element is the root, it returns itself.
        /// </summary>
        [Browsable(false)]
        public virtual FrameworkElement LogicalRoot
        {
            get
            {
                var parent = LogicalParent;
                if (parent is null)
                    return this;
                var result = parent.LogicalRoot;
                return result;
            }
        }

        /// <summary>
        /// Gets the topmost logical parent in the hierarchy of this element.
        /// </summary>
        /// <remarks>This property traverses the logical parent chain to
        /// find the highest-level parent.
        /// If the current element has no logical parent, the value
        /// is <see langword="null"/>.</remarks>
        [Browsable(false)]
        public virtual FrameworkElement? LogicalTopParent
        {
            get
            {
                var parent = LogicalParent;
                if (parent is null)
                    return null;
                var result = parent.LogicalTopParent;
                if (result is null)
                    return parent;
                return result;
            }
        }

        /// <summary>
        /// Gets logical parent.
        /// </summary>
        [Browsable(false)]
        public virtual FrameworkElement? LogicalParent
        {
            get => logicalParent;

            internal set
            {
                logicalParent = value;
            }
        }

        /// <summary>
        /// Gets an enumerable collection of all parent elements in the visual tree,
        /// starting from the immediate parent and traversing upward.
        /// </summary>
        /// <remarks>The enumeration begins with the immediate parent of the current element
        /// and continues up the visual tree until no more parents are found.
        /// This property is useful for scenarios where you need to inspect or process
        /// all ancestor elements of a given element.</remarks>
        [Browsable(false)]
        public virtual IEnumerable<FrameworkElement> LogicalParents
        {
            get
            {
                var result = LogicalParent;

                while (result != null)
                {
                    yield return result;
                    result = result.LogicalParent;
                }
            }
        }

        /// <summary>
        /// Same as setting <see cref="DataContext"/> property.
        /// </summary>
        /// <param name="value">The new data context value.</param>
        public void SetDataContext(object? value)
        {
            DataContext = value;
        }

        /// <summary>
        /// Recursively searches all child elements for an element with
        /// the specified name, and returns that element if found.
        /// </summary>
        /// <param name="name">The name of the element to be found.</param>
        /// <returns>The found element, or <c>null</c> if no element with the provided name
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
        /// Retrieves all child elements that have a name, including those nested within other named elements.
        /// </summary>
        /// <remarks>This method performs a recursive search through the logical children of the current
        /// element, yielding each child that has a non-empty name. It is useful for scenarios where you need to access
        /// named elements in a hierarchical structure.</remarks>
        /// <returns>An enumerable collection of <see cref="FrameworkElement"/> instances that represent the named child elements
        /// found in the logical tree.</returns>
        public virtual IEnumerable<FrameworkElement> GetNamedElementsRecursively()
        {
            foreach (var child in LogicalChildrenCollection)
            {
                if (!string.IsNullOrEmpty(child.Name))
                    yield return child;
                foreach (var element in child.GetNamedElementsRecursively())
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Retrieves all child elements of the current element and its descendants in a recursive manner.
        /// </summary>
        /// <remarks>This method traverses the logical tree of elements, yielding each child element
        /// before recursively retrieving elements from each child. It is useful for scenarios where a complete list of
        /// child elements is needed, such as for layout or rendering purposes.</remarks>
        /// <returns>An enumerable collection of <see cref="FrameworkElement"/> instances representing all child elements found
        /// within the current element and its descendants.</returns>
        public virtual IEnumerable<FrameworkElement> GetElementsRecursively()
        {
            foreach (var child in LogicalChildrenCollection)
            {
                yield return child;

                foreach (var element in child.GetElementsRecursively())
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Recursively searches all child elements for an element
        /// with the specified name,
        /// and throws an exception if the requested element is not found.
        /// </summary>
        /// <param name="name">The name of the element to be found.</param>
        /// <returns>The requested element. If no element with the provided name was found,
        /// an exception is thrown.</returns>
        /// <exception cref="InvalidOperationException">A element with the provided name was
        /// found.</exception>
        public virtual FrameworkElement FindElement(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return TryFindElement(name) ?? throw new InvalidOperationException(
                $"Element with name '{name}' was not found.");
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            DataContext = null;
            base.DisposeManaged();
        }

        /// <summary>
        /// Raises the <see cref="OnNameChanged"/> method and
        /// <see cref="NameChanged"/> event.
        /// </summary>
        protected virtual void RaiseNameChanged()
        {
            OnNameChanged();
            NameChanged?.Invoke(this, EventArgs.Empty);
            RaisePropertyChanged(nameof(Name));
            StaticControlEvents.RaiseNameChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Assigns values to fields of the current instance based on named elements retrieved recursively.
        /// </summary>
        /// <remarks>This method iterates through named elements and assigns them to corresponding fields
        /// if the field names match and the types are compatible. It is intended for use in scenarios where dynamic
        /// assignment of fields is required based on external data sources.</remarks>
        protected virtual void AssignNamedElementsToFields()
        {
            foreach (var namedElement in GetNamedElementsRecursively())
            {
                var fieldName = namedElement.Name;
                if (fieldName is null)
                    continue;
                var field = GetType().GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (field is null || !field.FieldType.IsAssignableFrom(namedElement.GetType()))
                    continue;
                field.SetValue(this, namedElement);
            }
        }

        /// <summary>
        /// Raises the <see cref="OnDataContextChanged"/> method and
        /// <see cref="DataContextChanged"/> event.
        /// </summary>
        /// <remarks>This method is called to notify subscribers that the data context has changed.
        /// Derived classes can override this method to provide additional behavior
        /// when the event is raised.</remarks>
        protected virtual void RaiseDataContextChanged(object? oldValue, object? newValue)
        {
            OnDataContextChanged(oldValue, newValue);
            DataContextChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, newValue));
            RaisePropertyChanged(nameof(DataContext));
            StaticControlEvents.RaiseDataContextChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the <see cref="Name"/> property changes.
        /// </summary>
        protected virtual void OnNameChanged()
        {
        }

        /// <summary>
        /// Called when the <see cref="DataContext"/> property changes.
        /// </summary>
        protected virtual void OnDataContextChanged(object? oldValue, object? newValue)
        {
        }
    }
}