using MimeKit;

namespace localsound.backend.Domain.Model.Communication
{
    public class EmailRequest
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public EmailRequest(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(null,x)));
            Subject = subject;
            Content = content;
        }
    }
}
