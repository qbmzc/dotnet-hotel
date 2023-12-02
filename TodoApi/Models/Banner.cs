namespace TodoApi.Models;

public class Banner
{
    public long Id { get; set; }
    public string? Image { get; set; }
    // public string? image { get; set; }
    public long? ThingId { get; set; }
    public string? CreateTime { get; set; }
}