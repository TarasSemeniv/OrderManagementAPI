using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementCore.DTOs.Inputs;
using OrderManagementCore.DTOs.Outputs;
using OrderManagementCore.Interfaces;

namespace OrderManagementAPI.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrdersAsync([FromQuery]int page = 0,
        [FromQuery]int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return Ok(await _orderService.GetOrdersAsync(page, pageSize, cancellationToken));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<OrderDto>> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Ok(await _orderService.GetOrderByIdAsync(id, cancellationToken));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto, CancellationToken cancellationToken = default)
    {
        var taskDto = await _orderService.AddOrderAsync(createOrderDto, cancellationToken);
        return Created("", taskDto);
    }
}