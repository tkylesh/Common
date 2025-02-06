using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CommonAPICommon;

namespace CommonAPIBusinessLayer.Configuration
{
    public class ISOConfig
    {
        public string ISOListener 
        { 
            get
            {
                return ConfigurationManager.AppSettings["ISOListener"].ToString();
            } 
        }
        public string ISOUN 
        { 
            get
            {
                return RijndaelSimple.DoRijndael(ConfigurationManager.AppSettings["ISOUN"].ToString(), EncrytionDirection.Decrypt);
            }
        }
        public string ISOPWD 
        { 
            get
            {
                return RijndaelSimple.DoRijndael(ConfigurationManager.AppSettings["ISOPWD"].ToString(), EncrytionDirection.Decrypt);
            }
        }
        public string ISOOrgID 
        { 
            get
            {
                return ConfigurationManager.AppSettings["ISOOrgID"].ToString();
            }
        }
    }
}
