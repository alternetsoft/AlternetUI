using System;

namespace NativeApi.Api
{
    public class PrinterSettings
    {
        public Duplex Duplex { get => throw new Exception(); set => throw new Exception(); }

        public int FromPage { get => throw new Exception(); set => throw new Exception(); }

        public int ToPage { get => throw new Exception(); set => throw new Exception(); }

        public int MinimumPage { get => throw new Exception(); set => throw new Exception(); }

        public int MaximumPage { get => throw new Exception(); set => throw new Exception(); }

        public PrintRange PrintRange { get => throw new Exception(); set => throw new Exception(); }

        public bool Collate { get => throw new Exception(); set => throw new Exception(); }

        public int Copies { get => throw new Exception(); set => throw new Exception(); }

        public bool PrintToFile { get => throw new Exception(); set => throw new Exception(); }

        public string? PrinterName { get => throw new Exception(); set => throw new Exception(); }

        public bool IsValid { get => throw new Exception(); }

        public bool IsDefaultPrinter { get => throw new Exception(); }

        public string? PrintFileName { get => throw new Exception(); set => throw new Exception(); }
    }
}