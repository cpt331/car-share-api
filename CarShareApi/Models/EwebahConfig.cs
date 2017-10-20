using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using NLog.Internal;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace CarShareApi.Models
{
    public class EwebahConfig
    {
        public string DbServer { get; set; }
        public string DbName { get; set; }
        public string DbUsername { get; set; }
        public string DbPassword { get; set; }

        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }

        public EwebahConfig()
        {
            DbServer = ConfigurationManager.AppSettings["DbServer"].Decode();
            DbName = ConfigurationManager.AppSettings["DbName"].Decode();
            DbUsername = ConfigurationManager.AppSettings["DbUsername"].Decode();
            DbPassword = ConfigurationManager.AppSettings["DbPassword"].Decode();

            SmtpUsername = ConfigurationManager.AppSettings["SmtpUsername"].Decode();
            SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"].Decode();
            SmtpServer = ConfigurationManager.AppSettings["SmtpServer"].Decode();
            SmtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].Decode());
        }

        
    }
}