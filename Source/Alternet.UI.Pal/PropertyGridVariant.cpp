#include "PropertyGridVariant.h"

namespace Alternet::UI
{
	/*static*/ void PropertyGridVariant::Delete(void* handle)
	{
		delete (wxVariant*)handle;
	}

	/*static*/ void* PropertyGridVariant::CreateVariant()
	{
		return new wxVariant();
	}
	
	PropertyGridVariant::PropertyGridVariant()
	{
	}
	
	PropertyGridVariant::~PropertyGridVariant()
	{
	}

}
