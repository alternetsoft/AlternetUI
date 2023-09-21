#pragma warning disable
using ApiCommon;
using Alternet.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_status_bar.html
    public class WxStatusBarFactory
    {
        public static int GetFieldsCount(IntPtr handle) => default;
        public static void SetStatusText(IntPtr handle, string text, int number = 0) { }
        public static string GetStatusText(IntPtr handle, int number = 0) => default;

        // change the currently shown text to the new one and save the current
        // value to be restored by the next call to PopStatusText()
        public static void PushStatusText(IntPtr handle, string text, int number = 0){}

        public static void PopStatusText(IntPtr handle, int number = 0) { }

        // Sets the widths of the fields in the status line.
        //
        // There are two types of fields: fixed widths and variable width fields. For the
        // fixed width fields you should specify their (constant) width in pixels. For
        // the variable width fields, specify a negative number which indicates how the
        // field should expand: the space left for all variable width fields is divided
        // between them according to the absolute value of this number. A variable width
        // field with width of -2 gets twice as much of it as a field with width -1 and so on.
        //
        // For example, to create one fixed width field of width 100 in the right
        // part of the status bar and two more fields which get 66% and 33% of the
        // remaining space correspondingly, you should use an array containing -2, -1 and 100.
        //
        // n	The number of fields in the status bar. Must be equal to the number passed
        // to SetFieldsCount() the last time it was called.
        //
        // widths_field	Contains an array of n integers, each of which is either an
        // absolute status field width in pixels if positive or indicates a variable
        // width field if negative. The special value NULL means that all fields should
        // get the same width.
        //
        // Remarks
        // The widths of the variable fields are calculated from the total width of all
        // fields, minus the sum of widths of the non-variable fields, divided
        // by the number of variable fields. 
        public static void SetStatusWidths(IntPtr handle, int[] widths){}

        public static void SetFieldsCount(IntPtr handle, int number) { }

        public static int GetStatusWidth(IntPtr handle, int n) => default;

        public static int GetStatusStyle(IntPtr handle, int n) => default;

        // Set the field border style to one of wxSB_XXX values.
        public static void SetStatusStyles(IntPtr handle, int[] styles){}

        // Get the position and size of the field's internal bounding rectangle
        public static Int32Rect GetFieldRect(IntPtr handle, int i) => default;

        // sets the minimal vertical size of the status bar
        public static void SetMinHeight(IntPtr handle, int height) { }

        // get the dimensions of the horizontal and vertical borders
        public static int GetBorderX(IntPtr handle) => default;
        public static int GetBorderY(IntPtr handle) => default;
    }
}