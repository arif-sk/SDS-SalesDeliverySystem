namespace SDS.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateddAt { get; set; }
    }
}
