#include "PropertyGridVariant.h"

namespace Alternet::UI
{
	/*static*/ void PropertyGridVariant::Delete(void* handle)
	{
		delete (PropertyGridVariant*)handle;
	}

	/*static*/ void* PropertyGridVariant::CreateVariant()
	{
		return new PropertyGridVariant();
	}
	
	PropertyGridVariant::PropertyGridVariant()
	{
	}
	
	PropertyGridVariant::~PropertyGridVariant()
	{
	}

	wxVariant& PropertyGridVariant::ToVar(void* handle)
	{
		return ((PropertyGridVariant*)handle)->variant;
	}

	void PropertyGridVariant::FromVariant(void* handle, wxVariant& value)
	{
		((PropertyGridVariant*)handle)->variant = value;
	}

	bool PropertyGridVariant::IsNull(void* handle)
	{
		return ToVar(handle).IsNull();
	}

	bool PropertyGridVariant::Unshare(void* handle)
	{
		return ToVar(handle).Unshare();
	}

	void PropertyGridVariant::MakeNull(void* handle)
	{
		ToVar(handle).MakeNull();
	}

	void PropertyGridVariant::Clear(void* handle)
	{
		ToVar(handle).Clear();
	}

	string PropertyGridVariant::GetValueType(void* handle)
	{
		return wxStr(ToVar(handle).GetType());
	}

	bool PropertyGridVariant::IsType(void* handle, const string& type)
	{
		return ToVar(handle).IsType(wxStr(type));
	}

	string PropertyGridVariant::MakeString(void* handle) 
	{
		return wxStr(ToVar(handle).MakeString());
	}

	Color PropertyGridVariant::GetColor(void* handle)
	{
		wxVariant value = ToVar(handle);
		if (value.IsNull())
			return Color();
		wxColour result;
		result << value;
		return result;
	}

	void PropertyGridVariant::SetColor(void* handle, const Color& val) 
	{
		wxColor color = val;
		wxVariant v;
		v << color;
		FromVariant(handle, v);
	}

	double PropertyGridVariant::GetDouble(void* handle) 
	{
		return ToVar(handle).GetDouble();
	}

	bool PropertyGridVariant::GetBool(void* handle) 
	{
		return ToVar(handle).GetBool();
	}

	int64_t PropertyGridVariant::GetLong(void* handle) 
	{
		return ToVar(handle).GetLongLong().GetValue();
	}

	DateTime PropertyGridVariant::GetDateTime(void* handle) 
	{
		return ToVar(handle).GetDateTime();
	}

	string PropertyGridVariant::GetString(void* handle) 
	{
		return wxStr(ToVar(handle).GetString());
	}

	void PropertyGridVariant::SetDouble(void* handle, double val) 
	{
		wxVariant v = val;
		FromVariant(handle, v);
	}

	void PropertyGridVariant::SetBool(void* handle, bool val) 
	{
		wxVariant v = val;
		FromVariant(handle, v);
	}

	void PropertyGridVariant::SetLong(void* handle, int64_t val) 
	{
		wxVariant v = wxLongLong(val);
		FromVariant(handle, v);
	}

	void PropertyGridVariant::SetInt(void* handle, int val) 
	{
		wxVariant v = val;
		FromVariant(handle, v);
	}

	void PropertyGridVariant::SetShort(void* handle, int16_t val) 
	{
		wxVariant v = val;
		FromVariant(handle, v);
	}

	void PropertyGridVariant::SetDateTime(void* handle, const DateTime& val) 
	{
		wxVariant v = wxDateTime(val);
		FromVariant(handle, v);
	}

	void PropertyGridVariant::SetString(void* handle, const string& value) 
	{
		wxVariant v = wxStr(value);
		FromVariant(handle, v);
	}
}
