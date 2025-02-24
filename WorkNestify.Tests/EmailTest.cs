using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using WorkNestify.Utilities;

namespace WorkNestify.Tests;

public class Tests
{
    private Mock<IConfiguration> _mockConfiguration;
    private IEmailSender _emailSender;

    [SetUp]
    public void Setup()
    {
        // Mock Configuration
        _mockConfiguration = new Mock<IConfiguration>();

        // Setup EmailSettings in the mock configuration
        _mockConfiguration
            .Setup(config => config.GetSection("EmailSettings")["SMTPServer"])
            .Returns("smtp.gmail.com");
        _mockConfiguration
            .Setup(config => config.GetSection("EmailSettings")["Port"])
            .Returns("587");
        _mockConfiguration
            .Setup(config => config.GetSection("EmailSettings")["Username"])
            .Returns("ducthai20704@gmail.com");
        _mockConfiguration
            .Setup(config => config.GetSection("EmailSettings")["Password"])
            .Returns("rzdu peoy wrby gbyk");
        _mockConfiguration
            .Setup(config => config.GetSection("EmailSettings")["SenderName"])
            .Returns("Work Nestify");

        // Initialize EmailSender with mocked configuration
        _emailSender = new EmailSender(_mockConfiguration.Object);
    }

    [Test]
    public async Task SendEmailAsync_ShouldSendEmailWithoutException()
    {
        // Arrange
        var recipientEmail = "thaindhe186951@fpt.edu.vn";
        var subject = "Test Email";
        var htmlMessage = "<p>This is a test email sent from a unit test.</p>";

        // Act & Assert
        Assert.DoesNotThrowAsync(
            async () => await _emailSender.SendEmailAsync(recipientEmail, subject, htmlMessage),
            "Sending an email should not throw any exceptions."
        );
    }
}