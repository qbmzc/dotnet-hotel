namespace TodoApi.Models;
// 标签

public class ErrorLog
{
    public long Id { get; set; }
    public string? Ip { get; set; }
    public string? Url { get; set; }
    public string? Method { get; set; }
    public string? Content { get; set; }
    public string? LogTime { get; set; }
}