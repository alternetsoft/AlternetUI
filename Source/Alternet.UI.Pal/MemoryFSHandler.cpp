#include "MemoryFSHandler.h"
#include "wx/fs_mem.h"

namespace Alternet::UI
{
	//-------------------------------------------------
	void MemoryFSHandler::RemoveFile(const string& filename)
	{	
		wxMemoryFSHandler::RemoveFile(wxStr(filename));
	}
	//-------------------------------------------------
	void MemoryFSHandler::AddTextFileWithMimeType(const string& filename,
		const string& textdata, const string& mimetype)
	{ 
		wxMemoryFSHandler::AddFileWithMimeType(wxStr(filename),
			wxStr(textdata), wxStr(mimetype));
	}
	//-------------------------------------------------
	void MemoryFSHandler::AddTextFile(const string& filename, const string& textdata)
	{
		wxMemoryFSHandler::AddFile(wxStr(filename),wxStr(textdata));
	}
	//-------------------------------------------------
	void MemoryFSHandler::AddFile(const string& filename, void* binarydata, int size)
	{
		wxMemoryFSHandler::AddFile(wxStr(filename), binarydata, size);
	}
	//-------------------------------------------------
	void MemoryFSHandler::AddFileWithMimeType(const string& filename, void* binarydata, 
		int size, const string& mimetype)
	{
		wxMemoryFSHandler::AddFileWithMimeType(wxStr(filename), binarydata, size, wxStr(mimetype));
	}
	//-------------------------------------------------
	MemoryFSHandler::MemoryFSHandler()
	{

	}
	//-------------------------------------------------
	MemoryFSHandler::~MemoryFSHandler()
	{

	}
	//-------------------------------------------------
}
//-------------------------------------------------
