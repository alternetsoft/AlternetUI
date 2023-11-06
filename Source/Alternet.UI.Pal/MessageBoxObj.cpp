#include "MessageBoxObj.h"

namespace Alternet::UI
{
    MessageBoxResult MessageBoxObj::Show(
        Window* owner,
        const string& text,
        optional<string> caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton)
    {
        auto style = GetStyle(buttons, icon, defaultButton);

        auto wxOwner = owner != nullptr ? owner->GetWxWindow() : nullptr;

        auto wxResult = wxMessageBox(
            wxStr(text),
            wxStr(caption.value_or(u"")),
            style,
            wxOwner);

        auto result = GetResult(wxResult);
        return result;
    }

    /*static*/ long MessageBoxObj::GetStyle(MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
    {
        long style = 0;

        switch (buttons)
        {
        case MessageBoxButtons::OK:
            style |= wxOK;
            break;
        case MessageBoxButtons::OKCancel:
            style |= wxOK | wxCANCEL;
            break;
        case MessageBoxButtons::YesNoCancel:
            style |= wxYES_NO | wxCANCEL;
            break;
        case MessageBoxButtons::YesNo:
            style |= wxYES_NO;
            break;
        default:
            throwExInvalidArgEnumValue(buttons);
        }

        switch (icon)
        {
        case MessageBoxIcon::None:
        default:
            style |= wxICON_NONE;
            break;
        case MessageBoxIcon::Information:
            style |= wxICON_INFORMATION;
            break;
        case MessageBoxIcon::Warning:
            style |= wxICON_WARNING;
            break;
        case MessageBoxIcon::Error:
            style |= wxICON_ERROR;
            break;
        case MessageBoxIcon::Question:
            style |= wxICON_QUESTION;
            break;
        }

        switch (defaultButton)
        {
        default:
        case MessageBoxDefaultButton::OK:
            style |= wxOK_DEFAULT;
            break;
        case MessageBoxDefaultButton::Cancel:
            style |= wxCANCEL_DEFAULT;
            break;
        case MessageBoxDefaultButton::Yes:
            style |= wxYES_DEFAULT;
            break;
        case MessageBoxDefaultButton::No:
            style |= wxNO_DEFAULT;
            break;
        }

        return style;
    }

    MessageBoxResult MessageBoxObj::GetResult(int wxResult)
    {
        MessageBoxResult result;
        switch (wxResult)
        {
        case wxOK:
            result = MessageBoxResult::OK;
            break;
        case wxYES:
            result = MessageBoxResult::Yes;
            break;
        case wxNO:
            result = MessageBoxResult::No;
            break;
        case wxCANCEL:
            result = MessageBoxResult::Cancel;
            break;
        default:
            throwEx(u"Unexpected wxMessageBox value.");
        }

        return result;
    }
}