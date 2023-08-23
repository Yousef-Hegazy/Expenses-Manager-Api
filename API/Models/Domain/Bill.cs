namespace API.Models.Domain;

public class Bill
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }
}