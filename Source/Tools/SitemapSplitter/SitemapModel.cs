
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9", IsNullable = false)]
public partial class urlset
{

    private urlsetUrl[] urlField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("url")]
    public urlsetUrl[] url
    {
        get
        {
            return this.urlField;
        }
        set
        {
            this.urlField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
public partial class urlsetUrl
{

    private string locField;

    private System.DateTime lastmodField;

    private string changefreqField;

    private decimal priorityField;

    /// <remarks/>
    public string loc
    {
        get
        {
            return this.locField;
        }
        set
        {
            this.locField = value;
        }
    }

    /// <remarks/>
    public System.DateTime lastmod
    {
        get
        {
            return this.lastmodField;
        }
        set
        {
            this.lastmodField = value;
        }
    }

    /// <remarks/>
    public string changefreq
    {
        get
        {
            return this.changefreqField;
        }
        set
        {
            this.changefreqField = value;
        }
    }

    /// <remarks/>
    public decimal priority
    {
        get
        {
            return this.priorityField;
        }
        set
        {
            this.priorityField = value;
        }
    }
}

