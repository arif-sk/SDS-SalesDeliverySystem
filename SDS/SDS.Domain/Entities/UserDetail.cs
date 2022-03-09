namespace SDS.Domain.Entities
{
    public class UserDetail: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
    }
}
