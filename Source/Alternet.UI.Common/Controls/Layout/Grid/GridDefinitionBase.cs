#nullable disable
#pragma warning disable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the functionality that is used by the <see cref="GridColumnCollection"/> and
    /// <see cref="GridRowCollection"/> classes. This is an abstract class.
    /// </summary>
    public abstract class GridDefinitionBase : FrameworkElement
    {
        internal GridDefinitionBase(bool isColumnDefinition)
        {
            _isColumnDefinition = isColumnDefinition;
            _parentIndex = -1;
        }

        // string sharedSizeGroup;

        ///// <summary>
        ///// SharedSizeGroup property.
        ///// </summary>
        // public string SharedSizeGroup
        // {
        //    get { return sharedSizeGroup; }
        //    set
        //    {
        //        if (!SharedSizeGroupPropertyValueValid(value))
        //            throw new ArgumentException();
        //        sharedSizeGroup = value;
        //        OnSharedSizeGroupPropertyChanged(value);
        //    }
        //}

        ///// <summary>
        ///// Callback to notify about entering model tree.
        ///// </summary>
        // internal void OnEnterParentTree()
        // {
        //    if (_sharedState == null)
        //    {
        //        //  start with getting SharedSizeGroup value.
        //        //  this property is NOT inherited which should result in better overall perf.
        //        string sharedSizeGroupId = SharedSizeGroup;
        //        if (sharedSizeGroupId != null)
        //        {
        //            // yet to do
        //            //var privateSharedSizeScope = GetPrivateSharedSizeScope();
        //            //if (privateSharedSizeScope != null)
        //            //{
        //            //    _sharedState = privateSharedSizeScope.EnsureSharedState(sharedSizeGroupId);
        //            //    _sharedState.AddMember(this);
        //            //}
        //        }
        //    }
        // }

        /// <summary>
        /// Callback to notify about exiting model tree.
        /// </summary>
        internal void OnExitParentTree()
        {
            _offset = 0;
            if (_sharedState != null)
            {
                _sharedState.RemoveMember(this);
                _sharedState = null;
            }
        }

        /// <summary>
        /// Performs action preparing definition to enter layout calculation mode.
        /// </summary>
        internal void OnBeforeLayout(Grid grid)
        {
            // reset layout state.
            _minSize = 0;
            LayoutWasUpdated = true;

            // defer verification for shared definitions
            if (_sharedState != null) { _sharedState.EnsureDeferredValidation(grid); }
        }

        /// <summary>
        /// Updates min size.
        /// </summary>
        /// <param name="minSize">New size.</param>
        internal void UpdateMinSize(Coord minSize)
        {
            _minSize = Math.Max(_minSize, minSize);
        }

        /// <summary>
        /// Sets min size.
        /// </summary>
        /// <param name="minSize">New size.</param>
        internal void SetMinSize(Coord minSize)
        {
            _minSize = minSize;
        }

        /// <remarks>
        /// This method needs to be internal to be accessible from derived classes.
        /// </remarks>
        internal void OnUserSizePropertyChanged(GridLength oldValue, GridLength newValue)
        {
            if (InParentLogicalTree)
            {
                if (_sharedState != null)
                {
                    _sharedState.Invalidate();
                }
                else
                {
                    Grid parentGrid = (Grid)LogicalParent;

                    if (((GridLength)oldValue).GridUnitType != ((GridLength)newValue).GridUnitType)
                        parentGrid.InvalidateCells();
                    parentGrid.PerformLayout();
                }
            }
        }

        /// <remarks>
        /// This method needs to be internal to be accessible from derived classes.
        /// </remarks>
        internal static bool IsUserSizePropertyValueValid(object value)
        {
            return (((GridLength)value).Value >= 0);
        }

        /// <remarks>
        /// This method needs to be internal to be accessible from derived classes.
        /// </remarks>
        internal void OnUserMinSizePropertyChanged(Coord newValue)
        {
            if (InParentLogicalTree)
            {
                Grid parentGrid = (Grid)LogicalParent;
                parentGrid.PerformLayout();
            }
        }

        /// <remarks>
        /// This method needs to be internal to be accessible from derived classes.
        /// </remarks>
        internal static bool IsUserMinSizePropertyValueValid(object value)
        {
            Coord v = (Coord)value;
            return (!MathUtils.IsNaN(v) && v >= 0.0d && !Coord.IsPositiveInfinity(v));
        }

        /// <remarks>
        /// This method needs to be internal to be accessible from derived classes.
        /// </remarks>
        internal void OnUserMaxSizePropertyChanged(Coord newValue)
        {
            if (InParentLogicalTree)
            {
                Grid parentGrid = (Grid)LogicalParent;
                parentGrid.PerformLayout();
            }
        }

        /// <remarks>
        /// This method needs to be internal to be accessible from derived classes.
        /// </remarks>
        internal static bool IsUserMaxSizePropertyValueValid(object value)
        {
            Coord v = (Coord)value;
            return (!MathUtils.IsNaN(v) && v >= 0.0d);
        }

        /// <remarks>
        /// This method reflects Grid.SharedScopeProperty state by setting / clearing
        /// dynamic property PrivateSharedSizeScopeProperty. Value of PrivateSharedSizeScopeProperty
        /// is a collection of SharedSizeState objects for the scope.
        /// Also PrivateSharedSizeScopeProperty is FrameworkPropertyMetadataOptions.Inherits property.
        /// So that all children
        /// elements belonging to a certain scope can easily access SharedSizeState collection. As well
        /// as been notified about enter / exit a scope.
        /// </remarks>
        internal static void OnIsSharedSizeScopePropertyChanged(AbstractControl control, bool newValue)
        {
            // yet to do
            ////  is it possible to optimize here something like this:
            ////  if ((bool)d.GetValue(Grid.IsSharedSizeScopeProperty) == (d.GetLocalValue(PrivateSharedSizeScopeProperty) != null)
            ////  { /* do nothing */ }
            //if (newValue)
            //{
            //    SharedSizeScope sharedStatesCollection = new SharedSizeScope();
            //    SetPrivateSharedSizeScope(sharedStatesCollection);
            //}
            //else
            //{
            //    SetPrivateSharedSizeScope(null);
            //}
        }

        /// <summary>
        /// Returns <c>true</c> if this definition is a part of shared group.
        /// </summary>
        internal bool IsShared
        {
            get { return (_sharedState != null); }
        }

        /// <summary>
        /// Internal accessor to user size field.
        /// </summary>
        internal GridLength UserSize
        {
            get { return (_sharedState != null ? _sharedState.UserSize : UserSizeValueCache); }
        }

        /// <summary>
        /// Internal accessor to user min size field.
        /// </summary>
        internal Coord UserMinSize
        {
            get { return (UserMinSizeValueCache); }
        }

        /// <summary>
        /// Internal accessor to user max size field.
        /// </summary>
        internal Coord UserMaxSize
        {
            get { return (UserMaxSizeValueCache); }
        }

        /// <summary>
        /// DefinitionBase's index in the parents collection.
        /// </summary>
        internal int Index
        {
            get
            {
                return (_parentIndex);
            }
            set
            {
                Debug.Assert(value >= -1 && _parentIndex != value);
                _parentIndex = value;
            }
        }

        /// <summary>
        /// Layout-time user size type.
        /// </summary>
        internal Grid.LayoutTimeSizeType SizeType
        {
            get { return (_sizeType); }
            set { _sizeType = value; }
        }

        /// <summary>
        /// Returns or sets measure size for the definition.
        /// </summary>
        internal Coord MeasureSize
        {
            get { return (_measureSize); }
            set { _measureSize = value; }
        }

        /// <summary>
        /// Returns definition's layout time type sensitive preferred size.
        /// </summary>
        /// <remarks>
        /// Returned value is guaranteed to be true preferred size.
        /// </remarks>
        internal Coord PreferredSize
        {
            get
            {
                Coord preferredSize = MinSize;
                if (_sizeType != Grid.LayoutTimeSizeType.Auto
                    && preferredSize < _measureSize)
                {
                    preferredSize = _measureSize;
                }
                return (preferredSize);
            }
        }

        /// <summary>
        /// Returns or sets size cache for the definition.
        /// </summary>
        internal Coord SizeCache
        {
            get { return (_sizeCache); }
            set { _sizeCache = value; }
        }

        /// <summary>
        /// Returns min size.
        /// </summary>
        internal Coord MinSize
        {
            get
            {
                Coord minSize = _minSize;
                if (UseSharedMinimum
                    && _sharedState != null
                    && minSize < _sharedState.MinSize)
                {
                    minSize = _sharedState.MinSize;
                }
                return (minSize);
            }
        }

        /// <summary>
        /// Returns min size, always taking into account shared state.
        /// </summary>
        internal Coord MinSizeForArrange
        {
            get
            {
                Coord minSize = _minSize;
                if (_sharedState != null
                    && (UseSharedMinimum || !LayoutWasUpdated)
                    && minSize < _sharedState.MinSize)
                {
                    minSize = _sharedState.MinSize;
                }
                return (minSize);
            }
        }

        /// <summary>
        /// Returns min size, never taking into account shared state.
        /// </summary>
        internal Coord RawMinSize
        {
            get { return _minSize; }
        }

        /// <summary>
        /// Offset.
        /// </summary>
        internal Coord FinalOffset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        /// <summary>
        /// Internal helper to access up-to-date UserSize property value.
        /// </summary>
        internal GridLength UserSizeValueCache
        {
            get
            {
                return _isColumnDefinition ? ((ColumnDefinition)this).Width : ((RowDefinition)this).Height;
            }
        }

        /// <summary>
        /// Internal helper to access up-to-date UserMinSize property value.
        /// </summary>
        internal Coord UserMinSizeValueCache
        {
            get
            {
                return _isColumnDefinition ? ((ColumnDefinition)this).MinWidth : ((RowDefinition)this).MinHeight;
            }
        }

        /// <summary>
        /// Internal helper to access up-to-date UserMaxSize property value.
        /// </summary>
        internal Coord UserMaxSizeValueCache
        {
            get
            {
                return _isColumnDefinition ? ((ColumnDefinition)this).MaxWidth : ((RowDefinition)this).MaxHeight;
            }
        }

        /// <summary>
        /// Protected. Returns <c>true</c> if this DefinitionBase instance is in parent's logical tree.
        /// </summary>
        internal bool InParentLogicalTree
        {
            get { return (_parentIndex != -1); }
        }

        /// <summary>
        /// SetFlags is used to set or unset one or multiple
        /// flags on the object.
        /// </summary>
        private void SetFlags(bool value, Flags flags)
        {
            _flags = value ? (_flags | flags) : (_flags & (~flags));
        }

        /// <summary>
        /// CheckFlagsAnd returns <c>true</c> if all the flags in the
        /// given bitmask are set on the object.
        /// </summary>
        private bool CheckFlagsAnd(Flags flags)
        {
            return ((_flags & flags) == flags);
        }

        //private void OnSharedSizeGroupPropertyChanged(string newValue)
        //{
        //    if (InParentLogicalTree)
        //    {
        //        string sharedSizeGroupId = newValue;

        //        if (_sharedState != null)
        //        {
        //            //  if definition is already registered AND shared size group id is changing,
        //            //  then un-register the definition from the current shared size state object.
        //            _sharedState.RemoveMember(this);
        //            _sharedState = null;
        //        }

        //        if ((_sharedState == null) && (sharedSizeGroupId != null))
        //        {
        //            //SharedSizeScope privateSharedSizeScope = GetPrivateSharedSizeScope(); // yezo todo
        //            //if (privateSharedSizeScope != null)
        //            //{
        //            //    //  if definition is not registered and both: shared size group id AND private shared scope
        //            //    //  are available, then register definition.
        //            //    _sharedState = privateSharedSizeScope.EnsureSharedState(sharedSizeGroupId);
        //            //    _sharedState.AddMember(this);
        //            //}
        //        }
        //    }
        //}

        ///// <remarks>
        ///// Verifies that Shared Size Group Property string
        ///// a) not empty.
        ///// b) contains only letters, digits and underscore ('_').
        ///// c) does not start with a digit.
        ///// </remarks>
        // private static bool SharedSizeGroupPropertyValueValid(object value)
        // {
        //    //  null is default value
        //    if (value == null)
        //    {
        //        return (true);
        //    }

        //    string id = (string)value;

        //    if (id != string.Empty)
        //    {
        //        int i = -1;
        //        while (++i < id.Length)
        //        {
        //            bool isDigit = Char.IsDigit(id[i]);

        //            if ((i == 0 && isDigit)
        //                || !(isDigit
        //                    || Char.IsLetter(id[i])
        //                    || '_' == id[i]))
        //            {
        //                break;
        //            }
        //        }

        //        if (i == id.Length)
        //        {
        //            return (true);
        //        }
        //    }

        //    return (false);
        //}

        /// <summary>
        /// </summary>
        /// <remark>
        /// OnPrivateSharedSizeScopePropertyChanged is called when new scope enters or
        /// existing scope just left. In both cases if the DefinitionBase object is already registered
        /// in SharedSizeState, it should un-register and register itself in a new one.
        /// </remark>
        private void OnPrivateSharedSizeScopePropertyChanged(SharedSizeScope newValue)
        {
            // DefinitionBase definition = this;

            // if (definition.InParentLogicalTree)
            // {
            //    SharedSizeScope privateSharedSizeScope = newValue;

            //    if (definition._sharedState != null)
            //    {
            //        //  if definition is already registered And shared size scope is changing,
            //        //  then un-register the definition from the current shared size state object.
            //        definition._sharedState.RemoveMember(definition);
            //        definition._sharedState = null;
            //    }

            //    if ((definition._sharedState == null) && (privateSharedSizeScope != null))
            //    {
            //        string sharedSizeGroup = definition.SharedSizeGroup;
            //        if (sharedSizeGroup != null)
            //        {
            //            //  if definition is not registered and both: shared size group id AND private shared scope
            //            //  are available, then register definition.
            //            definition._sharedState = privateSharedSizeScope.EnsureSharedState(definition.SharedSizeGroup);
            //            definition._sharedState.AddMember(definition);
            //        }
            //    }
            // }
        }

        static Dictionary<AbstractControl, SharedSizeScope> privateSharedSizeScopes = new Dictionary<AbstractControl, SharedSizeScope>();

        /// <summary>
        /// Private getter of shared state collection dynamic property.
        /// </summary>
        private static SharedSizeScope GetPrivateSharedSizeScope(AbstractControl control)
        {
            return privateSharedSizeScopes.TryGetValue(control, out var value) ? value : null;
        }

        /// <summary>
        /// Private getter of shared state collection dynamic property.
        /// </summary>
        private void SetPrivateSharedSizeScope(AbstractControl control, SharedSizeScope value)
        {
            privateSharedSizeScopes[control] = value;
            OnPrivateSharedSizeScopePropertyChanged(value);
        }

        /// <summary>
        /// Convenience accessor to UseSharedMinimum flag
        /// </summary>
        private bool UseSharedMinimum
        {
            get { return (CheckFlagsAnd(Flags.UseSharedMinimum)); }
            set { SetFlags(value, Flags.UseSharedMinimum); }
        }

        /// <summary>
        /// Convenience accessor to LayoutWasUpdated flag
        /// </summary>
        private bool LayoutWasUpdated
        {
            get { return (CheckFlagsAnd(Flags.LayoutWasUpdated)); }
            set { SetFlags(value, Flags.LayoutWasUpdated); }
        }

        private readonly bool _isColumnDefinition;      //  when "true", this is a ColumnDefinition; when "false" this is a RowDefinition (faster than a type check)
        private Flags _flags;                           //  flags reflecting various aspects of internal state
        private int _parentIndex;                       //  this instance's index in parent's children collection

        private Grid.LayoutTimeSizeType _sizeType;      //  layout-time user size type. it may differ from _userSizeValueCache.UnitType when calculating "to-content"

        private Coord _minSize;                        //  used during measure to accumulate size for "Auto" and "Star" DefinitionBase's
        private Coord _measureSize;                    //  size, calculated to be the input constraint size for Child.Measure
        private Coord _sizeCache;                      //  cache used for various purposes (sorting, caching, etc) during calculations
        private Coord _offset;                         //  offset of the DefinitionBase from left / top corner (assuming LTR case)

        private SharedSizeState _sharedState;           //  reference to shared state object this instance is registered with

        internal const bool ThisIsColumnDefinition = true;
        internal const bool ThisIsRowDefinition = false;

        [System.Flags]
        private enum Flags : byte
        {
            //
            //  bool flags
            //
            UseSharedMinimum = 0x00000020,     //  when "1", definition will take into account shared state's minimum
            LayoutWasUpdated = 0x00000040,     //  set to "1" every time the parent grid is measured
        }

        /// <summary>
        /// Collection of shared states objects for a single scope
        /// </summary>
        private class SharedSizeScope
        {
            /// <summary>
            /// Returns SharedSizeState object for a given group.
            /// Creates a new StatedState object if necessary.
            /// </summary>
            internal SharedSizeState EnsureSharedState(string sharedSizeGroup)
            {
                //  check that sharedSizeGroup is not default
                Debug.Assert(sharedSizeGroup != null);

                SharedSizeState sharedState = _registry[sharedSizeGroup] as SharedSizeState;
                if (sharedState == null)
                {
                    sharedState = new SharedSizeState(this, sharedSizeGroup);
                    _registry[sharedSizeGroup] = sharedState;
                }
                return (sharedState);
            }

            /// <summary>
            /// Removes an entry in the registry by the given key.
            /// </summary>
            internal void Remove(object key)
            {
                Debug.Assert(_registry.Contains(key));
                _registry.Remove(key);
            }

            private Hashtable _registry = new Hashtable();  //  storage for shared state objects
        }

        /// <summary>
        /// Implementation of per shared group state object
        /// </summary>
        private class SharedSizeState
        {
            /// <summary>
            /// Default ctor.
            /// </summary>
            internal SharedSizeState(SharedSizeScope sharedSizeScope, string sharedSizeGroupId)
            {
                Debug.Assert(sharedSizeScope != null && sharedSizeGroupId != null);
                _sharedSizeScope = sharedSizeScope;
                _sharedSizeGroupId = sharedSizeGroupId;
                _registry = new List<GridDefinitionBase>();
                _layoutUpdated = new EventHandler(OnLayoutUpdated);
                _broadcastInvalidation = true;
            }

            /// <summary>
            /// Adds / registers a definition instance.
            /// </summary>
            internal void AddMember(GridDefinitionBase member)
            {
                Debug.Assert(!_registry.Contains(member));
                _registry.Add(member);
                Invalidate();
            }

            /// <summary>
            /// Removes / un-registers a definition instance.
            /// </summary>
            /// <remarks>
            /// If the collection of registered definitions becomes empty
            /// instantiates self removal from owner's collection.
            /// </remarks>
            internal void RemoveMember(GridDefinitionBase member)
            {
                Invalidate();
                _registry.Remove(member);

                if (_registry.Count == 0)
                {
                    _sharedSizeScope.Remove(_sharedSizeGroupId);
                }
            }

            /// <summary>
            /// Propogates invalidations for all registered definitions.
            /// Resets its own state.
            /// </summary>
            internal void Invalidate()
            {
                _userSizeValid = false;

                if (_broadcastInvalidation)
                {
                    for (int i = 0, count = _registry.Count; i < count; ++i)
                    {
                        Grid parentGrid = (Grid)(_registry[i].LogicalParent);
                        parentGrid.InvalidateCells();
                    }
                    _broadcastInvalidation = false;
                }
            }

            /// <summary>
            /// Makes sure that one and only one layout updated handler is registered for this shared state.
            /// </summary>
            internal void EnsureDeferredValidation(AbstractControl layoutUpdatedHost)
            {
                if (_layoutUpdatedHost == null)
                {
                    _layoutUpdatedHost = layoutUpdatedHost;
                    //_layoutUpdatedHost.LayoutUpdated += _layoutUpdated; // yezo todo
                }
            }

            /// <summary>
            /// DefinitionBase's specific code.
            /// </summary>
            internal Coord MinSize
            {
                get
                {
                    if (!_userSizeValid) { EnsureUserSizeValid(); }
                    return (_minSize);
                }
            }

            /// <summary>
            /// DefinitionBase's specific code.
            /// </summary>
            internal GridLength UserSize
            {
                get
                {
                    if (!_userSizeValid) { EnsureUserSizeValid(); }
                    return (_userSize);
                }
            }

            private void EnsureUserSizeValid()
            {
                _userSize = new GridLength(1, GridUnitType.Auto);

                for (int i = 0, count = _registry.Count; i < count; ++i)
                {
                    Debug.Assert(_userSize.GridUnitType == GridUnitType.Auto
                                || _userSize.GridUnitType == GridUnitType.Pixel);

                    GridLength currentGridLength = _registry[i].UserSizeValueCache;
                    if (currentGridLength.GridUnitType == GridUnitType.Pixel)
                    {
                        if (_userSize.GridUnitType == GridUnitType.Auto)
                        {
                            _userSize = currentGridLength;
                        }
                        else if (_userSize.Value < currentGridLength.Value)
                        {
                            _userSize = currentGridLength;
                        }
                    }
                }
                //  taking maximum with user size effectively prevents squishy-ness.
                //  this is a "solution" to avoid shared definitions from been sized to
                //  different final size at arrange time, if / when different grids receive
                //  different final sizes.
                _minSize = _userSize.IsAbsolute ? _userSize.Value : 0;

                _userSizeValid = true;
            }

            /// <summary>
            /// OnLayoutUpdated handler. Validates that all participating definitions
            /// have updated min size value. Forces another layout update cycle if needed.
            /// </summary>
            private void OnLayoutUpdated(object sender, EventArgs e)
            {
                Coord sharedMinSize = 0;

                //  accumulate min size of all participating definitions
                for (int i = 0, count = _registry.Count; i < count; ++i)
                {
                    sharedMinSize = Math.Max(sharedMinSize, _registry[i]._minSize);
                }

                bool sharedMinSizeChanged = !MathUtils.AreClose(_minSize, sharedMinSize);

                //  compare accumulated min size with min sizes of the individual definitions
                for (int i = 0, count = _registry.Count; i < count; ++i)
                {
                    GridDefinitionBase definitionBase = _registry[i];

                    // we'll set d.UseSharedMinimum to maintain the invariant:
                    //      d.UseSharedMinimum iff d._minSize < this.MinSize
                    // i.e. iff d is not a "long-pole" definition.
                    //
                    // Measure/Arrange of d's Grid uses d._minSize for long-pole
                    // definitions, and max(d._minSize, shared size) for
                    // short-pole definitions.  This distinction allows us to react
                    // to changes in "long-pole-ness" more efficiently and correctly,
                    // by avoiding remeasures when a long-pole definition changes.
                    bool useSharedMinimum = !MathUtils.AreClose(definitionBase._minSize, sharedMinSize);

                    // before doing that, determine whether d's Grid needs to be remeasured.
                    // It's important _not_ to remeasure if the last measure is still
                    // valid, otherwise infinite loops are possible
                    bool measureIsValid;
                    if (!definitionBase.UseSharedMinimum)
                    {
                        // d was a long-pole.  measure is valid iff it's still a long-pole,
                        // since previous measure didn't use shared size.
                        measureIsValid = !useSharedMinimum;
                    }
                    else if (useSharedMinimum)
                    {
                        // d was a short-pole, and still is.  measure is valid
                        // iff the shared size didn't change
                        measureIsValid = !sharedMinSizeChanged;
                    }
                    else
                    {
                        // d was a short-pole, but is now a long-pole.  This can
                        // happen in several ways:
                        //  a. d's minSize increased to or past the old shared size
                        //  b. other long-pole definitions decreased, leaving
                        //      d as the new winner
                        // In the former case, the measure is valid - it used
                        // d's new larger minSize.  In the latter case, the
                        // measure is invalid - it used the old shared size,
                        // which is larger than d's (possibly changed) minSize
                        measureIsValid = (definitionBase.LayoutWasUpdated &&
                                        MathUtils.GreaterThanOrClose(definitionBase._minSize, this.MinSize));
                    }

                    if (!measureIsValid)
                    {
                        Grid parentGrid = (Grid)definitionBase.LogicalParent;
                        parentGrid.PerformLayout();
                    }
                    else if (!MathUtils.AreClose(sharedMinSize, definitionBase.SizeCache))
                    {
                        //  if measure is valid then also need to check arrange.
                        //  Note: definitionBase.SizeCache is volatile but at this point
                        //  it contains up-to-date final size
                        Grid parentGrid = (Grid)definitionBase.LogicalParent;

                        parentGrid.PerformLayout();
                    }

                    // now we can restore the invariant, and clear the layout flag
                    definitionBase.UseSharedMinimum = useSharedMinimum;
                    definitionBase.LayoutWasUpdated = false;
                }

                _minSize = sharedMinSize;

                // _layoutUpdatedHost.LayoutUpdated -= _layoutUpdated; // yezo todo
                _layoutUpdatedHost = null;

                _broadcastInvalidation = true;
            }

            private readonly SharedSizeScope _sharedSizeScope;  //  the scope this state belongs to
            private readonly string _sharedSizeGroupId;         //  Id of the shared size group this object is servicing
            private readonly List<GridDefinitionBase> _registry;    //  registry of participating definitions
            private readonly EventHandler _layoutUpdated;       //  instance event handler for layout updated event
            private AbstractControl _layoutUpdatedHost;               //  UIElement for which layout updated event handler is registered
            private bool _broadcastInvalidation;                //  "true" when broadcasting of invalidation is needed
            private bool _userSizeValid;                        //  "true" when _userSize is up to date
            private GridLength _userSize;                       //  shared state
            private Coord _minSize;                            //  shared state
        }
    }
}