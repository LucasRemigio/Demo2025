// // Copyright (c) 2024 Engibots. All rights reserved.

using MailKit.Net.Smtp;
using MailKit.Net.Imap;
using engimatrix.Config;
using engimatrix.Exceptions;

namespace engimatrix.Connector
{
    public class EmailServiceMailkit
    {
        private readonly string _server;
        private readonly int _port;
        private readonly bool _isSecure;
        private readonly string _username;
        private readonly string _password;

        public EmailServiceMailkit(string server, int port, bool isSecure, string username, string password)
        {
            _server = server;
            _port = port;
            _isSecure = isSecure;
            _username = username;
            _password = password;
        }

        public static EmailServiceMailkit Init(string account, bool isImap)
        {
            // DEBUG
            account = account.Split("_")[0];

            string server = ConfigManager.EmailServer;
            int port = Int32.Parse(isImap ? ConfigManager.IMAPPort : ConfigManager.SMTPPort);
            bool isSecure = true;
            string username = account;
            string password;

            // Get the password from the account
            if (!ConfigManager.MailboxCredentials.TryGetValue(account, out password))
            {
                throw new EmailNotFoundException($"Account {account} not found in the credentials.");
            }

            return new EmailServiceMailkit(server, port, isSecure, username, password);
        }

        public static async Task<ImapClient> GetAutenticatedImapClientAsync(string account)
        {
            EmailServiceMailkit emailService = Init(account, true);
            ImapClient client = new ImapClient();
            await client.ConnectAsync(emailService._server, emailService._port, emailService._isSecure);
            await client.AuthenticateAsync(emailService._username, emailService._password);
            return client;
        }

        public static async Task<SmtpClient> GetAutenticatedSmtpClientAsync(string account)
        {
            EmailServiceMailkit emailService = Init(account, false);
            SmtpClient client = new SmtpClient();
            await client.ConnectAsync(emailService._server, emailService._port, emailService._isSecure);
            await client.AuthenticateAsync(emailService._username, emailService._password);
            return client;
        }

    }
}
