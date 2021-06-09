using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models.SharedModels
{
    public class RulesLookUp
    {
        [JsonProperty("LookupValue")]
        public string LookUpValue { get; set; }

        //POCO
        public string LookUpType { get; set; }
    }
}
