#include "MemoryFSHandler.h"

#include <wx/fs_mem.h>

namespace Alternet::UI
{
	
	void MemoryFSHandler::RemoveFile(const NativeStringSpan& filename)
	{	
		wxMemoryFSHandler::RemoveFile(StringSpanToWx(filename));
	}
	
	void MemoryFSHandler::AddTextFileWithMimeType(const NativeStringSpan& filename,
		const NativeStringSpan& textdata, const NativeStringSpan& mimetype)
	{ 
		wxMemoryFSHandler::AddFileWithMimeType(StringSpanToWx(filename),
			StringSpanToWx(textdata), StringSpanToWx(mimetype));
	}
	
	void MemoryFSHandler::AddTextFile(const NativeStringSpan& filename, const NativeStringSpan& textdata)
	{
		wxMemoryFSHandler::AddFile(StringSpanToWx(filename), StringSpanToWx(textdata));
	}
	
	void MemoryFSHandler::AddFile(const NativeStringSpan& filename, void* binarydata, int size)
	{
		wxMemoryFSHandler::AddFile(StringSpanToWx(filename), binarydata, size);
	}
	
	void MemoryFSHandler::AddFileWithMimeType(const NativeStringSpan& filename, void* binarydata, 
		int size, const NativeStringSpan& mimetype)
	{
		wxMemoryFSHandler::AddFileWithMimeType(StringSpanToWx(filename), binarydata, size, StringSpanToWx(mimetype));
	}
	
	MemoryFSHandler::MemoryFSHandler()
	{

	}
	
	MemoryFSHandler::~MemoryFSHandler()
	{

	}
	
}

