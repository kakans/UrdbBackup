using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public static class ConfigurationSettings
    {
        public static string GetApplicationSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
        }
    }
}