namespace localsound.backend.Domain.ModelAdaptor
{
    public class EmailSettingsAdaptor
    {
        public const string EmailSettingsKey = "EmailSettings";

        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
