using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Thermus.Api.Services
{
    // Yo defino una interfaz para poder inyectar el servicio y testearlo fácil
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string bodyText, CancellationToken ct = default);
    }

    // Yo implemento el servicio real usando MailKit + Brevo (SMTP)
    public sealed class MailServices : IEmailService
    {
        private readonly SmtpSettings _smtp;

        public MailServices(IOptions<SmtpSettings> smtpOptions)
        {
            // Yo cargo la configuración SMTP desde appsettings + secrets/env
            _smtp = smtpOptions.Value;
        }

        public async Task SendAsync(string toEmail, string subject, string bodyText, CancellationToken ct = default)
        {
            // Yo armo el mensaje
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtp.FromName, _smtp.FromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = bodyText };

            using var client = new SmtpClient();

            // Yo pongo un timeout razonable para no colgarme si hay un problema de red
            client.Timeout = 10000;

            // Brevo: puerto 587 con STARTTLS
            await client.ConnectAsync(_smtp.Host, _smtp.Port, SecureSocketOptions.StartTls, ct);

            // Yo me autentico con el usuario SMTP de Brevo y la SMTP key (password)
            await client.AuthenticateAsync(_smtp.User, _smtp.Pass, ct);

            await client.SendAsync(message, ct);

            await client.DisconnectAsync(true, ct);
        }
    }
}
