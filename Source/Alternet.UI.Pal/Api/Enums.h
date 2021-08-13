// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
#pragma once

namespace Alternet::UI
{
    enum class FontStyle
    {
        Regular,
        Bold,
        Italic,
        Underlined,
        Strikethrough,
    };
    
    enum class GenericFontFamily
    {
        None,
        SansSerif,
        Serif,
        Monospace,
    };
    
    enum class ListBoxSelectionMode
    {
        Single,
        Multiple,
    };
    
    enum class ListViewSelectionMode
    {
        Single,
        Multiple,
    };
    
    enum class ListViewView
    {
        List,
        Details,
        SmallIcon,
        LargeIcon,
    };
    
    enum class TreeViewSelectionMode
    {
        Single,
        Multiple,
    };
    
    enum class WindowStartPosition
    {
        SystemDefaultLocation,
        SystemDefaultBounds,
        Manual,
        CenterScreen,
        CenterOwner,
    };
    
}
template<> struct enable_bitmask_operators<Alternet::UI::FontStyle> { static const bool enable = true; };
