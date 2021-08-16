using System;

namespace NativeApi.Api
{
    public abstract class Brush
    {
        public string Description { get => throw new Exception(); }
        public bool IsEqualTo(Font other) => throw new Exception();
        public string Serialize() => throw new Exception();
    }
}