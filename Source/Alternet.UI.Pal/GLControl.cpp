#include "GLControl.h"

namespace Alternet::UI
{
    int wxGLCanvas2::defaultMajorVersion = 0;
    int wxGLCanvas2::defaultMinorVersion = 0;

    GLControl::GLControl()
    {
    }

    GLControl::~GLControl()
    {
    }

    wxGLCanvas2* GLControl::GetGLCanvas()
    {
        return dynamic_cast<wxGLCanvas2*>(GetWxWindow());
    }

    void GLControl::CreateDummyOpenGlCanvas()
    {
        // Step 1: Create hidden canvas
        auto dummyCanvas = new wxGLCanvas(
            ParkingWindow::GetWindow(),
            wxGLCanvas2::GetGLAttributes(),
            wxID_ANY,
            wxDefaultPosition,
            wxSize(1, 1),
            wxFULL_REPAINT_ON_RESIZE | wxNO_BORDER);

        auto dummyContext = new wxGLContext(dummyCanvas);

        // Step 2: Make it current
        dummyCanvas->SetCurrent(*dummyContext);
    }

    bool GLControl::IsOpenGLAvailable()
    {
		return wxGLCanvas2::IsOpenGLAvailable();
    }

    void GLControl::SetUserPaint(bool value)
    {
    }

    bool GLControl::GetDefaultPaintUsed()
    {
        return _defaultPaintUsed;
    }

    void GLControl::SetDefaultPaintUsed(bool value)
    {
        _defaultPaintUsed = value;
    }

    SizeI GLControl::GetViewportSize()
    {
        return _viewport;
    }

    void GLControl::LogInfo()
    {
#define GL_FRAMEBUFFER_BINDING 0x8CA6
#define GL_SAMPLES 0x80A9

        GLint fbo = 0;
        glGetIntegerv(GL_FRAMEBUFFER_BINDING, &fbo);
        GLint samples = 0;
        glGetIntegerv(GL_SAMPLES, &samples);
        GLint stencilBits = 0;
        glGetIntegerv(GL_STENCIL_BITS, &stencilBits);
        GLint depthBits = 0;
        glGetIntegerv(GL_DEPTH_BITS, &depthBits);

        wxLogDebug("OpenGL Version: %s", glGetString(GL_VERSION));
        wxLogDebug("GL_VENDOR: %s", glGetString(GL_VENDOR));
        wxLogDebug("GL_RENDERER: %s", glGetString(GL_RENDERER));
        wxLogDebug("Framebuffer ID: %d", fbo);
        wxLogDebug("Samples: %d", samples);
        wxLogDebug("Stencil Bits: %d", stencilBits);
        wxLogDebug("Depth Bits: %d", depthBits);
    }

    void GLControl::OnSizeChanged(wxSizeEvent& event)
    {
		Control::OnSizeChanged(event);
        auto wxWnd = GetGLCanvas();

        int w, h;
        wxWnd->GetClientSize(&w, &h);

        wxWnd->SetCurrent();

        glViewport(0, 0, w, h);
        wxWnd->Refresh();
    }

    void GLControl::OnPaint(wxPaintEvent& event)
    {
        event.Skip();
        if (IsNullOrDeleting())
            return;
        auto wxWnd = GetGLCanvas();
        auto size = wxWnd->GetClientSize();

        if(size.x <= 0 || size.y <= 0)
            return;

        wxPaintDC dc(wxWnd); // Required on some platforms

        wxWnd->SetCurrent();

        if (!wxWnd->m_context->IsOK())
            return;

		_viewport = size;

        glViewport(0, 0, size.x, size.y);

        /*
        glClearColor(0.1f, 0.2f, 0.3f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        */

        _defaultPaintUsed = false;

        RaiseEvent(ControlEvent::Paint);

        if (!_defaultPaintUsed)
        {
            glFlush();
            wxWnd->SwapBuffers();
        }
    }

    wxWindow* GLControl::CreateWxWindowUnparented()
    {
        return new wxGLCanvas2();
    }

    wxWindow* GLControl::CreateWxWindowCore(wxWindow* parent)
    {
        auto style = GetDefaultStyle();
        auto window = new wxGLCanvas2(parent,
            wxGLCanvas2::GetGLAttributes(),
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            style);
        window->SetCurrent();
        return window;
    }

}
