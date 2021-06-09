using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;


namespace EDRMS.DemoConsole.App
{
    public class Uploader
    {
        private static string libName = "StagedDocuments";
        private static string baseUrl = "https://traffordhousingtrust.sharepoint.com";
        private static string siteUrl = baseUrl + "/sites/edrms-uat/property-uat";

        public static ClientContext GetClientContext(string siteUrl, string token)
        {
            var clientContext = new ClientContext(siteUrl);
            clientContext.ExecutingWebRequest += (object sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.RequestHeaders.Add("Authorization", $"Bearer {token}");
            };
            return clientContext;
        }


        public static void UploadFile(string token)
        {
            string filePath = @"C:\THT-Other\testdoc00.pdf";
            string fileName = Path.GetFileName(filePath);
            string stagedDocPath = "/sites/edrms-uat/property-uat/" + libName+"/"+fileName;
            try
            {
                using (ClientContext CContext = GetClientContext(siteUrl, token))
                {
                    Web myWeb = CContext.Web;
                    List myLib = myWeb.Lists.GetByTitle(libName);

                    //check if the file already exists in staging area:
                    var file = myWeb.GetFileByServerRelativeUrl(stagedDocPath);
                    CContext.Load(file, f => f.Exists); // Only load the Exists property
                    CContext.ExecuteQuery();

                    if (file.Exists)
                    {
                        CContext.Load(file, f => f.ListItemAllFields);
                        CContext.ExecuteQuery();

                        int id = file.ListItemAllFields.Id;
                        //delete the document before re-upload
                    }

                    /// <summary>
                    /// Method 1: use FileCreationInformation to handle uploaded documents data
                    /// Tip: use ContentStream of FileCreationInformation class to upload large files...
                    /// </summary>

                    System.IO.FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    FileCreationInformation fcInfo = new FileCreationInformation();
                    fcInfo.ContentStream = fs;
                    fcInfo.Url = fileName;
                    fcInfo.Overwrite = true;
                    
                    Microsoft.SharePoint.Client.File uploadedFileRef = myLib.RootFolder.Files.Add(fcInfo);
                    
                    //load all contenttypes that are applicable for the doc-library
                    CContext.Load(myLib.ContentTypes);
                    CContext.ExecuteQuery();

                    //assign content type to the uploaded documents
                    ContentType myContentType = myLib.ContentTypes.Where(ctx => ctx.Name == "BuildingSafetyContentType").First();

                    //get Id for the uploaded file
                    CContext.Load(uploadedFileRef, f => f.ListItemAllFields);
                    CContext.ExecuteQuery();
                    int docId = uploadedFileRef.ListItemAllFields.Id;

                    uploadedFileRef.ListItemAllFields["ContentTypeId"] = myContentType.Id;
                    uploadedFileRef.ListItemAllFields["BusinessArea"] = "ad5fbfe8-ea91-4a70-83da-491ab48838ee";
                    uploadedFileRef.ListItemAllFields["PlaceRef"] = "00010222550";

                    uploadedFileRef.ListItemAllFields.Update();
                    CContext.ExecuteQuery();

                    //update taxonomy metadata field
                    //ListItem myListItem = myLib.GetItemById(docId);
                    //CContext.Load(myListItem);
                    //CContext.ExecuteQuery();

                    //UpdateTaxonomyField(CContext, myLib, myListItem, "DocumentType", "Asbestos Document", "e4576479-e951-4f43-8815-de26c6817749");
                    //myListItem.Update();
                    //CContext.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void UploadFileToDestination(string token)
        {
            string filePath = @"C:\THT-Other\testdoc02.pdf";
            string fileName = Path.GetFileName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string destinationLib = "BuildingSafetyDocuments";
            string destinationPath = destinationLib + "/Test/Dummy";

            try
            {
                using (ClientContext CContext = GetClientContext(siteUrl, token))
                {
                    Web myWeb = CContext.Web;
                    List myLib = myWeb.Lists.GetByTitle(destinationLib);
                    CContext.Load(myLib);
                    CContext.ExecuteQuery();

                    Folder folder = myWeb.GetFolderByServerRelativeUrl(destinationPath);
                    CContext.Load(folder);

                    System.IO.FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    FileCreationInformation fcInfo = new FileCreationInformation();
                    fcInfo.ContentStream = fs;
                    fcInfo.Url = fileName;
                    fcInfo.Overwrite = true;

                    Microsoft.SharePoint.Client.File uploadedFileRef = folder.Files.Add(fcInfo);

                    //get details of the uploaded file
                    CContext.Load(uploadedFileRef, f => f.ListItemAllFields);
                    CContext.ExecuteQuery();

                    string fileUrl = baseUrl + uploadedFileRef.ListItemAllFields["FileRef"].ToString();

                    uploadedFileRef.ListItemAllFields["PlaceRef"] = "00010222550";
                    uploadedFileRef.ListItemAllFields.Update();
                    CContext.ExecuteQuery();

                    //create Link Item to the uploaded file
                    CreateDocumentLink(token, destinationLib, fileName, fileNameWithoutExtension, fileUrl, destinationPath);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void CreateDocumentLink(string token, string destinationLib, string fileName, string fileNameWithoutExtension, string fileUrl, string destinationPath)
        {
            try
            {
                using (ClientContext CContext = GetClientContext(siteUrl, token))
                {
                    Web myWeb = CContext.Web;
                    Folder folder = myWeb.GetFolderByServerRelativeUrl(destinationPath);
                    CContext.Load(folder);

                    var fileContent = Encoding.ASCII.GetBytes("[InternetShortcut]URL = " + fileUrl);
                    System.IO.Stream stream = new System.IO.MemoryStream(fileContent);
                    
                    FileCreationInformation itemCreateInfo = new FileCreationInformation();
                    itemCreateInfo.Url = fileNameWithoutExtension + ".url";
                    itemCreateInfo.Overwrite = true;
                    itemCreateInfo.ContentStream = stream;
                    var linkItem = folder.Files.Add(itemCreateInfo);
                    CContext.Load(linkItem, x => x.ListItemAllFields);
                    CContext.ExecuteQuery();
                    
                    FieldUrlValue u = new FieldUrlValue();
                    u.Url = fileUrl;
                    u.Description = "test link";

                    var _FileItem = linkItem.ListItemAllFields;
                    //_FileItem["Title"] = u;
                    _FileItem["_ShortcutUrl"] = u;
                    _FileItem["PlaceRef"] = "00010222550";
                    //_FileItem["URL"] = fileUrl; 
                   

                    _FileItem.Update();
                    CContext.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void UpdateStagedDoc(string token)
        {
            int docId = 183;
            try
            {
                using (ClientContext ctx = GetClientContext(siteUrl, token))
                {
                    Web myWeb = ctx.Web;
                    List myLib = myWeb.Lists.GetByTitle(libName);
                    ListItem myListItem = myLib.GetItemById(docId);

                    ctx.Load(myListItem);
                    ctx.ExecuteQuery();

                    //initialize ContentType class to be able to get its properties
                    ctx.Load(myListItem, f => f.ContentType);
                    ctx.ExecuteQuery();
                    string cTypeName = myListItem.ContentType.Name;

                    //update metadata
                    myListItem["PlaceRef"] = "1180011101";
                    myListItem["InspectionCompletionDate"] = Convert.ToDateTime("20/02/2020");
                    myListItem["ValidToDate"] = Convert.ToDateTime("04/02/2021");
                    myListItem["DocumentStatus"] = "Complete";

                    //BusinessArea
                    UpdateTaxonomyField(ctx, myLib, myListItem, "BusinessArea", "Property Services", "ad5fbfe8-ea91-4a70-83da-491ab48838ee");
                    //DocumentType
                    UpdateTaxonomyField(ctx, myLib, myListItem, "DocumentType", "Electricity Document", "aad3a5f1-a7da-4e13-a22a-34a046151e40");
                    //Contractor
                    UpdateTaxonomyField(ctx, myLib, myListItem, "Contractor", "British Gas", "0e373c1f-6ec0-48d6-81e4-662c9a0675b7");

                    myListItem.Update();
                    ctx.ExecuteQuery();

                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void DownloadFile(string token)
        {
            string siteUrl = "sites/edrms-uat/property-uat";
            string webUrl = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat/property-uat";
            string fileName = "0009700350_22_clarendon crescent_190812.pdf";
            string libName = "StagedDocuments";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = string.Format("{0}/_api/web/GetFileByServerRelativeUrl('/{1}/{2}/{3}')/$value", webUrl, siteUrl, libName, fileName);
                var response = client.GetByteArrayAsync(requestUrl).Result;
                //var fileContent = response.Content.ReadAsByteArrayAsync().Result;

            }
        }


        public static void UpdateTaxonomyField(ClientContext ctx, List myLib, ListItem myListItem, string fieldName, string fieldLabel, string fieldValue)
        {
            var field = myLib.Fields.GetByInternalNameOrTitle(fieldName);
            var taxKeywordField = ctx.CastTo<TaxonomyField>(field);
            TaxonomyFieldValue termValue = new TaxonomyFieldValue();
            termValue.TermGuid = fieldValue;
            termValue.Label = fieldLabel;
            taxKeywordField.SetFieldValueByValue(myListItem, termValue);

            taxKeywordField.Update();
        }

        public static void CreateContractorTaxonomyTerm(string token)
        {
            Console.WriteLine("creating taxonomy term in termstore...");
            Console.WriteLine("=============================");

            int lcid = 1033;
            Guid termStoreId = Guid.Parse("31b4bc290a244ae2beca96609810c505");
            Guid termGroupId = Guid.Parse("cdbc062b-53ae-4675-b35c-50b5d3fab214");
            Guid termSetId = Guid.Parse("5e7f7b17-a35e-403f-a836-4de99216a492");
            string baseUrl = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat";
            string termName = "Contractor 04";
            Guid termId = Guid.NewGuid();

            try
            {
                using (ClientContext ctx = GetClientContext(baseUrl, token))
                {
                    // Get the TaxonomySession
                    TaxonomySession taxonomySession = TaxonomySession.GetTaxonomySession(ctx);

                    // Get the term store by name
                    TermStore termStore = taxonomySession.TermStores.GetById(termStoreId);

                    // Get the term group by Name
                    TermGroup termGroup = termStore.Groups.GetById(termGroupId);

                    // Get the term set by Name
                    TermSet termSet = termGroup.TermSets.GetById(termSetId);

                    // Create a new term
                    Term newTerm = termSet.CreateTerm(termName, lcid, termId);
                    ctx.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }


        public static void MoveDocument(string token)
        {
            string srcUrl = siteUrl;
            string destUrl = "https://traffordhousingtrust.sharepoint.com/sites/edrms-uat/tenancy-uat";
            string srcLibName = libName;
            string destLibName = "StagedDocuments";
            int docId = 213;

            try
            {
                using (ClientContext ctx = GetClientContext(siteUrl, token))
                {
                    Web myWeb = ctx.Web;
                    List myLib = myWeb.Lists.GetByTitle(libName);
                    ListItem myListItem = myLib.GetItemById(docId);
                    ctx.Load(myListItem);
                    ctx.ExecuteQuery();

                    //initialize File class to be able to get its properties
                    ctx.Load(myListItem, f => f.File);
                    ctx.ExecuteQuery();

                    string fileName = myListItem.File.Name;

                    if (myListItem.FileSystemObjectType == FileSystemObjectType.File)
                    {
                        Microsoft.SharePoint.Client.File file = myListItem.File;
                        var fileStream = file.OpenBinaryStream();
                        ctx.Load(file);
                        ctx.ExecuteQuery();

                        //copy the file into MemoryStream into a Byte[]
                        var memoryStream = new MemoryStream();
                        fileStream.Value.CopyTo(memoryStream);

                        int uploadedFileId = CopyFile(token, destUrl, destLibName, fileName, memoryStream.ToArray());

                        //after successful upload, delete the uploaded file from source library
                        file.DeleteObject();
                        ctx.ExecuteQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private static int CopyFile(string token, string destUrl, string destLibName, string fName, byte[] file)
        {
            using (ClientContext ctx = GetClientContext(destUrl, token))
            {
                System.IO.Stream stream = new System.IO.MemoryStream(file);
                FileCreationInformation fcInfo = new FileCreationInformation();
                fcInfo.ContentStream = stream;
                fcInfo.Url = fName;
                fcInfo.Overwrite = true;

                Web myWeb = ctx.Web;
                List myLib = myWeb.Lists.GetByTitle(destLibName);
                Microsoft.SharePoint.Client.File uploadedFileRef = myLib.RootFolder.Files.Add(fcInfo);

                ctx.Load(myLib);
                ctx.ExecuteQuery();

                //get Id for the uploaded file
                ctx.Load(uploadedFileRef, f => f.ListItemAllFields);
                ctx.ExecuteQuery();
                return uploadedFileRef.ListItemAllFields.Id;
            }
        }


        public static void DeleteDocument(string token)
        {
            int docId = 270;

            try
            {
                using (ClientContext ctx = GetClientContext(siteUrl, token))
                {
                    Web myWeb = ctx.Web;
                    List myLib = myWeb.Lists.GetByTitle(libName);
                    ListItem myListItem = myLib.GetItemById(docId);
                    ctx.Load(myListItem);
                    ctx.ExecuteQuery();

                    //initialize File class to be able to get its properties
                    ctx.Load(myListItem, f => f.File);
                    ctx.ExecuteQuery();

                    if (myListItem.FileSystemObjectType == FileSystemObjectType.File)
                    {
                        Microsoft.SharePoint.Client.File file = myListItem.File;
                        var fileStream = file.OpenBinaryStream();
                        ctx.Load(file);
                        ctx.ExecuteQuery();

                        //delete the file from staging area
                        file.DeleteObject();
                        ctx.ExecuteQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
