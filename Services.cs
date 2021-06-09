using EDRMS.DemoConsole.App.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Collections.Generic;
using EDRMS.DemoConsole.App.Models.StagedDocs;
using EDRMS.DemoConsole.App.Models.SharedModels;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using ContentType = EDRMS.DemoConsole.App.Models.ContentType;

namespace EDRMS.DemoConsole.App
{
    public class Services
    {
        private static string baseUrl = "https://traffordhousingtrust.sharepoint.com";
        private static string siteUrl = baseUrl + "/sites/edrms-uat";

        public static void GetCoreEntitiesList(string token)
        {
            Console.WriteLine("getting contractors list...");
            Console.WriteLine("=============================");

            List<CoreEntityProperties> entities = new List<CoreEntityProperties>();
            Guid termStoreId = Guid.Parse("31b4bc290a244ae2beca96609810c505");
            Guid termGroupId = Guid.Parse("cdbc062b-53ae-4675-b35c-50b5d3fab214");
            Guid entityTermSetId = Guid.Parse("e2bc50d0-d798-4fa9-97ed-3c1c133ce7f7");
            string baseUrl = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat";

            using (ClientContext ctx = Uploader.GetClientContext(baseUrl, token))
            {
                // Get the TaxonomySession
                TaxonomySession taxonomySession = TaxonomySession.GetTaxonomySession(ctx);

                // Get the term store by Id
                TermStore termStore = taxonomySession.TermStores.GetById(termStoreId);

                // Get the term group by Id
                TermGroup termGroup = termStore.Groups.GetById(termGroupId);

                // Get the term set by Id
                TermSet termSet = termGroup.TermSets.GetById(entityTermSetId);

                // Get the term by Name 
                TermCollection terms = termSet.GetAllTerms();

                ctx.Load(terms);
                ctx.ExecuteQuery();

                if (terms != null && terms.Any())
                {
                    foreach (var c in terms)
                    {
                        CoreEntityProperties item = new CoreEntityProperties
                        {
                            TermGuid = c.Id,
                            TermName = c.Name
                        };

                        entities.Add(item);
                    }
                }
            }
        }

        public static void GetContractorsList(string token)
        {
            Console.WriteLine("getting contractors list...");
            Console.WriteLine("=============================");
            #region "----NOT NEEDED CODE----DO NOT REMOVE"

            //string url = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat/_api/web/lists/getbytitle('TaxonomyHiddenList')/items?$filter=IdForTermSet eq '" + contractorTermSetId + "'";
            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
            //var res = client.GetAsync(url).Result;
            //if (res.IsSuccessStatusCode)
            //{
            //    var data = res.Content.ReadAsStringAsync().Result;
            //    var listData = JsonConvert.DeserializeObject<ContractorList>(data).data.results;
            //}

            #endregion


            List<ContractorProperties> contractors = new List<ContractorProperties>();
            Guid termStoreId = Guid.Parse("31b4bc290a244ae2beca96609810c505");
            Guid termGroupId = Guid.Parse("cdbc062b-53ae-4675-b35c-50b5d3fab214");
            Guid contractorTermSetId = Guid.Parse("5e7f7b17-a35e-403f-a836-4de99216a492");
            string baseUrl = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat";

            using (ClientContext ctx = Uploader.GetClientContext(baseUrl, token))
            {
                // Get the TaxonomySession
                TaxonomySession taxonomySession = TaxonomySession.GetTaxonomySession(ctx);

                // Get the term store by Id
                TermStore termStore = taxonomySession.TermStores.GetById(termStoreId);

                // Get the term group by Id
                TermGroup termGroup = termStore.Groups.GetById(termGroupId);

                // Get the term set by Id
                TermSet termSet = termGroup.TermSets.GetById(contractorTermSetId);

                // Get the term by Name 
                TermCollection terms = termSet.GetAllTerms();

                ctx.Load(terms);
                ctx.ExecuteQuery();

                if (terms != null && terms.Any())
                {
                    foreach (var c in terms)
                    {
                        ContractorProperties item = new ContractorProperties
                        {
                            TermGuid = c.Id,
                            TermName = c.Name
                        };

                        contractors.Add(item);
                    }
                }
            }
        }

        public static string GetTaxonomyTerm(string termId, string token)
        {
            string baseUrl = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat";
            using (ClientContext ctx = Uploader.GetClientContext(baseUrl, token))
            {
                // Get the TaxonomySession
                TaxonomySession taxonomySession = TaxonomySession.GetTaxonomySession(ctx);
                var termObj = taxonomySession.GetTerm(Guid.Parse(termId));
                ctx.Load(termObj);
                ctx.ExecuteQuery();
                return termObj.Name;
            }

            #region "----CODE NOT NEEDED - API METHOD----"

            //string term = string.Empty;
            //Console.WriteLine("getting taxonomy data...");
            //Console.WriteLine("=============================");
            //string url = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat/_api/web/lists/getbytitle('TaxonomyHiddenList')/items?$filter=IdForTerm eq '" + termId + "'";
            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
            //var res = client.GetAsync(url).Result;
            //if (res.IsSuccessStatusCode)
            //{
            //    var data = res.Content.ReadAsStringAsync().Result;
            //    var taxonomyData = JsonConvert.DeserializeObject<TaxonomyData>(data).data.results.First();
            //    term = taxonomyData.TermName;
            //}

            //return term;

            #endregion
        }

        public static List<ContentTypeFields> GetContentTypeData(string contentTypeName, string token)
        {
            List<ContentTypeFields> contentFields = null;
            Console.WriteLine("getting content type data...");
            Console.WriteLine("=============================");
            string url = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat/_api/web/AvailableContentTypes?$select=Name,StringId&$filter=Name eq '" + contentTypeName + "'";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
            var res = client.GetAsync(url).Result;
            if (res.IsSuccessStatusCode)
            {
                var data = res.Content.ReadAsStringAsync().Result;
                var contentData = JsonConvert.DeserializeObject<ContentType>(data).data.results.First();

                //get content fields data
                Console.WriteLine("getting content type data...");
                Console.WriteLine("=============================");

                string contentUrl = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat/_api/web/AvailableContentTypes('" + contentData.ContentTypeId + "')/fields?$filter=Hidden eq false and Group ne '_Hidden'";
                var contentResult = client.GetAsync(contentUrl).Result;
                if (contentResult.IsSuccessStatusCode)
                {
                    var cData = contentResult.Content.ReadAsStringAsync().Result;
                    var cFields = JsonConvert.DeserializeObject<ContentTypeFieldsData>(cData).data.results;

                    if (cFields != null && cFields.Any())
                    {
                        foreach (var f in cFields)
                        {
                            f.ContentTypeId = contentData.ContentTypeId;
                            f.ContentTypeName = contentData.ContentTypeName;
                        }
                    }

                    contentFields = cFields;
                }
            }

            return contentFields;
        }

        public static List<ListItem> GetGasCertificates(string token)
        {
            siteUrl = baseUrl + "/Compliance";
            List<ListItem> items = new List<ListItem>();
            using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
            {
                Web myWeb = ctx.Web;
                List myList = myWeb.Lists.GetByTitle("GasCertificates");
                ListItemCollectionPosition position = null;
                // Page Size: 4000
                int rowLimit = 4000;
                ctx.Load(myList);
                ctx.ExecuteQuery();

                //configure CAML query
                CamlQuery query = new CamlQuery();

                query.ViewXml = @"<View Scope='RecursiveAll'>
                    <Query>
                        <OrderBy Override='TRUE'><FieldRef Name='ID'/></OrderBy>
                    </Query>
                    <ViewFields>
                        <FieldRef Name='Title'/><FieldRef Name='Modified' /><FieldRef Name='Editor' />
                    </ViewFields>
                    <RowLimit Paged='TRUE'>" + rowLimit + "</RowLimit></View>";
                do
                {
                    ListItemCollection listItems = null;
                    query.ListItemCollectionPosition = position;
                    listItems = myList.GetItems(query);
                    ctx.Load(listItems);
                    ctx.ExecuteQuery();
                    position = listItems.ListItemCollectionPosition;
                    items.AddRange(listItems.ToList());
                } while (position != null);
            }

            return items;
        }

        public static void GetCoreEntityListData(string token)
        {
            #region "USING --- API CALL"

            //Console.WriteLine("getting core entity settings list items...");
            //Console.WriteLine("=============================");
            //string url = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat/_api/web/lists/getbytitle('CoreEntityConfigSettings')/items?$select *";
            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
            //var res = client.GetAsync(url).Result;
            //if (res.IsSuccessStatusCode)
            //{
            //    var data = res.Content.ReadAsStringAsync().Result;
            //    var listData = JsonConvert.DeserializeObject<CoreEntitySettingsList>(data).data.results;

            //    //get taxonomy data
            //    if(listData!=null && listData.Any())
            //    {
            //        foreach(var i in listData)
            //        {
            //            i.CoreEntity.TermName = GetTaxonomyTerm(i.CoreEntity.TermGuid, token);
            //        }
            //    }
            //}

            #endregion

            #region "USING --- CSOM"

            List<CoreEntitySettingsListFields> list = new List<CoreEntitySettingsListFields>();
            using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
            {
                Web myWeb = ctx.Web;
                List myList = myWeb.Lists.GetByTitle("CoreEntityConfigSettings");
                ctx.Load(myList);
                ctx.ExecuteQuery();

                //configure VIEW
                View myView = myList.Views.GetByTitle("Default");
                ctx.Load(myView);
                ctx.ExecuteQuery();

                //configure CAML query
                CamlQuery query = new CamlQuery();
                query.ViewXml = myView.ViewQuery;

                ListItemCollection items = myList.GetItems(query);
                ctx.Load(items);
                ctx.ExecuteQuery();

                //select the metadata info from the list
                var listInfo = items.Select(i => (i.FieldValues));
                var jsonObj= JsonConvert.SerializeObject(listInfo);
                list = JsonConvert.DeserializeObject<List<CoreEntitySettingsListFields>>(jsonObj);

                if (list != null && list.Any())
                {
                    foreach (var i in list)
                    {
                        i.CoreEntity.TermName = i.CoreEntity.Label;
                    }
                }
            }

            #endregion
        }

        public static void GetConfigSettingsListData(string token)
        {
            Console.WriteLine("getting list items...");
            Console.WriteLine("=============================");
            string url = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat/_api/web/lists/getbytitle('DocTypeConfigSettings')/items?$select *";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
            var res = client.GetAsync(url).Result;
            if (res.IsSuccessStatusCode)
            {
                var data = res.Content.ReadAsStringAsync().Result;
                var listData = JsonConvert.DeserializeObject<ConfigSettingsList>(data).data.results;

                //get content type data and taxonomy data
                if (listData != null && listData.Any())
                {
                    foreach (var i in listData)
                    {
                        i.ContentTypeProperties = GetContentTypeData(i.DocumentContentType, token);
                        i.CoreEntity.TermName = GetTaxonomyTerm(i.CoreEntity.TermGuid, token);
                        i.BusinessArea.TermName = GetTaxonomyTerm(i.BusinessArea.TermGuid, token);
                        i.DocumentType.TermName = GetTaxonomyTerm(i.DocumentType.TermGuid, token);
                    }
                }
            }

        }

        public static void GetListDataByEntity(string token)
        {
            //Entity=Property
            string entityId = "8a8ecd3d-6e38-4d55-b37e-6cf3e7a1dcbe";
            Console.WriteLine("getting list items...");
            Console.WriteLine("=============================");
            string url = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat/_api/web/lists/getbytitle('DocTypeConfigSettings')/items?$select *&$filter=TaxCatchAll/IdForTerm eq '" + entityId + "'";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
            var res = client.GetAsync(url).Result;
            if (res.IsSuccessStatusCode)
            {
                var data = res.Content.ReadAsStringAsync().Result;
                var listData = JsonConvert.DeserializeObject<ConfigSettingsList>(data).data.results;

                //get content type data and taxonomy data
                if (listData != null && listData.Any())
                {
                    foreach (var i in listData)
                    {
                        i.ContentTypeProperties = GetContentTypeData(i.DocumentContentType, token);
                        i.CoreEntity.TermName = GetTaxonomyTerm(i.CoreEntity.TermGuid, token);
                        i.BusinessArea.TermName = GetTaxonomyTerm(i.BusinessArea.TermGuid, token);
                        i.DocumentType.TermName = GetTaxonomyTerm(i.DocumentType.TermGuid, token);
                    }
                }
            }
        }

        public static void StagedDocList(string token)
        {
            Console.WriteLine("getting staged doc items...");
            Console.WriteLine("=============================");
            string url = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat/property-uat/_api/web/lists/getbytitle('StagedDocuments')/items?$select=FileLeafRef,FileRef,*";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
            var res = client.GetAsync(url).Result;
            if (res.IsSuccessStatusCode)
            {
                var data = res.Content.ReadAsStringAsync().Result;
                var listData = JsonConvert.DeserializeObject<BuildingSafetyCertificateDoc>(data).data.results;

                //get taxonomy data
                if (listData != null && listData.Any())
                {
                    foreach (var i in listData)
                    {
                        if (i.BusinessArea != null)
                        {
                            i.BusinessArea.TermName = GetTaxonomyTerm(i.BusinessArea.TermGuid, token);
                        }

                        if (i.DocumentType != null)
                        {
                            i.DocumentType.TermName = GetTaxonomyTerm(i.DocumentType.TermGuid, token);
                        }

                        if (i.Contractor != null)
                        {
                            i.Contractor.TermName = GetTaxonomyTerm(i.Contractor.TermGuid, token);
                        }

                    }
                }
            }

        }

        public static void GetRulesForMetadataField(string token)
        {
            //string metaDataCol = "PlaceRef";
            string docTypeTermGuid = "9e1db995-6b1c-43e8-b196-b5f9ed149703";

            List<MetadataRulesConfigSettingsList> rulesList = new List<MetadataRulesConfigSettingsList>();

            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    Web myWeb = ctx.Web;
                    List myList = myWeb.Lists.GetByTitle("MetadataFieldRulesConfigSettings");
                    ctx.Load(myList);
                    ctx.ExecuteQuery();

                    //configure VIEW
                    View myView = myList.Views.GetByTitle("Default");
                    ctx.Load(myView);
                    ctx.ExecuteQuery();

                    //configure CAML query
                    CamlQuery query = new CamlQuery();
                    query.ViewXml = myView.ViewQuery;

                    ListItemCollection items = myList.GetItems(query);
                    ctx.Load(items);
                    ctx.ExecuteQuery();

                    //select the metadata info from the list
                    var listInfo = items.Select(i => (i.FieldValues));

                    //Data mapping
                    var jsonObj = JsonConvert.SerializeObject(listInfo);
                    rulesList= JsonConvert.DeserializeObject<List<MetadataRulesConfigSettingsList>>(jsonObj);

                    //Apply DocTypeID as filter
                    var filteredRulesList = rulesList.Where(i => i.DocumentType.TermGuid == docTypeTermGuid).ToList();
                    if(filteredRulesList!=null && filteredRulesList.Any())
                    {
                        foreach(var r in filteredRulesList)
                        {
                            var rules = r.RulesList;
                            if(rules!=null && rules.Any())
                            {
                                foreach(var i in rules)
                                {
                                    i.LookUpType = GetGenericRuleType(token, i.LookUpValue);
                                }
                            }
                        }
                    }


                    //Apply PlaceRef as filter
                    //var filteredItems = listInfo.Where(i => (i["MetadataInternalName"].ToString() == metaDataCol));
                    //var jsonObj= JsonConvert.SerializeObject(filteredItems);
                    //rulesList = JsonConvert.DeserializeObject<List<MetadataRulesConfigSettingsList>>(jsonObj);
                }
            }
            catch (Exception ex)
            {

            }
        }


        public static string GetGenericRuleType(string token,string ruleDesc)
        {
            string ruleType = string.Empty;

            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    Web myWeb = ctx.Web;
                    List myList = myWeb.Lists.GetByTitle("GenericRulesConfigSettings");
                    ctx.Load(myList);
                    ctx.ExecuteQuery();

                    //configure VIEW
                    View myView = myList.Views.GetByTitle("Default");
                    ctx.Load(myView);
                    ctx.ExecuteQuery();

                    //configure CAML query
                    CamlQuery query = new CamlQuery();
                    query.ViewXml = myView.ViewQuery;

                    ListItemCollection items = myList.GetItems(query);
                    ctx.Load(items);
                    ctx.ExecuteQuery();

                    //select the metadata info from the list
                    var listInfo = items.Select(i => (i.FieldValues));

                    //Data mapping
                    var jsonObj = JsonConvert.SerializeObject(listInfo);
                    var rulesList = JsonConvert.DeserializeObject<List<GenericRulesConfigSettingsList>>(jsonObj);

                    //Apply RuleDesc as filter
                    ruleType= rulesList.Where(i => i.RuleDescription.ToLower() == ruleDesc.ToLower()).FirstOrDefault().RuleType;
                }
            }
            catch (Exception ex)
            {

            }

            return ruleType;
        }

    }
}
