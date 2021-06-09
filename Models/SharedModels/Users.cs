using System;
using System.Collections.Generic;
using System.Text;

namespace EDRMS.DemoConsole.App.Models.SharedModels
{
    public class Users
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public bool IsSiteAdmin { get; set; }
    }
}
