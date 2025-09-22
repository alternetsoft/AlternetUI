#pragma warning disable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Grid
    {
        /// <inheritdoc/>
        public override void OnLayout()
        {
            GetPreferredSize(PreferredSizeContext.PositiveInfinity);

            var arrangeSize = ChildrenLayoutBounds.Size;
            try
            {
                ArrangeOverrideInProgress = true;

                if (_data == null)
                {
                    var children = Children;

                    for (int i = 0, count = children.Count; i < count; ++i)
                    {
                        var child = children[i];
                        if (child != null)
                        {
                            child.Bounds = new RectD(new PointD(), arrangeSize);
                        }
                    }
                }
                else
                {
                    Debug.Assert(DefinitionsU.Length > 0 && DefinitionsV.Length > 0);

                    SetFinalSize(DefinitionsU, arrangeSize.Width, true);
                    SetFinalSize(DefinitionsV, arrangeSize.Height, false);

                    var children = Children;

                    for (int currentCell = 0; currentCell < PrivateCells.Length; ++currentCell)
                    {
                        var cell = children[currentCell];
                        if (cell == null)
                        {
                            continue;
                        }

                        int columnIndex = PrivateCells[currentCell].ColumnIndex;
                        int rowIndex = PrivateCells[currentCell].RowIndex;
                        int columnSpan = PrivateCells[currentCell].ColumnSpan;
                        int rowSpan = PrivateCells[currentCell].RowSpan;

                        var cellRect = new RectD(
                            columnIndex == 0 ? 0.0f : DefinitionsU[columnIndex].FinalOffset,
                            rowIndex == 0 ? 0.0f : DefinitionsV[rowIndex].FinalOffset,
                            GetFinalSizeForRange(DefinitionsU, columnIndex, columnSpan),
                            GetFinalSizeForRange(DefinitionsV, rowIndex, rowSpan));

                        SetControlBounds(cell, cellRect);
                    }
                }
            }
            finally
            {
                SetValid();
                ArrangeOverrideInProgress = false;
            }
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            SizeD gridDesiredSize;
            ExtendedData extData = ExtData;

            try
            {
                ListenToNotifications = true;
                MeasureOverrideInProgress = true;

                if (extData == null)
                {
                    gridDesiredSize = new SizeD();
                    var children = Children;

                    for (int i = 0, count = children.Count; i < count; ++i)
                    {
                        AbstractControl child = children[i];
                        if (child != null)
                        {
                            var s = child.GetPreferredSizeLimited(context);
                            var childDesiredSize = new SizeD(
                                s.Width + child.Margin.Horizontal,
                                s.Height + child.Margin.Vertical);
                            gridDesiredSize.Width = Math.Max(
                                gridDesiredSize.Width,
                                childDesiredSize.Width);
                            gridDesiredSize.Height = Math.Max(
                                gridDesiredSize.Height,
                                childDesiredSize.Height);
                        }
                    }
                }
                else
                {
                    {
                        bool sizeToContentU = Coord.IsPositiveInfinity(context.AvailableSize.Width);
                        bool sizeToContentV = Coord.IsPositiveInfinity(context.AvailableSize.Height);

                        // Clear index information and rounding errors
                        if (RowDefinitionCollectionDirty || ColumnDefinitionCollectionDirty)
                        {
                            if (_definitionIndices != null)
                            {
                                Array.Clear(_definitionIndices, 0, _definitionIndices.Length);
                                _definitionIndices = null;
                            }

                            if (UseLayoutRounding)
                            {
                                if (_roundingErrors != null)
                                {
                                    Array.Clear(_roundingErrors, 0, _roundingErrors.Length);
                                    _roundingErrors = null;
                                }
                            }
                        }

                        ValidateDefinitionsUStructure();
                        ValidateDefinitionsLayout(DefinitionsU, sizeToContentU);

                        ValidateDefinitionsVStructure();
                        ValidateDefinitionsLayout(DefinitionsV, sizeToContentV);

                        CellsStructureDirty |= (SizeToContentU != sizeToContentU)
                            || (SizeToContentV != sizeToContentV);

                        SizeToContentU = sizeToContentU;
                        SizeToContentV = sizeToContentV;
                    }

                    ValidateCells();

                    Debug.Assert(DefinitionsU.Length > 0 && DefinitionsV.Length > 0);

                    //  Grid classifies cells into four groups depending on
                    //  the column / row type a cell belongs to (number corresponds to
                    //  group number):
                    //
                    //                   Px      Auto     Star
                    //               +--------+--------+--------+
                    //               |        |        |        |
                    //            Px |    1   |    1   |    3   |
                    //               |        |        |        |
                    //               +--------+--------+--------+
                    //               |        |        |        |
                    //          Auto |    1   |    1   |    3   |
                    //               |        |        |        |
                    //               +--------+--------+--------+
                    //               |        |        |        |
                    //          Star |    4   |    2   |    4   |
                    //               |        |        |        |
                    //               +--------+--------+--------+
                    //
                    //  The group number indicates the order in which cells are measured.
                    //  Certain order is necessary to be able to dynamically resolve star
                    //  columns / rows sizes which are used as input for measuring of
                    //  the cells belonging to them.
                    //
                    //  However, there are cases when topology of a grid causes cyclical
                    //  size dependences. For example:
                    //
                    //
                    //                         column width="Auto"      column width="*"
                    //                      +----------------------+----------------------+
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //  row height="Auto"   |                      |      cell 1 2        |
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //                      +----------------------+----------------------+
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //  row height="*"      |       cell 2 1       |                      |
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //                      |                      |                      |
                    //                      +----------------------+----------------------+
                    //
                    //  In order to accurately calculate constraint width for "cell 1 2"
                    //  (which is the remaining of grid's available width and calculated
                    //  value of Auto column), "cell 2 1" needs to be calculated first,
                    //  as it contributes to the Auto column's calculated value.
                    //  At the same time in order to accurately calculate constraint
                    //  height for "cell 2 1", "cell 1 2" needs to be calculated first,
                    //  as it contributes to Auto row height, which is used in the
                    //  computation of Star row resolved height.
                    //
                    //  to "break" this cyclical dependency we are making (arbitrary)
                    //  decision to treat cells like "cell 2 1" as if they appear in Auto
                    //  rows. And then recalculate them one more time when star row
                    //  heights are resolved.
                    //
                    //  (Or more strictly) the code below implement the following logic:
                    //
                    //                       +---------+
                    //                       |  enter  |
                    //                       +---------+
                    //                            |
                    //                            V
                    //                    +----------------+
                    //                    | Measure Group1 |
                    //                    +----------------+
                    //                            |
                    //                            V
                    //                          / - \
                    //                        /       \
                    //                  Y   /    Can    \    N
                    //            +--------|   Resolve   |-----------+
                    //            |         \  StarsV?  /            |
                    //            |           \       /              |
                    //            |             \ - /                |
                    //            V                                  V
                    //    +----------------+                       / - \
                    //    | Resolve StarsV |                     /       \
                    //    +----------------+               Y   /    Can    \    N
                    //            |                      +----|   Resolve   |------+
                    //            V                      |     \  StarsU?  /       |
                    //    +----------------+             |       \       /         |
                    //    | Measure Group2 |             |         \ - /           |
                    //    +----------------+             |                         V
                    //            |                      |                 +-----------------+
                    //            V                      |                 | Measure Group2' |
                    //    +----------------+             |                 +-----------------+
                    //    | Resolve StarsU |             |                         |
                    //    +----------------+             V                         V
                    //            |              +----------------+        +----------------+
                    //            V              | Resolve StarsU |        | Resolve StarsU |
                    //    +----------------+     +----------------+        +----------------+
                    //    | Measure Group3 |             |                         |
                    //    +----------------+             V                         V
                    //            |              +----------------+        +----------------+
                    //            |              | Measure Group3 |        | Measure Group3 |
                    //            |              +----------------+        +----------------+
                    //            |                      |                         |
                    //            |                      V                         V
                    //            |              +----------------+        +----------------+
                    //            |              | Resolve StarsV |        | Resolve StarsV |
                    //            |              +----------------+        +----------------+
                    //            |                      |                         |
                    //            |                      |                         V
                    //            |                      |                +------------------+
                    //            |                      |                | Measure Group2'' |
                    //            |                      |                +------------------+
                    //            |                      |                         |
                    //            +----------------------+-------------------------+
                    //                                   |
                    //                                   V
                    //                           +----------------+
                    //                           | Measure Group4 |
                    //                           +----------------+
                    //                                   |
                    //                                   V
                    //                               +--------+
                    //                               |  exit  |
                    //                               +--------+
                    //
                    //  where:
                    //  *   all [Measure GroupN] - regular children measure process -
                    //      each cell is measured given constraint size as an input
                    //      and each cell's desired size is accumulated on the
                    //      corresponding column / row;
                    //  *   [Measure Group2'] - is when each cell is measured with
                    //      infinity height as a constraint and a cell's desired
                    //      height is ignored;
                    //  *   [Measure Groups''] - is when each cell is measured (second
                    //      time during single Grid.MeasureOverride) regularly but its
                    //      returned width is ignored;
                    //
                    //  This algorithm is believed to be as close to ideal as possible.
                    //  It has the following drawbacks:
                    //  *   cells belonging to Group2 can be called to measure twice;
                    //  *   iff during second measure a cell belonging to Group2 returns
                    //      desired width greater than desired width returned the first
                    //      time, such a cell is going to be clipped, even though it
                    //      appears in Auto column.
                    //

                    MeasureCellsGroup(extData.CellGroup1, context.AvailableSize, false, false);

                    {
                        //  after Group1 is measured,  only Group3 may have cells
                        //  belonging to Auto rows.
                        bool canResolveStarsV = !HasGroup3CellsInAutoRows;

                        if (canResolveStarsV)
                        {
                            if (HasStarCellsV) { ResolveStar(DefinitionsV, context.AvailableSize.Height); }
                            MeasureCellsGroup(extData.CellGroup2, context.AvailableSize, false, false);
                            if (HasStarCellsU) { ResolveStar(DefinitionsU, context.AvailableSize.Width); }
                            MeasureCellsGroup(extData.CellGroup3, context.AvailableSize, false, false);
                        }
                        else
                        {
                            //  if at least one cell exists in Group2, it must be measured before
                            //  StarsU can be resolved.
                            bool canResolveStarsU = extData.CellGroup2 > PrivateCells.Length;
                            if (canResolveStarsU)
                            {
                                if (HasStarCellsU) { ResolveStar(DefinitionsU, context.AvailableSize.Width); }
                                MeasureCellsGroup(extData.CellGroup3, context.AvailableSize, false, false);
                                if (HasStarCellsV) { ResolveStar(DefinitionsV, context.AvailableSize.Height); }
                            }
                            else
                            {
                                // This is a revision to the algorithm employed for the cyclic
                                // dependency case described above. We now repeatedly
                                // measure Group3 and Group2 until their sizes settle. We
                                // also use a count heuristic to break a loop in case of one.

                                bool hasDesiredSizeUChanged = false;
                                int cnt = 0;

                                // Cache Group2MinWidths & Group3MinHeights
                                Coord[] group2MinSizes = CacheMinSizes(extData.CellGroup2, false);
                                Coord[] group3MinSizes = CacheMinSizes(extData.CellGroup3, true);

                                MeasureCellsGroup(extData.CellGroup2, context.AvailableSize, false, true);

                                do
                                {
                                    if (hasDesiredSizeUChanged)
                                    {
                                        // Reset cached Group3Heights
                                        ApplyCachedMinSizes(group3MinSizes, true);
                                    }

                                    if (HasStarCellsU)
                                    {
                                        ResolveStar(DefinitionsU, context.AvailableSize.Width);
                                    }

                                    MeasureCellsGroup(extData.CellGroup3, context.AvailableSize, false, false);

                                    // Reset cached Group2Widths
                                    ApplyCachedMinSizes(group2MinSizes, false);

                                    if (HasStarCellsV)
                                    {
                                        ResolveStar(DefinitionsV, context.AvailableSize.Height);
                                    }

                                    MeasureCellsGroup(
                                        extData.CellGroup2,
                                        context.AvailableSize,
                                        cnt == c_layoutLoopMaxCount,
                                        false,
                                        out hasDesiredSizeUChanged);
                                }

                                while (hasDesiredSizeUChanged && ++cnt <= c_layoutLoopMaxCount);
                            }
                        }
                    }

                    MeasureCellsGroup(extData.CellGroup4, context.AvailableSize, false, false);

                    gridDesiredSize = new SizeD(
                            CalculateDesiredSize(DefinitionsU),
                            CalculateDesiredSize(DefinitionsV));
                }
            }
            finally
            {
                MeasureOverrideInProgress = false;
            }

            return (gridDesiredSize);
        }
    }
}
