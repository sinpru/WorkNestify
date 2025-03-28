using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace WorkNestify.Utilities.EmailHelper;

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

    public async Task SendEmailConfirmationAsync(string email, string userName, string callbackUrl)
    {
        var htmlMessage = EmailTemplates.GetEmailConfirmationTemplate(userName, callbackUrl);
        await SendEmailAsync(email, "Confirm Your Email", htmlMessage);
    }

    public async Task SendPasswordResetAsync(string email, string userName, string callbackUrl)
    {
        var htmlMessage = EmailTemplates.GetPasswordResetTemplate(userName, callbackUrl);
        await SendEmailAsync(email, "Reset Your Password", htmlMessage);
    }

    public async Task SendResendEmailConfirmationAsync(string email, string userName, string callbackUrl)
    {
        var htmlMessage = EmailTemplates.GetResendEmailConfirmationTemplate(userName, callbackUrl);
        await SendEmailAsync(email, "Resend Email Confirmation", htmlMessage);
    }

    public async Task SendChangeEmailConfirmationAsync(string email, string userName, string newEmail, string callbackUrl)
    {
        var htmlMessage = EmailTemplates.GetChangeEmailConfirmationTemplate(userName, newEmail, callbackUrl);
        await SendEmailAsync(email, "Confirm Your New Email", htmlMessage);
    }

    public async Task SendVerificationEmailAsync(string email, string userName, string callbackUrl)
    {
        var htmlMessage = EmailTemplates.GetVerificationEmailTemplate(userName, callbackUrl);
        await SendEmailAsync(email, "Confirm Your Email", htmlMessage);
    }
}