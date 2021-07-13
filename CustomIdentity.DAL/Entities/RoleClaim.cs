namespace CustomIdentity.DAL.Entities
{
    public class RoleClaim
    {
        public int ClaimEntityId { get; set; }
        public ClaimEntity ClaimEntity  { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
