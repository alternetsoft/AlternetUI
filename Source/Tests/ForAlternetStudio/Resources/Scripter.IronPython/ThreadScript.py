def PaintLine(graph, brush, Y, len):
  graph.DrawLine(Pen(brush, 1), Point(0, Y), Point(len, Y))

def VisualSwap(graph, Y1, Y2, len1, len2):
  PaintLine(graph, SystemBrushes.Control, Y1, len1)
  PaintLine(graph, SystemBrushes.Control, Y2, len2)
  PaintLine(graph, Brushes.Red, Y1, len2)
  PaintLine(graph, Brushes.Red, Y2, len1)

def BubbleSort(ct):
  temp = 0
  MyStep = 3
  YPos1 = 0
  YPos2 = 0
  len1 = 0
  len2 = 0
  graph = pnBubbleSort.CreateGraphics()
  i = bubbleArray.Count - 1
  while i >= 0:
    j = 0
    while j < bubbleArray.Count - 1:
      if ct.IsCancellationRequested:
        ct.ThrowIfCancellationRequested()

      if bubbleArray[j] > bubbleArray[j + 1]:
        YPos1 = MyStep * (j + 1)
        YPos2 = MyStep * (j + 2)
        len1 = bubbleArray[j]
        len2 = bubbleArray[j + 1]
        temp = bubbleArray[j]
        bubbleArray[j] = bubbleArray[j + 1]
        bubbleArray[j + 1] = temp
        VisualSwap(graph, YPos1, YPos2, len1, len2)
      j = j + 1 
    i = i - 1

  graph.Dispose()

def SelectionSort(ct):
  temp = 0
  MyStep = 3
  YPos1 = 0
  YPos2 = 0
  len1 = 0
  len2 = 0
  graph = pnSelectionSort.CreateGraphics()
  i = selArray.Count - 1
  while  i >=0:
    j = selArray.Count - 1
    while j >=0:
      if selArray[i] > selArray[j]:
        if ct.IsCancellationRequested:
          ct.ThrowIfCancellationRequested()

        YPos1 = MyStep * (i + 1)
        YPos2 = MyStep * (j + 1)
        len1 = selArray[i]
        len2 = selArray[j]
        temp = selArray[i]
        selArray[i] = selArray[j]
        selArray[j] = temp
        VisualSwap(graph, YPos1, YPos2, len1, len2)
      j = j - 1
    i = i - 1
  graph.Dispose()

def DoQuickSort(graph, iLo, iHi, ct):
  temp = 0
  MyStep = 3
  lo = iLo
  hi = iHi
  YPos1 = 0
  YPos2 = 0
  len1 = 0
  len2 = 0
  mid = quickArray[int(((lo + hi) / 2))]
  while lo <= hi:
    if ct.IsCancellationRequested:
        ct.ThrowIfCancellationRequested()

    while quickArray[lo] < mid:
      lo = lo + 1

    while quickArray[hi] > mid:
      hi = hi - 1

    if lo <= hi:
      YPos1 = MyStep * (lo + 1)
      YPos2 = MyStep * (hi + 1)
      len1 = quickArray[lo]
      len2 = quickArray[hi]
      temp = quickArray[lo]
      quickArray[lo] = quickArray[hi]
      quickArray[hi] = temp
      lo = lo + 1
      hi = hi - 1
      VisualSwap(graph, YPos1, YPos2, len1, len2) 

  if hi > iLo:
      DoQuickSort(graph, iLo, hi, ct)
  if lo < iHi:
      DoQuickSort(graph, lo, iHi, ct)


def QuickSort(ct):
  graph = pnQuickSort.CreateGraphics()
  DoQuickSort(graph, 0, quickArray.Count - 1, ct)
  graph.Dispose()
