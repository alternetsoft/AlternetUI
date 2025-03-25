#include "ValidatorText.h"

namespace Alternet::UI
{
	wxTextValidator* ValidatorText::ToTextValidator(void* handle)
	{
		return (wxTextValidator*) handle;
	}

	void ValidatorText::DeleteValidatorText(void* handle)
	{
		delete ToTextValidator(handle);
	}

	void* ValidatorText::CreateValidatorText(int64_t style)
	{
		return new wxTextValidator((long)style);
	}

	ValidatorText::ValidatorText() 
	{

	}
	
	ValidatorText::~ValidatorText()
	{
	}

	string ValidatorText::IsValid(void* handle, const string& val)
	{
		return wxStr(ToTextValidator(handle)->IsValid(wxStr(val)));
	}

	int64_t ValidatorText::GetStyle(void* handle)
	{
		return ToTextValidator(handle)->GetStyle();
	}

	void ValidatorText::SetStyle(void* handle, int64_t style)
	{
		ToTextValidator(handle)->SetStyle(style);
	}

	void ValidatorText::SetCharIncludes(void* handle, const string& chars)
	{
		ToTextValidator(handle)->SetCharIncludes(wxStr(chars));
	}

	void ValidatorText::AddCharIncludes(void* handle, const string& chars)
	{
		ToTextValidator(handle)->AddCharIncludes(wxStr(chars));
	}

	string ValidatorText::GetCharIncludes(void* handle)
	{
		return wxStr(ToTextValidator(handle)->GetCharIncludes());
	}

	void ValidatorText::AddInclude(void* handle, const string& include)
	{
		ToTextValidator(handle)->AddInclude(wxStr(include));
	}

	void ValidatorText::AddExclude(void* handle, const string& exclude)
	{
		ToTextValidator(handle)->AddExclude(wxStr(exclude));
	}

	void ValidatorText::SetCharExcludes(void* handle, const string& chars)
	{
		ToTextValidator(handle)->SetCharExcludes(wxStr(chars));
	}

	void ValidatorText::AddCharExcludes(void* handle, const string& chars)
	{
		ToTextValidator(handle)->AddCharExcludes(wxStr(chars));
	}

	string ValidatorText::GetCharExcludes(void* handle)
	{												   
		return wxStr(ToTextValidator(handle)->GetCharExcludes());
	}

	void ValidatorText::ClearExcludes(void* handle)
	{
		wxArrayString a = wxArrayString();
		ToTextValidator(handle)->SetExcludes(a);
	}

	void ValidatorText::ClearIncludes(void* handle)
	{
		wxArrayString a = wxArrayString();
		ToTextValidator(handle)->SetIncludes(a);
	}
}
