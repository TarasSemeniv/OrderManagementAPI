using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementEntity.Models;

public class Order
{
    public int Id { get; set; }
    public Customer? Customer { get; set; }
    [ForeignKey(nameof(Models.Customer))]
    public int? CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}