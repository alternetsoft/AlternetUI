#pragma warning disable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alternet.Base.Collections;

namespace Alternet.UI
{
    public partial class Grid
    {
        /// <summary>
        /// Extended data instantiated on demand, when grid handles non-trivial case.
        /// </summary>
        private class ExtendedData
        {
            internal GridColumnCollection ColumnDefinitions;  //  collection of column definitions (logical tree support)
            internal GridRowCollection RowDefinitions;        //  collection of row definitions (logical tree support)
            internal GridDefinitionBase[] DefinitionsU;                 //  collection of column definitions used during calc
            internal GridDefinitionBase[] DefinitionsV;                 //  collection of row definitions used during calc
            internal CellCache[] CellCachesCollection;              //  backing store for logical children
            internal int CellGroup1;                                //  index of the first cell in first cell group
            internal int CellGroup2;                                //  index of the first cell in second cell group
            internal int CellGroup3;                                //  index of the first cell in third cell group
            internal int CellGroup4;                                //  index of the first cell in forth cell group
            internal GridDefinitionBase[] TempDefinitions;              //  temporary array used during layout for various purposes
                                                                        //  TempDefinitions.Length == Max(definitionsU.Length, definitionsV.Length)
        }

        /// <summary>
        /// Grid validity / property caches dirtiness flags
        /// </summary>
        [System.Flags]
        private enum Flags
        {
            // the foolowing flags let grid tracking dirtiness in more granular manner:
            // * Valid???Structure flags indicate that elements were added or removed.
            // * Valid???Layout flags indicate that layout time portion of the information
            // stored on the objects should be updated.
            ValidDefinitionsUStructure = 0x00000001,
            ValidDefinitionsVStructure = 0x00000002,
            ValidCellsStructure = 0x00000004,

            // boolean properties state
            ShowGridLinesPropertyValue = 0x00000100,   //  show grid lines ?

            // boolean flags
            ListenToNotifications = 0x00001000,   //  "0" when all notifications are ignored
            SizeToContentU = 0x00002000,   //  "1" if calculating to content in U direction
            SizeToContentV = 0x00004000,   //  "1" if calculating to content in V direction
            HasStarCellsU = 0x00008000,   //  "1" if at least one cell belongs to a Star column
            HasStarCellsV = 0x00010000,   //  "1" if at least one cell belongs to a Star row
            HasGroup3CellsInAutoRows = 0x00020000,   //  "1" if at least one cell of group 3 belongs to an Auto row
            MeasureOverrideInProgress = 0x00040000,   //  "1" while in the context of Grid.MeasureOverride
            ArrangeOverrideInProgress = 0x00080000,   //  "1" while in the context of Grid.ArrangeOverride
        }

        /// <summary>
        /// LayoutTimeSizeType is used internally and reflects layout-time size type.
        /// </summary>
        [System.Flags]
        internal enum LayoutTimeSizeType : byte
        {
            None = 0x00,
            Pixel = 0x01,
            Auto = 0x02,
            Star = 0x04,
        }

        /// <summary>
        /// CellCache stored calculated values of
        /// 1. attached cell positioning properties;
        /// 2. size type;
        /// 3. index of a next cell in the group;
        /// </summary>
        private struct CellCache
        {
            internal int ColumnIndex;
            internal int RowIndex;
            internal int ColumnSpan;
            internal int RowSpan;
            internal LayoutTimeSizeType SizeTypeU;
            internal LayoutTimeSizeType SizeTypeV;
            internal int Next;

            internal bool IsStarU { get { return (SizeTypeU & LayoutTimeSizeType.Star) != 0; } }

            internal bool IsAutoU { get { return (SizeTypeU & LayoutTimeSizeType.Auto) != 0; } }

            internal bool IsStarV { get { return (SizeTypeV & LayoutTimeSizeType.Star) != 0; } }

            internal bool IsAutoV { get { return (SizeTypeV & LayoutTimeSizeType.Auto) != 0; } }
        }

        /// <summary>
        /// Helper class for representing a key for a span in hashtable.
        /// </summary>
        private class SpanKey
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="start">Starting index of the span.</param>
            /// <param name="count">Span count.</param>
            /// <param name="u"><c>true</c> for columns; <c>false</c> for rows.</param>
            internal SpanKey(int start, int count, bool u)
            {
                _start = start;
                _count = count;
                _u = u;
            }

            /// <summary>
            /// <see cref="object.GetHashCode"/>
            /// </summary>
            public override int GetHashCode()
            {
                int hash = (_start ^ (_count << 2));

                if (_u) hash &= 0x7ffffff;
                else hash |= 0x8000000;

                return (hash);
            }

            /// <summary>
            /// <see cref="object.Equals(object)"/>
            /// </summary>
            public override bool Equals(object obj)
            {
                SpanKey sk = obj as SpanKey;
                return (sk != null
                        && sk._start == _start
                        && sk._count == _count
                        && sk._u == _u);
            }

            /// <summary>
            /// Returns start index of the span.
            /// </summary>
            internal int Start { get { return (_start); } }

            /// <summary>
            /// Returns span count.
            /// </summary>
            internal int Count { get { return (_count); } }

            /// <summary>
            /// Returns <c>true</c> if this is a column span.
            /// <c>false</c> if this is a row span.
            /// </summary>
            internal bool U { get { return (_u); } }

            private int _start;
            private int _count;
            private bool _u;
        }

        /// <summary>
        /// SpanPreferredDistributionOrderComparer.
        /// </summary>
        private class SpanPreferredDistributionOrderComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                GridDefinitionBase definitionX = x as GridDefinitionBase;
                GridDefinitionBase definitionY = y as GridDefinitionBase;

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    if (definitionX.UserSize.IsAuto)
                    {
                        if (definitionY.UserSize.IsAuto)
                        {
                            result = definitionX.MinSize.CompareTo(definitionY.MinSize);
                        }
                        else
                        {
                            result = -1;
                        }
                    }
                    else
                    {
                        if (definitionY.UserSize.IsAuto)
                        {
                            result = +1;
                        }
                        else
                        {
                            result = definitionX.PreferredSize.CompareTo(definitionY.PreferredSize);
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// SpanMaxDistributionOrderComparer.
        /// </summary>
        private class SpanMaxDistributionOrderComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                GridDefinitionBase definitionX = x as GridDefinitionBase;
                GridDefinitionBase definitionY = y as GridDefinitionBase;

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    if (definitionX.UserSize.IsAuto)
                    {
                        if (definitionY.UserSize.IsAuto)
                        {
                            result = definitionX.SizeCache.CompareTo(definitionY.SizeCache);
                        }
                        else
                        {
                            result = +1;
                        }
                    }
                    else
                    {
                        if (definitionY.UserSize.IsAuto)
                        {
                            result = -1;
                        }
                        else
                        {
                            result = definitionX.SizeCache.CompareTo(definitionY.SizeCache);
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// StarDistributionOrderComparer.
        /// </summary>
        private class StarDistributionOrderComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                GridDefinitionBase definitionX = x as GridDefinitionBase;
                GridDefinitionBase definitionY = y as GridDefinitionBase;

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    result = definitionX.SizeCache.CompareTo(definitionY.SizeCache);
                }

                return result;
            }
        }

        /// <summary>
        /// DistributionOrderComparer.
        /// </summary>
        private class DistributionOrderComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                GridDefinitionBase definitionX = x as GridDefinitionBase;
                GridDefinitionBase definitionY = y as GridDefinitionBase;

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    Coord xprime = definitionX.SizeCache - definitionX.MinSizeForArrange;
                    Coord yprime = definitionY.SizeCache - definitionY.MinSizeForArrange;
                    result = xprime.CompareTo(yprime);
                }

                return result;
            }
        }


        /// <summary>
        /// StarDistributionOrderIndexComparer.
        /// </summary>
        private class StarDistributionOrderIndexComparer : IComparer
        {
            private readonly GridDefinitionBase[] definitions;

            internal StarDistributionOrderIndexComparer(GridDefinitionBase[] definitions)
            {
                Debug.Assert(definitions != null);
                this.definitions = definitions;
            }

            public int Compare(object x, object y)
            {
                int? indexX = x as int?;
                int? indexY = y as int?;

                GridDefinitionBase definitionX = null;
                GridDefinitionBase definitionY = null;

                if (indexX != null)
                {
                    definitionX = definitions[indexX.Value];
                }

                if (indexY != null)
                {
                    definitionY = definitions[indexY.Value];
                }

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    result = definitionX.SizeCache.CompareTo(definitionY.SizeCache);
                }

                return result;
            }
        }

        /// <summary>
        /// DistributionOrderComparer.
        /// </summary>
        private class DistributionOrderIndexComparer : IComparer
        {
            private readonly GridDefinitionBase[] definitions;

            internal DistributionOrderIndexComparer(GridDefinitionBase[] definitions)
            {
                Debug.Assert(definitions != null);
                this.definitions = definitions;
            }

            public int Compare(object x, object y)
            {
                int? indexX = x as int?;
                int? indexY = y as int?;

                GridDefinitionBase definitionX = null;
                GridDefinitionBase definitionY = null;

                if (indexX != null)
                {
                    definitionX = definitions[indexX.Value];
                }

                if (indexY != null)
                {
                    definitionY = definitions[indexY.Value];
                }

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    Coord xprime = definitionX.SizeCache - definitionX.MinSizeForArrange;
                    Coord yprime = definitionY.SizeCache - definitionY.MinSizeForArrange;
                    result = xprime.CompareTo(yprime);
                }

                return result;
            }
        }

        /// <summary>
        /// RoundingErrorIndexComparer.
        /// </summary>
        private class RoundingErrorIndexComparer : IComparer
        {
            private readonly Coord[] errors;

            internal RoundingErrorIndexComparer(Coord[] errors)
            {
                Debug.Assert(errors != null);
                this.errors = errors;
            }

            public int Compare(object x, object y)
            {
                int? indexX = x as int?;
                int? indexY = y as int?;

                int result;

                if (!CompareNullRefs(indexX, indexY, out result))
                {
                    Coord errorX = errors[indexX.Value];
                    Coord errorY = errors[indexY.Value];
                    result = errorX.CompareTo(errorY);
                }

                return result;
            }
        }

        /// <summary>
        /// MinRatioComparer.
        /// Sort by w/min (stored in MeasureSize), descending.
        /// We query the list from the back, i.e. in ascending order of w/min.
        /// </summary>
        private class MinRatioComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                GridDefinitionBase definitionX = x as GridDefinitionBase;
                GridDefinitionBase definitionY = y as GridDefinitionBase;

                int result;

                if (!CompareNullRefs(definitionY, definitionX, out result))
                {
                    result = definitionY.MeasureSize.CompareTo(definitionX.MeasureSize);
                }

                return result;
            }
        }

        /// <summary>
        /// MaxRatioComparer.
        /// Sort by w/max (stored in SizeCache), ascending.
        /// We query the list from the back, i.e. in descending order of w/max.
        /// </summary>
        private class MaxRatioComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                GridDefinitionBase definitionX = x as GridDefinitionBase;
                GridDefinitionBase definitionY = y as GridDefinitionBase;

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    result = definitionX.SizeCache.CompareTo(definitionY.SizeCache);
                }

                return result;
            }
        }

        /// <summary>
        /// StarWeightComparer.
        /// Sort by *-weight (stored in MeasureSize), ascending.
        /// </summary>
        private class StarWeightComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                GridDefinitionBase definitionX = x as GridDefinitionBase;
                GridDefinitionBase definitionY = y as GridDefinitionBase;

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    result = definitionX.MeasureSize.CompareTo(definitionY.MeasureSize);
                }

                return result;
            }
        }

        /// <summary>
        /// MinRatioIndexComparer.
        /// </summary>
        private class MinRatioIndexComparer : IComparer
        {
            private readonly GridDefinitionBase[] definitions;

            internal MinRatioIndexComparer(GridDefinitionBase[] definitions)
            {
                Debug.Assert(definitions != null);
                this.definitions = definitions;
            }

            public int Compare(object x, object y)
            {
                int? indexX = x as int?;
                int? indexY = y as int?;

                GridDefinitionBase definitionX = null;
                GridDefinitionBase definitionY = null;

                if (indexX != null)
                {
                    definitionX = definitions[indexX.Value];
                }
                if (indexY != null)
                {
                    definitionY = definitions[indexY.Value];
                }

                int result;

                if (!CompareNullRefs(definitionY, definitionX, out result))
                {
                    result = definitionY.MeasureSize.CompareTo(definitionX.MeasureSize);
                }

                return result;
            }
        }

        /// <summary>
        /// MaxRatioIndexComparer.
        /// </summary>
        private class MaxRatioIndexComparer : IComparer
        {
            private readonly GridDefinitionBase[] definitions;

            internal MaxRatioIndexComparer(GridDefinitionBase[] definitions)
            {
                Debug.Assert(definitions != null);
                this.definitions = definitions;
            }

            public int Compare(object x, object y)
            {
                int? indexX = x as int?;
                int? indexY = y as int?;

                GridDefinitionBase definitionX = null;
                GridDefinitionBase definitionY = null;

                if (indexX != null)
                {
                    definitionX = definitions[indexX.Value];
                }
                if (indexY != null)
                {
                    definitionY = definitions[indexY.Value];
                }

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    result = definitionX.SizeCache.CompareTo(definitionY.SizeCache);
                }

                return result;
            }
        }

        /// <summary>
        /// MaxRatioIndexComparer.
        /// </summary>
        private class StarWeightIndexComparer : IComparer
        {
            private readonly GridDefinitionBase[] definitions;

            internal StarWeightIndexComparer(GridDefinitionBase[] definitions)
            {
                Debug.Assert(definitions != null);
                this.definitions = definitions;
            }

            public int Compare(object x, object y)
            {
                int? indexX = x as int?;
                int? indexY = y as int?;

                GridDefinitionBase definitionX = null;
                GridDefinitionBase definitionY = null;

                if (indexX != null)
                {
                    definitionX = definitions[indexX.Value];
                }

                if (indexY != null)
                {
                    definitionY = definitions[indexY.Value];
                }

                int result;

                if (!CompareNullRefs(definitionX, definitionY, out result))
                {
                    result = definitionX.MeasureSize.CompareTo(definitionY.MeasureSize);
                }

                return result;
            }
        }

        /// <summary>
        /// Implementation of a simple enumerator of grid's logical children
        /// </summary>
        private class GridChildrenCollectionEnumeratorSimple : IEnumerator
        {
            internal GridChildrenCollectionEnumeratorSimple(Grid grid, bool includeChildren)
            {
                Debug.Assert(grid != null);
                _currentEnumerator = -1;
                _enumerator0 = new GridColumnCollection.Enumerator(grid.ExtData != null ? grid.ExtData.ColumnDefinitions : null);
                _enumerator1 = new GridRowCollection.Enumerator(grid.ExtData != null ? grid.ExtData.RowDefinitions : null);
                _enumerator2Index = 0;
                if (includeChildren)
                {
                    _enumerator2Collection = grid.Children;
                    _enumerator2Count = _enumerator2Collection.Count;
                }
                else
                {
                    _enumerator2Collection = null;
                    _enumerator2Count = 0;
                }
            }

            public bool MoveNext()
            {
                while (_currentEnumerator < 3)
                {
                    if (_currentEnumerator >= 0)
                    {
                        switch (_currentEnumerator)
                        {
                            case (0): if (_enumerator0.MoveNext()) { _currentChild = _enumerator0.Current; return (true); } break;
                            case (1): if (_enumerator1.MoveNext()) { _currentChild = _enumerator1.Current; return (true); } break;
                            case (2):
                                if (_enumerator2Index < _enumerator2Count)
                                {
                                    _currentChild = _enumerator2Collection[_enumerator2Index];
                                    _enumerator2Index++;
                                    return (true);
                                }
                                break;
                        }
                    }
                    _currentEnumerator++;
                }
                return (false);
            }

            public Object Current
            {
                get
                {
                    if (_currentEnumerator == -1)
                    {
#pragma warning disable 6503 // IEnumerator.Current is documented to throw this exception
                        throw new InvalidOperationException();
                    }
                    if (_currentEnumerator >= 3)
                    {
#pragma warning restore 6503 // IEnumerator.Current is documented to throw this exception
                        throw new InvalidOperationException();
                    }

                    // assert below is not true anymore since UIElementCollection
                    // allowes for null children
                    // Debug.Assert(_currentChild != null);
                    return (_currentChild);
                }
            }

            public void Reset()
            {
                _currentEnumerator = -1;
                _currentChild = null;
                _enumerator0.Reset();
                _enumerator1.Reset();
                _enumerator2Index = 0;
            }

            private int _currentEnumerator;
            private Object _currentChild;
            private GridColumnCollection.Enumerator _enumerator0;
            private GridRowCollection.Enumerator _enumerator1;
            private Collection<AbstractControl> _enumerator2Collection;
            private int _enumerator2Index;
            private int _enumerator2Count;
        }

        internal enum Counters : int
        {
            Default = -1,

            MeasureOverride,
            _ValidateColsStructure,
            _ValidateRowsStructure,
            _ValidateCells,
            _MeasureCell,
            __MeasureChild,
            _CalculateDesiredSize,

            ArrangeOverride,
            _SetFinalSize,
            _ArrangeChildHelper2,
            _PositionCell,

            Count,
        }
    }
}
