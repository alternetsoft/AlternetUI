#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "UnmanagedStream.h"

namespace Alternet::UI
{
    class UnmanagedDataObject : public Object
    {
#include "Api/UnmanagedDataObject.inc"
    public:
        UnmanagedDataObject(wxDataObjectComposite* dataObject);

    private:
        wxDataObjectComposite* _dataObject = nullptr;

        optional<string> TryGetText();
        optional<string> TryGetFiles();
        optional<wxBitmap> TryGetBitmap();

        const string FormatNotPresentErrorMessage = u"The specified format is not present in this data object.";
    };

    namespace DataFormats
    {
        const string Text = u"Text";
        const string Files = u"Files";
        const string Bitmap = u"Bitmap";
    }
}
