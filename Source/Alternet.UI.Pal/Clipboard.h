#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "UnmanagedDataObject.h"

namespace Alternet::UI
{
    class Clipboard : public Object
    {
#include "Api/Clipboard.inc"
    public:

    private:

        optional<string> TryGetText();
        optional<string> TryGetFiles();
        optional<wxBitmap> TryGetBitmap();

        wxDataObjectComposite* GetCompositeDataObjectFromClipboard();

        const char16_t* ClipboardOpenErrorMessage = u"Error while opening the clipboard.";
    };
}
