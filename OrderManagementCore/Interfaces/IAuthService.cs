using OrderManagementCore.DTOs.Inputs;
using OrderManagementEntity.Models;

namespace OrderManagementCore.Interfaces;

public interface IAuthService
{
    Task<string> Register(RegisterCustomerDto registerCustomerDto, CancellationToken cancellationToken = default);
    Task<string> Login(string email, string password, CancellationToken cancellationToken = default);
}