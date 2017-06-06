using HacForo.Models;
using HacForo.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Data.Entity;

namespace HacForo.Common
{
    public class SessionManager
    {
        public static string GetAuthTicket(string userKey, string userData)
        {
            FormsAuthenticationTicket authTicket = new
                 FormsAuthenticationTicket(1, //version
                 userKey, // user name
                 DateTime.Now,             //creation
                 DateTime.Now.AddMinutes(30), //Expiration (you can set it to 1 month
                 true,  //Persistent
                 userData); // additional informations

            return FormsAuthentication.Encrypt(authTicket);
        }

        internal static User AuthenticateUser(string email, string password)
        {
            var crypto = new SimpleCrypto.PBKDF2();

            using (var db = new Models.HacForoContainer())
            {
                
                User user = db.UserSet.Include(u  => u.Threads)
                                    .Include(u => u.UserThreadPoints)
                                    .FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    if (user.Password == crypto.Compute(password, user.PasswordSalt))
                    {
                        return user;
                    }
                }
            }

            throw new UnauthorizedAccessException("The login data is invalid");
        }

        internal static UserDTO GetLoggedUser(HttpCookie authCookie)
        {
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                UserDTO cookieUser = serializer.Deserialize<UserDTO>(authTicket.UserData);

                return cookieUser;
            }

            throw new UnauthorizedAccessException("The login data is invalid");
        }
    }
}