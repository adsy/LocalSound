using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class AccountMessages
    {
        // Class can be used for more boolean flags in the future
        [ForeignKey("Account")]
        public Guid AppUserId { get; set; }
        public bool OnboardingMessageClosed { get; set; }

        public Account Account { get; set; }


        public AccountMessages CloseOnboardingMessage()
        {
            OnboardingMessageClosed = true;
            return this;
        }
    }
}
