using System.Diagnostics;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Crito.Services;

public class MailService : IDisposable
{

    private string from;
    private string smtp;
    private int port;
    private string userName;
    private string password;
    private SmtpClient client;

    public MailService(string from, string smtp, int port, string userName, string password)
    {
        this.from = from;
        this.smtp = smtp;
        this.port = port;
        this.userName = userName;
        this.password = password;
        client = new();
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        try
        {

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));

            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            await client.ConnectAsync(smtp, port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(userName, password);

            _ = await client.SendAsync(email);

        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

    #region IDisposable

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {

        if (disposedValue)
            return;

        if (disposing)
            client.DisconnectAsync(true).ConfigureAwait(false);

        disposedValue = true;

    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion

}
