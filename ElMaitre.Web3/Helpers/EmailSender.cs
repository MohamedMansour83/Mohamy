﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ElMaitre.Web.Logger;
using ElMaitre.Web.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ElMaitre.Web.Helpers
{

    public interface IEmailSender
    {
        Task<bool> Send(string mail, string subject, string body);
    }
    public class EmailSender : IEmailSender
    {

        private readonly ILoggerManager logger;
        public EmailSender(ILoggerManager logger)
        {
            this.logger = logger;
        }
        public async Task<bool> Send(string mail, string subject, string body)
        {

            //var smtpClient = new SmtpClient
            //{
            //    Host = "smtp.gmail.com", // set your SMTP server name here
            //    Port = 587, // Port 
            //    UseDefaultCredentials = false,
            //    EnableSsl = true,
            //    Credentials = new NetworkCredential("islam.apps123@gmail.com", "SOLOM19895050")
            //};

            NetworkCredential networkCredentials = new
            NetworkCredential("info@mohamy.co", "Ayman1988@");

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = networkCredentials;
            smtpClient.Host = "smtp.office365.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.ServicePoint.MaxIdleTime = 1;

            //using (var message = new MailMessage("islam.apps123@gmail.com", mail)
            using (var message = new MailMessage("info@mohamy.co", mail)
            {
                Subject = subject,
                Body = $"<body>{body}</body>",
                IsBodyHtml = true
            })
            {
                try
                {
                    await smtpClient.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    logger.LogInfo("Failed to send the mail");
                    logger.LogError(ex.ToString());
                    return false;
                }
            }

            return true;
        }
    }
}