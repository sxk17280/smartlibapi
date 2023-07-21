﻿using SmartLibraryManager.Common.Models;

namespace SmartLibraryManager.Models
{
    public class AppSettings
    {
        public JwtConfiguration JwtConfiguration { get; set; }
        public Emailconfiguration EmailConfiguration { get; set; }
        public DatabaseSettings DatabaseSettings { get; set; }
        public string AzureStorageConnectionString { get; set; }

    }
    public class Emailconfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
