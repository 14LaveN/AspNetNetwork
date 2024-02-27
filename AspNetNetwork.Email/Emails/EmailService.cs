using MailKit.Security;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using AspNetNetwork.Email.Emails.Settings;
using MimeKit;
using MimeKit.Text;
using AspNetNetwork.Email.Contracts.Emails;

namespace AspNetNetwork.Email.Emails;

/// <summary>
/// Represents the email service.
/// </summary>
public sealed class EmailService : IEmailService
{
    private readonly MailSettings _mailSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="maiLSettingsOptions">The mail settings options.</param>
    public EmailService(IOptions<MailSettings> maiLSettingsOptions) =>
        _mailSettings = maiLSettingsOptions.Value;

    /// <inheritdoc />
    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage
        {
            From =
            {
                new MailboxAddress(_mailSettings.SenderDisplayName, _mailSettings.SenderEmail)
            },
            To =
            {
                MailboxAddress.Parse(mailRequest.EmailTo)
            },
            Subject = mailRequest.Subject,
            Body = new TextPart(TextFormat.Text)
            {
                Text = mailRequest.Body
            }
        };

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);

        await smtpClient.AuthenticateAsync(_mailSettings.SenderEmail, _mailSettings.SmtpPassword);

        await smtpClient.SendAsync(email);

        await smtpClient.DisconnectAsync(true);
    }
}