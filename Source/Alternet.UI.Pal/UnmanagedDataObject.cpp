#include "UnmanagedDataObject.h"
#include "Api/InputStream.h"
#include "ManagedInputStream.h"

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

    wxDataObjectComposite* UnmanagedDataObject::GetDataObjectComposite()
    {
        return _dataObject;
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
        wxBitmapDataObject* object = nullptr;
        if (_dataObject->IsSupportedFormat(wxDF_DIB))
            object = static_cast<wxBitmapDataObject*>(_dataObject->GetObject(wxDF_DIB));

        if (object == nullptr)
            return nullopt;

        return object->GetBitmap();
    }

    bool UnmanagedDataObject::GetDataPresent(const string& format)
    {
        if (format == DataFormats::Text)
            return _dataObject->IsSupportedFormat(wxDF_TEXT) ||
            _dataObject->IsSupportedFormat(wxDF_OEMTEXT) ||
            _dataObject->IsSupportedFormat(wxDF_UNICODETEXT);

        if (format == DataFormats::Files)
            return _dataObject->IsSupportedFormat(wxDF_FILENAME);

        if (format == DataFormats::Bitmap)
            return _dataObject->IsSupportedFormat(wxDF_DIB);

        return false;
    }

    void* UnmanagedDataObject::OpenFormatsArray()
    {
        auto result = new std::vector<string>();

        if (GetDataPresent(DataFormats::Text))
            result->push_back(DataFormats::Text);

        if (GetDataPresent(DataFormats::Files))
            result->push_back(DataFormats::Files);

        if (GetDataPresent(DataFormats::Bitmap))
            result->push_back(DataFormats::Bitmap);

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
        if (!GetDataPresent(format))
            throwExInvalidArg(format, FormatNotPresentErrorMessage);

        auto bitmap = TryGetBitmap();
        if (bitmap.has_value())
        {
            auto image = bitmap.value().ConvertToImage();
            auto stream = new wxMemoryOutputStream();
            image.SaveFile(*stream, wxBitmapType::wxBITMAP_TYPE_PNG);

            return new UnmanagedStream(stream);
        }

        throwExNoInfo;
    }

    void UnmanagedDataObject::SetStringData(const string& format, const string& value)
    {
        auto textData = new wxTextDataObject(wxStr(value));
        if (format != DataFormats::Text)
            textData->SetFormat(wxDataFormat(wxStr(format)));

        _dataObject->Add(textData);
    }

    void UnmanagedDataObject::SetFileNamesData(const string& format, const string& value)
    {
        auto fileData = new wxFileDataObject();

        wxStringTokenizer tokenizer(wxStr(value), "|");
        while (tokenizer.HasMoreTokens())
        {
            auto fileName = tokenizer.GetNextToken();
            fileData->AddFile(fileName);
        }

        if (format != DataFormats::Files)
            fileData->SetFormat(wxDataFormat(wxStr(format)));

        _dataObject->Add(fileData);
    }

    void UnmanagedDataObject::SetStreamData(const string& format, void* value)
    {
        if (format != DataFormats::Bitmap)
            throwEx(u"Data format not supported: " + format);

        InputStream inputStream(value);
        ManagedInputStream managedInputStream(&inputStream);

        auto bitmap = wxBitmap(managedInputStream);

        auto bitmapData = new wxBitmapDataObject(bitmap);
        _dataObject->Add(bitmapData);
    }
}
