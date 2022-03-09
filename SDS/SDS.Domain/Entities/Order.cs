using static SDS.Domain.Enum.EnumExtensions;

namespace SDS.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNo { get; set; }
        public string Client { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductSpecification { get; set; }
        public double Budget { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string WarrantyInformation { get; set;}

        public string ApprovalNote { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public bool IsDisabled { get; set; }

        public int AppUserId { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public virtual IEnumerable<OrderApprovalChain> OrderApprovalChains { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdateddAt { get; set; }
    }
}
