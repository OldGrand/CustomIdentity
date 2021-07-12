namespace CustomIdentity.DAL.Entities
{
    public class ClaimEntity
    {
        public int Id { get; set; }

        public int ClaimTypeId { get; set; }
        public ClaimType ClaimType { get; set; }

        public int ClaimValueId { get; set; }
        public ClaimValue ClaimValue { get; set; }
    }
}
