namespace SkysFormsDemo.Data;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public decimal Price { get; set; }
    public int PopularityPercent { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastBought { get; set; }
    public string Ean13 { get; set; }
}