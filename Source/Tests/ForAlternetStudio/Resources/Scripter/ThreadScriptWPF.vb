Imports System
Imports System.Threading
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media

Public Class Script
    Public Shared Sub OnRenderBubble(ByVal dc As DrawingContext, ByVal bounds As Rect)
        Dim MyStep As Integer = 3

        For i As Integer = 0 To ScriptGlobalClass.bubbleArray.Count - 1
            PaintLine(dc, Brushes.Red, MyStep * (i + 1), CInt(ScriptGlobalClass.bubbleArray(i)))
        Next
    End Sub

    Public Shared Sub OnRenderSelection(ByVal dc As DrawingContext, ByVal bounds As Rect)
        Dim MyStep As Integer = 3

        For i As Integer = 0 To ScriptGlobalClass.selArray.Count - 1
            PaintLine(dc, Brushes.Red, MyStep * (i + 1), CInt(ScriptGlobalClass.selArray(i)))
        Next
    End Sub

    Public Shared Sub OnRenderQuick(ByVal dc As DrawingContext, ByVal bounds As Rect)
        Dim MyStep As Integer = 3

        For i As Integer = 0 To ScriptGlobalClass.quickArray.Count - 1
            PaintLine(dc, Brushes.Red, MyStep * (i + 1), CInt(ScriptGlobalClass.quickArray(i)))
        Next
    End Sub

    Private Shared Sub PaintLine(ByVal dc As DrawingContext, ByVal brush As Brush, ByVal Y As Integer, ByVal len As Integer)
        dc.DrawLine(New Pen(brush, 1), New Point(0, Y), New Point(len, Y))
    End Sub

    Public Shared Sub BubbleSort(ByVal ct As CancellationToken)
        Dim temp As Integer = 0
        Dim MyStep As Integer = 3
        Dim YPos1 As Integer = 0
        Dim YPos2 As Integer = 0
        Dim len1 As Integer = 0
        Dim len2 As Integer = 0

        For i As Integer = ScriptGlobalClass.bubbleArray.Count - 1 To 0 Step -1

            For j As Integer = 0 To ScriptGlobalClass.bubbleArray.Count - 2
                If ct.IsCancellationRequested Then ct.ThrowIfCancellationRequested()

                If CInt(ScriptGlobalClass.bubbleArray(j)) > CInt(ScriptGlobalClass.bubbleArray(j + 1)) Then
                    YPos1 = MyStep * (j + 1)
                    YPos2 = MyStep * (j + 2)
                    len1 = CInt(ScriptGlobalClass.bubbleArray(j))
                    len2 = CInt(ScriptGlobalClass.bubbleArray(j + 1))
                    temp = CInt(ScriptGlobalClass.bubbleArray(j))
                    ScriptGlobalClass.bubbleArray(j) = ScriptGlobalClass.bubbleArray(j + 1)
                    ScriptGlobalClass.bubbleArray(j + 1) = temp
                    ScriptGlobalClass.pnBubbleSort.Dispatcher.BeginInvoke(CType((Sub()
                                                                                     ScriptGlobalClass.pnBubbleSort.InvalidateVisual()
                                                                                 End Sub), Action))
                    Thread.Sleep(1)
                End If
            Next
        Next
    End Sub

    Public Shared Sub SelectionSort(ByVal ct As CancellationToken)
        Dim temp As Integer = 0
        Dim MyStep As Integer = 3
        Dim YPos1 As Integer = 0
        Dim YPos2 As Integer = 0
        Dim len1 As Integer = 0
        Dim len2 As Integer = 0

        For i As Integer = ScriptGlobalClass.selArray.Count - 1 To 0 Step -1

            For j As Integer = ScriptGlobalClass.selArray.Count - 1 To 0 Step -1

                If CInt(ScriptGlobalClass.selArray(i)) > CInt(ScriptGlobalClass.selArray(j)) Then
                    If ct.IsCancellationRequested Then ct.ThrowIfCancellationRequested()
                    YPos1 = MyStep * (i + 1)
                    YPos2 = MyStep * (j + 1)
                    len1 = CInt(ScriptGlobalClass.selArray(i))
                    len2 = CInt(ScriptGlobalClass.selArray(j))
                    temp = CInt(ScriptGlobalClass.selArray(i))
                    ScriptGlobalClass.selArray(i) = ScriptGlobalClass.selArray(j)
                    ScriptGlobalClass.selArray(j) = temp
                    ScriptGlobalClass.pnSelectionSort.Dispatcher.BeginInvoke(CType((Sub()
                                                                                        ScriptGlobalClass.pnSelectionSort.InvalidateVisual()
                                                                                    End Sub), Action))
                    Thread.Sleep(1)
                End If
            Next
        Next
    End Sub

    Public Shared Sub DoQuickSort(ByVal iLo As Integer, ByVal iHi As Integer, ByVal ct As CancellationToken)
        Dim temp As Integer = 0
        Dim MyStep As Integer = 3
        Dim lo As Integer = iLo
        Dim hi As Integer = iHi
        Dim YPos1 As Integer = 0
        Dim YPos2 As Integer = 0
        Dim len1 As Integer = 0
        Dim len2 As Integer = 0
        Dim mid As Integer = CInt(ScriptGlobalClass.quickArray(CInt(((lo + hi) / 2))))

        Do
            If ct.IsCancellationRequested Then ct.ThrowIfCancellationRequested()

            While CInt(ScriptGlobalClass.quickArray(lo)) < mid
                lo = lo + 1
            End While

            While CInt(ScriptGlobalClass.quickArray(hi)) > mid
                hi = hi - 1
            End While

            If lo <= hi Then
                YPos1 = MyStep * (lo + 1)
                YPos2 = MyStep * (hi + 1)
                len1 = CInt(ScriptGlobalClass.quickArray(lo))
                len2 = CInt(ScriptGlobalClass.quickArray(hi))
                temp = CInt(ScriptGlobalClass.quickArray(lo))
                ScriptGlobalClass.quickArray(lo) = ScriptGlobalClass.quickArray(hi)
                ScriptGlobalClass.quickArray(hi) = temp
                lo = lo + 1
                hi = hi - 1
                ScriptGlobalClass.pnQuickSort.Dispatcher.BeginInvoke(CType((Sub()
                                                                                ScriptGlobalClass.pnQuickSort.InvalidateVisual()
                                                                            End Sub), Action))
                Thread.Sleep(1)
            End If
        Loop While Not (lo > hi)

        If hi > iLo Then DoQuickSort(iLo, hi, ct)
        If lo < iHi Then DoQuickSort(lo, iHi, ct)
    End Sub

    Public Shared Sub QuickSort(ByVal ct As CancellationToken)
        DoQuickSort(0, ScriptGlobalClass.quickArray.Count - 1, ct)
    End Sub
End Class
