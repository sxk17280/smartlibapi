using Microsoft.Extensions.Configuration;
using SmartLibraryManager.Models;
using SmartLibraryManager.Services;
using SmartLibraryManager.Utilties;

namespace SmartLibraryManagerTests.Services
{
    public class EmailServiceTests
    {
        private EmailService _emailService;

        private readonly IConfiguration _configuration;
        private readonly EmailTemplateManager _templateManager;
        public EmailServiceTests()
        {
            _configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(@"appsettings.json", false, false)
               .AddEnvironmentVariables()
               .Build();
            _templateManager = new EmailTemplateManager();
        }

        [SetUp]
        public void Setup()
        {
            AppSettings appSettings = new AppSettings();
            _configuration.Bind(appSettings);
            _emailService = new EmailService(appSettings, null);
        }

        [Test]
        public void SendMail()
        {
            var body = _templateManager.GetTemplate(EmailTemplate.BookNotification);
            body = body.Replace("##USERID", "123456789");
            var message = new Message(new string[] { "saitejagoud123@gmail.com" }, "Test Email subject", body, new string[] { }, new string[] { }, true);

            var result = _emailService.SendEmail(message);

            Assert.IsTrue(result);
        }
    }
}
