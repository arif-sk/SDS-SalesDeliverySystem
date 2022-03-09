using static SDS.Domain.Enum.EnumExtensions;

namespace SDS.Domain.Entities
{
    public class OrderApprovalChain : BaseEntity
    {
        public int RoleGroupId { get; set; }
        public int AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int Level { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public string Notes { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateddAt { get; set; }
        public bool IsDisabled { get; set; }
    }
}
