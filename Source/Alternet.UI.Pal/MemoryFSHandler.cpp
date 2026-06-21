#include "MemoryFSHandler.h"

#include <wx/fs_mem.h>

namespace Alternet::UI
{
	
	void MemoryFSHandler::RemoveFile(const NativeStringSpan& filename)
	{	
		wxMemoryFSHandler::RemoveFile(wxStr(filename));
	}
	
	void MemoryFSHandler::AddTextFileWithMimeType(const NativeStringSpan& filename,
		const NativeStringSpan& textdata, const NativeStringSpan& mimetype)
	{ 
		wxMemoryFSHandler::AddFileWithMimeType(wxStr(filename),
			wxStr(textdata), wxStr(mimetype));
	}
	
	void MemoryFSHandler::AddTextFile(const NativeStringSpan& filename, const NativeStringSpan& textdata)
	{
		wxMemoryFSHandler::AddFile(wxStr(filename), wxStr(textdata));
	}
	
	void MemoryFSHandler::AddFile(const NativeStringSpan& filename, void* binarydata, int size)
	{
		wxMemoryFSHandler::AddFile(wxStr(filename), binarydata, size);
	}
	
	void MemoryFSHandler::AddFileWithMimeType(const NativeStringSpan& filename, void* binarydata, 
		int size, const NativeStringSpan& mimetype)
	{
		wxMemoryFSHandler::AddFileWithMimeType(wxStr(filename), binarydata, size, wxStr(mimetype));
	}
	
	MemoryFSHandler::MemoryFSHandler()
	{

	}
	
	MemoryFSHandler::~MemoryFSHandler()
	{

	}
	
}

