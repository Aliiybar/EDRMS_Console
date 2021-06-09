using EDRMS.DemoConsole.App.Models;
using EDRMS.DemoConsole.App.Models.SharedModels;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDRMS.DemoConsole.App
{
    public class Permissions
    {
        private static string baseUrl = "https://traffordhousingtrust.sharepoint.com";
        private static string siteUrl = baseUrl + "/sites/edrms-uat";

        #region "Helpers"

        public static string GetListItemsJson(string listName, string token)
        {
            string jsonObj = string.Empty;

            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    Web myWeb = ctx.Web;
                    List myList = myWeb.Lists.GetByTitle(listName);
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
                    jsonObj = JsonConvert.SerializeObject(listInfo);

                }
            }
            catch (Exception ex)
            {

            }

            return jsonObj;
        }

        public static bool IsSiteAdmin(string email, string token)
        {
            bool result = false;
            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    Web web = ctx.Web;
                    UserCollection users = web.SiteUsers;
                    ctx.Load(users, u => u.Include(item => item.IsSiteAdmin, item => item.Id, item => item.Email, item => item.Title));
                    ctx.ExecuteQuery();

                    var data = users.Where(x => x.Email.ToLower() == email.ToLower()).First();
                    result = data.IsSiteAdmin;
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public static string GroupDetails(int groupId, string token)
        {
            string groupName = string.Empty;
            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    Web web = ctx.Web;
                    var collGroup = ctx.Web.SiteGroups;
                    ctx.Load(collGroup);
                    ctx.ExecuteQuery();

                    var groupDetails = collGroup.Where(i => i.Id == groupId).First();
                    groupName = groupDetails.Title;
                }
            }
            catch (Exception ex)
            {

            }

            return groupName;
        }

        public static List<string> GetUserGroups(string userEmail, string token)
        {
            List<string> groupsList = new List<string>();

            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    Web web = ctx.Web;
                    var collGroup = ctx.Web.SiteGroups;
                    ctx.Load(collGroup);
                    ctx.ExecuteQuery();

                    foreach (var group in collGroup)
                    {
                        UserCollection collUser = group.Users;
                        ctx.Load(collUser);
                        ctx.ExecuteQuery();

                        foreach (var user in collUser)
                        {
                            if (user.Email.ToLower() == userEmail.ToLower())
                            {
                                groupsList.Add(group.Title);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return groupsList;
        }

        public static CoreEntitySettingsListFields GetCoreEntityDetails(string entityId, string token)
        {
            CoreEntitySettingsListFields entity = null;

            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    //get all core entities
                    var list = JsonConvert.DeserializeObject<List<CoreEntitySettingsListFields>>(GetListItemsJson("CoreEntityConfigSettings", token));
                    entity = list.Where(i => i.CoreEntity.TermGuid == entityId).First();
                }
            }
            catch (Exception ex)
            {

            }

            return entity;
        }

        public static List<CoreEntitySettingsListFields> GetCoreEntitiesByPermissions(string groupName, string token)
        {
            List<CoreEntitySettingsListFields> filteredList = new List<CoreEntitySettingsListFields>();

            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    //get all core entities
                    var list = JsonConvert.DeserializeObject<List<CoreEntitySettingsListFields>>(GetListItemsJson("CoreEntityConfigSettings", token));
                    filteredList = list.Where(i => i.PermissionGroups.Exists(f => f.GroupName.ToLower() == groupName.ToLower())).ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return filteredList;
        }


        #endregion


        public static Users GetPrimaryAdmin(string token)
        {
            Users user = new Users();

            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    Site osite = ctx.Site;
                    ctx.Load(osite, site => site.Owner);
                    ctx.ExecuteQuery();
                    var data = osite.Owner;

                    //mapping
                    user.Id = data.Id;
                    user.Email = data.Email;
                    user.Title = data.Title;
                    user.IsSiteAdmin = data.IsSiteAdmin;
                }
            }
            catch (Exception ex)
            {

            }

            return user;
        }


        public static void GetAllUsers(string token)
        {
            List<Users> usersList = new List<Users>();

            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    Web web = ctx.Web;
                    UserCollection users = web.SiteUsers;
                    ctx.Load(users, u => u.Include(item => item.IsSiteAdmin, item => item.Id, item => item.Email, item => item.Title));
                    ctx.ExecuteQuery();

                    //var admins = string.Join(";", web.SiteUsers.Where(u => u.IsSiteAdmin).Select(a => a.Title).ToList());
                    foreach (var u in users)
                    {
                        Users usr = new Users
                        {
                            Id = u.Id,
                            Email = u.Email,
                            Title = u.Title,
                            IsSiteAdmin = u.IsSiteAdmin
                        };

                        usersList.Add(usr);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        public static void GetAllGroupsWithUsers(string token)
        {
            List<UserGroup> groupsList = new List<UserGroup>();

            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    Web web = ctx.Web;
                    var groups = ctx.Web.SiteGroups;
                    ctx.Load(groups, grp => grp.Include(item => item.Users,
                                           item => item.Id,
                                           item => item.LoginName,
                                           item => item.PrincipalType,
                                           item => item.Title
                    ));
                    ctx.ExecuteQuery();

                    if (groups != null && groups.Any())
                    {
                        foreach (var group in groups)
                        {
                            UserGroup grp = new UserGroup
                            {
                                Id = group.Id,
                                LoginName = group.LoginName,
                                PrincipalType = group.PrincipalType.ToString(),
                                Title = group.Title
                            };

                            if (group.Users != null && group.Users.Any())
                            {
                                foreach (var usr in group.Users.Where(u => !string.IsNullOrWhiteSpace(u.Email)))
                                {
                                    Users user = new Users
                                    {
                                        Id = usr.Id,
                                        Email = usr.Email,
                                        Title = usr.Title,
                                        IsSiteAdmin = usr.IsSiteAdmin
                                    };

                                    grp.Users.Add(user);
                                }
                            }

                            groupsList.Add(grp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        public static void GetCoreEntitiesByUser(string token)
        {
            string userEmail = "ali.iybar@traffordhousingtrust.co.uk";
            List<CoreEntitySettingsListFields> list = new List<CoreEntitySettingsListFields>();

            //check if user is site admin
            if (IsSiteAdmin(userEmail, token))
            {
                //if site admin, load ALL core entities
                list = JsonConvert.DeserializeObject<List<CoreEntitySettingsListFields>>(GetListItemsJson("CoreEntityConfigSettings", token));
            }
            else
            {
                //get User Groups
                var userGroups = GetUserGroups(userEmail, token);
                if (userGroups != null && userGroups.Any())
                {
                    foreach (var grp in userGroups)
                    {
                        //get core entities list
                        list.AddRange(GetCoreEntitiesByPermissions(grp, token));
                    }
                }
            }

            //get unique core entities
            list = list.GroupBy(x => x.CoreEntity.TermGuid).Select(s=>s.FirstOrDefault()).ToList();
        }

        public static void GetDocTypesByUser(string token)
        {
            string entityId = "8a8ecd3d-6e38-4d55-b37e-6cf3e7a1dcbe";//property entity
            CoreEntitySettingsListFields coreEntity = GetCoreEntityDetails(entityId, token);

            string userEmail = "ali.iybar@traffordhousingtrust.co.uk";
            List<ConfigSettingsListFields> fList = new List<ConfigSettingsListFields>();

            //check if user is site admin
            if (IsSiteAdmin(userEmail, token))
            {
                //if site admin, load ALL Doc Types by core entity
                var list = JsonConvert.DeserializeObject<List<ConfigSettingsListFields>>(GetListItemsJson("DocTypeConfigSettings", token));
                fList = list.Where(i => i.CoreEntity.TermGuid == entityId).ToList();
            }
            else
            {
                //get User Groups
                var userGroups = GetUserGroups(userEmail, token);
                if (userGroups != null && userGroups.Any())
                {
                    foreach (var grp in userGroups)
                    {
                        //get docType list by entity
                        fList.AddRange(GetDocTypesByPermissions(coreEntity, grp, token));
                    }
                }
            }

            //get unique document types
            fList = fList.GroupBy(x => x.DocumentType.TermGuid).Select(s => s.FirstOrDefault()).ToList();
        }



        public static List<ConfigSettingsListFields> GetDocTypesByPermissions(CoreEntitySettingsListFields coreEntity, string groupName, string token)
        {
            List<ConfigSettingsListFields> filteredList = new List<ConfigSettingsListFields>();

            try
            {
                using (ClientContext ctx = Uploader.GetClientContext(siteUrl, token))
                {
                    //get all docTypes
                    var list = JsonConvert.DeserializeObject<List<ConfigSettingsListFields>>(GetListItemsJson("DocTypeConfigSettings", token));
                    //get docTypes by entity
                    list = list.Where(i => i.CoreEntity.TermGuid == coreEntity.CoreEntity.TermGuid).ToList();
                    
                    //inherit Permissions of Core Entity if No Permission groups are assigned to Doc Types
                    foreach(var docType in list.Where(i => i.PermissionGroups == null))
                    {
                        docType.PermissionGroups = coreEntity.PermissionGroups;
                    }

                    filteredList = list.Where(i => i.PermissionGroups.Exists(f => f.GroupName.ToLower() == groupName.ToLower())).ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return filteredList;
        }

        
    }
}
