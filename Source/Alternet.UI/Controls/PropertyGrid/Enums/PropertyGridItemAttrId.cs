using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines supported property attributes for <see cref="PropertyGrid"/> items.
    /// </summary>
    public enum PropertyGridItemAttrId
    {
        /// <summary>
        /// Set default value for property.
        /// </summary>
        DefaultValue,

        /// <summary>
        /// Universal, int or double. Minimum value for numeric properties.
        /// </summary>
        Min,

        /// <summary>
        /// Universal, int or double. Maximum value for numeric properties.
        /// </summary>
        Max,

        /// <summary>
        /// Universal, string. When set, will be shown as text after the displayed
        /// text value. Alternatively, if third column is enabled, text will be shown
        /// there (for any type of property).
        /// </summary>
        Units,

        /// <summary>
        /// When set, will be shown as 'greyed' text in property's value cell when
        /// the actual displayed value is blank.
        /// </summary>
        Hint,

        /// <summary>
        /// Universal, ArrayString. Set to enable auto-completion in any
        /// wxTextCtrl-based property editor.
        /// </summary>
        AutoComplete,

        /// <summary>
        /// BoolProperty and FlagsProperty specific. Value type is bool.
        /// Default value is False.
        /// When set to True, bool property will use check box instead of a
        /// combo box as its editor control. If you set this attribute
        /// for a FlagsProperty, it is automatically applied to child
        /// bool properties.
        /// </summary>
        UseCheckbox,

        /// <summary>
        /// BoolProperty and FlagsProperty specific. Value type is bool.
        /// Default value is False.
        /// Set to True for the bool property to cycle value on double click
        /// (instead of showing the popup listbox). If you set this attribute
        /// for a wxFlagsProperty, it is automatically applied to child
        /// bool properties.
        /// </summary>
        UseDClickCycling,

        /// <summary>
        /// FloatProperty (and similar) specific, int, default -1.
        /// Sets the (max) precision used when floating point value is rendered as
        /// text. The default -1 means infinite precision.
        /// </summary>
        Precision,

        /// <summary>
        /// The text will be echoed as asterisks.
        /// </summary>
        Password,

        /// <summary>
        /// Define base used by a UIntProperty. Valid constants are
        /// wxPG_BASE_OCT (= 8), wxPG_BASE_DEC (= 10), wxPG_BASE_HEX (= 16)
        /// and wxPG_BASE_HEXL (= 32)
        /// (lowercase characters).
        /// </summary>
        Base,

        /// <summary>
        /// Define prefix rendered to wxUIntProperty. Accepted constants
        /// wxPG_PREFIX_NONE (= 0), wxPG_PREFIX_0x (= 1), and wxPG_PREFIX_DOLLAR_SIGN (= 2).
        /// Only wxPG_PREFIX_NONE works with Decimal and Octal numbers.
        /// </summary>
        Prefix,

        /// <summary>
        /// Specific to EditorDialogProperty and derivatives, String, default is empty.
        /// Sets a specific title for the editor dialog.
        /// </summary>
        DialogTitle,

        /// <summary>
        /// FileProperty/ImageFileProperty specific, String, default is
        /// detected/varies.
        /// Sets the wildcard used in the triggered wxFileDialog. Format is the same.
        /// </summary>
        Wildcard,

        /// <summary>
        /// FileProperty/ImageFileProperty specific, int, default 1.
        /// When 0, only the file name is shown (i.e. drive and directory are hidden).
        /// </summary>
        ShowFullPath,

        /// <summary>
        /// Specific to FileProperty and derived properties, String, default empty.
        /// If set, then the filename is shown relative to the given path string.
        /// </summary>
        ShowRelativePath,

        /// <summary>
        /// Specific to FileProperty and derived properties, wxString,
        /// default is empty.
        /// Sets the initial path of where to look for files.
        /// </summary>
        InitialPath,

        /// <summary>
        /// Specific to FileProperty and derivatives, long, default is 0.
        /// Sets a specific FileDialog style for the file dialog.
        /// </summary>
        DialogStyle,

        /// <summary>
        /// ArrayStringProperty's string delimiter character. If this is
        /// a quotation mark or hyphen, then strings will be quoted instead
        /// (with given character).
        /// Default delimiter is quotation mark.
        /// </summary>
        Delimiter,

        /// <summary>
        /// Sets displayed date format for DateProperty.
        /// </summary>
        DateFormat,

        /// <summary>
        /// Sets DateTimePicker window style used with DateProperty. Default
        /// is (Default | ShowCentury). Using AllowNone will enable
        /// better unspecified value support in the editor.
        /// </summary>
        PickerStyle,

        /// <summary>
        /// SpinCtrl editor, int or double. How much number changes when button is
        /// pressed (or up/down on keyboard).
        /// </summary>
        Step,

        /// <summary>
        /// SpinCtrl editor, bool. If true, value wraps at Min/Max.
        /// </summary>
        Wrap,

        /// <summary>
        /// SpinCtrl editor, bool. If true, moving mouse when one of the spin
        /// buttons is depressed rapidly changing "spin" value.
        /// </summary>
        MotionSpin,

        /// <summary>
        /// MultiChoiceProperty, int.
        /// If 0, no user strings allowed. If 1, user strings appear before list
        /// strings. If 2, user strings appear after list string.
        /// </summary>
        UserStringMode,

        /// <summary>
        /// ColorProperty and its kind, int, default 1.
        /// Setting this attribute to 0 hides custom colour from property's list of
        /// choices.
        /// </summary>
        AllowCustom,

        /// <summary>
        /// wxColorProperty and its kind: Set to True in order to support editing
        /// alpha colour component.
        /// </summary>
        HasAlpha,
    }
}
