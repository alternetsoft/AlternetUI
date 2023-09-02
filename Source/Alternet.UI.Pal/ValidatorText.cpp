#include "ValidatorText.h"

namespace Alternet::UI
{
	void ValidatorText::DeleteValidatorText(void* handle)
	{
		delete (wxTextValidator*)handle;
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
}
