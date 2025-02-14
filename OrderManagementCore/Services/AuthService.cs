using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagementCore.DTOs.Inputs;
using OrderManagementCore.Interfaces;
using OrderManagementEntity.Models;
using OrderManagementStorage;

namespace OrderManagementCore.Services;

public class AuthService(IMapper mapper, OrderContext context, IConfiguration configuration) : IAuthService
{
    public async Task<string> Register(RegisterCustomerDto registerCustomerDto, CancellationToken cancellationToken = default)
    {
        if (!Regex.IsMatch(registerCustomerDto.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$"))
        {
            throw new ArgumentException("Email is not valid", nameof(registerCustomerDto.Email));
        }
        if (!Regex.IsMatch(registerCustomerDto.Password, @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&*()-+=])(?=\S+$).{8,20}$"))
        {
            throw new ArgumentException("Password is not valid", nameof(registerCustomerDto.Password));
        }
        if (await context.Customers.AnyAsync(x => x.Email.ToLower().Equals(registerCustomerDto.Email.ToLower()), cancellationToken))
        {
            throw new ArgumentException("Email is already taken", nameof(registerCustomerDto.Email));
        }
        
        var customer = mapper.Map<Customer>(registerCustomerDto);
        customer.Email = customer.Email.ToLower();
        customer.CreatedAt = DateTime.UtcNow;
        customer.Role = CustomerRole.Guest;
        customer.Password = PasswordHasher.GenerateHash(registerCustomerDto.Password, customer.CreatedAt);
        
        context.Customers.Add(customer);
        await context.SaveChangesAsync(cancellationToken);
        return GenerateToken(customer);
    }

    public async Task<string> Login(string email, string password, CancellationToken cancellationToken = default)
    {
        var customer = await context.Customers.FindAsync([email.ToLower()], cancellationToken);
        if (customer == null)
            throw new ArgumentException("Email is not found", nameof(email));
        
        var checkPassword = PasswordHasher.GenerateHash(password, customer.CreatedAt);
        if (checkPassword != customer.Password) 
            throw new ArgumentException("Wrong password", nameof(password));
        
        return GenerateToken(customer);
    }
    
    private string GenerateToken(Customer customer)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, customer.Email),
            new(ClaimTypes.Role, customer.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(configuration.GetSection("AppSettings:ExpireTime").Value!)),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}