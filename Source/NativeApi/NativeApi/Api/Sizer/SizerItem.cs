#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class SizerItem
    {
    }
}

/*  
 
    // window
    wxSizerItem( wxWindow *window,
                 int proportion=0,
                 int flag=0,
                 int border=0,
                 wxObject* userData=NULL );

    // window with flags
    wxSizerItem(wxWindow *window, const wxSizerFlags& flags)
    {
        Init(flags);

        DoSetWindow(window);
    }

    // subsizer
    wxSizerItem( wxSizer *sizer,
                 int proportion=0,
                 int flag=0,
                 int border=0,
                 wxObject* userData=NULL );

    // sizer with flags
    wxSizerItem(wxSizer *sizer, const wxSizerFlags& flags)
    {
        Init(flags);

        DoSetSizer(sizer);
    }

    // spacer
    wxSizerItem( int width,
                 int height,
                 int proportion=0,
                 int flag=0,
                 int border=0,
                 wxObject* userData=NULL);

    // spacer with flags
    wxSizerItem(int width, int height, const wxSizerFlags& flags)
    {
        Init(flags);

        DoSetSpacer(wxSize(width, height));
    }

    wxSizerItem();
    virtual ~wxSizerItem();

    virtual void DeleteWindows();

    // Enable deleting the SizerItem without destroying the contained sizer.
    void DetachSizer() { m_sizer = NULL; }

    // Enable deleting the SizerItem without resetting the sizer in the
    // contained window.
    void DetachWindow() { m_window = NULL; m_kind = Item_None; }

    virtual wxSize GetSize() const;
    virtual wxSize CalcMin();
    virtual void SetDimension( const wxPoint& pos, const wxSize& size );

    wxSize GetMinSize() const
        { return m_minSize; }
    wxSize GetMinSizeWithBorder() const;

    wxSize GetMaxSize() const
        { return IsWindow() ? m_window->GetMaxSize() : wxDefaultSize; }
    wxSize GetMaxSizeWithBorder() const;

    void SetMinSize(const wxSize& size)
    {
        if ( IsWindow() )
            m_window->SetMinSize(size);
        m_minSize = size;
    }
    void SetMinSize( int x, int y )
        { SetMinSize(wxSize(x, y)); }
    void SetInitSize( int x, int y )
        { SetMinSize(wxSize(x, y)); }

    // if either of dimensions is zero, ratio is assumed to be 1
    // to avoid "divide by zero" errors
    void SetRatio(int width, int height)
        { m_ratio = (width && height) ? ((float) width / (float) height) : 1; }
    void SetRatio(const wxSize& size)
        { SetRatio(size.x, size.y); }
    void SetRatio(float ratio)
        { m_ratio = ratio; }
    float GetRatio() const
        { return m_ratio; }

    virtual wxRect GetRect() { return m_rect; }

    // set a sizer item id (different from a window id, all sizer items,
    // including spacers, can have an associated id)
    void SetId(int id) { m_id = id; }
    int GetId() const { return m_id; }

    bool IsWindow() const { return m_kind == Item_Window; }
    bool IsSizer() const { return m_kind == Item_Sizer; }
    bool IsSpacer() const { return m_kind == Item_Spacer; }

    void SetProportion( int proportion )
        { m_proportion = proportion; }
    int GetProportion() const
        { return m_proportion; }
    void SetFlag( int flag )
        { m_flag = flag; }
    int GetFlag() const
        { return m_flag; }
    void SetBorder( int border )
        { m_border = border; }
    int GetBorder() const
        { return m_border; }

    wxWindow *GetWindow() const
        { return m_kind == Item_Window ? m_window : NULL; }
    wxSizer *GetSizer() const
        { return m_kind == Item_Sizer ? m_sizer : NULL; }
    wxSize GetSpacer() const;

    // This function behaves obviously for the windows and spacers but for the
    // sizers it returns true if any sizer element is shown and only returns
    // false if all of them are hidden. Also, it always returns true if
    // wxRESERVE_SPACE_EVEN_IF_HIDDEN flag was used.
    bool IsShown() const;

    void Show(bool show);

    void SetUserData(wxObject* userData)
        { delete m_userData; m_userData = userData; }
    wxObject* GetUserData() const
        { return m_userData; }
    wxPoint GetPosition() const
        { return m_pos; }

    // Called once the first component of an item has been decided. This is
    // used in algorithms that depend on knowing the size in one direction
    // before the min size in the other direction can be known.
    // Returns true if it made use of the information (and min size was changed).
    bool InformFirstDirection( int direction, int size, int availableOtherDir=-1 );

    // these functions delete the current contents of the item if it's a sizer
    // or a spacer but not if it is a window
    void AssignWindow(wxWindow *window)
    {
        Free();
        DoSetWindow(window);
    }

    void AssignSizer(wxSizer *sizer)
    {
        Free();
        DoSetSizer(sizer);
    }

    void AssignSpacer(const wxSize& size)
    {
        Free();
        DoSetSpacer(size);
    }

    void AssignSpacer(int w, int h) { AssignSpacer(wxSize(w, h)); }

#if WXWIN_COMPATIBILITY_2_8
    // these functions do not free the old sizer/spacer and so can easily
    // provoke the memory leaks and so shouldn't be used, use Assign() instead
    wxDEPRECATED( void SetWindow(wxWindow *window) );
    wxDEPRECATED( void SetSizer(wxSizer *sizer) );
    wxDEPRECATED( void SetSpacer(const wxSize& size) );
    wxDEPRECATED( void SetSpacer(int width, int height) );
#endif // WXWIN_COMPATIBILITY_2_8

 
 
 
 */