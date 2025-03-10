using OrderManagementEntity.Models;

namespace OrderManagementCore.DTOs.Outputs;

public class OrderDto
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public Status? Status { get; set; }
}