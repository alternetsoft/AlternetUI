using System;

namespace NativeApi.Api
{
    public class ListView : Control
    {
        public int ItemsCount { get; }

        public ImageList? SmallImageList { get; set; }
        
        public ImageList? LargeImageList { get; set; }

        public void InsertItemAt(int index, string text, int columnIndex, int imageIndex) => throw new Exception();

        public void RemoveItemAt(int index) => throw new Exception();

        public void ClearItems() => throw new Exception();

        public void InsertColumnAt(int index, string header) => throw new Exception();
        
        public void RemoveColumnAt(int index) => throw new Exception();

        public ListViewView CurrentView { get; set; }
    }
}