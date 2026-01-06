# 0.9.808 (2026 January 6)

- UserControl: Improve overlay tooltip positioning and fitting logic.
- ToolBar: Updates SpeedButton initialization to inherit parent background and foreground colors.
- ToolBar: Introduced AddItems and SetItems methods to allow adding or replacing ToolBar items using IMenuProperties.
- SpeedButton: Refactor label text color retrieval in SpeedButton. Introduced GetLabelTextColor.
- InnerPopupToolBar: Improved back and fore color handling by setting parent and default colors
- MauiUtils: Add ContextMenuUnderlayColorDark and ContextMenuUnderlayColorLight for overlay fill colors.
- MauiUtils: Refactors ShowContextMenu to support overlay and alignment, and improves context menu display logic.
- MauiUtils: Adds methods for converting client coordinates to parent-relative coordinates.
- MauiUtils: Add HideContextMenus and HideContextMenusInControlView methods to centralize hiding of context menus.
- Add ContainerSizeOverride property to PopupControl.
- Add DefaultMinPopupWidth to InnerPopupToolBar.
- Refactor context menu alignment and host control logic.
- Add NineRectsParts enum and utility methods to NineRects.
- RectI: Setters for SkiaLocation, SkiaSize, Left, Top, Right, Bottom, TopLeft, TopRight, BottomLeft, and BottomRight properties.
- RectD: Add ellipse bounding box and top-right offset methods.
- OverlayToolTipParams: Refactor tooltip location handling with offset support. Add LocationWithOffset and LocationWithoutOffset properties.
- ControlOverlayWithImage: Add image size and bounds properties.
- OverlayToolTipParams: Add ToolTip and AssociatedControl.
- Add CustomAttr property to IMenuItemProperties.
- Add HasMargin property to ToolBarSeparatorItem.
- Menu: Add PrependSeparator and PrependDisabledText methods, allowing insertion of a separator or a disabled text item at the beginning.
- Add BottomRight alignment to HVDropDownAlignment.
- Add Prepend method to ItemContainerElement.

---

Older items can be found [here](Documents/Whatsnew.History/whatsnew-2025.md)