using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

public class Script
{
    private static void PaintLine(Graphics graph, Brush brush, int Y, int len)
    {
        graph.DrawLine(new Pen(brush, 1), new Point(0, Y), new Point(len, Y));
    }

    private static void VisualSwap(Graphics graph, int Y1, int Y2, int len1, int len2)
    {
        PaintLine(graph, SystemBrushes.Control, Y1, len1);
        PaintLine(graph, SystemBrushes.Control, Y2, len2);
        PaintLine(graph, Brushes.Red, Y1, len2);
        PaintLine(graph, Brushes.Red, Y2, len1);
    }

    public static void BubbleSort(CancellationToken ct)
    {
        int temp = 0;
        int MyStep = 3;
        int YPos1 = 0;
        int YPos2 = 0;
        int len1 = 0;
        int len2 = 0;
        Graphics graph = ScriptGlobalClass.pnBubbleSort.CreateGraphics();
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
                    VisualSwap(graph, YPos1, YPos2, len1, len2);
                }
            }
        graph.Dispose();
    }

    public static void SelectionSort(CancellationToken ct)
    {
        int temp = 0;
        int MyStep = 3;
        int YPos1 = 0;
        int YPos2 = 0;
        int len1 = 0;
        int len2 = 0;
        Graphics graph = ScriptGlobalClass.pnSelectionSort.CreateGraphics();
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
                    VisualSwap(graph, YPos1, YPos2, len1, len2);
                }
        graph.Dispose();
    }

    public static void DoQuickSort(Graphics graph, int iLo, int iHi, CancellationToken ct)
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
                VisualSwap(graph, YPos1, YPos2, len1, len2);
            }
        } while (!(lo > hi));

        if (hi > iLo)
            DoQuickSort(graph, iLo, hi, ct);
        if (lo < iHi)
            DoQuickSort(graph, lo, iHi, ct);
    }
    public static void QuickSort(CancellationToken ct)
    {
        Graphics graph = ScriptGlobalClass.pnQuickSort.CreateGraphics();
        DoQuickSort(graph, 0, ScriptGlobalClass.quickArray.Count - 1, ct);
        graph.Dispose();
    }
}