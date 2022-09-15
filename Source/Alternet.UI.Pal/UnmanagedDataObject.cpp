#include "UnmanagedDataObject.h"

namespace Alternet::UI
{
    UnmanagedDataObject::UnmanagedDataObject()
    {
    }

    UnmanagedDataObject::~UnmanagedDataObject()
    {
    }

    void* UnmanagedDataObject::OpenFormatsArray()
    {
        return nullptr;
    }

    int UnmanagedDataObject::GetFormatsItemCount(void* array)
    {
        return 0;
    }

    string UnmanagedDataObject::GetFormatsItemAt(void* array, int index)
    {
        return string();
    }

    void UnmanagedDataObject::CloseFormatsArray(void* array)
    {
    }

    string UnmanagedDataObject::GetStringData(const string& format)
    {
        return string();
    }

    string UnmanagedDataObject::GetFileNamesData(const string& format)
    {
        return string();
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

    bool UnmanagedDataObject::GetDataPresent(const string& format)
    {
        return false;
    }
}
