using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using TicTacToR.Web.Models;

namespace TicTacToR.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "WBuUPmY9y8Kp38s6G8Xsg",
                consumerSecret: "m0CW16XiXsgtLNeVMyZ1rdmPzxl7DD3I39lqfPJHp38");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
