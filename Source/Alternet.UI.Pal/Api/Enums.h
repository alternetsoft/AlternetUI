// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
#pragma once

namespace Alternet::UI
{
    enum class BrushHatchStyle
    {
        BackwardDiagonal = 0,
        ForwardDiagonal = 1,
        DiagonalCross = 2,
        Cross = 3,
        Horizontal = 4,
        Vertical = 5,
    };
    
    enum class FontStyle
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        Underlined = 4,
        Strikethrough = 8,
    };
    
    enum class GenericFontFamily
    {
        None = 0,
        SansSerif = 1,
        Serif = 2,
        Monospace = 3,
    };
    
    enum class ListBoxSelectionMode
    {
        Single = 0,
        Multiple = 1,
    };
    
    enum class ListViewSelectionMode
    {
        Single = 0,
        Multiple = 1,
    };
    
    enum class ListViewView
    {
        List = 0,
        Details = 1,
        SmallIcon = 2,
        LargeIcon = 3,
    };
    
    enum class TreeViewSelectionMode
    {
        Single = 0,
        Multiple = 1,
    };
    
    enum class WindowStartPosition
    {
        SystemDefaultLocation = 0,
        SystemDefaultBounds = 1,
        Manual = 2,
        CenterScreen = 3,
        CenterOwner = 4,
    };
    
}
template<> struct enable_bitmask_operators<Alternet::UI::FontStyle> { static const bool enable = true; };
