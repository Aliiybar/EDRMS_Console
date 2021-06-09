using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models.SharedModels
{
    public class CoreEntity
    {
        [JsonProperty("TermGuid")]
        public string TermGuid { get; set; }
        public string TermName { get; set; }
        [JsonProperty("Label")]
        public string Label { get; set; }
    }

    public class CoreEntityProperties
    {
        public Guid TermGuid { get; set; }
        public string TermName { get; set; }
    }
}
