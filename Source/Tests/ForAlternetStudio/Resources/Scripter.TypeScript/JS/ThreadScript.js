///<reference path="clr.d.ts" />

function PaintLine(graph, brush, Y, len) {
	graph.DrawLine(new System.Drawing.Pen(brush, 1), new System.Drawing.Point(0, Y), new System.Drawing.Point(len, Y));
}

function VisualSwap(graph, Y1, Y2, len1, len2) {
	PaintLine(graph, System.Drawing.SystemBrushes.Control, Y1, len1);
	PaintLine(graph, System.Drawing.SystemBrushes.Control, Y2, len2);
	PaintLine(graph, System.Drawing.Brushes.Red, Y1, len2);
	PaintLine(graph, System.Drawing.Brushes.Red, Y2, len1);
}

function BubbleSort(ct) {
	var temp = 0;
	var MyStep = 3;
	var YPos1 = 0;
	var YPos2 = 0;
	var len1 = 0;
	var len2 = 0;
	var graph = pnBubbleSort.CreateGraphics();
	for (var i = (bubbleArray.Count - 1); (i >= 0); i--) {
		for (var j = 0; (j 
					< (bubbleArray.Count - 1)); j++) {
			if (ct.IsCancellationRequested) {
				ct.ThrowIfCancellationRequested();
			}
			
			if ((((bubbleArray[j])) > ((bubbleArray[(j + 1)])))) {
				YPos1 = (MyStep 
							* (j + 1));
				YPos2 = (MyStep 
							* (j + 2));
				len1 = ((bubbleArray[j]));
				len2 = ((bubbleArray[(j + 1)]));
				temp = ((bubbleArray[j]));
				bubbleArray[j] = bubbleArray[(j + 1)];
				bubbleArray[(j + 1)] = temp;
				VisualSwap(graph, YPos1, YPos2, len1, len2);
			}
			
		}
		
	}
	
	graph.Dispose();
}

function SelectionSort(ct) {
	var temp = 0;
	var MyStep = 3;
	var YPos1 = 0;
	var YPos2 = 0;
	var len1 = 0;
	var len2 = 0;
	var graph = pnSelectionSort.CreateGraphics();
	for (var i = (selArray.Count - 1); (i >= 0); i--) {
		for (var j = (selArray.Count - 1); (j >= 0); j--) {
			if ((((selArray[i])) > ((selArray[j])))) {
				if (ct.IsCancellationRequested) {
					ct.ThrowIfCancellationRequested();
				}
				
				YPos1 = (MyStep 
							* (i + 1));
				YPos2 = (MyStep 
							* (j + 1));
				len1 = ((selArray[i]));
				len2 = ((selArray[j]));
				temp = ((selArray[i]));
				selArray[i] = selArray[j];
				selArray[j] = temp;
				VisualSwap(graph, YPos1, YPos2, len1, len2);
			}
			
		}
		
	}
	
	graph.Dispose();
}

function DoQuickSort(graph, iLo, iHi, ct) {
	var temp = 0;
	var MyStep = 3;
	var lo = iLo;
	var hi = iHi;
	var YPos1 = 0;
	var YPos2 = 0;
	var len1 = 0;
	var len2 = 0;
	var mid = (Math.floor(quickArray[(Math.floor(((lo + hi) 
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
			VisualSwap(graph, YPos1, YPos2, len1, len2);
		}
		
	}
	
	if ((hi > iLo)) {
		DoQuickSort(graph, iLo, hi, ct);
	}
	
	if ((lo < iHi)) {
		DoQuickSort(graph, lo, iHi, ct);
	}
}

function QuickSort(ct) {
	var graph = pnQuickSort.CreateGraphics();
	DoQuickSort(graph, 0, (quickArray.Count - 1), ct);
	graph.Dispose();
}
