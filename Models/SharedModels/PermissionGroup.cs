using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models.SharedModels
{
    public class PermissionGroup
    {
        [JsonProperty("LookupId")]
        public int GroupId { get; set; }

        [JsonProperty("LookupValue")]
        public string GroupName { get; set; }
    }
}
