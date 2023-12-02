using System.Runtime.Serialization;

namespace TodoApi.Models;
// 标签

public class Ad
{
    public long Id { get; set; }
    public string? Image { get; set; }
    public string? Mobile { get; set; }
    public string? Link { get; set; }
    public string? CreateTime { get; set; }
}