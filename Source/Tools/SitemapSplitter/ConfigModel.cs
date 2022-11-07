
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class Configuration
{

    private ConfigurationOutput outputField;

    private ConfigurationPart[] partsField;

    /// <remarks/>
    public ConfigurationOutput Output
    {
        get
        {
            return this.outputField;
        }
        set
        {
            this.outputField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Part", IsNullable = false)]
    public ConfigurationPart[] Parts
    {
        get
        {
            return this.partsField;
        }
        set
        {
            this.partsField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigurationOutput
{

    private string fileNameField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FileName
    {
        get
        {
            return this.fileNameField;
        }
        set
        {
            this.fileNameField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigurationPart
{

    private string locationRegexField;

    private string outputFileSuffixField;

    /// <remarks/>
    public string LocationRegex
    {
        get
        {
            return this.locationRegexField;
        }
        set
        {
            this.locationRegexField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string OutputFileSuffix
    {
        get
        {
            return this.outputFileSuffixField;
        }
        set
        {
            this.outputFileSuffixField = value;
        }
    }
}

