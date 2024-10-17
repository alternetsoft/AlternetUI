using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public class Script
{

    public static void OnRenderBubble(DrawingContext dc, Rect bounds)
    {
        int MyStep = 3;

        for (int i = 0; i < ScriptGlobalClass.bubbleArray.Count; i++)
        {
            PaintLine(dc, Brushes.Red, MyStep * (i + 1), (int)ScriptGlobalClass.bubbleArray[i]);
        }
    }

    public static void OnRenderSelection(DrawingContext dc, Rect bounds)
    {
        int MyStep = 3;

        for (int i = 0; i < ScriptGlobalClass.selArray.Count; i++)
        {
            PaintLine(dc, Brushes.Red, MyStep * (i + 1), (int)ScriptGlobalClass.selArray[i]);
        }
    }

    public static void OnRenderQuick(DrawingContext dc, Rect bounds)
    {
        int MyStep = 3;

        for (int i = 0; i < ScriptGlobalClass.quickArray.Count; i++)
        {
            PaintLine(dc, Brushes.Red, MyStep * (i + 1), (int)ScriptGlobalClass.quickArray[i]);
        }
    }

    private static void PaintLine(DrawingContext dc, Brush brush, int Y, int len)
    {
        dc.DrawLine(new Pen(brush, 1), new Point(0, Y), new Point(len, Y));
    }

    public static void BubbleSort(CancellationToken ct)
    {
        int temp = 0;
        int MyStep = 3;
        int YPos1 = 0;
        int YPos2 = 0;
        int len1 = 0;
        int len2 = 0;
        for (int i = ScriptGlobalClass.bubbleArray.Count - 1; i >= 0; i--)
            for (int j = 0; j < ScriptGlobalClass.bubbleArray.Count - 1; j++)
            {
                if (ct.IsCancellationRequested)
                    ct.ThrowIfCancellationRequested();

                if ((int)ScriptGlobalClass.bubbleArray[j] > (int)ScriptGlobalClass.bubbleArray[j + 1])
                {
                    YPos1 = MyStep * (j + 1);
                    YPos2 = MyStep * (j + 2);
                    len1 = (int)ScriptGlobalClass.bubbleArray[j];
                    len2 = (int)ScriptGlobalClass.bubbleArray[j + 1];
                    temp = (int)ScriptGlobalClass.bubbleArray[j];
                    ScriptGlobalClass.bubbleArray[j] = ScriptGlobalClass.bubbleArray[j + 1];
                    ScriptGlobalClass.bubbleArray[j + 1] = temp;
                    ScriptGlobalClass.pnBubbleSort.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            ScriptGlobalClass.pnBubbleSort.InvalidateVisual();
                        }));

                    Thread.Sleep(1);
                }
            }
    }

    public static void SelectionSort(CancellationToken ct)
    {
        int temp = 0;
        int MyStep = 3;
        int YPos1 = 0;
        int YPos2 = 0;
        int len1 = 0;
        int len2 = 0;
        for (int i = ScriptGlobalClass.selArray.Count - 1; i >= 0; i--)
            for (int j = ScriptGlobalClass.selArray.Count - 1; j >= 0; j--)
                if ((int)ScriptGlobalClass.selArray[i] > (int)ScriptGlobalClass.selArray[j])
                {
                    if (ct.IsCancellationRequested)
                        ct.ThrowIfCancellationRequested();

                    YPos1 = MyStep * (i + 1);
                    YPos2 = MyStep * (j + 1);
                    len1 = (int)ScriptGlobalClass.selArray[i];
                    len2 = (int)ScriptGlobalClass.selArray[j];
                    temp = (int)ScriptGlobalClass.selArray[i];
                    ScriptGlobalClass.selArray[i] = ScriptGlobalClass.selArray[j];
                    ScriptGlobalClass.selArray[j] = temp;
                    ScriptGlobalClass.pnSelectionSort.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        ScriptGlobalClass.pnSelectionSort.InvalidateVisual();
                    }));

                    Thread.Sleep(1);
                }
    }

    public static void DoQuickSort(int iLo, int iHi, CancellationToken ct)
    {
        int temp = 0;
        int MyStep = 3;
        int lo = iLo;
        int hi = iHi;
        int YPos1 = 0;
        int YPos2 = 0;
        int len1 = 0;
        int len2 = 0;
        int mid = (int)ScriptGlobalClass.quickArray[(int)((lo + hi) / 2)];
        do
        {
            if (ct.IsCancellationRequested)
                ct.ThrowIfCancellationRequested();

            while ((int)ScriptGlobalClass.quickArray[lo] < mid)
            {
                lo = lo + 1;
            }
            while ((int)ScriptGlobalClass.quickArray[hi] > mid)
            {
                hi = hi - 1;
            }

            if (lo <= hi)
            {
                YPos1 = MyStep * (lo + 1);
                YPos2 = MyStep * (hi + 1);
                len1 = (int)ScriptGlobalClass.quickArray[lo];
                len2 = (int)ScriptGlobalClass.quickArray[hi];
                temp = (int)ScriptGlobalClass.quickArray[lo];
                ScriptGlobalClass.quickArray[lo] = ScriptGlobalClass.quickArray[hi];
                ScriptGlobalClass.quickArray[hi] = temp;
                lo = lo + 1;
                hi = hi - 1;
                ScriptGlobalClass.pnQuickSort.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        ScriptGlobalClass.pnQuickSort.InvalidateVisual();
                    }));

                Thread.Sleep(1);
            }
        } while (!(lo > hi));

        if (hi > iLo)
            DoQuickSort(iLo, hi, ct);
        if (lo < iHi)
            DoQuickSort(lo, iHi, ct);
    }

    public static void QuickSort(CancellationToken ct)
    {
        DoQuickSort(0, ScriptGlobalClass.quickArray.Count - 1, ct);
    }
}