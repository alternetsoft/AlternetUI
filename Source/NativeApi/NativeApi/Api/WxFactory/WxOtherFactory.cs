#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.Drawing;
using ApiCommon;
using NativeApi.Api.ManagedServers;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_rich_tool_tip.html
    // https://docs.wxwidgets.org/3.2/classwx_tool_tip.html
    // https://docs.wxwidgets.org/3.2/classwx_display.html
    // https://docs.wxwidgets.org/3.2/classwx_caret.html
    // https://docs.wxwidgets.org/3.2/classwx_cursor.html
    // https://docs.wxwidgets.org/3.2/classwx_renderer_native.html
    // https://docs.wxwidgets.org/3.2/classwx_file_system_watcher.html
    public class WxOtherFactory
    {
        // =================== Tests

        public static void TestPopupWindow(IntPtr parent, PointI pos, SizeI sz) { }

        // =================== ToolTip

        public static IntPtr CreateToolTip(string tip) => default;
        public static void DeleteToolTip(IntPtr handle) { }
        public static string ToolTipGetTip(IntPtr handle) => default;
        public static IntPtr ToolTipGetWindow(IntPtr handle) => default;
        public static void ToolTipSetTip(IntPtr handle, string tip) { }

        // Enable or disable tooltips globally. 
        // May not be supported on all platforms(eg. wxCocoa).
        public static void ToolTipEnable(bool flag) { }

        // Set the delay after which the tooltip disappears or how long a tooltip remains visible. 	
        // May not be supported on all platforms(eg. wxCocoa, GTK).
        public static void ToolTipSetAutoPop(long msecs) { }

        // Set the delay after which the tooltip appears. 
        // May not be supported on all platforms.
        public static void ToolTipSetDelay(long msecs) { }

        // Set tooltip maximal width in pixels. By default, tooltips are wrapped at a suitably
        // chosen width. You can pass -1 as width to disable wrapping them completely,
        // 0 to restore the default behaviour or an arbitrary positive value to wrap
        // them at the given width. Notice that this function does not change the width of
        // the tooltips created before calling it. Currently this function is wxMSW-only.
        public static void ToolTipSetMaxWidth(int width) { }

        // Set the delay between subsequent tooltips to appear. 
        public static void ToolTipSetReshow(long msecs) { }

        // =================== Cursor

        public static IntPtr CreateCursor() => default;

        public static IntPtr CreateCursor2(int cursorId) => default; // wxStockCursor     

        public static IntPtr CreateCursor3(string cursorName, int type,
            int hotSpotX = 0, int hotSpotY = 0) => default; // =wxCURSOR_DEFAULT_TYPE

        public static IntPtr CreateCursor4(Image image, int hotSpotX = 0, int hotSpotY = 0) => default;

        public static IntPtr CreateCursor5(IntPtr image, int hotSpotX = 0, int hotSpotY = 0) => default;

        public static void DeleteCursor(IntPtr handle) { }

        // Returns true if cursor data is present. 
        public static bool CursorIsOk(IntPtr handle) => default;

        public static PointI CursorGetHotSpot(IntPtr handle) => default;

        public static void SetCursor(IntPtr handle) { }

        // =================== Caret

        // blink time is measured in milliseconds and is the time elapsed
        // between 2 inversions of the caret (blink time of the caret is common
        // to all carets in the application, so these functions are static)
        public static int CaretGetBlinkTime() => default;
        public static void CaretSetBlinkTime(int milliseconds) { }

        public static void DeleteCaret(IntPtr handle) { }

        // Get the caret position(in pixels).
        public static PointI CaretGetPosition(IntPtr handle) => default;

        // Get the caret size.
        public static SizeI CaretGetSize(IntPtr handle) => default;

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

        // =================== Display

        // Defaultructor creating display object representing the primary display.
        public static IntPtr CreateDisplay() => default;

        //ructor, setting up a wxDisplay instance with the specified display.
        public static IntPtr CreateDisplay2(uint index) => default;

        //ructor creating the display object associated with the given window.
        public static IntPtr CreateDisplay3(IntPtr window) => default;

        public static void DeleteDisplay(IntPtr handle) { }

        // Returns the number of connected displays.
        public static uint DisplayGetCount() => default;

        public static bool DisplayIsOk(IntPtr handle) => default;

        // Returns the index of the display on which the given point lies,
        // or -1 if the point is not on any connected display.
        public static int DisplayGetFromPoint(PointI pt) => default;

        // Returns the index of the display on which the given window lies.
        public static int DisplayGetFromWindow(IntPtr win) => default;

        // Returns default display resolution for the current platform in pixels per inch. 
        public static int DisplayGetStdPPIValue() => default;

        // Returns default display resolution for the current platform as wxSize. 
        public static SizeI DisplayGetStdPPI() => default;

        // Returns the display's name.
        public static string DisplayGetName(IntPtr handle) => default;

        // Returns display resolution in pixels per inch.
        public static SizeI DisplayGetPPI(IntPtr handle) => default;

        // Returns scaling factor used by this display. 
        public static double DisplayGetScaleFactor(IntPtr handle) => default;

        // Returns true if the display is the primary display. 
        public static bool DisplayIsPrimary(IntPtr handle) => default;

        // Returns the client area of the display.
        public static RectI DisplayGetClientArea(IntPtr handle) => default;

        // Returns the bounding rectangle of the display
        public static RectI DisplayGetGeometry(IntPtr handle) => default;

        // =================== SystemSettings

        // Returns true if the port has certain feature.
        public static bool SystemSettingsHasFeature(int index) => default;

        // get a standard system font
        public static Font SystemSettingsGetFont(int index) => default;

        // Gets a standard system color
        public static Color SystemSettingsGetColor(int index) => default;

        // Gets a system-dependent metric.
        public static int SystemSettingsGetMetric(int index, IntPtr win = default) => default;

        // Returns the name if available or empty string otherwise.
        public static string SystemAppearanceGetName() => default;

        // Return true if the current system there is explicitly recognized as
        // being a dark theme or if the default window background is dark.
        public static bool SystemAppearanceIsDark() => default;

        // Return true if the background is darker than foreground. This is used by
        // IsDark() if there is no platform-specific way to determine whether a
        // dark mode is being used.
        public static bool SystemAppearanceIsUsingDarkBackground() => default;

        // ===================

        public static bool IsBusyCursor() => default;

        public static void BeginBusyCursor() { }

        public static void EndBusyCursor() { }

        // Ring the system bell.
        // This function is categorized as a GUI one and so is not thread-safe.
        // #include <wx/utils.h> 
        public static void Bell() { }

        public static string GetTextFromUser(string message, string caption, string defaultValue,
            IntPtr parent, int x = -1, int y = -1, bool centre = true) => default;

        public static long GetNumberFromUser(string message, string prompt, string caption,
            long value, long min, long max, IntPtr parent, PointI pos) => default;

        // ===================

        public static int RendererDrawHeaderButton(IntPtr renderer, IntPtr win,
            DrawingContext dc,
            RectI rect,
            int flags /*= 0*/,
            int sortArrow /*= wxHDR_SORT_ICON_NONE*/,
            IntPtr headerButtonParams /*= NULL*/) => default;

        // Draw the contents of a header control button (label, sort arrows, etc.)
        // Normally only called by DrawHeaderButton.
        public static int RendererDrawHeaderButtonContents(IntPtr renderer, IntPtr win,
            DrawingContext dc,
            RectI rect,
            int flags /*= 0*/,
            int sortArrow /*= wxHDR_SORT_ICON_NONE*/,
            IntPtr headerButtonParams /*= NULL*/) => default;

        // Returns the default height of a header button, either a fixed platform
        // height if available, or a generic height based on the window's font.
        public static int RendererGetHeaderButtonHeight(IntPtr renderer, IntPtr win) => default;

        // Returns the margin on left and right sides of header button's label
        public static int RendererGetHeaderButtonMargin(IntPtr renderer, IntPtr win) => default;

        // draw the expanded/collapsed icon for a tree control item
        public static void RendererDrawTreeItemButton(IntPtr renderer, IntPtr win,
            DrawingContext dc, RectI rect, int flags = 0) { }

        // draw the border for sash window: this border must be such that the sash
        // drawn by DrawSash() blends into it well
        public static void RendererDrawSplitterBorder(IntPtr renderer, IntPtr win,
            DrawingContext dc, RectI rect, int flags = 0) { }

        // draw a (vertical) sash
        public static void RendererDrawSplitterSash(IntPtr renderer, IntPtr win,
            DrawingContext dcReal,
            SizeI sizeReal,
            int position,
            int orientation,
            int flags = 0)
        {
        }

        // draw a combobox dropdown button
        // flags may use wxCONTROL_PRESSED and wxCONTROL_CURRENT
        public static void RendererDrawComboBoxDropButton(IntPtr renderer, IntPtr win,
            DrawingContext dc,
            RectI rect,
            int flags = 0)
        {
        }

        // draw a dropdown arrow
        // flags may use wxCONTROL_PRESSED and wxCONTROL_CURRENT
        public static void RendererDrawDropArrow(IntPtr renderer, IntPtr win,
            DrawingContext dc, RectI rect, int flags = 0)
        {
        }

        // draw check button
        // flags may use wxCONTROL_CHECKED, wxCONTROL_UNDETERMINED and wxCONTROL_CURRENT
        public static void RendererDrawCheckBox(IntPtr renderer, IntPtr win,
            DrawingContext dc,
            RectI rect,
            int flags = 0)
        {
        }

        // draw check mark
        // flags may use wxCONTROL_DISABLED
        public static void RendererDrawCheckMark(IntPtr renderer, IntPtr win,
            DrawingContext dc,
            RectI rect,
            int flags = 0)
        {
        }

        // Returns the default size of a check box.
        public static SizeI RendererGetCheckBoxSize(IntPtr renderer, IntPtr win, int flags = 0) => default;

        // Returns the default size of a check mark.
        public static SizeI RendererGetCheckMarkSize(IntPtr renderer, IntPtr win) => default;

        // Returns the default size of a expander.
        public static SizeI RendererGetExpanderSize(IntPtr renderer, IntPtr win) => default;

        // draw blank button
        //
        // flags may use wxCONTROL_PRESSED, wxCONTROL_CURRENT and wxCONTROL_ISDEFAULT
        public static void RendererDrawPushButton(IntPtr renderer, IntPtr win,
            DrawingContext dc,
            RectI rect,
            int flags = 0)
        {
        }

        // draw collapse button
        //
        // flags may use wxCONTROL_CHECKED, wxCONTROL_UNDETERMINED and wxCONTROL_CURRENT
        public static void RendererDrawCollapseButton(IntPtr renderer, IntPtr win,
            DrawingContext dc,
            RectI rect,
            int flags = 0)
        {
        }

        // Returns the default size of a collapse button
        public static SizeI RendererGetCollapseButtonSize(IntPtr renderer, IntPtr win,
            DrawingContext dc) => default;

        // draw rectangle indicating that an item in e.g. a list control
        // has been selected or focused
        //
        // flags may use
        // wxCONTROL_SELECTED (item is selected, e.g. draw background)
        // wxCONTROL_CURRENT (item is the current item, e.g. dotted border)
        // wxCONTROL_FOCUSED (the whole control has focus, e.g. blue background vs. grey otherwise)
        public static void RendererDrawItemSelectionRect(IntPtr renderer, IntPtr win,
            DrawingContext dc,
                RectI rect,
            int flags = 0)
        {
        }

        // draw the focus rectangle around the label contained in the given rect
        //
        // only wxCONTROL_SELECTED makes sense in flags here
        public static void RendererDrawFocusRect(IntPtr renderer, IntPtr win,
            DrawingContext dc,
                RectI rect,
            int flags = 0)
        {
        }

        // Draw a native wxChoice
        public static void RendererDrawChoice(IntPtr renderer, IntPtr win,
            DrawingContext dc,
                RectI rect,
            int flags = 0)
        {
        }

        // Draw a native wxComboBox
        public static void RendererDrawComboBox(IntPtr renderer, IntPtr win,
            DrawingContext dc,
                RectI rect,
            int flags = 0)
        {
        }

        // Draw a native wxTextCtrl frame
        public static void RendererDrawTextCtrl(IntPtr renderer, IntPtr win,
            DrawingContext dc,
                RectI rect,
            int flags = 0)
        {
        }

        // Draw a native wxRadioButton bitmap
        public static void RendererDrawRadioBitmap(IntPtr renderer, IntPtr win,
            DrawingContext dc,
                RectI rect,
            int flags = 0)
        {
        }

        // Draw a gauge with native style like a wxGauge would display.
        // wxCONTROL_SPECIAL flag must be used for drawing vertical gauges.
        public static void RendererDrawGauge(IntPtr renderer, IntPtr win,
            DrawingContext dc,
                RectI rect,
            int value,
            int max,
            int flags = 0)
        {
        }

        // Draw text using the appropriate color for normal and selected states.
        public static void RendererDrawItemText(IntPtr renderer, IntPtr win,
            DrawingContext dc,
            string text,
            RectI rect,
            int align /*= wxALIGN_LEFT | wxALIGN_TOP*/,
            int flags /*= 0*/,
            int ellipsizeMode /*= wxELLIPSIZE_END*/)
        { }

        public static string RendererGetVersion(IntPtr renderer) => default;

        // ===================

        /*
        Allocates size bytes of uninitialized storage.

        If allocation succeeds, returns a pointer to the lowest (first) byte in the allocated memory block that is suitably aligned for any scalar type (at least as strictly as std::max_align_t) (implicitly creating objects in the destination area).

        If size is zero, the behavior is implementation defined (null pointer may be returned, or some non-null pointer may be returned that may not be used to access storage, but has to be passed to std::free).

        Parameters
        size	-	number of bytes to allocate
        Return value
        On success, returns the pointer to the beginning of newly allocated memory. To avoid a memory leak, the returned pointer must be deallocated with std::free() or std::realloc().

        On failure, returns a null pointer.

        Notes
        This function does not call constructors or initialize memory in any way. There are no ready-to-use smart pointers that could guarantee that the matching deallocation function is called. The preferred method of memory allocation in C++ is using RAII-ready functions std::make_unique, std::make_shared, container constructors, etc, and, in low-level library code, new-expression. 
         */
        public static IntPtr MemoryAlloc(ulong size) => default; // malloc

        /*
        Defined in header <cstdlib>		
        void* realloc( void* ptr, std::size_t new_size );		
        Reallocates the given area of memory (implicitly creating objects in the destination area). It must be previously allocated by std::malloc, std::calloc or std::realloc and not yet freed with std::free, otherwise, the results are undefined.

        The reallocation is done by either:

        a) expanding or contracting the existing area pointed to by ptr, if possible. The contents of the area remain unchanged up to the lesser of the new and old sizes. If the area is expanded, the contents of the new part of the array are undefined.
        b) allocating a new memory block of size new_size bytes, copying memory area with size equal the lesser of the new and the old sizes, and freeing the old block.
        If there is not enough memory, the old memory block is not freed and null pointer is returned.

        If ptr is a null pointer, the behavior is the same as calling std::malloc(new_size).

        If new_size is zero, the behavior is implementation defined: null pointer may be returned (in which case the old memory block may or may not be freed) or some non-null pointer may be returned that may not be used to access storage. Such usage is deprecated (via C DR 400). 
         */

        public static IntPtr MemoryRealloc(IntPtr memory, ulong newSize) => default;

        /*
        Deallocates the space previously allocated by std::malloc, std::calloc, std::aligned_alloc(since C++17), or std::realloc.

        If ptr is a null pointer, the function does nothing.

        The behavior is undefined if the value of ptr does not equal a value returned earlier by std::malloc, std::calloc, std::aligned_alloc(since C++17), or std::realloc.

        The behavior is undefined if the memory area referred to by ptr has already been deallocated, that is, std::free or std::realloc has already been called with ptr as the argument and no calls to std::malloc, std::calloc, std::aligned_alloc(since C++17), or std::realloc resulted in a pointer equal to ptr afterwards.

        The behavior is undefined if after std::free returns, an access is made through the pointer ptr (unless another allocation function happened to result in a pointer value equal to ptr). 
         */
        public static void MemoryFree(IntPtr memory) { } // free

        /*
void* memcpy( void* dest, const void* src, std::size_t count );
Copies count bytes from the object pointed to by src to the object pointed to by dest. Both objects are reinterpreted as arrays of unsigned char.

If the objects overlap, the behavior is undefined.

If either dest or src is an invalid or null pointer, the behavior is undefined, even if count is zero.

If the objects are potentially-overlapping or not TriviallyCopyable, the behavior of memcpy is not specified and may be undefined.

Parameters
dest	-	pointer to the memory location to copy to
src	-	pointer to the memory location to copy from
count	-	number of bytes to copy
Return value
dest         
         */
        public static IntPtr MemoryCopy(IntPtr dest, IntPtr src, ulong count) => default;

        /*
Defined in header <cstring>
void* memmove( void* dest, const void* src, std::size_t count );
Copies count characters from the object pointed to by src to the object pointed to by dest. Both objects are reinterpreted as arrays of unsigned char.

The objects may overlap: copying takes place as if the characters were copied to a temporary character array and then the characters were copied from the array to dest.

If either dest or src is an invalid or null pointer, the behavior is undefined, even if count is zero.

If the objects are potentially-overlapping or not TriviallyCopyable, the behavior of memmove is not specified and may be undefined.

Parameters
dest	-	pointer to the memory location to copy to
src	-	pointer to the memory location to copy from
count	-	number of bytes to copy
Return value
dest
        */
        public static IntPtr MemoryMove(IntPtr dest, IntPtr src, ulong count) => default;

        /* 
        void* memset( void* dest, int ch, std::size_t count );
        Copies the value static_cast<unsigned char>(ch) into each of the first count characters of the object pointed to by dest. If the object is a potentially-overlapping subobject or is not TriviallyCopyable (e.g., scalar, C-compatible struct, or an array of trivially copyable type), the behavior is undefined. If count is greater than the size of the object pointed to by dest, the behavior is undefined.

        Parameters
        dest	-	pointer to the object to fill
        ch	-	fill byte
        count	-	number of bytes to fill
        Return value
        dest 
         */
        public static IntPtr MemorySet(IntPtr dest, int fillByte, ulong count) => default;

        // =================== FsWatcher

        public static IntPtr FsWatcherCreate() => default;

        public static void FsWatcherDelete(IntPtr handle) { }


        /// <summary>
        /// Adds path to currently watched files.
        /// </summary>
        /// <param name="path">The name of the path to watch.</param>
        /// <param name="events">An optional filter to receive only events of particular types.
        /// This is currently implemented only for Linux.</param>
        /// <returns></returns>
        /// <remarks>
        /// The path argument can currently only be a directory and any changes to
        /// this directory itself or its immediate children will generate the events.
        /// Use AddTree() to monitor the directory recursively.
        /// Note that on platforms that use symbolic links, you should consider
        /// the possibility that path is a symlink. 
        /// </remarks>
        public static bool FsWatcherAdd(IntPtr handle, string path, int events) => default;

        /// <summary>
        /// This is the same as Add(), but also recursively adds every file/directory in
        /// the tree rooted at path. Additionally a file mask can be specified to include
        /// only files matching that particular mask.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="events"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// This method is implemented efficiently on Windows and MacOS, but should be
        /// used with care on other platforms for directories with lots of children
        /// (e.g. the root directory) as it calls Add() for each subdirectory, potentially
        /// creating a lot of watches and taking a long time to execute.
        /// </remarks>
        public static bool FsWatcherAddTree(IntPtr handle, string path, int events, string filter) => default;

        /// <summary>
        /// Returns the number of currently watched paths.
        /// </summary>
        /// <returns></returns>
        public static int FsWatcherGetWatchedPathsCount(IntPtr handle) => default;

        /// <summary>
        /// Removes path from the list of watched paths.
        /// </summary>
        /// <param name="path">Path to remove.</param>
        /// <returns></returns>
        public static bool FsWatcherRemove(IntPtr handle, string path) => default;

        /// <summary>
        /// Clears the list of currently watched paths.
        /// </summary>
        /// <returns></returns>
        public static bool FsWatcherRemoveAll(IntPtr handle) => default;

        /// <summary>
        /// This is the same as Remove(), but also removes every file/directory belonging
        /// to the tree rooted at path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FsWatcherRemoveTree(IntPtr handle, string path) => default;

        /// <summary>
        /// Associates the file system watcher with the given handler object.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="handler"></param>
        /// <remarks>
        /// All the events generated by this object will be passed to the specified owner.
        /// </remarks>
        public static void FsWatcherSetOwner(IntPtr handle, IntPtr handler) { }

        // ===================

        public static IntPtr SoundCreate() => default;

        // Constructs a wave object from a file or, under Windows, from a Windows resource.
        // Call IsOk() to determine whether this succeeded.
        // fileName    - The filename or Windows resource.
        // isResource  - true if fileName is a resource, false if it is a filename.
        public static IntPtr SoundCreate2(string fileName, bool isResource = false ) => default;

        // Constructs a wave object from in-memory data.
        // size	- Size of the buffer pointer to by data.
        // data - The buffer containing the sound data in WAV format.
        public static IntPtr SoundCreate4(ulong size, IntPtr data) => default;

        public static void SoundDelete(IntPtr handle) { }

        // #define 	wxSOUND_SYNC   0
        // #define wxSOUND_ASYNC   1
        // #define wxSOUND_LOOP   2
        // Plays the sound file.
        // If another sound is playing, it will be interrupted.
        // Returns true on success, false otherwise.Note that in general it
        // is possible to delete the object which is being asynchronously played
        // any time after calling this function and the sound would continue playing,
        // however this currently doesn't work under Windows for sound objects loaded
        // from memory data.
        // The possible values for flags are:
        // wxSOUND_SYNC: Play will block and wait until the sound is replayed.
        // wxSOUND_ASYNC: Sound is played asynchronously, Play returns immediately.
        // wxSOUND_ASYNC|wxSOUND_LOOP: Sound is played asynchronously and loops until another
        // sound is played, Stop() is called or the program terminates.
        public static bool SoundPlay2(string filename, uint flags /*= wxSOUND_ASYNC*/) => default;

        public static bool SoundPlay(IntPtr handle, uint flags /*= wxSOUND_ASYNC*/) => default;

        public static void SoundStop() { }

        // Returns true if the object contains a successfully loaded file or
        // resource, false otherwise.
        public static bool SoundIsOk(IntPtr handle) => default;

        // ===================

        public static void UIActionSimulatorDelete(IntPtr handle) { }

        public static IntPtr UIActionSimulatorCreate() => default;

        // Press and release a key.
        // keycode - Key to operate on, as an integer. It is interpreted as a wxKeyCode.
        // modifiers - A combination of wxKeyModifier flags to be pressed with the given keycode.
        public static bool UIActionSimulatorChar(IntPtr handle, int keycode,
            int modifiers/* = wxMOD_NONE*/) => default;

        // Press a key.
        // If you are using modifiers then it needs to
        // be paired with an identical KeyUp or the modifiers will not be released (MSW and macOS).
        // keycode	- Key to operate on, as an integer. It is interpreted as a wxKeyCode.
        // modifiers - A combination of wxKeyModifier flags to be pressed with the given keycode.
        public static bool UIActionSimulatorKeyDown(IntPtr handle, int keycode,
            int modifiers/* = wxMOD_NONE*/) => default;

        // Release a key.
        // keycode - Key to operate on, as an integer. It is interpreted as a wxKeyCode.
        // modifiers - A combination of wxKeyModifier flags to be pressed with the given keycode.
        public static bool UIActionSimulatorKeyUp(IntPtr handle, int keycode,
            int modifiers/* = wxMOD_NONE*/) => default;

        // Click a mouse button.
        // button - Button to press.
        public static bool UIActionSimulatorMouseClick(IntPtr handle,
            int button/* = wxMOUSE_BTN_LEFT*/) => default;

        // Double-click a mouse button.
        // button - Button to press.
        public static bool UIActionSimulatorMouseDblClick(IntPtr handle,
            int button/* = wxMOUSE_BTN_LEFT*/) => default;

        // Press a mouse button.
        // button - Button to press. Valid constants are wxMOUSE_BTN_LEFT, wxMOUSE_BTN_MIDDLE, and wxMOUSE_BTN_RIGHT.
        public static bool UIActionSimulatorMouseDown(IntPtr handle,
            int button/* = wxMOUSE_BTN_LEFT*/) => default;

        // Perform a drag and drop operation.
        // x1 - x start coordinate, in screen coordinates.
        // y1 - y start coordinate, in screen coordinates.
        // x2 - x destination coordinate, in screen coordinates.
        // y2 - y destination coordinate, in screen coordinates.
        // button - Button to press.
        public static bool UIActionSimulatorMouseDragDrop(IntPtr handle, long x1,
            long y1, long x2, long y2, int button /*= wxMOUSE_BTN_LEFT*/) => default;

        // Move the mouse to the specified coordinates.
        // point - Point to move to, in screen coordinates.
        public static bool UIActionSimulatorMouseMove(IntPtr handle, PointI point) => default;

        // Release a mouse button.
        // button - Button to press. 
        public static bool UIActionSimulatorMouseUp(IntPtr handle,
            int button /*= wxMOUSE_BTN_LEFT*/) => default;

        // Simulate selection of an item with the given text.
        // This method selects an item in the currently focused
        // ComboBox, ListBox and similar controls.
        // It does it by simulating keyboard events, so the behaviour
        // should be the same as if the item was really selected by the user.
        // Notice that the implementation of this method uses wxYield() and
        // so events can be dispatched from it.
        // text - The text of the item to select.
        // Returns true if the item text was successfully selected or false if
        // the currently focused window is not one of the controls allowing
        // item selection or if the item with the given text was not found in it.
        public static bool UIActionSimulatorSelect(IntPtr handle, string text) => default;

        // Emulate typing in the keys representing the given string.
        // Currently only the ASCII letters are universally supported.
        // Digits and punctuation characters can be used with the standard QWERTY (US) keyboard
        // layout but may not work with other layouts.
        // text - The string, containing only US ASCII characters, to type.
        public static bool UIActionSimulatorText(IntPtr handle, string text) => default;

        public static void UIActionSimulatorYield() { }

        // ===================
    }
}

/*

*/