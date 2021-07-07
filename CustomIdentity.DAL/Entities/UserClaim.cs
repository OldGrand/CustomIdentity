namespace CustomIdentity.DAL.Entities
{
    public class UserClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; } //TODO Replace with enum
        public string ClaimValue { get; set; } //TODO Replace with enum
    }
}
