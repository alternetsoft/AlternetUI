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

        wxDataObjectComposite* GetDataObjectComposite();

        static optional<wxString> TryGetText(wxDataObjectComposite* dataObject);
        static optional<wxArrayString> TryGetFiles(wxDataObjectComposite* dataObject);
        static optional<wxBitmap> TryGetBitmap(wxDataObjectComposite* dataObject);

        static wxBitmapDataObject* TryGetBitmapDataObject(wxDataObjectComposite* dataObject);

    private:
        wxDataObjectComposite* _dataObject = nullptr;

        static optional<wxString> TryGetFilesString(wxDataObjectComposite* dataObject);

        static wxDataFormat GetBitmapDataFormat();
        static std::vector<wxDataFormat> GetTextDataFormats();

        const wxString FormatNotPresentErrorMessage
            = "The specified format is not present in this data object.";
    };

    namespace DataFormats
    {
        const wxString Text = "Text";
        const wxString Files = "Files";
        const wxString Bitmap = "Bitmap";
        const wxString Persistent = "AlternetUI10PersistentObject";
    }
}
