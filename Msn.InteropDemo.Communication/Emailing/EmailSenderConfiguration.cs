namespace Msn.InteropDemo.Communication.Emailing
{
    public class EmailSenderConfiguration
    {
        public string ServerName { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool EnableSSL { get; set; }
    }
}
