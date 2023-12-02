using System.Runtime.Serialization;

namespace TodoApi.Models;
//房间
public class Room
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Cover { get; set; }
    public string? Description { get; set; }
    public string? Price { get; set; }
    public string? Window { get; set; }
    public string? Sheshi { get; set; }
    public string? Status { get; set; }
    public string? CreateTime { get; set; }
    public string? Score { get; set; }
    public string? Pv { get; set; }
    public string? RecommendCount { get; set; }
    public string? WishCount { get; set; }
    public string? CollectCount { get; set; }
    public long? ClassificationId { get; set; }

    [IgnoreDataMember]
    public List<long>? Tags;
    [IgnoreDataMember]
    public FormFile? ImageFile;
}