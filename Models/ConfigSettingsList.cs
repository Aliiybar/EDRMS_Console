using EDRMS.DemoConsole.App.Models.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models
{
    public class ConfigSettingsListFields
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("DocumentType")]
        public DocumentType DocumentType { get; set; }

        [JsonProperty("Directorate")]
        public CoreEntity CoreEntity { get; set; }

        [JsonProperty("BusinessArea")]
        public BusinessArea BusinessArea { get; set; }

        [JsonProperty("DocumentContentType")]
        public string DocumentContentType { get; set; }

        [JsonProperty("PermissionGroups")]
        public List<PermissionGroup> PermissionGroups { get; set; }

        [JsonProperty("ExpiryPeriod")]
        public decimal ExpiryPeriod { get; set; }

        //POCO
        public List<ContentTypeFields> ContentTypeProperties { get; set; }
    }

    public class ConfigSettingsListResults
    {
        [JsonProperty("results")]
        public List<ConfigSettingsListFields> results { get; set; }
    }

    public class ConfigSettingsList
    {
        [JsonProperty("d")]
        public ConfigSettingsListResults data { get; set; }
    }
    

}
