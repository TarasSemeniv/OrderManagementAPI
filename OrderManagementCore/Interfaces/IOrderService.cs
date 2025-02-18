using OrderManagementCore.DTOs.Inputs;
using OrderManagementCore.DTOs.Outputs;
using OrderManagementEntity.Models;

namespace OrderManagementCore.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetOrdersAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<OrderDto> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<OrderDto> AddOrderAsync(CreateOrderDto createOrderDto, CancellationToken cancellationToken = default);
    Task<OrderDto> UpdateStatusForOrderAsync(Status status, int id, CancellationToken cancellationToken = default);
    Task<OrderDto> UpdateOrderAsync(OrderDto order, CancellationToken cancellationToken = default);
    Task DeleteOrderByIdAsync(int id, CancellationToken cancellationToken = default);
}