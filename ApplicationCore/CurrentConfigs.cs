namespace ApplicationCore
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }

    public class PubnubKeys
    {
        public string PublishKey { get; set; }
        public string SubscribeKey { get; set; }
    }
}
