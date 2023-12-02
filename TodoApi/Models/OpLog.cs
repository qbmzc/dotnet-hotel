namespace TodoApi.Models;

public class OpLog
{
    public long Id { get; set; }
    public string? ReIp { get; set; }
    public string? ReTime { get; set; }
    public string? ReUa { get; set; }
    public string? ReUrl { get; set; }
    public string? ReMethod { get; set; }
    public string? ReContent { get; set; }
    public string? AccessTime { get; set; }
}