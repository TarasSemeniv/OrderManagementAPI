using OrderManagementEntity.Models;

namespace OrderManagementCore.DTOs.Inputs;

public class CreateOrderDto
{
    public int? CustomerId { get; set; }
    
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public Status? Status { get; set; }
}