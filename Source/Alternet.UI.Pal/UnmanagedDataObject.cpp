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

    /*static*/ optional<string> UnmanagedDataObject::TryGetText(wxDataObjectComposite* dataObject)
    {
        wxTextDataObject* object = nullptr;

        for(auto format : GetTextDataFormats())
        {
            if (dataObject->IsSupportedFormat(format))
            {
                object = static_cast<wxTextDataObject*>(dataObject->GetObject(format));
                break;
            }
        }

        if (object == nullptr)
            return nullopt;

        return wxStr(object->GetText());
    }

    /*static*/ std::vector<wxDataFormat> UnmanagedDataObject::GetTextDataFormats()
    {
        std::vector<wxDataFormat> formats =
        {
            wxDF_TEXT,
            wxDF_UNICODETEXT,
#ifdef __WXMSW__
            wxDF_OEMTEXT
#endif
        };
        
        return formats;
    }

    wxDataObjectComposite* UnmanagedDataObject::GetDataObjectComposite()
    {
        return _dataObject;
    }

    /*static*/ optional<wxArrayString> UnmanagedDataObject::TryGetFiles(wxDataObjectComposite* dataObject)
    {
        wxFileDataObject* object = nullptr;
        if (dataObject->IsSupportedFormat(wxDF_FILENAME))
            object = static_cast<wxFileDataObject*>(dataObject->GetObject(wxDF_FILENAME));

        if (object == nullptr)
            return nullopt;

        return object->GetFilenames();
    }

    /*static*/ optional<string> UnmanagedDataObject::TryGetFilesString(wxDataObjectComposite* dataObject)
    {
        auto fileNames = TryGetFiles(dataObject);

        if (!fileNames.has_value())
            return nullopt;

        wxString result;

        int i = 0;
        for (auto fileName : fileNames.value())
        {
            result.Append(fileName);
            if (++i < fileNames.value().Count())
                result.Append("|");
        }

        return wxStr(result);
    }

    /*static*/ optional<wxBitmap> UnmanagedDataObject::TryGetBitmap(wxDataObjectComposite* dataObject)
    {
        wxBitmapDataObject* object = nullptr;
        auto format = GetBitmapDataFormat();
        if (dataObject->IsSupportedFormat(format))
            object = static_cast<wxBitmapDataObject*>(dataObject->GetObject(format));

        if (object == nullptr)
            return nullopt;

        return object->GetBitmap();
    }

    /*static*/ wxDataFormat UnmanagedDataObject::GetBitmapDataFormat()
    {
#ifdef __WXMSW__
        return wxDF_DIB;
#else
        return wxDF_BITMAP;
#endif
    }

    bool UnmanagedDataObject::GetDataPresent(const string& format)
    {
        if (format == DataFormats::Text)
        {
            for(auto format : GetTextDataFormats())
                if (_dataObject->IsSupportedFormat(format))
                    return true;
        }

        if (format == DataFormats::Files)
            return _dataObject->IsSupportedFormat(wxDF_FILENAME);

        if (format == DataFormats::Bitmap)
            return _dataObject->IsSupportedFormat(GetBitmapDataFormat());

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

        auto text = TryGetText(_dataObject);
        if (text.has_value())
            return text.value();

        throwExNoInfo;
    }

    string UnmanagedDataObject::GetFileNamesData(const string& format)
    {
        if (!GetDataPresent(format))
            throwExInvalidArg(format, FormatNotPresentErrorMessage);

        auto fileNames = TryGetFilesString(_dataObject);
        if (fileNames.has_value())
            return fileNames.value();

        throwExNoInfo;
    }

    UnmanagedStream* UnmanagedDataObject::GetStreamData(const string& format)
    {
        if (!GetDataPresent(format))
            throwExInvalidArg(format, FormatNotPresentErrorMessage);

        auto bitmap = TryGetBitmap(_dataObject);
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
