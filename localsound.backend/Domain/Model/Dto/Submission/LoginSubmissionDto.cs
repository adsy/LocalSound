namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class LoginSubmissionDto
    {
        public LoginSubmissionDto(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
