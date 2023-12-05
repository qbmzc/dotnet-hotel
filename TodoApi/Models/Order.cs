using System.Runtime.Serialization;

namespace TodoApi.Models;
//订单
public class Order
{
    public long Id { get; set; }
    public string? Status { get; set; }
    public string? OrderTime { get; set; }
    public string? PayTime { get; set; }
    public string? UserId { get; set; }
    //RoomId
    public string? ThingId { get; set; }
    public string? Count { get; set; }
    public string? OrderNumber { get; set; }
    public string? CheckIn { get; set; }
    public string? CheckOut { get; set; }
    public string? Remark { get; set; }
    //用户名称


    public string? Username { get; set; }

    public string? Title { get; set; }
  

    public string? Cover { get; set; }

    public string? Price { get; set; }
}