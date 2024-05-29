#pragma warning disable
#nullable disable

namespace Alternet.UI
{
    internal static class BooleanBoxes
    {
        internal static object TrueBox = true;
        internal static object FalseBox = false;

        internal static object Box(bool value)
        {
            if (value)
            {
                return TrueBox;
            }
            else
            {
                return FalseBox;
            }
        }
    }

    internal static class NullableBooleanBoxes
    {
        internal static object TrueBox = (bool?)true;
        internal static object FalseBox = (bool?)false;
        internal static object NullBox = (bool?)null;

        internal static object Box(bool? value)
        {
            if (value.HasValue)
            {
                if (value == true)
                {
                    return TrueBox;
                }
                else
                {
                    return FalseBox;
                }
            }
            else
            {
                return NullBox;
            }
        }
    }
}