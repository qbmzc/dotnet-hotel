namespace TodoApi.Models;

public class Notice
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? CreateTime { get; set; }
}