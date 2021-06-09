using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models
{
    public class TaxonomyDataFields
    {
        [JsonProperty("Title")]
        public string TermName { get; set; }

        [JsonProperty("IdForTermStore")]
        public string TermStoreId { get; set; }

        [JsonProperty("IdForTermSet")]
        public string TermSetId { get; set; }
    }

    public class TaxonomyDataResults
    {
        [JsonProperty("results")]
        public List<TaxonomyDataFields> results { get; set; }
    }

    public class TaxonomyData
    {
        [JsonProperty("d")]
        public TaxonomyDataResults data { get; set; }
    }
}
