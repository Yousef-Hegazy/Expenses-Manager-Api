namespace API.Models.Domain;

public class Loan
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }
    public int Period { get; set; }
}