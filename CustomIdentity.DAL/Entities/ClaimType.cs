using System.Collections.Generic;

namespace CustomIdentity.DAL.Entities
{
    public class ClaimType
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public ICollection<ClaimEntity> ClaimEntities { get; set; }
    }
}
