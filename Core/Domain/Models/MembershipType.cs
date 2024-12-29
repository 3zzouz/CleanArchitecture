namespace Core.Domain.Models
{
    public class MembershipType
    {
        public int Id { get; set; }
        public int SignUpFee { get; set; }
        public int DurationInMonth { get; set; }
        public double DiscountRate { get; set; }
        public ICollection<Customer>? Customers { get; set; }
    }
}
