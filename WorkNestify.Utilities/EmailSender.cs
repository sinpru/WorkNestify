using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace WorkNestify.Utilities;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");

        var fromAddress = new MailAddress(emailSettings["Username"], emailSettings["SenderName"]);
        var toAddress = new MailAddress(email);
        var fromPassword = emailSettings["Password"];

        var smtp = new SmtpClient
        {
            Host = emailSettings["SMTPServer"],
            Port = int.Parse(emailSettings["Port"]),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };

        using (var message = new MailMessage(fromAddress, toAddress)
               {
                   Subject = subject,
                   Body = htmlMessage,
                   IsBodyHtml = true
               })
        {
            await smtp.SendMailAsync(message);
        }
    }
}