using Domain.Common;
using Domain.Interfaces;
using Domain.Utility;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly Mail _mail;
      
        private readonly Secrets _secrets;

        public EmailSender(IOptions<Mail> mail,IOptions<Secrets> secrets)
        {
            _mail = mail.Value;
            _secrets = secrets.Value;
        }
        public async Task SendEmailAsync(Email emailmodel)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mail.Email));
            email.To.Add(MailboxAddress.Parse(emailmodel.ToEmail));
            email.Subject = emailmodel.Subject;
            var builder = new BodyBuilder();
            if(emailmodel.Attachments!= null)
            {
                byte[] filebytes;
                foreach (var file in emailmodel.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using(var ms=new MemoryStream())
                        {
                            file.CopyTo(ms);    
                            filebytes= ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, filebytes);
                    }
                }
            }
            builder.TextBody = emailmodel.Body;
            email.Body=builder.ToMessageBody();
            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect(Encrypter.DecryptString(_secrets.masterkey, _mail.Host), _mail.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mail.Email, Encrypter.DecryptString(_secrets.masterkey, _mail.Password));
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
