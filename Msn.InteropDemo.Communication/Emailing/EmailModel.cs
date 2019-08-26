namespace Msn.InteropDemo.Communication.Emailing
{
    public class EmailModel
    {
        public string Subject { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Body { get; set; }

        public bool IsBodyHml { get; set; }

        public string Attachments { get; set; }

    }
}
