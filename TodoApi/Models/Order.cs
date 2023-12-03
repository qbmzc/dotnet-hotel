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
    public string? RoomId { get; set; }
    public string? Count { get; set; }
    public string? OrderNumber { get; set; }
    public string? ReceiverAddress { get; set; }
    public string? ReceiverName { get; set; }
    public string? ReceiverPhone { get; set; }
    public string? Remark { get; set; }
    //用户名称
    [IgnoreDataMember]

    public string? Username { get; set; }
    [IgnoreDataMember]
    //商品名称

    public string? Title { get; set; }
    //封面
    [IgnoreDataMember]
    

    public string? Cover { get; set; }
    //价格
    [IgnoreDataMember]

    public string? Price { get; set; }
}