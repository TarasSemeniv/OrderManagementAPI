namespace OrderManagementEntity.Models;

public class Customer
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Email { get; set; }
    public string Password { get; set; }
    
    public CustomerRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}