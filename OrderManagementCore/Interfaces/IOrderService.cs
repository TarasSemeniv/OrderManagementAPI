using OrderManagementCore.DTOs.Inputs;
using OrderManagementCore.DTOs.Outputs;
using OrderManagementEntity.Models;

namespace OrderManagementCore.Interfaces;

public interface IOrderService
{
    IEnumerable<OrderDto> GetOrders();
    OrderDto GetOrderById(int id);
    OrderDto AddOrder(CreateOrderDto orderDto);
}