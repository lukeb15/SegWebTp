using HacForo.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace HacForo.Common
{
    public class MyPrincipal : IPrincipal
    {
        public MyPrincipal(string userKey, UserDTO user)
        {
            Identity = new GenericIdentity(userKey);
            User = user;
        }
        public IIdentity Identity
        {
            get;
            private set;
        }
        public UserDTO User { get; set; }
        public bool IsInRole(string role)
        {
            return true;
        }
    }
}