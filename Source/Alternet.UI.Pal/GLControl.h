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

#ifdef  __WXMSW__
#define DEFAULT_ASK_GL_45 true
#endif

#ifdef __WXOSX__
#define DEFAULT_ASK_GL_45 false
#endif

#ifdef __WXGTK__
#define DEFAULT_ASK_GL_45 true
#endif

		static int defaultMajorVersion;
		static int defaultMinorVersion;

        void SetCurrent()
        {
            if (m_context == nullptr)
            {
                if (defaultMajorVersion != 0)
                {
                    m_context = CreateContext(defaultMajorVersion, defaultMinorVersion);
                }
                else
                {
                    if (DEFAULT_ASK_GL_45)
                        m_context = CreateContext(4, 5);

                    if (m_context == nullptr)
                        m_context = CreateContext(4, 1);

					if (m_context == nullptr)
						m_context = CreateContext(3, 3);
                }
            }

            if(m_context != nullptr)
                m_context->SetCurrent(*this);
        }

        wxGLContext* CreateContext(int majorVersion, int minorVersion)
        {
            auto ctxAttrs = GetGLContextAttributes(majorVersion, minorVersion);
            auto result = new wxGLContext(this, nullptr, &ctxAttrs);

            if (!result->IsOK())
            {
                delete result;
				return nullptr;
            }
            else
            {
                defaultMajorVersion = majorVersion;
                defaultMinorVersion = minorVersion;
            }

            return result;
        }

        static wxGLContextAttrs GetGLContextAttributes(int majorVersion, int minorVersion)
        {
            wxGLContextAttrs ctxAttrs;
            ctxAttrs
                .PlatformDefaults()
                .CoreProfile()
                .OGLVersion(majorVersion, minorVersion)
                .EndList();
            return ctxAttrs;
        }

        static wxGLAttributes GetGLAttributes()
        {
            wxGLAttributes attrs;
            attrs.PlatformDefaults().RGBA().DoubleBuffer().FrameBuffersRGB()
                .Stencil(8)
                .EndList();
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
    protected:
        virtual void OnSizeChanged(wxSizeEvent& event) override;
    private:
        wxSize _viewport;
        bool _defaultPaintUsed = false;
    };
}
