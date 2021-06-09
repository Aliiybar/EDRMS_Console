using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models.SharedModels
{
    public class Contractor
    {
        [JsonProperty("TermGuid")]
        public string TermGuid { get; set; }
        public string TermName { get; set; }
        [JsonProperty("Label")]
        public string Label { get; set; }

    }

    public class ContractorProperties
    {
        public Guid TermGuid { get; set; }
        public string TermName { get; set; }
    }

    public class ContractorFields
    {
        [JsonProperty("IdForTerm")]
        public string ContractorId { get; set; }
        [JsonProperty("Title")]
        public string ContractorName { get; set; }
    }

    public class ContractorListResults
    {
        [JsonProperty("results")]
        public List<ContractorFields> results { get; set; }
    }

    public class ContractorList
    {
        [JsonProperty("d")]
        public ContractorListResults data { get; set; }
    }
}
