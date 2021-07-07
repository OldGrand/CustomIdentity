namespace CustomIdentity.BLL.Models.Account
{
    public class LoginResultModel
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
