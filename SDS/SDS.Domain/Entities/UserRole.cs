namespace SDS.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public int AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public int Level { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateddAt { get; set; }
    }
}
