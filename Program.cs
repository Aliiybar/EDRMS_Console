using EDRMS.DemoConsole.App.Models;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDRMS.DemoConsole.App
{
    class Program
    {
        private const string clientId = "e75d10d3-191d-4d04-9637-b4bade9a3876";
        private const string secret = "fzxhUZl2XfgxFkNhEG15bKDU+78GWVkl2J42NvHYRdY=";        
        // To test Gas Certificates Function
        private const string clientId1 = "34e05b42-8d0a-40ab-9bb0-432d2015cd7a";
        private const string secret1 = "nCvypfeecXdOyFcTB+99T85kb3zWgdTewchanLUpZJc=";


        private const string tenantId = "da999321-031a-4f61-80ef-a3679f77e3b4";
        private const string resourceId = "00000003-0000-0ff1-ce00-000000000000/traffordhousingtrust.sharepoint.com";
        private const string baseAddress = "https://accounts.accesscontrol.windows.net/";

        static void Main(string[] args)
        {
            //Generate Token
            string token = string.Empty;
            Console.WriteLine("generating token.....");
            Console.WriteLine("======================");

            var client = new HttpClient();
            var nameValueCollection = new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id",  clientId1 + "@"+ tenantId),
                new KeyValuePair<string, string>("client_secret", secret1),
                new KeyValuePair<string, string>("resource", resourceId + "@" + tenantId)
            };

            var url = baseAddress + tenantId + "/tokens/OAuth/2";
            var result = client.PostAsync(url, new FormUrlEncodedContent(nameValueCollection)).Result;
            if (result.IsSuccessStatusCode)
            {
                var tokenObj = result.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<SharePointToken>(tokenObj);
                token = data.access_token;
                Console.WriteLine("token: " + token);
                Console.WriteLine("=================");
            }

            //LIST - Operations

            Services.GetGasCertificates(token);
           // Services.GetCoreEntityListData(token);
            //Services.GetConfigSettingsListData(token);
            //Services.GetListDataByEntity(token);
            //Services.StagedDocList(token);
            //Services.GetCoreEntitiesList(token);
            //Services.GetContractorsList(token);
            //Services.GetRulesForMetadataField(token);


            //UPLOAD - Operations

            //Uploader.UploadFile(token);
            //Uploader.UpdateStagedDoc(token);
            //Uploader.DownloadFile(token);
            //Uploader.CreateContractorTaxonomyTerm(token);
            //Uploader.MoveDocument(token);
            //Uploader.UploadFileToDestination(token);
            //Uploader.DeleteDocument(token);


            //Permissions
            //Permissions.GetPrimaryAdmin(token);
            //Permissions.IsSiteAdmin(token);
            //Permissions.GetAllUsers(token);
            //Permissions.GetAllGroupsWithUsers(token);
            //Permissions.GetUserGroups(token);
            //Permissions.GetCoreEntitiesByUser(token);
            //Permissions.GetDocTypesByUser(token);
        }
    }
}
