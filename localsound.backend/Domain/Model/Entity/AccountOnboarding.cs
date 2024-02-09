using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class AccountOnboarding
    {
        // Class can be used for more boolean flags in the future
        [ForeignKey("Account")]
        public Guid AppUserId { get; set; }
        public bool AccountSetupCompleted { get; set; }

        public Account Account { get; set; }
    }
}
