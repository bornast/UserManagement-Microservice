﻿namespace Sindikat.Identity.Domain.Entities
{
    public class UserClaim
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual Claim Claim { get; set; }
        public int ClaimId { get; set; }
        public string ClaimValue { get; set; }
    }
}
