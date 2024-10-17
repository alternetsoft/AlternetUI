def OnRenderBubble(dc, bounds):
  MyStep = 3
  i = 0
  while i < bubbleArray.Count:
    PaintLine(dc, System.Windows.Media.Brushes.Red, MyStep * (i + 1), bubbleArray[i])
    i = i + 1

def OnRenderSelection(dc, bounds):
  MyStep = 3
  i = 0
  while i < selArray.Count:
    PaintLine(dc, System.Windows.Media.Brushes.Red, MyStep * (i + 1), selArray[i])
    i = i + 1

def OnRenderQuick(dc, bounds):
  MyStep = 3
  i = 0
  while i < quickArray.Count:
    PaintLine(dc, System.Windows.Media.Brushes.Red, MyStep * (i + 1), quickArray[i])
    i = i + 1

def PaintLine(dc, brush, Y, len):
  dc.DrawLine(System.Windows.Media.Pen(brush, 1), Point(0, Y), Point(len, Y))

def BubbleSort(ct):
  temp = 0
  MyStep = 3
  YPos1 = 0
  YPos2 = 0
  len1 = 0
  len2 = 0
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
        pnBubbleSort.Dispatcher.BeginInvoke(Action(lambda: pnBubbleSort.InvalidateVisual()))
        System.Threading.Thread.Sleep(1)

      j = j + 1 
    i = i - 1

def SelectionSort(ct):
  temp = 0
  MyStep = 3
  YPos1 = 0
  YPos2 = 0
  len1 = 0
  len2 = 0
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
        pnSelectionSort.Dispatcher.BeginInvoke(Action(lambda: pnSelectionSort.InvalidateVisual()))
        System.Threading.Thread.Sleep(1)

      j = j - 1
    i = i - 1

def DoQuickSort(iLo, iHi, ct):
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
      pnQuickSort.Dispatcher.BeginInvoke(Action(lambda: pnQuickSort.InvalidateVisual()))
      System.Threading.Thread.Sleep(1)

  if hi > iLo:
      DoQuickSort(iLo, hi, ct)
  if lo < iHi:
      DoQuickSort(lo, iHi, ct)


def QuickSort(ct):
  DoQuickSort(0, quickArray.Count - 1, ct)
