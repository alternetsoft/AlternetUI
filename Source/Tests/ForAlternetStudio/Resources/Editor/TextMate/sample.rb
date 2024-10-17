def partition(items, left, right, pindex)
   pvalue = items [pindex]
   swap(items, pindex, right)
   sindex = left
   for i in left .. right-1
       if items[i] <= pvalue
          swap(items, sindex, i)
          sindex = sindex + 1
       end
   end
 
   swap(items, right, sindex)
   return sindex
end
 
def swap (arr, l, r)
  tmp = arr [l]
  arr[l] = arr[r]
  arr[r] = tmp
end
 
def qsort(items, left, right)
	if (right > left)
	    pIndex = left
	    newPindex = partition(items, left, right, pIndex)
	    qsort(items, left, newPindex-1)
	    qsort(items, newPindex+1, right)
	end
 
end
 
def quicksort(items) 
  qsort(items, 0, items.size - 1)
end