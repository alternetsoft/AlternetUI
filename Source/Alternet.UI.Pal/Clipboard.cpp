#include "Clipboard.h"

namespace Alternet::UI
{
    Clipboard::Clipboard()
    {
    }
    
    Clipboard::~Clipboard()
    {
    }
    
    UnmanagedDataObject* Clipboard::GetDataObject()
    {
        if (!wxTheClipboard->Open())
            throwEx(u"Error while opening the clipboard.");

        auto compositeDataObject = GetCompositeDataObject();
        if (compositeDataObject->GetFormatCount() == 0)
        {
            delete compositeDataObject;
            return nullptr;
        }

        wxTheClipboard->Close();
        return new UnmanagedDataObject(compositeDataObject);
    }

    optional<string> Clipboard::TryGetText()
    {
        wxTextDataObject data;
        if (!wxTheClipboard->GetData(data))
            return nullopt;

        return wxStr(data.GetText());
    }

    optional<string> Clipboard::TryGetFiles()
    {
        return optional<string>();
    }

    optional<wxBitmap> Clipboard::TryGetBitmap()
    {
        return optional<wxBitmap>();
    }

    wxDataObjectComposite* Clipboard::GetCompositeDataObject()
    {
        auto result = new wxDataObjectComposite();

        auto textData = new wxTextDataObject();
        if (wxTheClipboard->GetData(*textData))
            result->Add(textData);
        else
            delete textData;

        auto fileData = new wxFileDataObject();
        if (wxTheClipboard->GetData(*fileData))
            result->Add(fileData);
        else
            delete fileData;

        auto bitmapData = new wxBitmapDataObject();
        if (wxTheClipboard->GetData(*bitmapData))
            result->Add(bitmapData);
        else
            delete bitmapData;

        return result;
    }
}
