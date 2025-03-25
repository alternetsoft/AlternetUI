#include "Validator.h"

namespace Alternet::UI
{
	Validator::Validator()
	{

	}

	Validator::~Validator()
	{
	}

	/*static*/ void Validator::SuppressBellOnError(bool suppress)
	{
		wxValidator::SuppressBellOnError(suppress);
	}

	/*static*/ bool Validator::IsSilent()
	{
		return wxValidator::IsSilent();
	}
}
