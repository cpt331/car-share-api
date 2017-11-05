using System.Configuration;

namespace CarShareApi.Models
{
    public class EwebahConfig
    {
        public EwebahConfig()
        {
            //this assists masking the credentials used to secure the DB and 
            //the SMTP mailer

            //DB Credentials being parsed
            DbServer = ConfigurationManager.AppSettings["DbServer"].Decode();
            DbName = ConfigurationManager.AppSettings["DbName"].Decode();
            DbUsername =
                ConfigurationManager.AppSettings["DbUsername"].Decode();
            DbPassword =
                ConfigurationManager.AppSettings["DbPassword"].Decode();

            //SMTP Credentials being parsed
            SmtpUsername =
                ConfigurationManager.AppSettings["SmtpUsername"].Decode();
            SmtpPassword =
                ConfigurationManager.AppSettings["SmtpPassword"].Decode();
            SmtpServer =
                ConfigurationManager.AppSettings["SmtpServer"].Decode();
            SmtpPort =
                int.Parse(ConfigurationManager.AppSettings
                ["SmtpPort"].Decode());
        }

        public string DbServer { get; set; }
        public string DbName { get; set; }
        public string DbUsername { get; set; }
        public string DbPassword { get; set; }

        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
    }
}