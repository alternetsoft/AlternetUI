#include "UnmanagedDataObject.h"

namespace Alternet::UI
{
    UnmanagedDataObject::UnmanagedDataObject() : _dataObject(new wxDataObjectComposite())
    {
    }

    UnmanagedDataObject::UnmanagedDataObject(wxDataObjectComposite* dataObject) : _dataObject(dataObject)
    {
    }

    UnmanagedDataObject::~UnmanagedDataObject()
    {
        if (_dataObject != nullptr)
        {
            delete _dataObject;
            _dataObject = nullptr;
        }
    }

    optional<string> UnmanagedDataObject::TryGetText()
    {
        wxTextDataObject* object = nullptr;
        if (_dataObject->IsSupportedFormat(wxDF_TEXT))
            object = static_cast<wxTextDataObject*>(_dataObject->GetObject(wxDF_TEXT));
        else if (_dataObject->IsSupportedFormat(wxDF_UNICODETEXT))
            object = static_cast<wxTextDataObject*>(_dataObject->GetObject(wxDF_UNICODETEXT));
        else if (_dataObject->IsSupportedFormat(wxDF_OEMTEXT))
            object = static_cast<wxTextDataObject*>(_dataObject->GetObject(wxDF_OEMTEXT));

        if (object == nullptr)
            return nullopt;

        return wxStr(object->GetText());
    }

    optional<string> UnmanagedDataObject::TryGetFiles()
    {
        wxFileDataObject* object = nullptr;
        if (_dataObject->IsSupportedFormat(wxDF_FILENAME))
            object = static_cast<wxFileDataObject*>(_dataObject->GetObject(wxDF_FILENAME));

        if (object == nullptr)
            return nullopt;

        auto fileNames = object->GetFilenames();
        wxString result;

        int i = 0;
        for (auto fileName : fileNames)
        {
            result.Append(fileName);
            if (++i < fileNames.Count())
                result.Append("|");
        }

        return wxStr(result);
    }

    optional<wxBitmap> UnmanagedDataObject::TryGetBitmap()
    {
        return optional<wxBitmap>();
    }

    bool UnmanagedDataObject::GetDataPresent(const string& format)
    {
        if (format == DataFormats::Text)
            return _dataObject->IsSupportedFormat(wxDF_TEXT) ||
            _dataObject->IsSupportedFormat(wxDF_OEMTEXT) ||
            _dataObject->IsSupportedFormat(wxDF_UNICODETEXT);

        if (format == DataFormats::Files)
            return _dataObject->IsSupportedFormat(wxDF_FILENAME);

        return false;
    }

    void* UnmanagedDataObject::OpenFormatsArray()
    {
        auto result = new std::vector<string>();

        if (GetDataPresent(DataFormats::Text))
            result->push_back(DataFormats::Text);

        if (GetDataPresent(DataFormats::Files))
            result->push_back(DataFormats::Files);

        return result;
    }

    int UnmanagedDataObject::GetFormatsItemCount(void* array)
    {
        return ((std::vector<string>*)array)->size();
    }

    string UnmanagedDataObject::GetFormatsItemAt(void* array, int index)
    {
        return (*((std::vector<string>*)array))[index];
    }

    void UnmanagedDataObject::CloseFormatsArray(void* array)
    {
        delete (std::vector<string>*)array;
    }

    string UnmanagedDataObject::GetStringData(const string& format)
    {
        if (!GetDataPresent(format))
            throwExInvalidArg(format, FormatNotPresentErrorMessage);

        auto text = TryGetText();
        if (text.has_value())
            return text.value();

        throwExNoInfo;
    }

    string UnmanagedDataObject::GetFileNamesData(const string& format)
    {
        if (!GetDataPresent(format))
            throwExInvalidArg(format, FormatNotPresentErrorMessage);

        auto fileNames = TryGetFiles();
        if (fileNames.has_value())
            return fileNames.value();

        throwExNoInfo;
    }

    UnmanagedStream* UnmanagedDataObject::GetStreamData(const string& format)
    {
        return nullptr;
    }

    void UnmanagedDataObject::SetStringData(const string& format, const string& value)
    {
    }

    void UnmanagedDataObject::SetFileNamesData(const string& format, const string& value)
    {
    }

    void UnmanagedDataObject::SetStreamData(const string& format, void* value)
    {
    }
}
