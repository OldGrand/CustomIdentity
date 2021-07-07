namespace CustomIdentity.BLL.Models.Account
{
    public class ResetPasswordModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string ResetToken { get; set; }
    }
}
