namespace OrderManagementCore.DTOs.Inputs;

public class RegisterCustomerDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Email { get; set; }
    public string Password { get; set; }
}