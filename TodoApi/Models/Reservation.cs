namespace TodoApi.Models;
// 标签

public class Reservation
{
    public long Id { get; set; }
    public string? UserId { get; set; }
    public string? RoomId { get; set; }
    public string? BookingDate { get; set; }
    public string? ModificationDate { get; set; }
    public string? CancellationDate { get; set; }
}