namespace OrderManagementEntity.Models;

public class Order
{
    public int Id { get; set; }
    public Customer? Customer { get; set; }
    
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}