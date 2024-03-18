using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Security.Services.Interfaces;
using Security.Settings;
using static System.Net.Mime.MediaTypeNames;


namespace Security.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly MailSettings _mailSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public EmailService(ILogger<EmailService> logger, IOptions<MailSettings> mailSettings,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mailSettings = mailSettings.Value;
            _env = env;
        }

        public async Task<bool> SendEmail(string receiverEmail, string subject, string text) {
           

            try {
              

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_mailSettings.SenderMail));
                email.To.Add(MailboxAddress.Parse(receiverEmail));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) {
                    Text = text
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.SenderMail, _mailSettings.AuthPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            } catch (Exception e) {
                _logger.LogError("{msg} {stackTrace}", e.Message, e.StackTrace);
                return false;
            }
        }
    }
}

