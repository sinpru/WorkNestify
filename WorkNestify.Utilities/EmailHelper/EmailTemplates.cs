using System.Text.Encodings.Web;

namespace WorkNestify.Utilities.EmailHelper;

public static class EmailTemplates
{
    private static string AppName => "Work Nestify";
    private static string SupportEmail => "work.nestify.contact@gmail.com";
    private static string WebsiteUrl => "https://www.worknestify.com";

    private static string GetFooter() => $@"
    <div style='text-align: center; padding: 20px 0; border-top: 1px solid #E9ECEF; margin-top: 20px; font-size: 14px; color: #6C757D; font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif;'>
        Need help? Contact us at <a href='mailto:{SupportEmail}' style='color: #1A3C34; text-decoration: none;'>{SupportEmail}</a><br>
        {AppName} - <a href='{WebsiteUrl}' style='color: #1A3C34; text-decoration: none;'>{WebsiteUrl}</a>
    </div>";

    public static string GetEmailConfirmationTemplate(string userName, string callbackUrl)
    {
        return $@"
        <div style='background-color: #F8F9FA; padding: 40px 20px; font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #FFFFFF; border: 1px solid #E9ECEF;'>
                <div style='padding: 30px;'>
                    <h1 style='font-family: Georgia, serif; font-size: 24px; color: #1a1a1a; margin: 0 0 20px;'>
                        Welcome to {AppName}!
                    </h1>
                    <p style='font-size: 16px; color: #212529; line-height: 1.6; margin: 0 0 20px;'>
                        Dear {userName},<br><br>
                        Thank you for signing up with {AppName}! To complete your registration, please confirm your email address by clicking the button below:
                    </p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='background-color: #1a1a1a; color: #FFFFFF; padding: 12px 24px; text-decoration: none; display: inline-block; font-size: 16px; font-weight: 600; font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif;'>
                            Confirm Email
                        </a>
                    </div>
                    <p style='font-size: 14px; color: #6C757D; line-height: 1.6; margin: 0 0 20px;'>
                        Or paste this link into your browser:<br>
                        <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='color: #1A3C34; word-break: break-all;'>{HtmlEncoder.Default.Encode(callbackUrl)}</a>
                    </p>
                    <p style='font-size: 14px; color: #6C757D; line-height: 1.6; margin: 0 0 20px;'>
                        This link will expire in 24 hours for security purposes. If you don’t confirm within this time, you may need to request a new confirmation email.<br><br>
                        If you didn’t create an account with us, please ignore this email or contact our support team at <a href='mailto:{SupportEmail}' style='color: #1A3C34;'>{SupportEmail}</a>.
                    </p>
                    <p style='font-size: 16px; color: #212529; line-height: 1.6; margin: 0;'>
                        Welcome aboard!<br>
                        The {AppName} Team
                    </p>
                </div>
                {GetFooter()}
            </div>
        </div>";
    }

    public static string GetPasswordResetTemplate(string userName, string callbackUrl)
    {
        return $@"
        <div style='background-color: #F8F9FA; padding: 40px 20px; font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #FFFFFF; border: 1px solid #E9ECEF;'>
                <div style='padding: 30px;'>
                    <h1 style='font-family: Georgia, serif; font-size: 24px; color: #1a1a1a; margin: 0 0 20px;'>
                        Password Reset Request
                    </h1>
                    <p style='font-size: 16px; color: #212529; line-height: 1.6; margin: 0 0 20px;'>
                        Dear {userName},<br><br>
                        We received a request to reset your {AppName} password. Click the button below to reset it:
                    </p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='background-color: #1a1a1a; color: #FFFFFF; padding: 12px 24px; text-decoration: none; display: inline-block; font-size: 16px; font-weight: 600; font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif;'>
                            Reset Password
                        </a>
                    </div>
                    <p style='font-size: 14px; color: #6C757D; line-height: 1.6; margin: 0 0 20px;'>
                        Or paste this link into your browser:<br>
                        <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='color: #1A3C34; word-break: break-all;'>{HtmlEncoder.Default.Encode(callbackUrl)}</a>
                    </p>
                    <p style='font-size: 14px; color: #6C757D; line-height: 1.6; margin: 0 0 20px;'>
                        This link will expire in 1 hour. If you didn’t request a password reset, please ignore this email or contact our support team at <a href='mailto:{SupportEmail}' style='color: #1A3C34;'>{SupportEmail}</a> if you’re concerned about your account security.
                    </p>
                    <p style='font-size: 16px; color: #212529; line-height: 1.6; margin: 0;'>
                        Best regards,<br>
                        The {AppName} Team
                    </p>
                </div>
                {GetFooter()}
            </div>
        </div>";
    }

    public static string GetResendEmailConfirmationTemplate(string userName, string callbackUrl)
    {
        return $@"
        <div style='display: none; font-size: 1px; color: #F8F9FA; line-height: 1px; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;'>
            Resend confirmation email for your Work Nestify account.
        </div>
        <div style='background-color: #F8F9FA; padding: 40px 20px; font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #FFFFFF; border: 1px solid #E9ECEF;'>
                <div style='padding: 30px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <img src='https://www.worknestify.com/logo.png' alt='Work Nestify Logo' style='max-width: 150px;'>
                    </div>
                    <h1 style='font-family: Georgia, serif; font-size: 24px; color: #1a1a1a; margin: 0 0 20px;'>
                        Resend Email Confirmation
                    </h1>
                    <p style='font-size: 16px; color: #212529; line-height: 1.6; margin: 0 0 20px;'>
                        Dear {userName},<br><br>
                        We received a request to resend your email confirmation for {AppName}. Please confirm your email address by clicking the button below:
                    </p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='background-color: #1a1a1a; color: #FFFFFF; padding: 12px 24px; text-decoration: none; display: inline-block; font-size: 16px; font-weight: 600; font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif; border: 1px solid #FFFFFF;'>
                            Confirm Email
                        </a>
                    </div>
                    <p style='font-size: 14px; color: #6C757D; line-height: 1.6; margin: 0 0 20px;'>
                        Or paste this link into your browser:<br>
                        <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='color: #1A3C34; word-break: break-all; text-decoration: underline; font-weight: 500;'>{HtmlEncoder.Default.Encode(callbackUrl)}</a>
                    </p>
                    <p style='font-size: 14px; color: #6C757D; line-height: 1.6; margin: 0 0 20px;'>
                        This link will expire in 24 hours for security purposes. If you don’t confirm within this time, you may need to request another confirmation email.<br><br>
                        If you didn’t request this email, please ignore it or contact our support team at <a href='mailto:{SupportEmail}' style='color: #1A3C34;'>{SupportEmail}</a>.
                    </p>
                    <p style='font-size: 16px; color: #212529; line-height: 1.6; margin: 0;'>
                        Best regards,<br>
                        The {AppName} Team
                    </p>
                </div>
                {GetFooter()}
            </div>
        </div>";
    }

    public static string GetChangeEmailConfirmationTemplate(string userName, string newEmail, string callbackUrl)
    {
        return $@"
        <div style='display: none; font-size: 1px; color: #F8F9FA; line-height: 1px; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;'>
            Confirm your new email address for Work Nestify.
        </div>
        <div style='background-color: #F8F9FA; padding: 40px 20px; font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #FFFFFF; border: 1px solid #E9ECEF;'>
                <div style='padding: 30px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <img src='https://www.worknestify.com/logo.png' alt='Work Nestify Logo' style='max-width: 150px;'>
                    </div>
                    <h1 style='font-family: Georgia, serif; font-size: 24px; color: #1a1a1a; margin: 0 0 20px;'>
                        Confirm Your New Email Address
                    </h1>
                    <p style='font-size: 16px; color: #212529; line-height: 1.6; margin: 0 0 20px;'>
                        Dear {userName},<br><br>
                        You’ve requested to change your email address for {AppName} to <strong>{newEmail}</strong>. Please confirm this change by clicking the button below:
                    </p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='background-color: #1a1a1a; color: #FFFFFF; padding: 12px 24px; text-decoration: none; display: inline-block; font-size: 16px; font-weight: 600; font-family: ""Nunito Sans"", Helvetica, Arial, sans-serif; border: 1px solid #FFFFFF;'>
                            Confirm New Email
                        </a>
                    </div>
                    <p style='font-size: 14px; color: #6C757D; line-height: 1.6; margin: 0 0 20px;'>
                        Or paste this link into your browser:<br>
                        <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='color: #1A3C34; word-break: break-all; text-decoration: underline; font-weight: 500;'>{HtmlEncoder.Default.Encode(callbackUrl)}</a>
                    </p>
                    <p style='font-size: 14px; color: #6C757D; line-height: 1.6; margin: 0 0 20px;'>
                        This link will expire in 24 hours for security purposes. If you don’t confirm within this time, you may need to request another email change.<br><br>
                        If you didn’t request this change, please ignore this email or contact our support team at <a href='mailto:{SupportEmail}' style='color: #1A3C34;'>{SupportEmail}</a>.
                    </p>
                    <p style='font-size: 16px; color: #212529; line-height: 1.6; margin: 0;'>
                        Best regards,<br>
                        The {AppName} Team
                    </p>
                </div>
                {GetFooter()}
            </div>
        </div>";
    }

    public static string GetVerificationEmailTemplate(string userName, string callbackUrl)
    {
        return GetEmailConfirmationTemplate(userName, callbackUrl);
    }
}