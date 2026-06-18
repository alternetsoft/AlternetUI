using System;
using System.Collections;
using System.Collections.Specialized;

namespace Alternet.UI;

/// <summary>
/// The delegate to use for handlers that receive the CollectionChanged event.
/// </summary>
public delegate void ListChangedEventHandler(object? sender, ListChangedEventArgs e);

/// <summary>
/// Provides data for the collection changed event.
/// </summary>
public class ListChangedEventArgs : BaseEventArgs
{
    private const string ErrWrongActionForCtor = "Wrong action {0} for ctor";
    private const string ErrResetActionRequiresNullItem = "Reset action requires null item";
    private const string ErrResetActionRequiresIndexMinus1 = "Reset action requires starting index to be -1";
    private const string ErrMustBeResetAddOrRemoveActionForCtor
        = "Must be reset, add, or remove action for ctor";

    private int newStartingIndex = -1;
    private int oldStartingIndex = -1;
    private NotifyCollectionChangedAction action;
    private IList? newItems;
    private IList? oldItems;

    /// <summary>
    /// Gets the action that caused the event.
    /// </summary>
    public NotifyCollectionChangedAction Action
    {
        get
        {
            return action;
        }

        set
        {
            action = value;
        }
    }

    /// <summary>
    /// Gets the list of new items involved in the change.
    /// </summary>
    public IList? NewItems
    {
        get
        {
            return newItems;
        }

        internal set
        {
            newItems = value;
        }
    }

    /// <summary>
    /// Gets the list of old items involved in the change.
    /// </summary>
    public IList? OldItems
    {
        get
        {
            return oldItems;
        }

        internal set
        {
            oldItems = value;
        }
    }

    /// <summary>
    /// Gets the new starting index.
    /// </summary>
    public int NewStartingIndex
    {
        get
        {
            return newStartingIndex;
        }

        internal set
        {
            newStartingIndex = value;
        }
    }

    /// <summary>
    /// Gets the old starting index
    /// </summary>
    public int OldStartingIndex
    {
        get
        {
            return oldStartingIndex;
        }

        internal set
        {
            oldStartingIndex = value;
        }
    }

    /// <summary>
    /// Initializes a new instance of the ListChangedEventArgs class. 
    /// </summary>
    public ListChangedEventArgs()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListChangedEventArgs"/>
    /// class that describes a Reset change.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <exception cref="ArgumentException">Thrown when the action is not Reset.</exception>
    public ListChangedEventArgs(NotifyCollectionChangedAction action)
    {
        if (action != NotifyCollectionChangedAction.Reset)
        {
            throw new ArgumentException(
                SR.Format(ErrWrongActionForCtor, NotifyCollectionChangedAction.Reset), nameof(action));
        }

        this.action = action;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListChangedEventArgs"/>
    /// class that describes an Add, Remove, or Reset change.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="changedItem">The item affected by the change.</param>
    public ListChangedEventArgs(NotifyCollectionChangedAction action, object? changedItem)
        : this(action, changedItem, -1)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListChangedEventArgs"/>
    /// class that describes an Add, Remove, or Reset change.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="changedItem">The item affected by the change.</param>
    /// <param name="index">The index at which the change occurred.</param>
    /// <exception cref="ArgumentException">Thrown when the action is not valid for this constructor.</exception>
    public ListChangedEventArgs(NotifyCollectionChangedAction action, object? changedItem, int index)
    {
        switch (action)
        {
            case NotifyCollectionChangedAction.Reset:
                if (changedItem != null)
                {
                    throw new ArgumentException(ErrResetActionRequiresNullItem, nameof(action));
                }

                if (index != -1)
                {
                    throw new ArgumentException(ErrResetActionRequiresIndexMinus1, nameof(action));
                }

                break;
            case NotifyCollectionChangedAction.Add:
                newItems = new SingleItemReadOnlyList(changedItem);
                newStartingIndex = index;
                break;
            case NotifyCollectionChangedAction.Remove:
                oldItems = new SingleItemReadOnlyList(changedItem);
                oldStartingIndex = index;
                break;
            default:
                throw new ArgumentException(ErrMustBeResetAddOrRemoveActionForCtor, nameof(action));
        }

        this.action = action;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListChangedEventArgs"/>
    /// class that describes an Add, Remove, or Reset change.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="changedItems">The items affected by the change.</param>
    public ListChangedEventArgs(NotifyCollectionChangedAction action, IList? changedItems)
        : this(action, changedItems, -1)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListChangedEventArgs"/> class
    /// that describes an Add, Remove, or Reset change.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="changedItems">The items affected by the change.</param>
    /// <param name="startingIndex">The index at which the change occurred.</param>
    /// <exception cref="ArgumentException">Thrown when the action is not valid for this constructor.</exception>
    public ListChangedEventArgs(NotifyCollectionChangedAction action, IList? changedItems, int startingIndex)
    {
        switch (action)
        {
            case NotifyCollectionChangedAction.Reset:
                if (changedItems != null)
                {
                    throw new ArgumentException(ErrResetActionRequiresNullItem, nameof(action));
                }

                if (startingIndex != -1)
                {
                    throw new ArgumentException(ErrResetActionRequiresIndexMinus1, nameof(action));
                }

                break;
            case NotifyCollectionChangedAction.Add:
            case NotifyCollectionChangedAction.Remove:
                ArgumentNullException.ThrowIfNull(changedItems, nameof(changedItems));
                ArgumentOutOfRangeException.ThrowIfLessThan(startingIndex, -1, nameof(startingIndex));
                if (action == NotifyCollectionChangedAction.Add)
                {
                    newItems = new ReadOnlyListWrapper(changedItems);
                    newStartingIndex = startingIndex;
                }
                else
                {
                    oldItems = new ReadOnlyListWrapper(changedItems);
                    oldStartingIndex = startingIndex;
                }

                break;
            default:
                throw new ArgumentException(ErrMustBeResetAddOrRemoveActionForCtor, nameof(action));
        }

        this.action = action;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListChangedEventArgs"/> class that
    /// describes a replace change.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="newItem">The new item involved in the change.</param>
    /// <param name="oldItem">The old item involved in the change.</param>
    public ListChangedEventArgs(NotifyCollectionChangedAction action, object? newItem, object? oldItem)
        : this(action, newItem, oldItem, -1)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListChangedEventArgs"/>
    /// class that describes a replace change.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="newItem">The new item involved in the change.</param>
    /// <param name="oldItem">The old item involved in the change.</param>
    /// <param name="index">The index at which the change occurred.</param>
    /// <exception cref="ArgumentException">Thrown when the action is not valid for this constructor.</exception>
    public ListChangedEventArgs(
        NotifyCollectionChangedAction action,
        object? newItem,
        object? oldItem,
        int index)
    {
        if (action != NotifyCollectionChangedAction.Replace)
        {
            throw new ArgumentException(
                SR.Format(ErrWrongActionForCtor, NotifyCollectionChangedAction.Replace), nameof(action));
        }

        this.action = action;
        newItems = new SingleItemReadOnlyList(newItem);
        oldItems = new SingleItemReadOnlyList(oldItem);
        newStartingIndex = (oldStartingIndex = index);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListChangedEventArgs"/>
    /// class that describes a replace change.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="newItems">The new items involved in the change.</param>
    /// <param name="oldItems">The old items involved in the change.</param>
    public ListChangedEventArgs(
        NotifyCollectionChangedAction action,
        IList newItems,
        IList oldItems)
        : this(action, newItems, oldItems, -1)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListChangedEventArgs"/> class that
    /// describes a replace change.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="newItems">The new items involved in the change.</param>
    /// <param name="oldItems">The old items involved in the change.</param>
    /// <param name="startingIndex">The index at which the change occurred.</param>
    /// <exception cref="ArgumentException">Thrown when the action is not valid for this constructor.</exception>
    public ListChangedEventArgs(
        NotifyCollectionChangedAction action,
        IList newItems,
        IList oldItems,
        int startingIndex)
    {
        if (action != NotifyCollectionChangedAction.Replace)
        {
            throw new ArgumentException(
                SR.Format(ErrWrongActionForCtor, NotifyCollectionChangedAction.Replace), nameof(action));
        }

        ArgumentNullException.ThrowIfNull(newItems, nameof(newItems));
        ArgumentNullException.ThrowIfNull(oldItems, nameof(oldItems));
        this.action = action;
        this.newItems = new ReadOnlyListWrapper(newItems);
        this.oldItems = new ReadOnlyListWrapper(oldItems);
        newStartingIndex = (oldStartingIndex = startingIndex);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListChangedEventArgs"/>
    /// class that describes a move change.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="changedItem">The item that was moved.</param>
    /// <param name="index">The new index of the item.</param>
    /// <param name="oldIndex">The old index of the item.</param>
    /// <exception cref="ArgumentException">Thrown when the action is not valid for this constructor.</exception>
    public ListChangedEventArgs(
        NotifyCollectionChangedAction action,
        object? changedItem,
        int index,
        int oldIndex)
    {
        if (action != NotifyCollectionChangedAction.Move)
        {
            throw new ArgumentException(
                SR.Format(ErrWrongActionForCtor, NotifyCollectionChangedAction.Move), nameof(action));
        }

        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        this.action = action;
        newItems = (oldItems = new SingleItemReadOnlyList(changedItem));
        newStartingIndex = index;
        oldStartingIndex = oldIndex;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListChangedEventArgs"/>
    /// class that describes a move change with multiple items.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="changedItems">The items that were moved.</param>
    /// <param name="index">The new index of the items.</param>
    /// <param name="oldIndex">The old index of the items.</param>
    /// <exception cref="ArgumentException">Thrown when the action is not valid for this constructor.</exception>
    public ListChangedEventArgs(
        NotifyCollectionChangedAction action,
        IList? changedItems,
        int index,
        int oldIndex)
    {
        if (action != NotifyCollectionChangedAction.Move)
        {
            throw new ArgumentException(
                SR.Format(ErrWrongActionForCtor, NotifyCollectionChangedAction.Move), nameof(action));
        }

        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        this.action = action;
        newItems = oldItems = (changedItems != null) ? new ReadOnlyListWrapper(changedItems) : null;
        newStartingIndex = index;
        oldStartingIndex = oldIndex;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ListChangedEventArgs"/> class that is a copy of the current instance.
    /// </summary>
    /// <param name="convertItems">A function to convert the items in the list. Optional. If not provided,
    /// the items are copied as-is.</param>
    /// <returns>A new instance of <see cref="ListChangedEventArgs"/> that is a copy of the current instance.</returns>
    public virtual ListChangedEventArgs Clone(Func<IList?, IList?>? convertItems = null)
    {
        ListChangedEventArgs args = new();

        args.NewStartingIndex = NewStartingIndex;
        args.OldStartingIndex = OldStartingIndex;
        args.Action = Action;

        if (convertItems != null)
        {
            args.NewItems = convertItems(NewItems);
            args.OldItems = convertItems(OldItems);
        }
        else
        {
            args.NewItems = NewItems;
            args.OldItems = OldItems;
        }

        return args;
    }
}