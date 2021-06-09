using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models
{
    public class GenericRulesConfigSettingsList
    {
        [JsonProperty("RuleType")]
        public string RuleType { get; set; }

        [JsonProperty("RuleDescription")]
        public string RuleDescription { get; set; }
    }
}
