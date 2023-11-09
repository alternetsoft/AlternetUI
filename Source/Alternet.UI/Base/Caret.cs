using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class Caret : DisposableObject
    {
        protected override void DisposeUnmanagedResources()
        {
            Native.WxOtherFactory.DeleteCaret(Handle);
        }
    }
}

/*
// Get the caret position(in pixels).
public static Int32Point CaretGetPosition(IntPtr handle) => default;

// Get the caret size.
public static Int32Size CaretGetSize(IntPtr handle) => default;

// Move the caret to given position(in logical coordinates).
public static void CaretMove(IntPtr handle, int x, int y) { }

// Changes the size of the caret.
public static void CaretSetSize(IntPtr handle, int width, int height) { }

public static IntPtr CreateCaret() => default;

// Creates a caret with the given size(in pixels) and associates it with the window.
public static IntPtr CreateCaret2(IntPtr window, int width, int height) => default;

// Get the window the caret is associated with.
public static IntPtr CaretGetWindow(IntPtr handle) => default;

// Hides the caret, same as Show(false).
public static void CaretHide(IntPtr handle) { }

// Returns true if the caret was created successfully.
public static bool CaretIsOk(IntPtr handle) => default;

// Returns true if the caret is visible and false if it
// is permanently hidden(if it is blinking and not shown
// currently but will be after the next blink, this method still returns true).
public static bool CaretIsVisible(IntPtr handle) => default;

// Shows or hides the caret.
public static void CaretShow(IntPtr handle, bool show = true) { }
*/