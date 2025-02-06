using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CommonAPIBusinessLayer
{
    public static class GlobalParameters
    {
        public static string VerifyALIATokenURL
        {
            get { return ConfigurationManager.AppSettings["VerifyALIATokenURL"]; }
        }
        public static string VerifyALIATokenPassword
        {
            get { return ConfigurationManager.AppSettings["VerifyALIATokenPassword"]; }
        }
        public static string VerifyALIATokenClientSecret
        {
            get { return ConfigurationManager.AppSettings["VerifyALIATokenClientSecret"]; }
        }
        public static string VerifyALIATokenClientId
        {
            get { return ConfigurationManager.AppSettings["VerifyALIATokenClientId"]; }
        }
        public static string VerifyALIATokenUsername
        {
            get { return ConfigurationManager.AppSettings["VerifyALIATokenUsername"]; }
        }
        public static string VerifyALIAURL
        {
            get { return ConfigurationManager.AppSettings["VerifyALIAURL"]; }
        }
        public static string MembershipCertSerialNumber
        {
            get { return ConfigurationManager.AppSettings["NewMemberCertSerialNumber"]; }
        }
        public static string MembershipBaseUrl
        {
            get { return ConfigurationManager.AppSettings["NewMemberURL"]; }
        }
        //public static string MemberSearchURL
        //{
        //    get { return ConfigurationManager.AppSettings["MemberSearchURL"]; }
        //}
        public static string MembershipClientId
        {
            get { return ConfigurationManager.AppSettings["MemberSearchClientId"]; }
        }
        public static string MembershipClientSecret
        {
            get { return ConfigurationManager.AppSettings["MemberSearchClientSecret"]; }
        }
        //public static string MembershipClientId
        //{
        //    get { return ConfigurationManager.AppSettings["MemberSearchClientId"]; }
        //}
        //public static string MemberSearchClientSecret
        //{
        //    get { return ConfigurationManager.AppSettings["MemberSearchClientSecret"]; }
        //}
        public static string VerifyToken
        {
            get { return ConfigurationManager.AppSettings["VerifyToken"]; }
        }
        public static string PDFBasePath
        {
            get { return ConfigurationManager.AppSettings["PDFBasePath"]; }
        }
        //public static string IdCardURL
        //{
        //    get { return ConfigurationManager.AppSettings["IdCardUrl"]; }
        //}
    }
}
