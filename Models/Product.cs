namespace Core.Models;

public class Product
{
    // product model
    public int Id { get; set; }
    public int FarmerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime ProductionDate { get; set; }

    public Farmer? Farmer { get; set; }
}
