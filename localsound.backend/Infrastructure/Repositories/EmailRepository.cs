using localsound.backend.Domain;
using localsound.backend.Domain.Model.Communication;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Repositories;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;

namespace localsound.backend.Infrastructure.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        public EmailSettingsAdaptor _emailSettings;
        public ILogger<EmailRepository> _logger;

        public EmailRepository(IOptions<EmailSettingsAdaptor> emailSettings, ILogger<EmailRepository> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendConfirmEmailTokenMessageAsync(string token, string email)
        {
            var registrationTokenMessage = new EmailRequest(new string[] { email }, "Confirm your email", Constants.messageContent.Replace("{token}", token));
            await SendEmailAsync(registrationTokenMessage);
        }

        public async Task SendEmailAsync(EmailRequest message)
        {
            var emailMessage = CreateEmailMessage(message);
            await Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailRequest message)
        {
            var builder = new BodyBuilder();
            //var image = builder.LinkedResources.Add("../../PocketShop.Backend/PocketShop.Backend/wwwroot/assets/logo.png");

            //image.ContentId = MimeUtils.GenerateMessageId();

            builder.HtmlBody = string.Format(@"<div style=""width: 100%;max-width: 500px;margin: auto; color:black;""><div style=""text-align:center; border-bottom: 2px solid #232323; padding-bottom: 20px;""><img style=""height:150px; margin: auto;"" src=""https://localsoundstorage.blob.core.windows.net/assets/logo3.png""></div>" + message.Content + "</div>");


            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("LocalSound", _emailSettings.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            emailMessage.Body = builder.ToMessageBody();

            return emailMessage;
        }
        private async Task Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                    await client.SendAsync(mailMessage);
                }
                catch (Exception e)
                {
                    var message = $"{nameof(EmailRepository)} - {nameof(Send)} - {e.Message}";
                    _logger.LogError(e, message);
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
