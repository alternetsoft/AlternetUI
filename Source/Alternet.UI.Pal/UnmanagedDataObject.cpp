#include "UnmanagedDataObject.h"
#include "Api/InputStream.h"
#include "ManagedInputStream.h"

namespace Alternet::UI
{
    UnmanagedDataObject::UnmanagedDataObject() : _dataObject(new wxDataObjectComposite())
    {
    }

    UnmanagedDataObject::UnmanagedDataObject(wxDataObjectComposite* dataObject)
        : _dataObject(dataObject)
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

    /*static*/ optional<wxString> UnmanagedDataObject::TryGetText(wxDataObjectComposite* dataObject)
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

        return object->GetText();
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

    /*static*/ optional<wxArrayString> UnmanagedDataObject::TryGetFiles(
        wxDataObjectComposite* dataObject)
    {
        wxFileDataObject* object = nullptr;
        if (dataObject->IsSupportedFormat(wxDF_FILENAME))
            object = static_cast<wxFileDataObject*>(dataObject->GetObject(wxDF_FILENAME));

        if (object == nullptr)
            return nullopt;

        return object->GetFilenames();
    }

    /*static*/ optional<wxString> UnmanagedDataObject::TryGetFilesString(
        wxDataObjectComposite* dataObject)
    {
        auto fileNames = TryGetFiles(dataObject);

        if (!fileNames.has_value())
            return nullopt;

        wxString result;

        size_t i = 0;
        for (auto fileName : fileNames.value())
        {
            result.Append(fileName);
            if (++i < fileNames.value().Count())
                result.Append("|");
        }

        return result;
    }

    /*static*/ optional<wxBitmap> UnmanagedDataObject::TryGetBitmap(wxDataObjectComposite* dataObject)
    {
        auto object = TryGetBitmapDataObject(dataObject);
        if (object == nullptr)
            return nullopt;

        return object->GetBitmap();
    }

    /*static*/ wxBitmapDataObject* UnmanagedDataObject::TryGetBitmapDataObject(
        wxDataObjectComposite* dataObject)
    {
        wxBitmapDataObject* object = nullptr;
        auto format = GetBitmapDataFormat();
        if (dataObject->IsSupportedFormat(format))
            return static_cast<wxBitmapDataObject*>(dataObject->GetObject(format));

        return nullptr;
    }

    /*static*/ wxDataFormat UnmanagedDataObject::GetBitmapDataFormat()
    {
#ifdef __WXMSW__
        return wxDF_DIB;
#else
        return wxDF_BITMAP;
#endif
    }

    bool UnmanagedDataObject::GetDataPresent(const NativeStringSpan& format)
    {
		auto wxFormat = wxStr(format);

        if (wxFormat == DataFormats::Text)
        {
            for(auto format : GetTextDataFormats())
                if (_dataObject->IsSupportedFormat(format))
                    return true;
        }

        if (wxFormat == DataFormats::Files)
            return _dataObject->IsSupportedFormat(wxDF_FILENAME);

        if (wxFormat == DataFormats::Bitmap)
            return _dataObject->IsSupportedFormat(GetBitmapDataFormat());

        auto frmt = wxDataFormat(wxFormat);
        return _dataObject->IsSupportedFormat(frmt);
    }

    bool UnmanagedDataObject::GetNativeDataPresent(int format)
    {
        auto fmt = wxDataFormat((wxDataFormatId)format);
        return _dataObject->IsSupportedFormat(fmt);
    }

    bool IsStandardDataFormat(wxDataFormat& df)
    {
        auto m_format = df.GetType();

        return m_format > 0 && m_format < wxDF_PRIVATE;
    }

    void* UnmanagedDataObject::OpenFormatsArray()
    {
        auto result = new std::vector<NativeStringSpan>();

        if (GetDataPresent(wxStr(DataFormats::Text)))
            result->push_back(wxStr(DataFormats::Text));

        if (GetDataPresent(wxStr(DataFormats::Files)))
            result->push_back(wxStr(DataFormats::Files));

        if (GetDataPresent(wxStr(DataFormats::Bitmap)))
            result->push_back(wxStr(DataFormats::Bitmap));

        /*
        if (GetDataPresent(wxStr(DataFormats::Persistent)))
            result->push_back(wxStr(DataFormats::Persistent));
        */

        auto fmtCount = _dataObject->GetFormatCount();

        wxDataFormat* formats = new wxDataFormat[fmtCount];
        _dataObject->GetAllFormats(formats);

        size_t n;
        for (n = 0; n < fmtCount; n++)
        {
            if (IsStandardDataFormat(formats[n]))
                continue;
            auto st = formats[n].GetId();
            result->push_back(wxStr(st));
        }

        delete[] formats;

        return result;
    }

    int UnmanagedDataObject::GetFormatsItemCount(void* array)
    {
        return ((std::vector<NativeStringSpan>*)array)->size();
    }

    NativeStringSpan UnmanagedDataObject::GetFormatsItemAt(void* array, int index)
    {
        return (*((std::vector<NativeStringSpan>*)array))[index];
    }

    void UnmanagedDataObject::CloseFormatsArray(void* array)
    {
        delete (std::vector<NativeStringSpan>*)array;
    }

    NativeStringSpan UnmanagedDataObject::GetStringData(const NativeStringSpan& format)
    {
		auto wxFormat = wxStr(format);

        if (!GetDataPresent(format))
            throwExInvalidArg(wxFormat, FormatNotPresentErrorMessage);

        auto text = TryGetText(_dataObject);
        if (text.has_value())
            return wxStr(text.value());

        throwExNoInfo;
    }

    NativeStringSpan UnmanagedDataObject::GetFileNamesData(const NativeStringSpan& format)
    {
		auto wxFormat = wxStr(format);

        if (!GetDataPresent(format))
            throwExInvalidArg(wxFormat, FormatNotPresentErrorMessage);

        auto fileNames = TryGetFilesString(_dataObject);
        if (fileNames.has_value())
            return wxStr(fileNames.value());

        throwExNoInfo;
    }

    UnmanagedStream* UnmanagedDataObject::GetStreamData(const NativeStringSpan& format)
    {
        if (!GetDataPresent(format))
            return nullptr;

        if (wxStr(format) == DataFormats::Bitmap)
        {
            auto bitmap = TryGetBitmap(_dataObject);
            if (bitmap.has_value())
            {
                auto image = bitmap.value().ConvertToImage();
                auto stream = new wxMemoryOutputStream();
                image.SaveFile(*stream, wxBitmapType::wxBITMAP_TYPE_PNG);

                return new UnmanagedStream(stream);
            }
        }

        if (wxStr(format) == DataFormats::Persistent)
        {
            auto fmt = wxDataFormat(DataFormats::Persistent);
            if (_dataObject->IsSupportedFormat(fmt))
            {
                auto dobject = static_cast<wxCustomDataObject*>(_dataObject->GetObject(fmt));
                auto stream = new wxMemoryOutputStream();
                auto size = dobject->GetDataSize();
                auto ptr = dobject->GetData();
                stream->Write(ptr, size);
                stream->SeekO(0);
                return new UnmanagedStream(stream);
            }
            else
                return nullptr;
        }

        return nullptr;
    }

    void UnmanagedDataObject::SetStringData(const NativeStringSpan& format, const NativeStringSpan& value)
    {
        auto textData = new wxTextDataObject(wxStr(value));
        if (wxStr(format) != DataFormats::Text)
            textData->SetFormat(wxDataFormat(wxStr(format)));

        _dataObject->Add(textData);
    }

    void UnmanagedDataObject::SetFileNamesData(const NativeStringSpan& format, const NativeStringSpan& value)
    {
        auto fileData = new wxFileDataObject();

        wxStringTokenizer tokenizer(wxStr(value), "|");
        while (tokenizer.HasMoreTokens())
        {
            auto fileName = tokenizer.GetNextToken();
            fileData->AddFile(fileName);
        }

        if (wxStr(format) != DataFormats::Files)
            fileData->SetFormat(wxDataFormat(wxStr(format)));

        _dataObject->Add(fileData);
    }

    void UnmanagedDataObject::SetStreamData(const NativeStringSpan& format, void* value)
    {
        InputStream inputStream(value);
        ManagedInputStream managedInputStream(&inputStream);

        if (wxStr(format) == DataFormats::Bitmap)
        {
            auto bitmap = wxBitmap(managedInputStream);
            auto bitmapData = new wxBitmapDataObject(bitmap);
            _dataObject->Add(bitmapData);
            return;
        }

        auto size = managedInputStream.GetLength();
        unsigned char* pchBuffer = new unsigned char[size];
        managedInputStream.Read(pchBuffer, size);

        auto customData = new wxCustomDataObject();
        customData->SetFormat(wxStr(format));
        if(customData->SetData(size, pchBuffer))
            _dataObject->Add(customData);
        delete[] pchBuffer;
    }
}