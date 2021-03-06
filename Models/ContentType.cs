using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models
{
    public class ContentTypeFields
    {
        [JsonProperty("EntityPropertyName")]
        public string EntityPropertyName { get; set; }

        [JsonProperty("StaticName")]
        public string StaticName { get; set; }

        [JsonProperty("InternalName")]
        public string InternalName { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Required")]
        public bool Required { get; set; }

        [JsonProperty("Hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("TypeAsString")]
        public string TypeAsString { get; set; }

        [JsonProperty("TypeDisplayName")]
        public string TypeDisplayName { get; set; }

        [JsonProperty("TypeShortDescription")]
        public string TypeShortDescription { get; set; }

        [JsonProperty("ValidationFormula")]
        public string ValidationFormula { get; set; }

        [JsonProperty("ValidationMessage")]
        public string ValidationMessage { get; set; }

        //POCO
        public string ContentTypeName { get; set; }

        public string ContentTypeId { get; set; }
    }

    public class ContentTypeFieldsResult
    {
        [JsonProperty("results")]
        public List<ContentTypeFields> results { get; set; }
    }

    public class ContentTypeFieldsData
    {
        [JsonProperty("d")]
        public ContentTypeFieldsResult data { get; set; }
    }

    public class ContentTypeProperties
    {
        [JsonProperty("Name")]
        public string ContentTypeName { get; set; }

        [JsonProperty("StringId")]
        public string ContentTypeId { get; set; }
    }

    public class ContentTypeDataResult
    {
        [JsonProperty("results")]
        public List<ContentTypeProperties> results { get; set; }
    }

    public class ContentType
    {
        [JsonProperty("d")]
        public ContentTypeDataResult data { get; set; }
    }
}
