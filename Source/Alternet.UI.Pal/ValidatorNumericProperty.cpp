#include "ValidatorNumericProperty.h"

namespace Alternet::UI
{
	ValidatorNumericProperty::ValidatorNumericProperty()
	{

	}

	ValidatorNumericProperty::~ValidatorNumericProperty()
	{
	}

	void ValidatorNumericProperty::DeleteValidatorNumericProperty(void* handle)
	{
		delete (wxNumericPropertyValidator*)handle;
	}

	void* ValidatorNumericProperty::CreateValidatorNumericProperty(int numericType, int valBase)
	{
		return new wxNumericPropertyValidator(
			(wxNumericPropertyValidator::NumericType)numericType, valBase);
	}
}
