using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace TodoApi.Models;
public class User
{
    public long Id { get; set; }
    public string? Username { get; set; }

    public string? Password { get; set; }
    [IgnoreDataMember]
    public string? RePassword { get; set; }

    // public string? RePassword { get; set; }
    public string? Nickname { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    public string? Role { get; set; }
    public string? Status { get; set; }
    public string? Avatar { get; set; }
    public string? Token { get; set; }
    public string? CreateTime { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PostalCode { get; set; }
    public string? CardType { get; set; }
    public string? CreditCard { get; set; }

    public string? ExpirationDate { get; set; }

    public string? Holder { get; set; }


}