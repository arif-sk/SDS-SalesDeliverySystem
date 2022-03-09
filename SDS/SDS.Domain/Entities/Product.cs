namespace SDS.Domain.Entities
{
    public class Product: BaseEntity
    {
        public string ManufacturingDetails { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateddAt { get; set; }
    }
}
