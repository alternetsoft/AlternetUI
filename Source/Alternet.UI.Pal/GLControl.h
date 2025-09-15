#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"

#include <wx/glcanvas.h>

namespace Alternet::UI
{

    class wxGLCanvas2 : public wxGLCanvas, public wxWidgetExtender
    {
    public:
        wxGLCanvas2()
        {
        }

        wxGLCanvas2(wxWindow* parent,
            const wxGLAttributes& dispAttrs,
            wxWindowID id = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = 0,
            const wxString& name = wxGLCanvasName,
            const wxPalette& palette = wxNullPalette)
            : wxGLCanvas(parent, dispAttrs, id, pos, size, style, name, palette)
        {
        }

        void SetCurrent()
        {
            if (m_context == nullptr)
            {
                auto ctxAttrs = GetGLContextAttributes(3, 3);
                m_context = new wxGLContext(this, nullptr, &ctxAttrs);
            }

            m_context->SetCurrent(*this);
            // wxGLCanvas::SetCurrent(*m_context);
        }

        static wxGLContextAttrs GetGLContextAttributes(int majorVersion, int minorVersion)
        {
            wxGLContextAttrs ctxAttrs;
            ctxAttrs.CoreProfile().OGLVersion(majorVersion, minorVersion).EndList();
            return ctxAttrs;
        }

        static wxGLAttributes GetGLAttributes()
        {
            wxGLAttributes attrs;
            attrs.PlatformDefaults().RGBA().DoubleBuffer().Depth(16).Stencil(8).EndList();
            return attrs;
        }

        static bool IsOpenGLAvailable()
        {
            wxGLAttributes attrs = GetGLAttributes();

            if (wxGLCanvas::IsDisplaySupported(attrs))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        wxGLContext* m_context = nullptr;
    protected:
    private:
    };

    class GLControl : public Control
    {
#include "Api/GLControl.inc"
    public:
        virtual wxWindow* CreateWxWindowUnparented() override;
        virtual wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        virtual void SetUserPaint(bool value) override;

        wxGLCanvas2* GetGLCanvas();

        virtual void OnPaint(wxPaintEvent& event) override;

        void LogInfo();
    private:
        wxSize _viewport;
        bool _defaultPaintUsed = false;
    };
}
