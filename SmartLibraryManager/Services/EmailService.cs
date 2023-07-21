using MailKit;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using SmartLibraryManager.Common.Models;
using SmartLibraryManager.Models;
using SmartLibraryManager.ViewModels;
using Newtonsoft.Json;
using Azure.Storage.Queues;
using MongoDB.Driver.Core.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Azure.Storage.Queues.Models;

namespace SmartLibraryManager.Services
{
    public class EmailService
    {
        private readonly AppSettings _appSettings;
        private readonly IMongoCollection<EmailQueue> _emailQueueCollection;
        private const string _queueName = "smartlibraryemailqueue";
        public EmailService(AppSettings appSettings, IOptions<DatabaseSettings> bookStoreDatabaseSettings)
        {
            _appSettings = appSettings;

            var mongoClient = new MongoClient(
          bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);
            _emailQueueCollection = mongoDatabase.GetCollection<EmailQueue>("EmailQueue");

        }

        public bool SendEmailToTable(Message message)
        {
            _emailQueueCollection.InsertOne(new EmailQueue
            {
                EmailProperties = Newtonsoft.Json.JsonConvert.SerializeObject(message)
            });

            return true;
        }

        public bool SendEmailInstantly(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            return Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_appSettings.EmailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Cc.AddRange(message.Cc);
            emailMessage.Bcc.AddRange(message.Bcc);

            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(message.IsHtml ? TextFormat.Html : TextFormat.Plain) { Text = message.Content };

            return emailMessage;
        }

        private bool Send(MimeMessage mailMessage)
        {
            using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    smtpClient.Connect(_appSettings.EmailConfiguration.SmtpServer, _appSettings.EmailConfiguration.Port, true);
                    smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    smtpClient.Authenticate(_appSettings.EmailConfiguration.Username, _appSettings.EmailConfiguration.Password);
                    smtpClient.MessageSent += OnMessageSent;
                    smtpClient.Send(mailMessage);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    smtpClient.Disconnect(true);
                }
            }
        }
        private void OnMessageSent(object sender, MessageSentEventArgs e)
        {
            Console.WriteLine("The message was sent!");
        }

        public void SendEmailPHPProject(Email email)
        {
            SendEmail(new Message(new string[] { email.To }, email.Subject, email.Body, new string[] { }, new string[] { }, true));
        }

        public bool SendEmail(Message message)
        {
            //Sends to a Queue

            QueueClient queue = new QueueClient(_appSettings.AzureStorageConnectionString, _queueName.ToLower());

            var newMessage = new NewMessage()
            {
                Content = message.Content,
                IsHtml = true,
                Subject = message.Subject,
                To = message.To.Select(x => x.Address).ToList()
            };

            var email = Newtonsoft.Json.JsonConvert.SerializeObject(newMessage);
            
            if (null != queue.CreateIfNotExistsAsync().Result)
            {
                Console.WriteLine("The queue was created.");
            }

            var response = queue.SendMessageAsync(Base64Encode(email)).Result;
            return true;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

    }
}
