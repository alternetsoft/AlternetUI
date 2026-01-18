#nullable disable
#pragma warning disable
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a flexible grid area that consists of columns and rows.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class Grid : ContainerControl
    {
        //  used in fp calculations
        private const Coord c_epsilon = 1e-5f;
        // used as max for clipping * values during normalization
        private const Coord c_starClip = 1e38f;
        // 5 is an arbitrary constant chosen to end the measure loop
        private const int c_layoutLoopMaxCount = 5;

        /// <summary>
        /// Gets or sets whether to show debug corners when control is painted.
        /// </summary>
        public static bool ShowDebugCorners = false;

        private static readonly LocalDataStoreSlot s_tempDefinitionsDataSlot
            = Thread.AllocateDataSlot();
        private static readonly IComparer s_spanPreferredDistributionOrderComparer
            = new SpanPreferredDistributionOrderComparer();
        private static readonly IComparer s_spanMaxDistributionOrderComparer
            = new SpanMaxDistributionOrderComparer();
        private static readonly IComparer s_starDistributionOrderComparer
            = new StarDistributionOrderComparer();
        private static readonly IComparer s_distributionOrderComparer
            = new DistributionOrderComparer();
        private static readonly IComparer s_minRatioComparer = new MinRatioComparer();
        private static readonly IComparer s_maxRatioComparer = new MaxRatioComparer();
        private static readonly IComparer s_starWeightComparer = new StarWeightComparer();

        //  extended data instantiated on demand, for non-trivial case handling only
        private ExtendedData _data;

        //  grid validity / property caches dirtiness flags
        private Flags _flags;

        // Keeps track of definition indices.
        private int[] _definitionIndices;

        // Stores unrounded values and rounding errors during layout rounding.
        private Coord[] _roundingErrors;

        private bool StarDefinitionsCanExceedAvailableSpace = true;
        private bool UseLayoutRounding;

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public Grid(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Grid"/>.
        /// </summary>
        public Grid()
        {
            CanSelect = false;
            TabStop = false;
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override IEnumerable<FrameworkElement> LogicalChildrenCollection =>
            base.LogicalChildrenCollection.Concat(ColumnDefinitions).Concat(RowDefinitions);

        /// <summary>
        /// Gets a <see cref="GridColumnCollection"/> defined on this instance
        /// of <see cref="Grid"/>.
        /// </summary>
        /// <remarks>
        /// This collection contains one or more <see cref="ColumnDefinition"/> objects.
        /// Each such <see cref="ColumnDefinition"/> becomes a placeholder
        /// representing a column in the final grid layout.
        /// </remarks>
        public GridColumnCollection ColumnDefinitions
        {
            get
            {
                if (_data == null) { _data = new ExtendedData(); }
                if (_data.ColumnDefinitions == null)
                {
                    _data.ColumnDefinitions = new GridColumnCollection(this);
                }

                return (_data.ColumnDefinitions);
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Grid;

        /// <summary>
        /// Gets a <see cref="GridRowCollection"/> defined on this instance of <see cref="Grid"/>.
        /// </summary>
        /// <remarks>
        /// This collection contains one or more <see cref="RowDefinition"/> objects.
        /// Each such <see cref="RowDefinition"/> becomes a placeholder representing
        /// a column in the final grid layout.
        /// </remarks>
        public GridRowCollection RowDefinitions
        {
            get
            {
                if (_data == null) { _data = new ExtendedData(); }
                if (_data.RowDefinitions == null)
                {
                    _data.RowDefinitions = new GridRowCollection(this);
                }

                return (_data.RowDefinitions);
            }
        }

        /// <inheritdoc/>
        protected override void OnCellChanged(EventArgs e)
        {
            base.OnCellChanged(e);
            OnCellAttachedPropertyChanged(this);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        protected override void OnChildInserted(int index, AbstractControl childControl)
        {
            base.OnChildInserted(index, childControl);
            OnChildrenChanged();
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        protected override void OnChildRemoved(AbstractControl childControl)
        {
            base.OnChildRemoved(childControl);
            OnChildrenChanged();
        }

        void SetControlBounds(AbstractControl control, RectD bounds)
        {
            var preferredSize = control.GetPreferredSizeLimited(bounds.Size);

            var horizontalPosition =
                AlignHorizontal(
                    bounds,
                    control,
                    preferredSize,
                    control.HorizontalAlignment);
            var verticalPosition =
                AlignVertical(
                    bounds,
                    control,
                    preferredSize,
                    control.VerticalAlignment);

            control.Bounds = new RectD(
                horizontalPosition.Origin,
                verticalPosition.Origin,
                horizontalPosition.Size,
                verticalPosition.Size);
        }

        void OnChildrenChanged()
        {
            CellsStructureDirty = true;
        }

        internal void InvalidateCells()
        {
            CellsStructureDirty = true;
        }

        internal Coord GetFinalColumnDefinitionWidth(int columnIndex)
        {
            Coord value = 0;

            Debug.Assert(_data != null);

            //  actual value calculations require structure to be up-to-date
            if (!ColumnDefinitionCollectionDirty)
            {
                GridDefinitionBase[] definitions = DefinitionsU;
                value = definitions[(columnIndex + 1) % definitions.Length].FinalOffset;
                if (columnIndex != 0) { value -= definitions[columnIndex].FinalOffset; }
            }
            return (value);
        }

        /// <summary>
        /// Returns final height for a row.
        /// </summary>
        /// <remarks>
        /// Used from public RowDefinition ActualHeight. Calculates final
        /// height using offset data.
        /// </remarks>
        internal Coord GetFinalRowDefinitionHeight(int rowIndex)
        {
            Coord value = 0;

            Debug.Assert(_data != null);

            //  actual value calculations require structure to be up-to-date
            if (!RowDefinitionCollectionDirty)
            {
                GridDefinitionBase[] definitions = DefinitionsV;
                value = definitions[(rowIndex + 1) % definitions.Length].FinalOffset;
                if (rowIndex != 0) { value -= definitions[rowIndex].FinalOffset; }
            }
            return (value);
        }

        internal bool MeasureOverrideInProgress
        {
            get { return (CheckFlagsAnd(Flags.MeasureOverrideInProgress)); }
            set { SetFlags(value, Flags.MeasureOverrideInProgress); }
        }

        internal bool ArrangeOverrideInProgress
        {
            get { return (CheckFlagsAnd(Flags.ArrangeOverrideInProgress)); }
            set { SetFlags(value, Flags.ArrangeOverrideInProgress); }
        }

        internal bool ColumnDefinitionCollectionDirty
        {
            get { return (!CheckFlagsAnd(Flags.ValidDefinitionsUStructure)); }
            set { SetFlags(!value, Flags.ValidDefinitionsUStructure); }
        }

        internal bool RowDefinitionCollectionDirty
        {
            get { return (!CheckFlagsAnd(Flags.ValidDefinitionsVStructure)); }
            set { SetFlags(!value, Flags.ValidDefinitionsVStructure); }
        }

        private void ValidateCells()
        {
            if (CellsStructureDirty)
            {
                ValidateCellsCore();
                CellsStructureDirty = false;
            }
        }

        private void ValidateCellsCore()
        {
            var children = Children;
            ExtendedData extData = ExtData;

            extData.CellCachesCollection = new CellCache[children.Count];
            extData.CellGroup1 = int.MaxValue;
            extData.CellGroup2 = int.MaxValue;
            extData.CellGroup3 = int.MaxValue;
            extData.CellGroup4 = int.MaxValue;

            bool hasStarCellsU = false;
            bool hasStarCellsV = false;
            bool hasGroup3CellsInAutoRows = false;

            for (int i = PrivateCells.Length - 1; i >= 0; --i)
            {
                var child = children[i];
                if (child == null)
                {
                    continue;
                }

                CellCache cell = new CellCache();

                //  read and cache child positioning properties

                //  read indices from the corresponding properties
                //      clamp to value < number_of_columns
                //      column >= 0 is guaranteed by property value validation callback
                cell.ColumnIndex = Math.Min(GetColumn(child), DefinitionsU.Length - 1);
                //      clamp to value < number_of_rows
                //      row >= 0 is guaranteed by property value validation callback
                cell.RowIndex = Math.Min(GetRow(child), DefinitionsV.Length - 1);

                //  read span properties
                //      clamp to not exceed beyond right side of the grid
                //      column_span > 0 is guaranteed by property value validation callback
                cell.ColumnSpan = Math.Min(
                    GetColumnSpan(child),
                    DefinitionsU.Length - cell.ColumnIndex);

                //      clamp to not exceed beyond bottom side of the grid
                //      row_span > 0 is guaranteed by property value validation callback
                cell.RowSpan = Math.Min(GetRowSpan(child), DefinitionsV.Length - cell.RowIndex);

                Debug.Assert(0 <= cell.ColumnIndex && cell.ColumnIndex < DefinitionsU.Length);
                Debug.Assert(0 <= cell.RowIndex && cell.RowIndex < DefinitionsV.Length);

                //
                //  calculate and cache length types for the child
                //

                cell.SizeTypeU
                    = GetLengthTypeForRange(DefinitionsU, cell.ColumnIndex, cell.ColumnSpan);
                cell.SizeTypeV
                    = GetLengthTypeForRange(DefinitionsV, cell.RowIndex, cell.RowSpan);

                hasStarCellsU |= cell.IsStarU;
                hasStarCellsV |= cell.IsStarV;

                //  distribute cells into four groups.

                if (!cell.IsStarV)
                {
                    if (!cell.IsStarU)
                    {
                        cell.Next = extData.CellGroup1;
                        extData.CellGroup1 = i;
                    }
                    else
                    {
                        cell.Next = extData.CellGroup3;
                        extData.CellGroup3 = i;

                        // remember if this cell belongs to auto row
                        hasGroup3CellsInAutoRows |= cell.IsAutoV;
                    }
                }
                else
                {
                    if (cell.IsAutoU
                        // note below: if spans through Star column it is NOT Auto
                        && !cell.IsStarU)
                    {
                        cell.Next = extData.CellGroup2;
                        extData.CellGroup2 = i;
                    }
                    else
                    {
                        cell.Next = extData.CellGroup4;
                        extData.CellGroup4 = i;
                    }
                }

                PrivateCells[i] = cell;
            }

            HasStarCellsU = hasStarCellsU;
            HasStarCellsV = hasStarCellsV;
            HasGroup3CellsInAutoRows = hasGroup3CellsInAutoRows;
        }

        /// <summary>
        /// Initializes DefinitionsU member either to user supplied ColumnDefinitions collection
        /// or to a default single element collection. DefinitionsU gets trimmed to size.
        /// </summary>
        /// <remarks>
        /// This is one of two methods, where ColumnDefinitions and DefinitionsU are directly accessed.
        /// All the rest measure / arrange / render code must use DefinitionsU.
        /// </remarks>
        private void ValidateDefinitionsUStructure()
        {
            if (ColumnDefinitionCollectionDirty)
            {
                ExtendedData extData = ExtData;

                if (extData.ColumnDefinitions == null)
                {
                    if (extData.DefinitionsU == null)
                    {
                        extData.DefinitionsU = new GridDefinitionBase[] { new ColumnDefinition() };
                    }
                }
                else
                {
                    extData.ColumnDefinitions.InternalTrimToSize();

                    if (extData.ColumnDefinitions.InternalCount == 0)
                    {
                        //  if column definitions collection is empty
                        //  mockup array with one column
                        extData.DefinitionsU = new GridDefinitionBase[] { new ColumnDefinition() };
                    }
                    else
                    {
                        extData.DefinitionsU = extData.ColumnDefinitions.InternalItems;
                    }
                }

                ColumnDefinitionCollectionDirty = false;
            }

            Debug.Assert(ExtData.DefinitionsU != null && ExtData.DefinitionsU.Length > 0);
        }

        /// <summary>
        /// Initializes DefinitionsV member either to user supplied RowDefinitions collection
        /// or to a default single element collection. DefinitionsV gets trimmed to size.
        /// </summary>
        /// <remarks>
        /// This is one of two methods, where RowDefinitions and DefinitionsV are directly accessed.
        /// All the rest measure / arrange / render code must use DefinitionsV.
        /// </remarks>
        private void ValidateDefinitionsVStructure()
        {
            if (RowDefinitionCollectionDirty)
            {
                ExtendedData extData = ExtData;

                if (extData.RowDefinitions == null)
                {
                    if (extData.DefinitionsV == null)
                    {
                        extData.DefinitionsV = new GridDefinitionBase[] { new RowDefinition() };
                    }
                }
                else
                {
                    extData.RowDefinitions.InternalTrimToSize();

                    if (extData.RowDefinitions.InternalCount == 0)
                    {
                        //  if row definitions collection is empty
                        //  mockup array with one row
                        extData.DefinitionsV = new GridDefinitionBase[] { new RowDefinition() };
                    }
                    else
                    {
                        extData.DefinitionsV = extData.RowDefinitions.InternalItems;
                    }
                }

                RowDefinitionCollectionDirty = false;
            }

            Debug.Assert(ExtData.DefinitionsV != null && ExtData.DefinitionsV.Length > 0);
        }

        /// <summary>
        /// Validates layout time size type information on given array of definitions.
        /// Sets MinSize and MeasureSizes.
        /// </summary>
        /// <param name="definitions">Array of definitions to update.</param>
        /// <param name="treatStarAsAuto">if "true" then star definitions are treated as Auto.</param>
        private void ValidateDefinitionsLayout(
            GridDefinitionBase[] definitions,
            bool treatStarAsAuto)
        {
            for (int i = 0; i < definitions.Length; ++i)
            {
                definitions[i].OnBeforeLayout(this);

                Coord userMinSize = definitions[i].UserMinSize;
                Coord userMaxSize = definitions[i].UserMaxSize;
                Coord userSize = 0;

                switch (definitions[i].UserSize.GridUnitType)
                {
                    case (GridUnitType.Pixel):
                        definitions[i].SizeType = LayoutTimeSizeType.Pixel;
                        userSize = definitions[i].UserSize.Value;
                        // this was brought with NewLayout and defeats squishy behavior
                        userMinSize = Math.Max(userMinSize, Math.Min(userSize, userMaxSize));
                        break;
                    case (GridUnitType.Auto):
                        definitions[i].SizeType = LayoutTimeSizeType.Auto;
                        userSize = Coord.PositiveInfinity;
                        break;
                    case (GridUnitType.Star):
                        if (treatStarAsAuto)
                        {
                            definitions[i].SizeType = LayoutTimeSizeType.Auto;
                            userSize = Coord.PositiveInfinity;
                        }
                        else
                        {
                            definitions[i].SizeType = LayoutTimeSizeType.Star;
                            userSize = Coord.PositiveInfinity;
                        }
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }

                definitions[i].UpdateMinSize(userMinSize);
                definitions[i].MeasureSize = Math.Max(userMinSize, Math.Min(userSize, userMaxSize));
            }
        }

        private Coord[] CacheMinSizes(int cellsHead, bool isRows)
        {
            Coord[] minSizes = isRows
                ? new Coord[DefinitionsV.Length] : new Coord[DefinitionsU.Length];

            for (int j = 0; j < minSizes.Length; j++)
            {
                minSizes[j] = -1;
            }

            int i = cellsHead;
            do
            {
                if (isRows)
                {
                    minSizes[PrivateCells[i].RowIndex]
                        = DefinitionsV[PrivateCells[i].RowIndex].RawMinSize;
                }
                else
                {
                    minSizes[PrivateCells[i].ColumnIndex]
                        = DefinitionsU[PrivateCells[i].ColumnIndex].RawMinSize;
                }

                i = PrivateCells[i].Next;
            } while (i < PrivateCells.Length);

            return minSizes;
        }

        private void ApplyCachedMinSizes(Coord[] minSizes, bool isRows)
        {
            for (int i = 0; i < minSizes.Length; i++)
            {
                if (MathUtils.GreaterThanOrClose(minSizes[i], 0))
                {
                    if (isRows)
                    {
                        DefinitionsV[i].SetMinSize(minSizes[i]);
                    }
                    else
                    {
                        DefinitionsU[i].SetMinSize(minSizes[i]);
                    }
                }
            }
        }

        private void MeasureCellsGroup(
            int cellsHead,
            SizeD referenceSize,
            bool ignoreDesiredSizeU,
            bool forceInfinityV)
        {
            bool unusedHasDesiredSizeUChanged;
            MeasureCellsGroup(
                cellsHead,
                referenceSize,
                ignoreDesiredSizeU,
                forceInfinityV,
                out unusedHasDesiredSizeUChanged);
        }

        /// <summary>
        /// Measures one group of cells.
        /// </summary>
        /// <param name="cellsHead">Head index of the cells chain.</param>
        /// <param name="referenceSize">Reference size for spanned cells
        /// calculations.</param>
        /// <param name="ignoreDesiredSizeU">When "true" cells' desired
        /// width is not registered in columns.</param>
        /// <param name="forceInfinityV">Passed through to MeasureCell.
        /// When "true" cells' desired height is not registered in rows.</param>
        /// <param name="hasDesiredSizeUChanged"></param>
        private void MeasureCellsGroup(
            int cellsHead,
            SizeD referenceSize,
            bool ignoreDesiredSizeU,
            bool forceInfinityV,
            out bool hasDesiredSizeUChanged)
        {
            hasDesiredSizeUChanged = false;

            if (cellsHead >= PrivateCells.Length)
            {
                return;
            }

            var children = Children;
            Hashtable spanStore = null;
            bool ignoreDesiredSizeV = forceInfinityV;

            int i = cellsHead;
            do
            {
                var child = children[i];
                var s = child.GetPreferredSizeLimited(referenceSize);
                var childPreferredSize = new SizeD(
                    s.Width + child.Margin.Horizontal,
                    s.Height + child.Margin.Vertical);
                Coord oldWidth = childPreferredSize.Width;

                MeasureCell(i, forceInfinityV);

                hasDesiredSizeUChanged |= !MathUtils.AreClose(oldWidth, childPreferredSize.Width);

                if (!ignoreDesiredSizeU)
                {
                    if (PrivateCells[i].ColumnSpan == 1)
                    {
                        DefinitionsU[PrivateCells[i].ColumnIndex].UpdateMinSize(
                            Math.Min(
                                childPreferredSize.Width,
                                DefinitionsU[PrivateCells[i].ColumnIndex].UserMaxSize));
                    }
                    else
                    {
                        RegisterSpan(
                            ref spanStore,
                            PrivateCells[i].ColumnIndex,
                            PrivateCells[i].ColumnSpan,
                            true,
                            childPreferredSize.Width);
                    }
                }

                if (!ignoreDesiredSizeV)
                {
                    if (PrivateCells[i].RowSpan == 1)
                    {
                        DefinitionsV[PrivateCells[i].RowIndex].UpdateMinSize(
                            Math.Min(
                                childPreferredSize.Height,
                                DefinitionsV[PrivateCells[i].RowIndex].UserMaxSize));
                    }
                    else
                    {
                        RegisterSpan(
                            ref spanStore,
                            PrivateCells[i].RowIndex,
                            PrivateCells[i].RowSpan,
                            false,
                            childPreferredSize.Height);
                    }
                }

                i = PrivateCells[i].Next;
            } while (i < PrivateCells.Length);

            if (spanStore != null)
            {
                foreach (DictionaryEntry e in spanStore)
                {
                    SpanKey key = (SpanKey)e.Key;
                    Coord requestedSize = (Coord)e.Value;

                    EnsureMinSizeInDefinitionRange(
                        key.U ? DefinitionsU : DefinitionsV,
                        key.Start,
                        key.Count,
                        requestedSize,
                        key.U ? referenceSize.Width : referenceSize.Height);
                }
            }
        }

        /// <summary>
        /// Helper method to register a span information for delayed processing.
        /// </summary>
        /// <param name="store">Reference to a hash table object used as storage.</param>
        /// <param name="start">Span starting index.</param>
        /// <param name="count">Span count.</param>
        /// <param name="u"><c>true</c> if this is a column span. <c>false</c>
        /// if this is a row span.</param>
        /// <param name="value">Value to store. If an entry already exists
        /// the biggest value is stored.</param>
        private static void RegisterSpan(
            ref Hashtable store,
            int start,
            int count,
            bool u,
            Coord value)
        {
            if (store == null)
            {
                store = new Hashtable();
            }

            SpanKey key = new SpanKey(start, count, u);
            object o = store[key];

            if (o == null
                || value > (Coord)o)
            {
                store[key] = value;
            }
        }

        /// <summary>
        /// Takes care of measuring a single cell.
        /// </summary>
        /// <param name="cell">Index of the cell to measure.</param>
        /// <param name="forceInfinityV">If "true" then cell is always
        /// calculated to infinite height.</param>
        private void MeasureCell(
            int cell,
            bool forceInfinityV)
        {
            Coord cellMeasureWidth;
            Coord cellMeasureHeight;

            if (PrivateCells[cell].IsAutoU
                && !PrivateCells[cell].IsStarU)
            {
                // if cell belongs to at least one Auto column and not a single Star column
                //  then it should be calculated "to content", thus it is possible to "shortcut"
                //  calculations and simply assign PositiveInfinity here.
                cellMeasureWidth = Coord.PositiveInfinity;
            }
            else
            {
                // otherwise...
                cellMeasureWidth = GetMeasureSizeForRange(
                                        DefinitionsU,
                                        PrivateCells[cell].ColumnIndex,
                                        PrivateCells[cell].ColumnSpan);
            }

            if (forceInfinityV)
            {
                cellMeasureHeight = Coord.PositiveInfinity;
            }
            else if (PrivateCells[cell].IsAutoV
                    && !PrivateCells[cell].IsStarV)
            {
                //  if cell belongs to at least one Auto row and not a single Star row
                //  then it should be calculated "to content", thus it is possible to "shortcut"
                //  calculations and simply assign PositiveInfinity here.
                cellMeasureHeight = Coord.PositiveInfinity;
            }
            else
            {
                cellMeasureHeight = GetMeasureSizeForRange(
                                        DefinitionsV,
                                        PrivateCells[cell].RowIndex,
                                        PrivateCells[cell].RowSpan);
            }

            var child = Children[cell];
            if (child != null)
            {
                var childConstraint = new SizeD(cellMeasureWidth, cellMeasureHeight);
                // child.Measure(childConstraint); // yezo
            }
        }

        /// <summary>
        /// Calculates one dimensional measure size for given definitions' range.
        /// </summary>
        /// <param name="definitions">Source array of definitions to read values from.</param>
        /// <param name="start">Starting index of the range.</param>
        /// <param name="count">Number of definitions included in the range.</param>
        /// <returns>Calculated measure size.</returns>
        /// <remarks>
        /// For "Auto" definitions MinWidth is used in place of PreferredSize.
        /// </remarks>
        private Coord GetMeasureSizeForRange(
            GridDefinitionBase[] definitions,
            int start,
            int count)
        {
            Debug.Assert(0 < count && 0 <= start && (start + count) <= definitions.Length);

            Coord measureSize = 0;
            int i = start + count - 1;

            do
            {
                measureSize += (definitions[i].SizeType == LayoutTimeSizeType.Auto)
                    ? definitions[i].MinSize
                    : definitions[i].MeasureSize;
            } while (--i >= start);

            return (measureSize);
        }

        /// <summary>
        /// Accumulates length type information for given definition's range.
        /// </summary>
        /// <param name="definitions">Source array of definitions to read values from.</param>
        /// <param name="start">Starting index of the range.</param>
        /// <param name="count">Number of definitions included in the range.</param>
        /// <returns>Length type for given range.</returns>
        private LayoutTimeSizeType GetLengthTypeForRange(
            GridDefinitionBase[] definitions,
            int start,
            int count)
        {
            Debug.Assert(0 < count && 0 <= start && (start + count) <= definitions.Length);

            LayoutTimeSizeType lengthType = LayoutTimeSizeType.None;
            int i = start + count - 1;

            do
            {
                lengthType |= definitions[i].SizeType;
            } while (--i >= start);

            return (lengthType);
        }

        /// <summary>
        /// Distributes min size back to definition array's range.
        /// </summary>
        /// <param name="start">Start of the range.</param>
        /// <param name="count">Number of items in the range.</param>
        /// <param name="requestedSize">Minimum size that should "fit"
        /// into the definitions range.</param>
        /// <param name="definitions">Definition array receiving distribution.</param>
        /// <param name="percentReferenceSize">Size used to resolve percentages.</param>
        private void EnsureMinSizeInDefinitionRange(
            GridDefinitionBase[] definitions,
            int start,
            int count,
            Coord requestedSize,
            Coord percentReferenceSize)
        {
            Debug.Assert(1 < count && 0 <= start && (start + count) <= definitions.Length);

            //  avoid processing when asked to distribute "0"
            if (!_IsZero(requestedSize))
            {
                // temp array used to remember definitions for sorting
                GridDefinitionBase[] tempDefinitions = TempDefinitions;

                int end = start + count;
                int autoDefinitionsCount = 0;
                Coord rangeMinSize = 0;
                Coord rangePreferredSize = 0;
                Coord rangeMaxSize = 0;
                Coord maxMaxSize = 0; // maximum of maximum sizes

                //  first accumulate the necessary information:
                //  a) sum up the sizes in the range;
                //  b) count the number of auto definitions in the range;
                //  c) initialize temp array
                //  d) cache the maximum size into SizeCache
                //  e) accumulate max of max sizes
                for (int i = start; i < end; ++i)
                {
                    Coord minSize = definitions[i].MinSize;
                    Coord preferredSize = definitions[i].PreferredSize;
                    Coord maxSize = Math.Max(definitions[i].UserMaxSize, minSize);

                    rangeMinSize += minSize;
                    rangePreferredSize += preferredSize;
                    rangeMaxSize += maxSize;

                    definitions[i].SizeCache = maxSize;

                    //  sanity check: no matter what, but min size must always be the smaller;
                    //  max size must be the biggest; and preferred should be in between
                    Debug.Assert(minSize <= preferredSize
                                && preferredSize <= maxSize
                                && rangeMinSize <= rangePreferredSize
                                && rangePreferredSize <= rangeMaxSize);

                    if (maxMaxSize < maxSize) maxMaxSize = maxSize;
                    if (definitions[i].UserSize.IsAuto) autoDefinitionsCount++;
                    tempDefinitions[i - start] = definitions[i];
                }

                // avoid processing if the range already big enough
                if (requestedSize > rangeMinSize)
                {
                    if (requestedSize <= rangePreferredSize)
                    {
                        // requestedSize fits into preferred size of the range.
                        // distribute according to the following logic:
                        // * do not distribute into auto definitions
                        // - they should continue to stay "tight";
                        // * for all non-auto definitions distribute to equi-size
                        // min sizes, without exceeding preferred size.
                        //
                        // in order to achieve that, definitions are sorted in a way
                        // that all auto definitions
                        // are first, then definitions follow ascending order with
                        // PreferredSize as the key of sorting.
                        Coord sizeToDistribute;
                        int i;

                        Array.Sort(tempDefinitions, 0, count, s_spanPreferredDistributionOrderComparer);
                        for (i = 0, sizeToDistribute = requestedSize; i < autoDefinitionsCount; ++i)
                        {
                            //  sanity check: only auto definitions allowed in this loop
                            Debug.Assert(tempDefinitions[i].UserSize.IsAuto);

                            //  adjust sizeToDistribute value by subtracting auto definition min size
                            sizeToDistribute -= (tempDefinitions[i].MinSize);
                        }

                        for (; i < count; ++i)
                        {
                            //  sanity check: no auto definitions allowed in this loop
                            Debug.Assert(!tempDefinitions[i].UserSize.IsAuto);

                            Coord newMinSize = Math.Min(
                                sizeToDistribute / (count - i),
                                tempDefinitions[i].PreferredSize);
                            if (newMinSize > tempDefinitions[i].MinSize)
                            {
                                tempDefinitions[i].UpdateMinSize(newMinSize);
                            }
                            sizeToDistribute -= newMinSize;
                        }

                        //  sanity check: requested size must all be distributed
                        Debug.Assert(_IsZero(sizeToDistribute));
                    }
                    else if (requestedSize <= rangeMaxSize)
                    {
                        //  requestedSize bigger than preferred size, but fit into max
                        //  size of the range.
                        //  distribute according to the following logic:
                        //  * do not distribute into auto definitions,
                        //  if possible - they should continue to stay "tight";
                        //  * for all non-auto definitions distribute to
                        //  euqi-size min sizes, without exceeding max size.
                        //
                        //  in order to achieve that, definitions are sorted
                        //  in a way that all non-auto definitions
                        //  are last, then definitions follow ascending
                        //  order with MaxSize as the key of sorting.
                        //
                        Coord sizeToDistribute;
                        int i;

                        Array.Sort(tempDefinitions, 0, count, s_spanMaxDistributionOrderComparer);
                        for (i = 0, sizeToDistribute = requestedSize - rangePreferredSize;
                            i < count - autoDefinitionsCount; ++i)
                        {
                            //  sanity check: no auto definitions allowed in this loop
                            Debug.Assert(!tempDefinitions[i].UserSize.IsAuto);

                            Coord preferredSize = tempDefinitions[i].PreferredSize;
                            Coord newMinSize = preferredSize
                                + sizeToDistribute / (count - autoDefinitionsCount - i);
                            tempDefinitions[i].UpdateMinSize(
                                Math.Min(newMinSize, tempDefinitions[i].SizeCache));
                            sizeToDistribute -= (tempDefinitions[i].MinSize - preferredSize);
                        }

                        for (; i < count; ++i)
                        {
                            //  sanity check: only auto definitions allowed in this loop
                            Debug.Assert(tempDefinitions[i].UserSize.IsAuto);

                            Coord preferredSize = tempDefinitions[i].MinSize;
                            Coord newMinSize = preferredSize + sizeToDistribute / (count - i);
                            tempDefinitions[i].UpdateMinSize(
                                Math.Min(newMinSize, tempDefinitions[i].SizeCache));
                            sizeToDistribute -= (tempDefinitions[i].MinSize - preferredSize);
                        }

                        //  sanity check: requested size must all be distributed
                        Debug.Assert(_IsZero(sizeToDistribute));
                    }
                    else
                    {
                        //  requestedSize bigger than max size of the range.
                        //  distribute according to the following logic:
                        //  * for all definitions distribute to equi-size min sizes.
                        Coord equalSize = requestedSize / count;

                        if (equalSize < maxMaxSize
                            && !_AreClose(equalSize, maxMaxSize))
                        {
                            //  equi-size is less than maximum of maxSizes.
                            //  in this case distribute so that smaller definitions grow faster than
                            //  bigger ones.
                            Coord totalRemainingSize = maxMaxSize * count - rangeMaxSize;
                            Coord sizeToDistribute = requestedSize - rangeMaxSize;

                            //  sanity check: totalRemainingSize and sizeToDistribute
                            //  must be real positive numbers
                            Debug.Assert(!Coord.IsInfinity(totalRemainingSize)
                                        && !MathUtils.IsNaN(totalRemainingSize)
                                        && totalRemainingSize > 0
                                        && !Coord.IsInfinity(sizeToDistribute)
                                        && !MathUtils.IsNaN(sizeToDistribute)
                                        && sizeToDistribute > 0);

                            for (int i = 0; i < count; ++i)
                            {
                                Coord deltaSize = (maxMaxSize - tempDefinitions[i].SizeCache)
                                    * sizeToDistribute / totalRemainingSize;
                                tempDefinitions[i].UpdateMinSize(tempDefinitions[i].SizeCache + deltaSize);
                            }
                        }
                        else
                        {
                            //
                            //  equi-size is greater or equal to maximum of max sizes.
                            //  all definitions receive equalSize as their mim sizes.
                            //
                            for (int i = 0; i < count; ++i)
                            {
                                tempDefinitions[i].UpdateMinSize(equalSize);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolves Star's for given array of definitions.
        /// </summary>
        /// <param name="definitions">Array of definitions to resolve stars.</param>
        /// <param name="availableSize">All available size.</param>
        /// <remarks>
        /// Must initialize LayoutSize for all Star entries in given array of definitions.
        /// </remarks>
        private void ResolveStar(
            GridDefinitionBase[] definitions,
            Coord availableSize)
        {
            if (StarDefinitionsCanExceedAvailableSpace)
            {
                ResolveStarLegacy(definitions, availableSize);
            }
            else
            {
                ResolveStarMaxDiscrepancy(definitions, availableSize);
            }
        }

        // original implementation, used from 3.0 through 4.6.2
        private void ResolveStarLegacy(
            GridDefinitionBase[] definitions,
            Coord availableSize)
        {
            GridDefinitionBase[] tempDefinitions = TempDefinitions;
            int starDefinitionsCount = 0;
            Coord takenSize = 0;

            for (int i = 0; i < definitions.Length; ++i)
            {
                switch (definitions[i].SizeType)
                {
                    case (LayoutTimeSizeType.Auto):
                        takenSize += definitions[i].MinSize;
                        break;
                    case (LayoutTimeSizeType.Pixel):
                        takenSize += definitions[i].MeasureSize;
                        break;
                    case (LayoutTimeSizeType.Star):
                        {
                            tempDefinitions[starDefinitionsCount++] = definitions[i];

                            Coord starValue = definitions[i].UserSize.Value;

                            if (_IsZero(starValue))
                            {
                                definitions[i].MeasureSize = 0;
                                definitions[i].SizeCache = 0;
                            }
                            else
                            {
                                //  clipping by c_starClip guarantees that sum of even
                                //  a very big number of max'ed out star values
                                //  can be summed up without overflow
                                starValue = Math.Min(starValue, c_starClip);

                                //  Note: normalized star value is temporary cached into MeasureSize
                                definitions[i].MeasureSize = starValue;
                                Coord maxSize
                                    = Math.Max(definitions[i].MinSize, definitions[i].UserMaxSize);
                                maxSize = Math.Min(maxSize, c_starClip);
                                definitions[i].SizeCache = maxSize / starValue;
                            }
                        }
                        break;
                }
            }

            if (starDefinitionsCount > 0)
            {
                Array.Sort(tempDefinitions, 0, starDefinitionsCount, s_starDistributionOrderComparer);

                //  the 'do {} while' loop below calculates sum of star weights in
                //  order to avoid fp overflow...
                //  partial sum value is stored in each definition's SizeCache member.
                //  this way the algorithm guarantees (starValue <= definition.SizeCache) and thus
                //  (starValue / definition.SizeCache) will never overflow due to sum of
                //  star weights becoming zero.
                //  this is an important change from previous implementation where the
                //  following was possible:
                //  ((BigValueStar + SmallValueStar) - BigValueStar) resulting in 0...
                Coord allStarWeights = 0;
                int i = starDefinitionsCount - 1;
                do
                {
                    allStarWeights += tempDefinitions[i].MeasureSize;
                    tempDefinitions[i].SizeCache = allStarWeights;
                } while (--i >= 0);

                i = 0;
                do
                {
                    Coord resolvedSize;
                    Coord starValue = tempDefinitions[i].MeasureSize;

                    if (_IsZero(starValue))
                    {
                        resolvedSize = tempDefinitions[i].MinSize;
                    }
                    else
                    {
                        Coord userSize = Math.Max(availableSize - takenSize, 0)
                            * (starValue / tempDefinitions[i].SizeCache);
                        resolvedSize = Math.Min(userSize, tempDefinitions[i].UserMaxSize);
                        resolvedSize = Math.Max(tempDefinitions[i].MinSize, resolvedSize);
                    }

                    tempDefinitions[i].MeasureSize = resolvedSize;
                    takenSize += resolvedSize;
                } while (++i < starDefinitionsCount);
            }
        }

        // new implementation as of 4.7.  Several improvements:
        // 1. Allocate to *-defs hitting their min or max constraints, before allocating
        //      to other *-defs.  A def that hits its min uses more space than its
        //      proportional share, reducing the space available to everyone else.
        //      The legacy algorithm deducted this space only from defs processed
        //      after the min;  the new algorithm deducts it proportionally from all
        //      defs.   This avoids the "*-defs exceed available space" problem,
        //      and other related problems where *-defs don't receive proportional
        //      allocations even though no constraints are preventing it.
        // 2. When multiple defs hit min or max, resolve the one with maximum
        //      discrepancy (defined below).   This avoids discontinuities - small
        //      change in available space resulting in large change to one def's allocation.
        // 3. Correct handling of large *-values, including Infinity.
        private void ResolveStarMaxDiscrepancy(
            GridDefinitionBase[] definitions,
            Coord availableSize)
        {
            int defCount = definitions.Length;
            GridDefinitionBase[] tempDefinitions = TempDefinitions;
            int minCount = 0, maxCount = 0;
            Coord takenSize = 0;
            Coord totalStarWeight = 0;
            int starCount = 0;      // number of unresolved *-definitions
            Coord scale = 1; // scale factor applied to each *-weight;  negative = "Infinity is present"

            // Phase 1.  Determine the maximum *-weight and prepare to adjust *-weights
            Coord maxStar = 0;
            for (int i = 0; i < defCount; ++i)
            {
                GridDefinitionBase def = definitions[i];

                if (def.SizeType == LayoutTimeSizeType.Star)
                {
                    ++starCount;
                    def.MeasureSize = 1;  // meaning "not yet resolved in phase 3"
                    if (def.UserSize.Value > maxStar)
                    {
                        maxStar = def.UserSize.Value;
                    }
                }
            }

            if (Coord.IsPositiveInfinity(maxStar))
            {
                // negative scale means one or more of the weights was Infinity
                scale = -1;
            }
            else if (starCount > 0)
            {
                // if maxStar * starCount > Coord.Max, summing all the weights could cause
                // floating-point overflow.  To avoid that, scale the weights by a factor to keep
                // the sum within limits.  Choose a power of 2, to preserve precision.
                var power = Math.Floor(Math.Log(Coord.MaxValue / maxStar / starCount, 2));
                if (power < 0)
                {
                    scale = (Coord)Math.Pow(2, power - 4); // -4 is just for paranoia
                }
            }

            // normally Phases 2 and 3 execute only once.  But certain unusual combinations of weights
            // and constraints can defeat the algorithm, in which case we repeat Phases 2 and 3.
            // More explanation below...
            for (bool runPhase2and3 = true; runPhase2and3;)
            {
                // Phase 2.   Compute total *-weight W and available space S.
                // For *-items that have Min or Max constraints, compute the ratios used to decide
                // whether proportional space is too big or too small and add the item to the
                // corresponding list.  (The "min" list is in the first half of tempDefinitions,
                // the "max" list in the second half.  TempDefinitions has capacity at least
                // 2*defCount, so there's room for both lists.)
                totalStarWeight = 0;
                takenSize = 0;
                minCount = maxCount = 0;

                for (int i = 0; i < defCount; ++i)
                {
                    GridDefinitionBase def = definitions[i];

                    switch (def.SizeType)
                    {
                        case (LayoutTimeSizeType.Auto):
                            takenSize += definitions[i].MinSize;
                            break;
                        case (LayoutTimeSizeType.Pixel):
                            takenSize += def.MeasureSize;
                            break;
                        case (LayoutTimeSizeType.Star):
                            if (def.MeasureSize < 0)
                            {
                                takenSize += -def.MeasureSize;  // already resolved
                            }
                            else
                            {
                                Coord starWeight = StarWeight(def, scale);
                                totalStarWeight += starWeight;

                                if (def.MinSize > 0)
                                {
                                    // store ratio w/min in MeasureSize (for now)
                                    tempDefinitions[minCount++] = def;
                                    def.MeasureSize = starWeight / def.MinSize;
                                }

                                Coord effectiveMaxSize = Math.Max(def.MinSize, def.UserMaxSize);
                                if (!Coord.IsPositiveInfinity(effectiveMaxSize))
                                {
                                    // store ratio w/max in SizeCache (for now)
                                    tempDefinitions[defCount + maxCount++] = def;
                                    def.SizeCache = starWeight / effectiveMaxSize;
                                }
                            }
                            break;
                    }
                }

                // Phase 3.  Resolve *-items whose proportional sizes are too big or too small.
                int minCountPhase2 = minCount, maxCountPhase2 = maxCount;
                Coord takenStarWeight = 0;
                Coord remainingAvailableSize = availableSize - takenSize;
                Coord remainingStarWeight = totalStarWeight - takenStarWeight;
                Array.Sort(tempDefinitions, 0, minCount, s_minRatioComparer);
                Array.Sort(tempDefinitions, defCount, maxCount, s_maxRatioComparer);

                while (minCount + maxCount > 0 && remainingAvailableSize > 0)
                {
                    // the calculation
                    // remainingStarWeight = totalStarWeight - takenStarWeight
                    // is subject to catastrophic cancellation if the two terms are nearly equal,
                    // which leads to meaningless results.   Check for that, and recompute from
                    // the remaining definitions.   [This leads to quadratic behavior in really
                    // pathological cases - but they'd never arise in practice.]
                    const Coord starFactor = 1 / 256;
                    
                    // lose more than 8 bits of precision -> recalculate

                    if (remainingStarWeight < totalStarWeight * starFactor)
                    {
                        takenStarWeight = 0;
                        totalStarWeight = 0;

                        for (int i = 0; i < defCount; ++i)
                        {
                            GridDefinitionBase def = definitions[i];
                            if (def.SizeType == LayoutTimeSizeType.Star && def.MeasureSize > 0)
                            {
                                totalStarWeight += StarWeight(def, scale);
                            }
                        }

                        remainingStarWeight = totalStarWeight - takenStarWeight;
                    }

                    Coord minRatio = (minCount > 0)
                        ? tempDefinitions[minCount - 1].MeasureSize : Coord.PositiveInfinity;
                    Coord maxRatio = (maxCount > 0)
                        ? tempDefinitions[defCount + maxCount - 1].SizeCache : -1;

                    // choose the def with larger ratio to the current proportion ("max discrepancy")
                    Coord proportion = remainingStarWeight / remainingAvailableSize;
                    bool? chooseMin = Choose(minRatio, maxRatio, proportion);

                    // if no def was chosen, advance to phase 4;  the current proportion doesn't
                    // conflict with any min or max values.
                    if (!(chooseMin.HasValue))
                    {
                        break;
                    }

                    // get the chosen definition and its resolved size
                    GridDefinitionBase resolvedDef;
                    Coord resolvedSize;
                    if (chooseMin == true)
                    {
                        resolvedDef = tempDefinitions[minCount - 1];
                        resolvedSize = resolvedDef.MinSize;
                        --minCount;
                    }
                    else
                    {
                        resolvedDef = tempDefinitions[defCount + maxCount - 1];
                        resolvedSize = Math.Max(resolvedDef.MinSize, resolvedDef.UserMaxSize);
                        --maxCount;
                    }

                    // resolve the chosen def, deduct its contributions from W and S.
                    // Defs resolved in phase 3 are marked by storing the negative of their resolved
                    // size in MeasureSize, to distinguish them from a pending def.
                    takenSize += resolvedSize;
                    resolvedDef.MeasureSize = -resolvedSize;
                    takenStarWeight += StarWeight(resolvedDef, scale);
                    --starCount;

                    remainingAvailableSize = availableSize - takenSize;
                    remainingStarWeight = totalStarWeight - takenStarWeight;

                    // advance to the next candidate defs, removing ones that have been resolved.
                    // Both counts are advanced, as a def might appear in both lists.
                    while (minCount > 0 && tempDefinitions[minCount - 1].MeasureSize < 0)
                    {
                        --minCount;
                        tempDefinitions[minCount] = null;
                    }
                    while (maxCount > 0 && tempDefinitions[defCount + maxCount - 1].MeasureSize < 0)
                    {
                        --maxCount;
                        tempDefinitions[defCount + maxCount] = null;
                    }
                }

                // decide whether to run Phase2 and Phase3 again.  There are 3 cases:
                // 1. There is space available, and *-defs remaining.  This is the
                //      normal case - move on to Phase 4 to allocate the remaining
                //      space proportionally to the remaining *-defs.
                // 2. There is space available, but no *-defs.  This implies at least one
                //      def was resolved as 'max', taking less space than its proportion.
                //      If there are also 'min' defs, reconsider them - we can give
                //      them more space.   If not, all the *-defs are 'max', so there's
                //      no way to use all the available space.
                // 3. We allocated too much space.   This implies at least one def was
                //      resolved as 'min'.  If there are also 'max' defs, reconsider
                //      them, otherwise the over-allocation is an inevitable consequence
                //      of the given min constraints.
                // Note that if we return to Phase2, at least one *-def will have been
                // resolved.  This guarantees we don't run Phase2+3 infinitely often.
                runPhase2and3 = false;
                if (starCount == 0 && takenSize < availableSize)
                {
                    // if no *-defs remain and we haven't allocated all the space, reconsider the defs
                    // resolved as 'min'.   Their allocation can be increased to make up the gap.
                    for (int i = minCount; i < minCountPhase2; ++i)
                    {
                        GridDefinitionBase def = tempDefinitions[i];
                        if (def != null)
                        {
                            def.MeasureSize = 1;      // mark as 'not yet resolved'
                            ++starCount;
                            runPhase2and3 = true;       // found a candidate, so re-run Phases 2 and 3
                        }
                    }
                }

                if (takenSize > availableSize)
                {
                    // if we've allocated too much space, reconsider the defs
                    // resolved as 'max'.   Their allocation can be decreased to make up the gap.
                    for (int i = maxCount; i < maxCountPhase2; ++i)
                    {
                        GridDefinitionBase def = tempDefinitions[defCount + i];
                        if (def != null)
                        {
                            def.MeasureSize = 1;      // mark as 'not yet resolved'
                            ++starCount;
                            runPhase2and3 = true;    // found a candidate, so re-run Phases 2 and 3
                        }
                    }
                }
            }

            // Phase 4.  Resolve the remaining defs proportionally.
            starCount = 0;
            for (int i = 0; i < defCount; ++i)
            {
                GridDefinitionBase def = definitions[i];

                if (def.SizeType == LayoutTimeSizeType.Star)
                {
                    if (def.MeasureSize < 0)
                    {
                        // this def was resolved in phase 3 - fix up its measure size
                        def.MeasureSize = -def.MeasureSize;
                    }
                    else
                    {
                        // this def needs resolution, add it to the list, sorted by *-weight
                        tempDefinitions[starCount++] = def;
                        def.MeasureSize = StarWeight(def, scale);
                    }
                }
            }

            if (starCount > 0)
            {
                Array.Sort(tempDefinitions, 0, starCount, s_starWeightComparer);

                // compute the partial sums of *-weight, in increasing order of weight
                // for minimal loss of precision.
                totalStarWeight = 0;
                for (int i = 0; i < starCount; ++i)
                {
                    GridDefinitionBase def = tempDefinitions[i];
                    totalStarWeight += def.MeasureSize;
                    def.SizeCache = totalStarWeight;
                }

                // resolve the defs, in decreasing order of weight
                for (int i = starCount - 1; i >= 0; --i)
                {
                    GridDefinitionBase def = tempDefinitions[i];
                    Coord resolvedSize = (def.MeasureSize > 0)
                        ? Math.Max(availableSize - takenSize, 0) * (def.MeasureSize / def.SizeCache)
                        : 0;

                    // min and max should have no effect by now, but just in case...
                    resolvedSize = Math.Min(resolvedSize, def.UserMaxSize);
                    resolvedSize = Math.Max(def.MinSize, resolvedSize);

                    def.MeasureSize = resolvedSize;
                    takenSize += resolvedSize;
                }
            }
        }

        /// <summary>
        /// Calculates desired size for given array of definitions.
        /// </summary>
        /// <param name="definitions">Array of definitions to use for calculations.</param>
        /// <returns>Desired size.</returns>
        private Coord CalculateDesiredSize(
            GridDefinitionBase[] definitions)
        {
            Coord desiredSize = 0;

            for (int i = 0; i < definitions.Length; ++i)
            {
                desiredSize += definitions[i].MinSize;
            }

            return (desiredSize);
        }

        /// <summary>
        /// Calculates and sets final size for all definitions in the given array.
        /// </summary>
        /// <param name="definitions">Array of definitions to process.</param>
        /// <param name="finalSize">Final size to lay out to.</param>
        /// <param name="columns">True if sizing column definitions, false for rows</param>
        private void SetFinalSize(
            GridDefinitionBase[] definitions,
            Coord finalSize,
            bool columns)
        {
            if (StarDefinitionsCanExceedAvailableSpace)
            {
                SetFinalSizeLegacy(definitions, finalSize, columns);
            }
            else
            {
                SetFinalSizeMaxDiscrepancy(definitions, finalSize, columns);
            }
        }

        /// <summary>
        /// Calculates the value to be used for layout rounding at high DPI.
        /// </summary>
        /// <param name="value">Input value to be rounded.</param>
        /// <param name="dpiScale">Ratio of screen's DPI to layout DPI</param>
        /// <returns>Adjusted value that will produce layout rounding
        /// on screen at high dpi.</returns>
        /// <remarks>This is a layout helper method. It takes DPI into account
        /// and also does not return
        /// the rounded value if it is unacceptable for layout, e.g. Infinity or NaN.
        /// It's a helper associated with
        /// UseLayoutRounding  property and should not be used as a
        /// general rounding utility.</remarks>
        static Coord RoundLayoutValue(Coord value, Coord dpiScale)
        {
            Coord newValue;

            // If DPI == 1, don't use DPI-aware rounding.
            if (!MathUtils.AreClose(dpiScale, 1.0))
            {
                newValue = (Coord)Math.Round(value * dpiScale) / dpiScale;
                // If rounding produces a value unacceptable to layout (NaN, Infinity or MaxValue),
                // use the original value.
                if (MathUtils.IsNaN(newValue) ||
                    Coord.IsInfinity(newValue) ||
                    MathUtils.AreClose(newValue, Coord.MaxValue))
                {
                    newValue = value;
                }
            }
            else
            {
                newValue = (Coord)Math.Round(value);
            }

            return newValue;
        }

        // original implementation, used from 3.0 through 4.6.2
        private void SetFinalSizeLegacy(
            GridDefinitionBase[] definitions,
            Coord finalSize,
            bool columns)
        {
            int starDefinitionsCount = 0;            //  traverses form the first entry up
            int nonStarIndex = definitions.Length;   //  traverses from the last entry down
            Coord allPreferredArrangeSize = 0;
            bool useLayoutRounding = this.UseLayoutRounding;
            int[] definitionIndices = DefinitionIndices;
            Coord[] roundingErrors = null;

            // If using layout rounding, check whether rounding needs to compensate for high DPI
            Coord dpi = 1; // yet to do

            if (useLayoutRounding)
            {
                //DpiScale dpiScale = GetDpi(); // yet to do
                //dpi = columns ? dpiScale.DpiScaleX : dpiScale.DpiScaleY;
                //roundingErrors = RoundingErrors;
            }

            for (int i = 0; i < definitions.Length; ++i)
            {
                //  if definition is shared then is cannot be star
                Debug.Assert(!definitions[i].IsShared || !definitions[i].UserSize.IsStar);

                if (definitions[i].UserSize.IsStar)
                {
                    Coord starValue = definitions[i].UserSize.Value;

                    if (_IsZero(starValue))
                    {
                        //  cache normalized star value temporary into MeasureSize
                        definitions[i].MeasureSize = 0;
                        definitions[i].SizeCache = 0;
                    }
                    else
                    {
                        // clipping by c_starClip guarantees that sum of even a very
                        // big number of max'ed out star values
                        //  can be summed up without overflow
                        starValue = Math.Min(starValue, c_starClip);

                        //  Note: normalized star value is temporary cached into MeasureSize
                        definitions[i].MeasureSize = starValue;
                        Coord maxSize
                            = Math.Max(definitions[i].MinSizeForArrange, definitions[i].UserMaxSize);
                        maxSize = Math.Min(maxSize, c_starClip);
                        definitions[i].SizeCache = maxSize / starValue;
                        if (useLayoutRounding)
                        {
                            roundingErrors[i] = definitions[i].SizeCache;
                            definitions[i].SizeCache = RoundLayoutValue(definitions[i].SizeCache, dpi);
                        }
                    }
                    definitionIndices[starDefinitionsCount++] = i;
                }
                else
                {
                    Coord userSize = 0;

                    switch (definitions[i].UserSize.GridUnitType)
                    {
                        case (GridUnitType.Pixel):
                            userSize = definitions[i].UserSize.Value;
                            break;

                        case (GridUnitType.Auto):
                            userSize = definitions[i].MinSizeForArrange;
                            break;
                    }

                    Coord userMaxSize;

                    if (definitions[i].IsShared)
                    {
                        //  overriding userMaxSize effectively prevents squishy-ness.
                        //  this is a "solution" to avoid shared definitions from been sized to
                        //  different final size at arrange time, if / when different grids receive
                        //  different final sizes.
                        userMaxSize = userSize;
                    }
                    else
                    {
                        userMaxSize = definitions[i].UserMaxSize;
                    }

                    definitions[i].SizeCache
                        = Math.Max(definitions[i].MinSizeForArrange, Math.Min(userSize, userMaxSize));
                    if (useLayoutRounding)
                    {
                        roundingErrors[i] = definitions[i].SizeCache;
                        definitions[i].SizeCache = RoundLayoutValue(definitions[i].SizeCache, dpi);
                    }

                    allPreferredArrangeSize += definitions[i].SizeCache;
                    definitionIndices[--nonStarIndex] = i;
                }
            }

            //  indices should meet
            Debug.Assert(nonStarIndex == starDefinitionsCount);

            if (starDefinitionsCount > 0)
            {
                StarDistributionOrderIndexComparer starDistributionOrderIndexComparer
                    = new StarDistributionOrderIndexComparer(definitions);
                Array.Sort(
                    definitionIndices,
                    0,
                    starDefinitionsCount,
                    starDistributionOrderIndexComparer);

                //  the 'do {} while' loop below calculates sum of star weights
                //  in order to avoid fp overflow...
                //  partial sum value is stored in each definition's SizeCache member.
                //  this way the algorithm guarantees (starValue <= definition.SizeCache) and thus
                //  (starValue / definition.SizeCache) will never overflow due to sum of
                //  star weights becoming zero.
                //  this is an important change from previous implementation where the
                //  following was possible:
                //  ((BigValueStar + SmallValueStar) - BigValueStar) resulting in 0...
                Coord allStarWeights = 0;
                int i = starDefinitionsCount - 1;
                do
                {
                    allStarWeights += definitions[definitionIndices[i]].MeasureSize;
                    definitions[definitionIndices[i]].SizeCache = allStarWeights;
                } while (--i >= 0);

                i = 0;
                do
                {
                    Coord resolvedSize;
                    Coord starValue = definitions[definitionIndices[i]].MeasureSize;

                    if (_IsZero(starValue))
                    {
                        resolvedSize = definitions[definitionIndices[i]].MinSizeForArrange;
                    }
                    else
                    {
                        Coord userSize = Math.Max(finalSize - allPreferredArrangeSize, 0)
                            * (starValue / definitions[definitionIndices[i]].SizeCache);
                        resolvedSize = Math.Min(
                            userSize,
                            definitions[definitionIndices[i]].UserMaxSize);
                        resolvedSize = Math.Max(
                            definitions[definitionIndices[i]].MinSizeForArrange,
                            resolvedSize);
                    }

                    definitions[definitionIndices[i]].SizeCache = resolvedSize;
                    if (useLayoutRounding)
                    {
                        roundingErrors[definitionIndices[i]]
                            = definitions[definitionIndices[i]].SizeCache;
                        definitions[definitionIndices[i]].SizeCache
                            = RoundLayoutValue(definitions[definitionIndices[i]].SizeCache, dpi);
                    }

                    allPreferredArrangeSize += definitions[definitionIndices[i]].SizeCache;
                } while (++i < starDefinitionsCount);
            }

            if (allPreferredArrangeSize > finalSize
                && !_AreClose(allPreferredArrangeSize, finalSize))
            {
                DistributionOrderIndexComparer distributionOrderIndexComparer
                    = new DistributionOrderIndexComparer(definitions);
                Array.Sort(definitionIndices, 0, definitions.Length, distributionOrderIndexComparer);
                Coord sizeToDistribute = finalSize - allPreferredArrangeSize;

                for (int i = 0; i < definitions.Length; ++i)
                {
                    int definitionIndex = definitionIndices[i];
                    Coord final = definitions[definitionIndex].SizeCache
                        + (sizeToDistribute / (definitions.Length - i));
                    Coord finalOld = final;
                    final = Math.Max(final, definitions[definitionIndex].MinSizeForArrange);
                    final = Math.Min(final, definitions[definitionIndex].SizeCache);

                    if (useLayoutRounding)
                    {
                        roundingErrors[definitionIndex] = final;
                        final = RoundLayoutValue(finalOld, dpi);
                        final = Math.Max(final, definitions[definitionIndex].MinSizeForArrange);
                        final = Math.Min(final, definitions[definitionIndex].SizeCache);
                    }

                    sizeToDistribute -= (final - definitions[definitionIndex].SizeCache);
                    definitions[definitionIndex].SizeCache = final;
                }

                allPreferredArrangeSize = finalSize - sizeToDistribute;
            }

            if (useLayoutRounding)
            {
                if (!_AreClose(allPreferredArrangeSize, finalSize))
                {
                    // Compute deltas
                    for (int i = 0; i < definitions.Length; ++i)
                    {
                        roundingErrors[i] = roundingErrors[i] - definitions[i].SizeCache;
                        definitionIndices[i] = i;
                    }

                    // Sort rounding errors
                    RoundingErrorIndexComparer roundingErrorIndexComparer
                        = new RoundingErrorIndexComparer(roundingErrors);
                    Array.Sort(definitionIndices, 0, definitions.Length, roundingErrorIndexComparer);
                    Coord adjustedSize = allPreferredArrangeSize;
                    Coord dpiIncrement = RoundLayoutValue(1, dpi);

                    if (allPreferredArrangeSize > finalSize)
                    {
                        int i = definitions.Length - 1;
                        while ((adjustedSize > finalSize
                            && !_AreClose(adjustedSize, finalSize)) && i >= 0)
                        {
                            GridDefinitionBase definition = definitions[definitionIndices[i]];
                            Coord final = definition.SizeCache - dpiIncrement;
                            final = Math.Max(final, definition.MinSizeForArrange);
                            if (final < definition.SizeCache)
                            {
                                adjustedSize -= dpiIncrement;
                            }
                            definition.SizeCache = final;
                            i--;
                        }
                    }
                    else if (allPreferredArrangeSize < finalSize)
                    {
                        int i = 0;
                        while ((adjustedSize < finalSize
                            && !_AreClose(adjustedSize, finalSize)) && i < definitions.Length)
                        {
                            GridDefinitionBase definition = definitions[definitionIndices[i]];
                            Coord final = definition.SizeCache + dpiIncrement;
                            final = Math.Max(final, definition.MinSizeForArrange);
                            if (final > definition.SizeCache)
                            {
                                adjustedSize += dpiIncrement;
                            }
                            definition.SizeCache = final;
                            i++;
                        }
                    }
                }
            }

            definitions[0].FinalOffset = 0;
            for (int i = 0; i < definitions.Length; ++i)
            {
                definitions[(i + 1) % definitions.Length].FinalOffset
                    = definitions[i].FinalOffset + definitions[i].SizeCache;
            }
        }

        // new implementation, as of 4.7.  This incorporates the same algorithm
        // as in ResolveStarMaxDiscrepancy.  It differs in the same way that SetFinalSizeLegacy
        // differs from ResolveStarLegacy, namely (a) leaves results in def.SizeCache
        // instead of def.MeasureSize, (b) implements LayoutRounding if requested,
        // (c) stores intermediate results differently.
        // The LayoutRounding logic is improved:
        // 1. Use pre-rounded values during proportional allocation.  This avoids the
        //      same kind of problems arising from interaction with min/max that
        //      motivated the new algorithm in the first place.
        // 2. Use correct "nudge" amount when distributing roundoff space.   This
        //      comes into play at high DPI - greater than 134.
        // 3. Applies rounding only to real pixel values (not to ratios)
        private void SetFinalSizeMaxDiscrepancy(
            GridDefinitionBase[] definitions,
            Coord finalSize,
            bool columns)
        {
            int defCount = definitions.Length;
            int[] definitionIndices = DefinitionIndices;
            int minCount = 0, maxCount = 0;
            Coord takenSize = 0;
            Coord totalStarWeight = 0;
            int starCount = 0;      // number of unresolved *-definitions
            Coord scale = 1;   // scale factor applied to each *-weight; negative = "has Infinity"

            // Phase 1.  Determine the maximum *-weight and prepare to adjust *-weights
            Coord maxStar = 0;
            for (int i = 0; i < defCount; ++i)
            {
                GridDefinitionBase def = definitions[i];

                if (def.UserSize.IsStar)
                {
                    ++starCount;
                    def.MeasureSize = 1;  // meaning "not yet resolved in phase 3"
                    if (def.UserSize.Value > maxStar)
                    {
                        maxStar = def.UserSize.Value;
                    }
                }
            }

            if (Coord.IsPositiveInfinity(maxStar))
            {
                // negative scale means one or more of the weights was Infinity
                scale = -1;
            }
            else if (starCount > 0)
            {
                // if maxStar * starCount > Coord.Max, summing all the weights could cause
                // floating-point overflow.  To avoid that, scale the weights by a factor to keep
                // the sum within limits.  Choose a power of 2, to preserve precision.
                var power = Math.Floor(Math.Log(Coord.MaxValue / maxStar / starCount, 2));
                if (power < 0)
                {
                    scale = (Coord)Math.Pow(2, power - 4.0); // -4 is just for paranoia
                }
            }


            // normally Phases 2 and 3 execute only once.  But certain unusual combinations of weights
            // and constraints can defeat the algorithm, in which case we repeat Phases 2 and 3.
            // More explanation below...
            for (bool runPhase2and3 = true; runPhase2and3;)
            {
                // Phase 2.   Compute total *-weight W and available space S.
                // For *-items that have Min or Max constraints, compute the ratios used to decide
                // whether proportional space is too big or too small and add the item to the
                // corresponding list.  (The "min" list is in the first half of definitionIndices,
                // the "max" list in the second half.  DefinitionIndices has capacity at least
                // 2*defCount, so there's room for both lists.)
                totalStarWeight = 0;
                takenSize = 0;
                minCount = maxCount = 0;

                for (int i = 0; i < defCount; ++i)
                {
                    GridDefinitionBase def = definitions[i];

                    if (def.UserSize.IsStar)
                    {
                        Debug.Assert(!def.IsShared, "*-defs cannot be shared");

                        if (def.MeasureSize < 0)
                        {
                            takenSize += -def.MeasureSize;  // already resolved
                        }
                        else
                        {
                            Coord starWeight = StarWeight(def, scale);
                            totalStarWeight += starWeight;

                            if (def.MinSizeForArrange > 0)
                            {
                                // store ratio w/min in MeasureSize (for now)
                                definitionIndices[minCount++] = i;
                                def.MeasureSize = starWeight / def.MinSizeForArrange;
                            }

                            Coord effectiveMaxSize = Math.Max(def.MinSizeForArrange, def.UserMaxSize);
                            if (!Coord.IsPositiveInfinity(effectiveMaxSize))
                            {
                                // store ratio w/max in SizeCache (for now)
                                definitionIndices[defCount + maxCount++] = i;
                                def.SizeCache = starWeight / effectiveMaxSize;
                            }
                        }
                    }
                    else
                    {
                        Coord userSize = 0;

                        switch (def.UserSize.GridUnitType)
                        {
                            case (GridUnitType.Pixel):
                                userSize = def.UserSize.Value;
                                break;

                            case (GridUnitType.Auto):
                                userSize = def.MinSizeForArrange;
                                break;
                        }

                        Coord userMaxSize;

                        if (def.IsShared)
                        {
                            //  overriding userMaxSize effectively prevents squishy-ness.
                            //  this is a "solution" to avoid shared definitions from been sized to
                            //  different final size at arrange time, if / when different grids receive
                            //  different final sizes.
                            userMaxSize = userSize;
                        }
                        else
                        {
                            userMaxSize = def.UserMaxSize;
                        }

                        def.SizeCache
                            = Math.Max(def.MinSizeForArrange, Math.Min(userSize, userMaxSize));
                        takenSize += def.SizeCache;
                    }
                }

                // Phase 3.  Resolve *-items whose proportional sizes are too big or too small.
                int minCountPhase2 = minCount, maxCountPhase2 = maxCount;
                Coord takenStarWeight = 0;
                Coord remainingAvailableSize = finalSize - takenSize;
                Coord remainingStarWeight = totalStarWeight - takenStarWeight;

                MinRatioIndexComparer minRatioIndexComparer = new MinRatioIndexComparer(definitions);
                Array.Sort(definitionIndices, 0, minCount, minRatioIndexComparer);
                MaxRatioIndexComparer maxRatioIndexComparer = new MaxRatioIndexComparer(definitions);
                Array.Sort(definitionIndices, defCount, maxCount, maxRatioIndexComparer);

                while (minCount + maxCount > 0 && remainingAvailableSize > 0)
                {
                    // the calculation
                    //            remainingStarWeight = totalStarWeight - takenStarWeight
                    // is subject to catastrophic cancellation if the two terms are nearly equal,
                    // which leads to meaningless results.   Check for that, and recompute from
                    // the remaining definitions.   [This leads to quadratic behavior in really
                    // pathological cases - but they'd never arise in practice.]
                    const Coord starFactor = 1 / 256; // lose more than 8 bits of precision -> recalculate
                    if (remainingStarWeight < totalStarWeight * starFactor)
                    {
                        takenStarWeight = 0;
                        totalStarWeight = 0;

                        for (int i = 0; i < defCount; ++i)
                        {
                            GridDefinitionBase def = definitions[i];
                            if (def.UserSize.IsStar && def.MeasureSize > 0)
                            {
                                totalStarWeight += StarWeight(def, scale);
                            }
                        }

                        remainingStarWeight = totalStarWeight - takenStarWeight;
                    }

                    Coord minRatio = (minCount > 0)
                        ? definitions[definitionIndices[minCount - 1]].MeasureSize
                        : Coord.PositiveInfinity;
                    Coord maxRatio = (maxCount > 0)
                        ? definitions[definitionIndices[defCount + maxCount - 1]].SizeCache : -1;

                    // choose the def with larger ratio to the current proportion ("max discrepancy")
                    Coord proportion = remainingStarWeight / remainingAvailableSize;
                    bool? chooseMin = Choose(minRatio, maxRatio, proportion);

                    // if no def was chosen, advance to phase 4;  the current proportion doesn't
                    // conflict with any min or max values.
                    if (!(chooseMin.HasValue))
                    {
                        break;
                    }

                    // get the chosen definition and its resolved size
                    int resolvedIndex;
                    GridDefinitionBase resolvedDef;
                    Coord resolvedSize;
                    if (chooseMin == true)
                    {
                        resolvedIndex = definitionIndices[minCount - 1];
                        resolvedDef = definitions[resolvedIndex];
                        resolvedSize = resolvedDef.MinSizeForArrange;
                        --minCount;
                    }
                    else
                    {
                        resolvedIndex = definitionIndices[defCount + maxCount - 1];
                        resolvedDef = definitions[resolvedIndex];
                        resolvedSize = Math.Max(resolvedDef.MinSizeForArrange, resolvedDef.UserMaxSize);
                        --maxCount;
                    }

                    // resolve the chosen def, deduct its contributions from W and S.
                    // Defs resolved in phase 3 are marked by storing the negative of their resolved
                    // size in MeasureSize, to distinguish them from a pending def.
                    takenSize += resolvedSize;
                    resolvedDef.MeasureSize = -resolvedSize;
                    takenStarWeight += StarWeight(resolvedDef, scale);
                    --starCount;

                    remainingAvailableSize = finalSize - takenSize;
                    remainingStarWeight = totalStarWeight - takenStarWeight;

                    // advance to the next candidate defs, removing ones that have been resolved.
                    // Both counts are advanced, as a def might appear in both lists.
                    while (minCount > 0 && definitions[definitionIndices[minCount - 1]].MeasureSize < 0)
                    {
                        --minCount;
                        definitionIndices[minCount] = -1;
                    }
                    while (maxCount > 0
                        && definitions[definitionIndices[defCount + maxCount - 1]].MeasureSize < 0)
                    {
                        --maxCount;
                        definitionIndices[defCount + maxCount] = -1;
                    }
                }

                // decide whether to run Phase2 and Phase3 again.  There are 3 cases:
                // 1. There is space available, and *-defs remaining.  This is the
                //      normal case - move on to Phase 4 to allocate the remaining
                //      space proportionally to the remaining *-defs.
                // 2. There is space available, but no *-defs.  This implies at least one
                //      def was resolved as 'max', taking less space than its proportion.
                //      If there are also 'min' defs, reconsider them - we can give
                //      them more space.   If not, all the *-defs are 'max', so there's
                //      no way to use all the available space.
                // 3. We allocated too much space.   This implies at least one def was
                //      resolved as 'min'.  If there are also 'max' defs, reconsider
                //      them, otherwise the over-allocation is an inevitable consequence
                //      of the given min constraints.
                // Note that if we return to Phase2, at least one *-def will have been
                // resolved.  This guarantees we don't run Phase2+3 infinitely often.
                runPhase2and3 = false;
                if (starCount == 0 && takenSize < finalSize)
                {
                    // if no *-defs remain and we haven't allocated all the space, reconsider the defs
                    // resolved as 'min'.   Their allocation can be increased to make up the gap.
                    for (int i = minCount; i < minCountPhase2; ++i)
                    {
                        if (definitionIndices[i] >= 0)
                        {
                            GridDefinitionBase def = definitions[definitionIndices[i]];
                            def.MeasureSize = 1;      // mark as 'not yet resolved'
                            ++starCount;
                            runPhase2and3 = true;       // found a candidate, so re-run Phases 2 and 3
                        }
                    }
                }

                if (takenSize > finalSize)
                {
                    // if we've allocated too much space, reconsider the defs
                    // resolved as 'max'.   Their allocation can be decreased to make up the gap.
                    for (int i = maxCount; i < maxCountPhase2; ++i)
                    {
                        if (definitionIndices[defCount + i] >= 0)
                        {
                            GridDefinitionBase def = definitions[definitionIndices[defCount + i]];
                            def.MeasureSize = 1;      // mark as 'not yet resolved'
                            ++starCount;
                            runPhase2and3 = true;    // found a candidate, so re-run Phases 2 and 3
                        }
                    }
                }
            }

            // Phase 4.  Resolve the remaining defs proportionally.
            starCount = 0;
            for (int i = 0; i < defCount; ++i)
            {
                GridDefinitionBase def = definitions[i];

                if (def.UserSize.IsStar)
                {
                    if (def.MeasureSize < 0)
                    {
                        // this def was resolved in phase 3 - fix up its size
                        def.SizeCache = -def.MeasureSize;
                    }
                    else
                    {
                        // this def needs resolution, add it to the list, sorted by *-weight
                        definitionIndices[starCount++] = i;
                        def.MeasureSize = StarWeight(def, scale);
                    }
                }
            }

            if (starCount > 0)
            {
                StarWeightIndexComparer starWeightIndexComparer
                    = new StarWeightIndexComparer(definitions);
                Array.Sort(definitionIndices, 0, starCount, starWeightIndexComparer);

                // compute the partial sums of *-weight, in increasing order of weight
                // for minimal loss of precision.
                totalStarWeight = 0;
                for (int i = 0; i < starCount; ++i)
                {
                    GridDefinitionBase def = definitions[definitionIndices[i]];
                    totalStarWeight += def.MeasureSize;
                    def.SizeCache = totalStarWeight;
                }

                // resolve the defs, in decreasing order of weight.
                for (int i = starCount - 1; i >= 0; --i)
                {
                    GridDefinitionBase def = definitions[definitionIndices[i]];
                    Coord resolvedSize = (def.MeasureSize > 0)
                        ? Math.Max(finalSize - takenSize, 0) * (def.MeasureSize / def.SizeCache) : 0;

                    // min and max should have no effect by now, but just in case...
                    resolvedSize = Math.Min(resolvedSize, def.UserMaxSize);
                    resolvedSize = Math.Max(def.MinSizeForArrange, resolvedSize);

                    // Use the raw (unrounded) sizes to update takenSize, so that
                    // proportions are computed in the same terms as in phase 3;
                    // this avoids errors arising from min/max constraints.
                    takenSize += resolvedSize;
                    def.SizeCache = resolvedSize;
                }
            }

            // Phase 5.  Apply layout rounding.  We do this after fully allocating
            // unrounded sizes, to avoid breaking assumptions in the previous phases
            if (UseLayoutRounding)
            {
                // DpiScale dpiScale = GetDpi(); // yet to do
                // Coord dpi = columns ? dpiScale.DpiScaleX : dpiScale.DpiScaleY;
                var dpi = 1.0f;
                Coord[] roundingErrors = RoundingErrors;
                Coord roundedTakenSize = 0;

                // round each of the allocated sizes, keeping track of the deltas
                for (int i = 0; i < definitions.Length; ++i)
                {
                    GridDefinitionBase def = definitions[i];
                    Coord roundedSize = RoundLayoutValue(def.SizeCache, dpi);
                    roundingErrors[i] = (roundedSize - def.SizeCache);
                    def.SizeCache = roundedSize;
                    roundedTakenSize += roundedSize;
                }

                // The total allocation might differ from finalSize due to rounding
                // effects.  Tweak the allocations accordingly.

                // Theoretical and historical note.  The problem at hand - allocating
                // space to columns (or rows) with *-weights, min and max constraints,
                // and layout rounding - has a long history.  Especially the special
                // case of 50 columns with min=1 and available space=435 - allocating
                // seats in the U.S. House of Representatives to the 50 states in
                // proportion to their population.  There are numerous algorithms
                // and papers dating back to the 1700's, including the book:
                // Balinski, M. and H. Young, Fair Representation, Yale University
                // Press, New Haven, 1982.
                //
                // One surprising result of all this research is that *any* algorithm
                // will suffer from one or more undesirable features such as the
                // "population paradox" or the "Alabama paradox", where (to use our terminology)
                // increasing the available space by one pixel might actually decrease
                // the space allocated to a given column, or increasing the weight of
                // a column might decrease its allocation.   This is worth knowing
                // in case someone complains about this behavior;  it's not a bug so
                // much as something inherent to the problem.  Cite the book mentioned
                // above or one of the 100s of references, and resolve as WontFix.
                //
                // Fortunately, our scenarios tend to have a small number of columns (~10 or fewer)
                // each being allocated a large number of pixels (~50 or greater), and
                // people don't even notice the kind of 1-pixel anomalies that are
                // theoretically inevitable, or don't care if they do.  At least they shouldn't
                // care - no one should be using the results Alternet UI's grid layout to make
                // quantitative decisions; its job is to produce a reasonable display, not
                // to allocate seats in Congress.
                //
                // Our algorithm is more susceptible to paradox than the one currently
                // used for Congressional allocation ("Huntington-Hill" algorithm), but
                // it is faster to run:  O(N log N) vs. O(S * N), where N=number of
                // definitions, S = number of available pixels.  And it produces
                // adequate results in practice, as mentioned above.
                //
                // To reiterate one point:  all this only applies when layout rounding
                // is in effect.  When fractional sizes are allowed, the algorithm
                // behaves as well as possible, subject to the min/max constraints
                // and precision of floating-point computation.  (However, the resulting
                // display is subject to anti-aliasing problems.   TANSTAAFL.)

                if (!_AreClose(roundedTakenSize, finalSize))
                {
                    // Compute deltas
                    for (int i = 0; i < definitions.Length; ++i)
                    {
                        definitionIndices[i] = i;
                    }

                    // Sort rounding errors
                    RoundingErrorIndexComparer roundingErrorIndexComparer
                        = new RoundingErrorIndexComparer(roundingErrors);
                    Array.Sort(definitionIndices, 0, definitions.Length, roundingErrorIndexComparer);
                    Coord adjustedSize = roundedTakenSize;
                    Coord dpiIncrement = 1 / dpi;

                    if (roundedTakenSize > finalSize)
                    {
                        int i = definitions.Length - 1;
                        while ((adjustedSize > finalSize
                            && !_AreClose(adjustedSize, finalSize)) && i >= 0)
                        {
                            GridDefinitionBase definition = definitions[definitionIndices[i]];
                            Coord final = definition.SizeCache - dpiIncrement;
                            final = Math.Max(final, definition.MinSizeForArrange);
                            if (final < definition.SizeCache)
                            {
                                adjustedSize -= dpiIncrement;
                            }
                            definition.SizeCache = final;
                            i--;
                        }
                    }
                    else if (roundedTakenSize < finalSize)
                    {
                        int i = 0;
                        while ((adjustedSize < finalSize
                            && !_AreClose(adjustedSize, finalSize)) && i < definitions.Length)
                        {
                            GridDefinitionBase definition = definitions[definitionIndices[i]];
                            Coord final = definition.SizeCache + dpiIncrement;
                            final = Math.Max(final, definition.MinSizeForArrange);
                            if (final > definition.SizeCache)
                            {
                                adjustedSize += dpiIncrement;
                            }
                            definition.SizeCache = final;
                            i++;
                        }
                    }
                }
            }

            // Phase 6.  Compute final offsets
            definitions[0].FinalOffset = 0;
            for (int i = 0; i < definitions.Length; ++i)
            {
                definitions[(i + 1) % definitions.Length].FinalOffset
                    = definitions[i].FinalOffset + definitions[i].SizeCache;
            }
        }

        /// <summary>
        /// Choose the ratio with maximum discrepancy from the current proportion.
        /// Returns:
        ///     true    if proportion fails a min constraint but not a max, or
        ///                 if the min constraint has higher discrepancy
        ///     false   if proportion fails a max constraint but not a min, or
        ///                 if the max constraint has higher discrepancy
        ///     null    if proportion doesn't fail a min or max constraint
        /// The discrepancy is the ratio of the proportion to the max- or min-ratio.
        /// When both ratios hit the constraint,  minRatio &lt; proportion &lt; maxRatio,
        /// and the minRatio has higher discrepancy if
        ///         (proportion / minRatio) &gt; (maxRatio / proportion)
        /// </summary>
        private static bool? Choose(Coord minRatio, Coord maxRatio, Coord proportion)
        {
            if (minRatio < proportion)
            {
                if (maxRatio > proportion)
                {
                    // compare proportion/minRatio : maxRatio/proportion, but
                    // do it carefully to avoid floating-point overflow or underflow
                    // and divide-by-0.
                    var minPower = Math.Floor(Math.Log(minRatio, 2));
                    var maxPower = Math.Floor(Math.Log(maxRatio, 2));
                    Coord f = (Coord)Math.Pow(2, Math.Floor((minPower + maxPower) / 2));
                    if ((proportion / f) * (proportion / f) > (minRatio / f) * (maxRatio / f))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else if (maxRatio > proportion)
            {
                return false;
            }

            return null;
        }

        /// <summary>
        /// Sorts row/column indices by rounding error if layout rounding is applied.
        /// </summary>
        /// <param name="x">Index, rounding error pair</param>
        /// <param name="y">Index, rounding error pair</param>
        /// <returns>1 if x.Value > y.Value, 0 if equal, -1 otherwise</returns>
        private static int CompareRoundingErrors(KeyValuePair<int, Coord> x, KeyValuePair<int, Coord> y)
        {
            if (x.Value < y.Value)
            {
                return -1;
            }
            else if (x.Value > y.Value)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Calculates final (aka arrange) size for given range.
        /// </summary>
        /// <param name="definitions">Array of definitions to process.</param>
        /// <param name="start">Start of the range.</param>
        /// <param name="count">Number of items in the range.</param>
        /// <returns>Final size.</returns>
        private Coord GetFinalSizeForRange(
            GridDefinitionBase[] definitions,
            int start,
            int count)
        {
            Coord size = 0;
            int i = start + count - 1;

            do
            {
                size += definitions[i].SizeCache;
            } while (--i >= start);

            return (size);
        }

        /// <summary>
        /// Clears dirty state for the grid and its columns / rows
        /// </summary>
        private void SetValid()
        {
            ExtendedData extData = ExtData;
            if (extData != null)
            {
                if (extData.TempDefinitions != null)
                {
                    //  TempDefinitions has to be cleared to avoid "memory leaks"
                    Array.Clear(
                        extData.TempDefinitions,
                        0,
                        Math.Max(DefinitionsU.Length,
                        DefinitionsV.Length));
                    extData.TempDefinitions = null;
                }
            }
        }

        /// <summary>
        /// Returns <c>true</c> if ColumnDefinitions collection is not empty
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeColumnDefinitions()
        {
            ExtendedData extData = ExtData;
            return (extData != null
                    && extData.ColumnDefinitions != null
                    && extData.ColumnDefinitions.Count > 0);
        }

        /// <summary>
        /// Returns <c>true</c> if RowDefinitions collection is not empty
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeRowDefinitions()
        {
            ExtendedData extData = ExtData;
            return (extData != null
                    && extData.RowDefinitions != null
                    && extData.RowDefinitions.Count > 0);
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

        /// <summary>
        /// CheckFlagsOr returns <c>true</c> if at least one flag in the
        /// given bitmask is set.
        /// </summary>
        /// <remarks>
        /// If no bits are set in the given bitmask, the method returns
        /// <c>true</c>.
        /// </remarks>
        private bool CheckFlagsOr(Flags flags)
        {
            return (flags == 0 || (_flags & flags) != 0);
        }

        private static void OnShowGridLinesPropertyChanged(Grid grid, bool newValue)
        {
            if (grid.ExtData != null    // trivial grid is 1 by 1. there is no grid lines anyway
                && grid.ListenToNotifications)
            {
                grid.Invalidate();
            }

            grid.SetFlags(newValue, Flags.ShowGridLinesPropertyValue);
        }

        private static void OnCellAttachedPropertyChanged(AbstractControl child)
        {
            if (child != null)
            {
                var grid = child.Parent as Grid;
                if (grid != null
                    && grid.ExtData != null
                    && grid.ListenToNotifications)
                {
                    grid.CellsStructureDirty = true;
                    grid.PerformLayout();
                }
            }
        }

        private static bool IsIntValueNotNegative(object value)
        {
            return ((int)value >= 0);
        }

        private static bool IsIntValueGreaterThanZero(object value)
        {
            return ((int)value > 0);
        }

        /// <summary>
        /// Helper for Comparer methods.
        /// </summary>
        /// <returns>
        /// true iff one or both of x and y are null, in which case result holds
        /// the relative sort order.
        /// </returns>
        private static bool CompareNullRefs(object x, object y, out int result)
        {
            result = 2;

            if (x == null)
            {
                if (y == null)
                {
                    result = 0;
                }
                else
                {
                    result = -1;
                }
            }
            else
            {
                if (y == null)
                {
                    result = 1;
                }
            }

            return (result != 2);
        }

        /// <summary>
        /// Private version returning array of column definitions.
        /// </summary>
        private GridDefinitionBase[] DefinitionsU
        {
            get { return (ExtData.DefinitionsU); }
        }

        /// <summary>
        /// Private version returning array of row definitions.
        /// </summary>
        private GridDefinitionBase[] DefinitionsV
        {
            get { return (ExtData.DefinitionsV); }
        }

        /// <summary>
        /// Helper accessor to layout time array of definitions.
        /// </summary>
        private GridDefinitionBase[] TempDefinitions
        {
            get
            {
                ExtendedData extData = ExtData;
                int requiredLength = Math.Max(DefinitionsU.Length, DefinitionsV.Length) * 2;

                if (extData.TempDefinitions == null
                    || extData.TempDefinitions.Length < requiredLength)
                {
                    WeakReference tempDefinitionsWeakRef
                        = (WeakReference)Thread.GetData(s_tempDefinitionsDataSlot);
                    if (tempDefinitionsWeakRef == null)
                    {
                        extData.TempDefinitions = new GridDefinitionBase[requiredLength];
                        Thread.SetData(
                            s_tempDefinitionsDataSlot,
                            new WeakReference(extData.TempDefinitions));
                    }
                    else
                    {
                        extData.TempDefinitions = (GridDefinitionBase[])tempDefinitionsWeakRef.Target;
                        if (extData.TempDefinitions == null
                            || extData.TempDefinitions.Length < requiredLength)
                        {
                            extData.TempDefinitions = new GridDefinitionBase[requiredLength];
                            tempDefinitionsWeakRef.Target = extData.TempDefinitions;
                        }
                    }
                }
                return (extData.TempDefinitions);
            }
        }

        /// <summary>
        /// Helper accessor to definition indices.
        /// </summary>
        private int[] DefinitionIndices
        {
            get
            {
                int requiredLength
                    = Math.Max(Math.Max(DefinitionsU.Length, DefinitionsV.Length), 1) * 2;

                if (_definitionIndices == null || _definitionIndices.Length < requiredLength)
                {
                    _definitionIndices = new int[requiredLength];
                }

                return _definitionIndices;
            }
        }

        /// <summary>
        /// Helper accessor to rounding errors.
        /// </summary>
        private Coord[] RoundingErrors
        {
            get
            {
                int requiredLength = Math.Max(DefinitionsU.Length, DefinitionsV.Length);

                if (_roundingErrors == null && requiredLength == 0)
                {
                    _roundingErrors = new Coord[1];
                }
                else if (_roundingErrors == null || _roundingErrors.Length < requiredLength)
                {
                    _roundingErrors = new Coord[requiredLength];
                }
                return _roundingErrors;
            }
        }

        /// <summary>
        /// Private version returning array of cells.
        /// </summary>
        private CellCache[] PrivateCells
        {
            get { return (ExtData.CellCachesCollection); }
        }

        /// <summary>
        /// Convenience accessor to ValidCellsStructure bit flag.
        /// </summary>
        private bool CellsStructureDirty
        {
            get { return (!CheckFlagsAnd(Flags.ValidCellsStructure)); }
            set { SetFlags(!value, Flags.ValidCellsStructure); }
        }

        /// <summary>
        /// Convenience accessor to ListenToNotifications bit flag.
        /// </summary>
        private bool ListenToNotifications
        {
            get { return (CheckFlagsAnd(Flags.ListenToNotifications)); }
            set { SetFlags(value, Flags.ListenToNotifications); }
        }

        /// <summary>
        /// Convenience accessor to SizeToContentU bit flag.
        /// </summary>
        private bool SizeToContentU
        {
            get { return (CheckFlagsAnd(Flags.SizeToContentU)); }
            set { SetFlags(value, Flags.SizeToContentU); }
        }

        /// <summary>
        /// Convenience accessor to SizeToContentV bit flag.
        /// </summary>
        private bool SizeToContentV
        {
            get { return (CheckFlagsAnd(Flags.SizeToContentV)); }
            set { SetFlags(value, Flags.SizeToContentV); }
        }

        /// <summary>
        /// Convenience accessor to HasStarCellsU bit flag.
        /// </summary>
        private bool HasStarCellsU
        {
            get { return (CheckFlagsAnd(Flags.HasStarCellsU)); }
            set { SetFlags(value, Flags.HasStarCellsU); }
        }

        /// <summary>
        /// Convenience accessor to HasStarCellsV bit flag.
        /// </summary>
        private bool HasStarCellsV
        {
            get { return (CheckFlagsAnd(Flags.HasStarCellsV)); }
            set { SetFlags(value, Flags.HasStarCellsV); }
        }

        /// <summary>
        /// Convenience accessor to HasGroup3CellsInAutoRows bit flag.
        /// </summary>
        private bool HasGroup3CellsInAutoRows
        {
            get { return (CheckFlagsAnd(Flags.HasGroup3CellsInAutoRows)); }
            set { SetFlags(value, Flags.HasGroup3CellsInAutoRows); }
        }

        /// <summary>
        /// fp version of <c>d == 0</c>.
        /// </summary>
        /// <param name="d">Value to check.</param>
        /// <returns><c>true</c> if d == 0.</returns>
        private static bool _IsZero(Coord d)
        {
            return (Math.Abs(d) < c_epsilon);
        }

        /// <summary>
        /// fp version of <c>d1 == d2</c>
        /// </summary>
        /// <param name="d1">First value to compare</param>
        /// <param name="d2">Second value to compare</param>
        /// <returns><c>true</c> if d1 == d2</returns>
        private static bool _AreClose(Coord d1, Coord d2)
        {
            return (Math.Abs(d1 - d2) < c_epsilon);
        }

        /// <summary>
        /// Returns reference to extended data bag.
        /// </summary>
        private ExtendedData ExtData
        {
            get { return (_data); }
        }

        /// <summary>
        /// Returns *-weight, adjusted for scale computed during Phase 1
        /// </summary>
        static Coord StarWeight(GridDefinitionBase def, Coord scale)
        {
            if (scale < 0)
            {
                // if one of the *-weights is Infinity, adjust the weights by mapping
                // Infinity to 1 and everything else to 0:  the infinite items share the
                // available space equally, everyone else gets nothing.
                return (Coord.IsPositiveInfinity(def.UserSize.Value)) ? 1 : 0;
            }
            else
            {
                return def.UserSize.Value * scale;
            }
        }
    }
}