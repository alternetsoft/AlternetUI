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
    // https://docs.wxwidgets.org/3.2/classwx_tree_ctrl.html
    public class WxTreeViewFactory
    {
        public static void SetItemBold(IntPtr handle, IntPtr item, bool bold = true) { }

        public static Color GetItemTextColor(IntPtr handle, IntPtr item) => default;

        public static Color GetItemBackgroundColor(IntPtr handle, IntPtr item) => default;

        public static void SetItemTextColor(IntPtr handle, IntPtr item, Color color) {}

        public static void SetItemBackgroundColor(IntPtr handle, IntPtr item, Color color) { }

        public static void ResetItemTextColor(IntPtr handle, IntPtr item) { }

        public static void ResetItemBackgroundColor(IntPtr handle, IntPtr item) { }

    }
}

/*
        // get the total number of items in the control
    virtual unsigned int GetCount() const = 0;

        // indent is the number of pixels the children are indented relative to
        // the parents position. SetIndent() also redraws the control
        // immediately.
    virtual unsigned int GetIndent() const = 0;
    virtual void SetIndent(unsigned int indent) = 0;

        // spacing is the number of pixels between the start and the Text
        // (has no effect under wxMSW)
    unsigned int GetSpacing() const { return m_spacing; }
    void SetSpacing(unsigned int spacing) { m_spacing = spacing; }

        // In addition to {Set,Get,Assign}ImageList() methods inherited from
        // wxWithImages, this control has similar functions for the state image
        // list that can be used to show a state icon corresponding to an
        // app-defined item state (for example, checked/unchecked).
    wxImageList *GetStateImageList() const

    virtual void SetStateImageList(wxImageList *imageList) = 0;

    void AssignStateImageList(wxImageList *imageList)

    // Functions to work with tree ctrl items. Unfortunately, they can _not_ be
    // member functions of wxTreeItem because they must know the tree the item
    // belongs to for Windows implementation and storing the pointer to
    // wxTreeCtrl in each wxTreeItem is just too much waste.

        // retrieve items label
    virtual wxString GetItemText(const wxTreeItemId& item) const = 0;
        // get one of the images associated with the item (normal by default)
    virtual int GetItemImage(const wxTreeItemId& item,
                     wxTreeItemIcon which = wxTreeItemIcon_Normal) const = 0;
        // get the data associated with the item
    virtual wxTreeItemData *GetItemData(const wxTreeItemId& item) const = 0;

    virtual wxFont GetItemFont(const wxTreeItemId& item) const = 0;

        // get the items state
    int GetItemState(const wxTreeItemId& item) const

        // set items label
    virtual void SetItemText(const wxTreeItemId& item, const wxString& text) = 0;
        // set one of the images associated with the item (normal by default)
    virtual void SetItemImage(const wxTreeItemId& item,
                              int image,
                              wxTreeItemIcon which = wxTreeItemIcon_Normal) = 0;
        // associate some data with the item
    virtual void SetItemData(const wxTreeItemId& item, wxTreeItemData *data) = 0;

        // force appearance of [+] button near the item. This is useful to
        // allow the user to expand the items which don't have any children now
        // - but instead add them only when needed, thus minimizing memory
        // usage and loading time.
    virtual void SetItemHasChildren(const wxTreeItemId& item,
                                    bool has = true) = 0;

        // the item will be shown with a drop highlight
    virtual void SetItemDropHighlight(const wxTreeItemId& item,
                                      bool highlight = true) = 0;

        // set the items font (should be of the same height for all items)
    virtual void SetItemFont(const wxTreeItemId& item,
                             const wxFont& font) = 0;

        // set the items state (special state values: wxTREE_ITEMSTATE_NONE/NEXT/PREV)
    void SetItemState(const wxTreeItemId& item, int state);

        // is the item visible (it might be outside the view or not expanded)?
    virtual bool IsVisible(const wxTreeItemId& item) const = 0;
        // does the item has any children?
    virtual bool ItemHasChildren(const wxTreeItemId& item) const = 0;
        // same as above
    bool HasChildren(const wxTreeItemId& item) const
      { return ItemHasChildren(item); }
        // is the item expanded (only makes sense if HasChildren())?
    virtual bool IsExpanded(const wxTreeItemId& item) const = 0;
        // is this item currently selected (the same as has focus)?
    virtual bool IsSelected(const wxTreeItemId& item) const = 0;
        // is item text in bold font?
    virtual bool IsBold(const wxTreeItemId& item) const = 0;
        // is the control empty?
    bool IsEmpty() const;


        // if 'recursively' is false, only immediate children count, otherwise
        // the returned number is the number of all items in this branch
    virtual size_t GetChildrenCount(const wxTreeItemId& item,
                                    bool recursively = true) const = 0;

    // wxTreeItemId.IsOk() will return false if there is no such item

        // get the root tree item
    virtual wxTreeItemId GetRootItem() const = 0;

        // get the item currently selected (may return NULL if no selection)
    virtual wxTreeItemId GetSelection() const = 0;

        // get the items currently selected, return the number of such item
        //
        // NB: this operation is expensive and can take a long time for a
        //     control with a lot of items (~ O(number of items)).
    virtual size_t GetSelections(wxArrayTreeItemIds& selections) const = 0;

        // get the last item to be clicked when the control has wxTR_MULTIPLE
        // equivalent to GetSelection() if not wxTR_MULTIPLE
    virtual wxTreeItemId GetFocusedItem() const = 0;

        // Clears the currently focused item
    virtual void ClearFocusedItem() = 0;
        // Sets the currently focused item. Item should be valid
    virtual void SetFocusedItem(const wxTreeItemId& item) = 0;

        // get the parent of this item (may return NULL if root)
    virtual wxTreeItemId GetItemParent(const wxTreeItemId& item) const = 0;

        // for this enumeration function you must pass in a "cookie" parameter
        // which is opaque for the application but is necessary for the library
        // to make these functions reentrant (i.e. allow more than one
        // enumeration on one and the same object simultaneously). Of course,
        // the "cookie" passed to GetFirstChild() and GetNextChild() should be
        // the same!

        // get the first child of this item
    virtual wxTreeItemId GetFirstChild(const wxTreeItemId& item,
                                       wxTreeItemIdValue& cookie) const = 0;
        // get the next child
    virtual wxTreeItemId GetNextChild(const wxTreeItemId& item,
                                      wxTreeItemIdValue& cookie) const = 0;
        // get the last child of this item - this method doesn't use cookies
    virtual wxTreeItemId GetLastChild(const wxTreeItemId& item) const = 0;

        // get the next sibling of this item
    virtual wxTreeItemId GetNextSibling(const wxTreeItemId& item) const = 0;
        // get the previous sibling
    virtual wxTreeItemId GetPrevSibling(const wxTreeItemId& item) const = 0;

        // get first visible item
    virtual wxTreeItemId GetFirstVisibleItem() const = 0;
        // get the next visible item: item must be visible itself!
        // see IsVisible() and wxTreeCtrl::GetFirstVisibleItem()
    virtual wxTreeItemId GetNextVisible(const wxTreeItemId& item) const = 0;
        // get the previous visible item: item must be visible itself!
    virtual wxTreeItemId GetPrevVisible(const wxTreeItemId& item) const = 0;

        // add the root node to the tree
    virtual wxTreeItemId AddRoot(const wxString& text,
                                 int image = -1, int selImage = -1,
                                 wxTreeItemData *data = NULL) = 0;

        // insert a new item in as the first child of the parent
    wxTreeItemId PrependItem(const wxTreeItemId& parent,
                             const wxString& text,
                             int image = -1, int selImage = -1,
                             wxTreeItemData *data = NULL)

        // insert a new item after a given one
    wxTreeItemId InsertItem(const wxTreeItemId& parent,
                            const wxTreeItemId& idPrevious,
                            const wxString& text,
                            int image = -1, int selImage = -1,
                            wxTreeItemData *data = NULL)
    {
        return DoInsertAfter(parent, idPrevious, text, image, selImage, data);
    }

        // insert a new item before the one with the given index
    wxTreeItemId InsertItem(const wxTreeItemId& parent,
                            size_t pos,
                            const wxString& text,
                            int image = -1, int selImage = -1,
                            wxTreeItemData *data = NULL)
    {
        return DoInsertItem(parent, pos, text, image, selImage, data);
    }

        // insert a new item in as the last child of the parent
    wxTreeItemId AppendItem(const wxTreeItemId& parent,
                            const wxString& text,
                            int image = -1, int selImage = -1,
                            wxTreeItemData *data = NULL)

        // delete this item and associated data if any
    virtual void Delete(const wxTreeItemId& item) = 0;
        // delete all children (but don't delete the item itself)
        // NB: this won't send wxEVT_TREE_ITEM_DELETED events
    virtual void DeleteChildren(const wxTreeItemId& item) = 0;
        // delete all items from the tree
        // NB: this won't send wxEVT_TREE_ITEM_DELETED events
    virtual void DeleteAllItems() = 0;

        // expand this item
    virtual void Expand(const wxTreeItemId& item) = 0;
        // expand the item and all its children recursively
    void ExpandAllChildren(const wxTreeItemId& item);
        // expand all items
    void ExpandAll();
        // collapse the item without removing its children
    virtual void Collapse(const wxTreeItemId& item) = 0;
        // collapse the item and all its children
    void CollapseAllChildren(const wxTreeItemId& item);
        // collapse all items
    void CollapseAll();
        // collapse the item and remove all children
    virtual void CollapseAndReset(const wxTreeItemId& item) = 0;
        // toggles the current state
    virtual void Toggle(const wxTreeItemId& item) = 0;

        // remove the selection from currently selected item (if any)
    virtual void Unselect() = 0;
        // unselect all items (only makes sense for multiple selection control)
    virtual void UnselectAll() = 0;
        // select this item
    virtual void SelectItem(const wxTreeItemId& item, bool select = true) = 0;
        // selects all (direct) children for given parent (only for
        // multiselection controls)
    virtual void SelectChildren(const wxTreeItemId& parent) = 0;
        // unselect this item
    void UnselectItem(const wxTreeItemId& item) { SelectItem(item, false); }
        // toggle item selection
    void ToggleItemSelection(const wxTreeItemId& item)

        // make sure this item is visible (expanding the parent item and/or
        // scrolling to this item if necessary)
    virtual void EnsureVisible(const wxTreeItemId& item) = 0;
        // scroll to this item (but don't expand its parent)
    virtual void ScrollTo(const wxTreeItemId& item) = 0;

        // start editing the item label: this (temporarily) replaces the item
        // with a one line edit control. The item will be selected if it hadn't
        // been before. textCtrlClass parameter allows you to create an edit
        // control of arbitrary user-defined class deriving from wxTextCtrl.
    virtual wxTextCtrl *EditLabel(const wxTreeItemId& item,
                      wxClassInfo* textCtrlClass = wxCLASSINFO(wxTextCtrl)) = 0;
        // returns the same pointer as StartEdit() if the item is being edited,
        // NULL otherwise (it's assumed that no more than one item may be
        // edited simultaneously)
    virtual wxTextCtrl *GetEditControl() const = 0;
        // end editing and accept or discard the changes to item label
    virtual void EndEditLabel(const wxTreeItemId& item,
                              bool discardChanges = false) = 0;

        // Enable or disable beep when incremental match doesn't find any item.
        // Only implemented in the generic version currently.
    virtual void EnableBellOnNoMatch(bool WXUNUSED(on) = true) { }

    // sorting
    // -------

        // this function is called to compare 2 items and should return -1, 0
        // or +1 if the first item is less than, equal to or greater than the
        // second one. The base class version performs alphabetic comparison
        // of item labels (GetText)
    virtual int OnCompareItems(const wxTreeItemId& item1,
                               const wxTreeItemId& item2)
    {
        return wxStrcmp(GetItemText(item1), GetItemText(item2));
    }

        // sort the children of this item using OnCompareItems
        //
        // NB: this function is not reentrant and not MT-safe (FIXME)!
    virtual void SortChildren(const wxTreeItemId& item) = 0;

        // determine to which item (if any) belongs the given point (the
        // coordinates specified are relative to the client area of tree ctrl)
        // and, in the second variant, fill the flags parameter with a bitmask
        // of wxTREE_HITTEST_xxx constants.
    wxTreeItemId HitTest(const wxPoint& point) const
        { int dummy; return DoTreeHitTest(point, dummy); }
    wxTreeItemId HitTest(const wxPoint& point, int& flags) const
        { return DoTreeHitTest(point, flags); }

        // get the bounding rectangle of the item (or of its label only)
    virtual bool GetBoundingRect(const wxTreeItemId& item,
                                 wxRect& rect,
                                 bool textOnly = false) const = 0;


 */