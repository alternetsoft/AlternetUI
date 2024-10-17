///<reference path="clr.d.ts" />

class Script
{
    public static BubbleSort(ct: System.Threading.CancellationToken)
    {
        let temp: number = 0;
        let MyStep: number = 3;
        let YPos1: number = 0;
        let YPos2: number = 0;
        let len1: number = 0;
        let len2: number = 0;
        for (let i: number = (bubbleArray.Count - 1); (i >= 0); i--) {
            for (let j: number = 0; (j 
                        < (bubbleArray.Count - 1)); j++) {
                if (ct.IsCancellationRequested) {
                    ct.ThrowIfCancellationRequested();
                }
                
                if (((Math.floor(bubbleArray[j])) > (Math.floor(bubbleArray[(j + 1)])))) {
                    YPos1 = (MyStep 
                                * (j + 1));
                    YPos2 = (MyStep 
                                * (j + 2));
                    len1 = (Math.floor(bubbleArray[j]));
                    len2 = (Math.floor(bubbleArray[(j + 1)]));
                    temp = (Math.floor(bubbleArray[j]));
                    bubbleArray[j] = bubbleArray[(j + 1)];
                    bubbleArray[(j + 1)] = temp;

// // @ts-ignore
                    // pnBubbleSort.Dispatcher.BeginInvoke(new System.Action(() =>
                       // {
// // @ts-ignore
                           // pnBubbleSort.InvalidateVisual();
                       // }));

                    System.Threading.Thread.Sleep(1);
                }
                
            }
            
        }
    }

    public static SelectionSort(ct: System.Threading.CancellationToken)
    {
        let temp: number = 0;
        let MyStep: number = 3;
        let YPos1: number = 0;
        let YPos2: number = 0;
        let len1: number = 0;
        let len2: number = 0;
        for (let i: number = (selArray.Count - 1); (i >= 0); i--) {
            for (let j: number = (selArray.Count - 1); (j >= 0); j--) {
                if (((Math.floor(selArray[i])) > (Math.floor(selArray[j])))) {
                    if (ct.IsCancellationRequested) {
                        ct.ThrowIfCancellationRequested();
                    }
                    
                    YPos1 = (MyStep 
                                * (i + 1));
                    YPos2 = (MyStep 
                                * (j + 1));
                    len1 = (Math.floor(selArray[i]));
                    len2 = (Math.floor(selArray[j]));
                    temp = (Math.floor(selArray[i]));
                    selArray[i] = selArray[j];
                    selArray[j] = temp;
// // @ts-ignore
                    // pnSelectionSort.Dispatcher.BeginInvoke(new System.Action(() =>
                    // {
// // @ts-ignore
                        // pnSelectionSort.InvalidateVisual();
                    // }));

                    System.Threading.Thread.Sleep(1);
                }
                
            }
            
        }
    }

    public static DoQuickSort(iLo: number, iHi: number, ct: System.Threading.CancellationToken)
    {
        let temp: number = 0;
        let MyStep: number = 3;
        let lo: number = iLo;
        let hi: number = iHi;
        let YPos1: number = 0;
        let YPos2: number = 0;
        let len1: number = 0;
        let len2: number = 0;
        let mid: number = (Math.floor(quickArray[(Math.floor(((lo + hi) 
                    / 2)))]));
        for (
        ; !(lo > hi); 
        ) {
            if (ct.IsCancellationRequested) {
                ct.ThrowIfCancellationRequested();
            }
            
            while (((Math.floor(quickArray[lo])) < mid)) {
                lo = (lo + 1);
            }
            
            while (((Math.floor(quickArray[hi])) > mid)) {
                hi = (hi - 1);
            }
            
            if ((lo <= hi)) {
                YPos1 = (MyStep 
                            * (lo + 1));
                YPos2 = (MyStep 
                            * (hi + 1));
                len1 = (Math.floor(quickArray[lo]));
                len2 = (Math.floor(quickArray[hi]));
                temp = (Math.floor(quickArray[lo]));
                quickArray[lo] = quickArray[hi];
                quickArray[hi] = temp;
                lo = (lo + 1);
                hi = (hi - 1);
// // @ts-ignore
                // pnQuickSort.Dispatcher.BeginInvoke(new System.Action(() =>
                    // {
// // @ts-ignore
                        // pnQuickSort.InvalidateVisual();
                    // }));

                System.Threading.Thread.Sleep(1);
            }
            
        }
        
        if ((hi > iLo)) {
            Script.DoQuickSort(iLo, hi, ct);
        }
        
        if ((lo < iHi)) {
            Script.DoQuickSort(lo, iHi, ct);
        }
    }

    public static QuickSort(ct: System.Threading.CancellationToken)
    {
        Script.DoQuickSort(0, quickArray.Count - 1, ct);
    }
}

function BubbleSort(ct: System.Threading.CancellationToken)
{
    Script.BubbleSort(ct);
}

function SelectionSort(ct: System.Threading.CancellationToken)
{
    Script.SelectionSort(ct);
}

function QuickSort(ct: System.Threading.CancellationToken)
{
    Script.QuickSort(ct);
}