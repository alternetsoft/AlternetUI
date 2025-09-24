using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a collection of constants and utilities for implementing Material Design
    /// principles in user interface components.
    /// This class includes standardized values and settings based on Material Design guidelines.
    /// </summary>
    /// <remarks>
    /// See <see href="https://m3.material.io/">Material Design guidelines</see> for more information.
    /// </remarks>
    public static partial class MaterialDesign
    {
        /// <summary>
        /// Provides standardized corner radius values for Material Design components.
        /// Values are based on Material 3 shape system guidelines.
        /// </summary>
        public static class CornerRadius
        {
            /// <summary>
            /// Default corner radius for buttons (e.g., Elevated, Text, Outlined).
            /// </summary>
            public static Coord Button = 4f;

            /// <summary>
            /// Corner radius for text fields and input controls.
            /// </summary>
            public static Coord TextField = 4f;

            /// <summary>
            /// Corner radius for cards and surfaces with elevation.
            /// </summary>
            public static Coord Card = 12f;

            /// <summary>
            /// Corner radius for modal dialogs.
            /// </summary>
            public static Coord Dialog = 28f;

            /// <summary>
            /// Corner radius for bottom sheets (applied to top corners only).
            /// </summary>
            public static Coord BottomSheetTopCorners = 16f;

            /// <summary>
            /// Corner radius for Floating Action Button (FAB).
            /// Typically rendered as a circle.
            /// </summary>
            public static Coord FloatingActionButton = 28f;

            /// <summary>
            /// Corner radius for navigation drawers and side sheets.
            /// </summary>
            public static Coord Drawer = 16f;

            /// <summary>
            /// Corner radius for menus and popups.
            /// </summary>
            public static Coord Menu = 4f;

            /// <summary>
            /// Corner radius for tooltips.
            /// </summary>
            public static Coord Tooltip = 4f;
        }
    }
}
