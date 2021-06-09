using EDRMS.DemoConsole.App.Models.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models
{
    public class MetadataRulesConfigSettingsList
    {
        [JsonProperty("MetadataInternalName")]
        public string MetadataInternalName { get; set; }

        [JsonProperty("DocumentType")]
        public DocumentType DocumentType { get; set; }

        [JsonProperty("RulesLookUp")]
        public List<RulesLookUp> RulesList { get; set; }
        
    }
}
