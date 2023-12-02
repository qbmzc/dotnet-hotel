namespace TodoApi.Models;

public class Comment
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    public string? CommentTime { get; set; }
    public string? LikeCount { get; set; }
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? ThingId { get; set; }
    public string? Title { get; set; }
    public string? Cover { get; set; }
}