#pragma warning disable
using NativeApi.Api.ManagedServers;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_image.html
    public class GenericImage
    {
        // Creates an empty wxImage object without an alpha channel.
        public static IntPtr CreateGenericImage() => default;

        // Creates an image with the given size and clears it if requested.
        public static IntPtr CreateGenericImageWithSize(int width, int height, bool clear = true) => default;

        // Creates an image from a file.
        public static IntPtr CreateGenericImageFromFileWithBitmapType(string name,
            int bitmapType /*= wxBITMAP_TYPE_ANY*/,
            int index = -1) => default;

        // Creates an image from a file using MIME-types to specify the type.
        public static IntPtr CreateGenericImageFromFileWithMimeType(string name, string mimetype,
            int index = -1) => default;

        // Creates an image from a stream.
        public static IntPtr CreateGenericImageFromStreamWithBitmapData(InputStream stream,
            int bitmapType /*= wxBITMAP_TYPE_ANY*/, int index = -1) => default;

        // Creates an image from a stream using MIME-types to specify the type.
        public static IntPtr CreateGenericImageFromStreamWithMimeType(InputStream stream,
            string mimetype,
            int index = -1) => default;

        // Creates an image from data in memory.
        public static IntPtr CreateGenericImage(int width, int height, IntPtr data,
            bool static_data = false) => default;

        // Creates an image from data in memory.
        public static IntPtr CreateGenericImageWithAlpha(int width, int height, IntPtr data, IntPtr alpha,
            bool static_data = false) => default;

        public static void DeleteImage(IntPtr handle) { }

        // Sets the alpha value for the given pixel.
        public static void SetAlpha(IntPtr handle, int x, int y, byte alpha) { }

        // Removes the alpha channel from the image.
        public static void ClearAlpha(IntPtr handle) { }

        // Sets the flags used for loading image files by this object.
        public static void SetLoadFlags(IntPtr handle, int flags) { }

        // Specifies whether there is a mask or not.
        public static void SetMask(IntPtr handle, bool hasMask = true) { }

        // Sets the mask color for this image(and tells the image to use the mask).
        public static void SetMaskColor(IntPtr handle, byte red, byte green, byte blue) { }

        // Sets image's mask so that the pixels that have RGB value of mr,mg,mb in
        // mask will be masked in the image.
        public static bool SetMaskFromImage(IntPtr handle, IntPtr image, byte mr,
            byte mg, byte mb) => default;

        // Sets a user-defined option.
        public static void SetOptionString(IntPtr handle, string name, string value) { }

        // Sets a user-defined option. 
        public static void SetOptionInt(IntPtr handle, string name, int value) { }

        // Sets the color of the pixel at the given x and y coordinate.
        public static void SetRGB(IntPtr handle, int x, int y, byte r, byte g, byte b) { }

        // Sets the color of the pixels within the given rectangle.
        public static void SetRGBRect(IntPtr handle, Int32Rect rect, byte red,
            byte green, byte blue) { }

        // Sets the type of image returned by GetType().
        public static void SetType(IntPtr handle, int type) { }

        // Sets the default value for the flags used for loading image files. 
        public static void SetDefaultLoadFlags(int flags) { } // real static

        // Returns the file load flags used for this object.
        public static int GetLoadFlags(IntPtr handle) => default;

        // Returns an identical copy of this image.
        public static IntPtr Copy(IntPtr handle) => default;

        // Creates a fresh image.
        public static bool Create(IntPtr handle, int width, int height, bool clear = true) => default;

        // Initialize the image data with zeroes(the default) or with the byte value given as value.
        public static void Clear(IntPtr handle, byte value = 0) { }

        // Destroys the image data.
        public static void DestroyImageData(IntPtr handle) { }

        // Initializes the image alpha channel data.
        public static void InitAlpha(IntPtr handle) { }

        // Blurs the image in both horizontal and vertical directions by the specified
        // pixel blurRadius.
        public static IntPtr Blur(IntPtr handle, int blurRadius) => default;

        // Blurs the image in the horizontal direction only.
        public static IntPtr BlurHorizontal(IntPtr handle, int blurRadius) => default;

        // Blurs the image in the vertical direction only.
        public static IntPtr BlurVertical(IntPtr handle, int blurRadius) => default;

        // Returns a mirrored copy of the image.
        public static IntPtr Mirror(IntPtr handle, bool horizontally = true) => default;

        // Copy the data of the given image to the specified position in this image.
        public static void Paste(IntPtr handle, IntPtr image, int x, int y,
            int alphaBlend /*= wxIMAGE_ALPHA_BLEND_OVER*/) { }

        // Replaces the color specified by r1,g1,b1 by the color r2,g2,b2.
        public static void Replace(IntPtr handle, byte r1, byte g1, byte b1, byte r2,
            byte g2, byte b2) { }

        //Changes the size of the image in-place by scaling it: after a call to this
        //function,the image will have the given width and height.
        public static IntPtr Rescale(IntPtr handle, int width, int height,
            int quality /*= wxIMAGE_QUALITY_NORMAL*/) => default;

        //Changes the size of the image in-place without scaling it by adding either a border
        //with the given color or cropping as necessary.
        public static IntPtr Resize(IntPtr handle, Int32Size size, Int32Point pos, int red = -1,
            int green = -1, int blue = -1) => default;

        //Rotates the image about the given point, by angle radians.
        public static IntPtr Rotate(IntPtr handle, double angle, Int32Point rotationCentre,
            bool interpolating /*= true*/, out Int32Point offsetAfterRotation /*= NULL*/) => default;

        //Returns a copy of the image rotated 90 degrees in the direction indicated by clockwise.
        public static IntPtr Rotate90(IntPtr handle, bool clockwise = true) => default;

        // Returns a copy of the image rotated by 180 degrees.
        public static IntPtr Rotate180(IntPtr handle) => default;

        // Rotates the hue of each pixel in the image by angle, which is a double in the
        // range [-1.0..+1.0], where -1.0 corresponds to -360 degrees and +1.0 corresponds
        // to +360 degrees.
        public static void RotateHue(IntPtr handle, double angle) { }
           
        //Changes the saturation of each pixel in the image.
        public static void ChangeSaturation(IntPtr handle, double factor) { }

        // Changes the brightness(value) of each pixel in the image.
        public static void ChangeBrightness(IntPtr handle, double factor) { }

        // Changes the hue, the saturation and the brightness(value) of each pixel in the image.
        public static void ChangeHSV(IntPtr handle, double angleH, double factorS, double factorV) { }

        // Returns a scaled version of the image.
        public static IntPtr Scale(IntPtr handle, int width, int height,
            int quality /*= wxIMAGE_QUALITY_NORMAL*/) => default;

        // Returns a resized version of this image without scaling it by adding either a
        // border with the given color or cropping as necessary.
        public static IntPtr Size(IntPtr handle, Int32Size size, Int32Point pos, int red = -1,
            int green = -1, int blue = -1) => default;

        // If the image has alpha channel, this method converts it to mask.
        public static bool ConvertAlphaToMask(IntPtr handle,
            byte threshold /*= wxIMAGE_ALPHA_THRESHOLD*/) => default;

        // If the image has alpha channel, this method converts it to mask using the
        // specified color as the mask color.
        public static bool ConvertAlphaToMask(IntPtr handle, byte mr, byte mg, byte mb,
            byte threshold /*= wxIMAGE_ALPHA_THRESHOLD*/) => default;

        // Returns a greyscale version of the image.
        public static IntPtr ConvertToGreyscale(IntPtr handle, double weight_r, double weight_g,
            double weight_b) => default;
           
        //Returns a greyscale version of the image.
        public static IntPtr ConvertToGreyscale(IntPtr handle) => default;

        // Returns monochromatic version of the image.
        public static IntPtr ConvertToMono(IntPtr handle, byte r, byte g, byte b) => default;

        // Returns disabled(dimmed) version of the image.
        public static IntPtr ConvertToDisabled(IntPtr handle, byte brightness = 255) => default;

        // Returns a changed version of the image based on the given lightness.
        public static IntPtr ChangeLightness(IntPtr handle, int alpha) => default;

        // Return alpha value at given pixel location.
        public static byte GetAlpha(IntPtr handle, int x, int y) => default;

        // Returns the red intensity at the given coordinate.
        public static byte GetRed(IntPtr handle, int x, int y) => default;

        // Returns the green intensity at the given coordinate.
        public static byte GetGreen(IntPtr handle, int x, int y) => default;

        // Returns the blue intensity at the given coordinate.
        public static byte GetBlue(IntPtr handle, int x, int y) => default;

        // Gets the red value of the mask color.
        public static byte GetMaskRed(IntPtr handle) => default;

        // Gets the green value of the mask color.
        public static byte GetMaskGreen(IntPtr handle) => default;

        // Gets the blue value of the mask color.
        public static byte GetMaskBlue(IntPtr handle) => default;

        // Gets the width of the image in pixels.
        public static int GetWidth(IntPtr handle) => default;

        // Gets the height of the image in pixels.
        public static int GetHeight(IntPtr handle) => default;

        // Returns the size of the image in pixels.
        public static Int32Size GetSize(IntPtr handle) => default;

        // Gets a user-defined string-valued option.
        public static string GetOptionString(IntPtr handle, string name) => default;

        // Gets a user-defined integer-valued option.
        public static int GetOptionInt(IntPtr handle, string name) => default;

        // Returns a sub image of the current one as long as the rect belongs entirely to the image.
        public static IntPtr GetSubImage(IntPtr handle, Int32Rect rect) => default;

        // Gets the type of image found by LoadFile() or specified with SaveFile().
        public static int GetBitmapType(IntPtr handle) => default;

        // Returns true if this image has alpha channel, false otherwise.
        public static bool HasAlpha(IntPtr handle) => default;

        // Returns true if there is a mask active, false otherwise.
        public static bool HasMask(IntPtr handle) => default;

        // Returns true if the given option is present.
        public static bool HasOption(IntPtr handle, string name) => default;

        // Returns true if image data is present.
        public static bool IsOk(IntPtr handle) => default;

        // Returns true if the given pixel is transparent, i.e. either has the mask
        // color if this image has a mask or if this image has alpha channel and alpha value of
        // this pixel is strictly less than threshold.
        public static bool IsTransparent(IntPtr handle, int x, int y,
            byte threshold /*= wxIMAGE_ALPHA_THRESHOLD*/) => default;

        // Loads an image from an input stream.
        public static bool LoadFileFromStreamWithBitmapType(IntPtr handle, InputStream stream,
            int bitmapType /*= wxBITMAP_TYPE_ANY*/, int index = -1) => default;

        // Loads an image from a file.
        public static bool LoadFileWithBitmapType(IntPtr handle, string name,
            int bitmapType /*= wxBITMAP_TYPE_ANY*/, int index = -1) => default;

        // Loads an image from a file.
        public static bool LoadFileWithMimeType(IntPtr handle, string name, string mimetype,
            int index = -1) => default;

        // Loads an image from an input stream.
        public static bool LoadFileFromStreamWithMimeType(IntPtr handle, InputStream stream,
            string mimetype, int index = -1) => default;

        // Saves an image in the given stream.
        public static bool SaveFileToStreamWithMimeType(IntPtr handle, OutputStream stream,
            string mimetype) => default;

        // Saves an image in the named file.
        public static bool SaveFileWithBitmapType(IntPtr handle, string name, int bitmapType) => default;

        // Saves an image in the named file.
        public static bool SaveFileWithMimeType(IntPtr handle, string name, string mimetype) => default;

        // Saves an image in the named file.
        public static bool SaveFile(IntPtr handle, string name) => default;

        // Saves an image in the given stream.
        public static bool SaveFileToStreamWithBitmapType(IntPtr handle, OutputStream stream,
            int type) => default;

        // Returns true if at least one of the available image handlers can read the file
        // with the given name.
        public static bool CanRead(string filename) => default; // real static

        // Returns true if at least one of the available image handlers can read the data in
        // the given stream.
        public static bool CanReadStream(InputStream stream) => default;// real static

        // Returns the currently used default file load flags.
        public static int GetDefaultLoadFlags() => default;// real static

        // Iterates all registered wxImageHandler objects, and returns a string containing
        // file extension masks suitable for passing to file open/save dialog boxes.
        public static String GetImageExtWildcard() => default;// real static

        // Register an image handler.
        public static void AddHandler(IntPtr handler) { }// real static

        // Deletes all image handlers.
        public static void CleanUpHandlers() { }// real static

        // Finds the handler with the given name.
        public static IntPtr FindHandler(string name) => default;// real static

        // Finds the handler associated with the given extension and type.
        public static IntPtr FindHandler(string extension, int bitmapType) => default;// real static

        // Finds the handler associated with the given image type.
        public static IntPtr FindHandler(int bitmapType) => default;// real static

        // Finds the handler associated with the given MIME type.
        public static IntPtr FindHandlerMime(string mimetype) => default;// real static

        //Adds a handler at the start of the static list of format handlers.
        public static void InsertHandler(IntPtr handler) { }// real static

        //Finds the handler with the given name, and removes it.
        public static bool RemoveHandler(string name) => default;// real static

        public static int GetImageCountInFile(string filename,
            int bitmapType /*= wxBITMAP_TYPE_ANY*/) => default;// real static
                                                                                                           /If the image file contains more than one image and the image handler is capable of retrieving these individually, this function will return the number of available images.
        public static int GetImageCountInStream(InputStream stream,
            int bitmapType /*= wxBITMAP_TYPE_ANY*/) => default;// real static
                                                                                                                 //If the image file contains more than one image and the image handler is capable of retrieving these individually, this function will return the number of available images.
 	    // Returns pointer to the array storing the alpha values for this image.
        public static IntPtr GetAlphaData(IntPtr handle) => default;
 
 	    // Returns the image data as an array.
        public static IntPtr GetData(IntPtr handle) => default;

        // Creates a fresh image.
        public static bool CreateData(IntPtr handle, int width, int height, IntPtr data,
            bool static_data = false) => default;

        // Creates a fresh image.
        public static bool CreateAlphaData(IntPtr handle, int width, int height, IntPtr data,
            IntPtr alpha, bool static_data = false) => default;

        //Sets the image data without performing checks.
        public static void SetData(IntPtr handle, IntPtr data, bool static_data = false) { }

        //This is an overloaded member function, provided for convenience.
        public static void SetDataWithSize(IntPtr handle, IntPtr data, int new_width, int new_height, bool static_data = false) { }

        //This function is similar to SetData() and has similar restrictions.
        public static void SetAlpha(IntPtr handle, IntPtr alpha = default, bool static_data = false) { }
    }
}

/*

// Converts a color in RGB color space to HSV color space.
public static wxImage::HSVValue RGBtoHSV(wxImage::RGBValue &rgb) => default;// real static
 
// Converts a color in HSV color space to RGB color space.
public static wxImage::RGBValue HSVtoRGB(wxImage::HSVValue &hsv) => default;// real static
 
// Associates a palette with the image.
public static void 	SetPalette(wxPalette &palette){}
 
// Creates an image from XPM data.
public static IntPtr CreateGenericImage(char **xpmData) => default;

// Computes the histogram of the image.
public static ulong ComputeHistogram(wxImageHistogram &histogram) => default;

// Finds the first color that is never used in the image.
public static bool FindFirstUnusedColor(byte* r, byte* g, byte* b, byte startR = 1,
    byte startG = 0, byte startB = 0) => default;

// Get the current mask color or find a suitable unused color that could be used as a mask color.
public static bool 	GetOrFindMaskColor(byte *r, byte *g, byte *b) => default;
 
// Returns the palette associated with the image.
public static wxPalette & 	GetPalette() => default;

// Returns the static list of image format handlers.
public static wxList & 	GetHandlers() => default;// real static
  
 */