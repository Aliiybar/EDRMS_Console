using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models.SharedModels
{
    public class UserGroup
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string PrincipalType { get; set; }
        public string Title { get; set; }
        public List<Users> Users { get; set; } = new List<Users>();

    }
}
