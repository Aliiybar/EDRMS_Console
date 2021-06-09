using EDRMS.DemoConsole.App.Models.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models
{
    public class CoreEntitySettingsListFields
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Directorate")]
        public CoreEntity CoreEntity { get; set; }

        [JsonProperty("PrimaryFieldInternalName")]
        public string PrimaryFieldInternalName { get; set; }

        [JsonProperty("SiteUrl")]
        public string SiteUrl { get; set; }

        [JsonProperty("StagePathUrl")]
        public string StagePathUrl { get; set; }

        [JsonProperty("PermissionGroups")]
        public List<PermissionGroup> PermissionGroups { get; set; }
    }

    public class CoreEntitySettingsListResult
    {
        [JsonProperty("results")]
        public List<CoreEntitySettingsListFields> results { get; set; }
    }

    public class CoreEntitySettingsList
    {
        [JsonProperty("d")]
        public CoreEntitySettingsListResult data { get; set; }
    }
}
