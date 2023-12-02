using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Models;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;

public DbSet<Models.User> User { get; set; } = default!;

public DbSet<Models.Room> Room { get; set; } = default!;

public DbSet<Models.RoomWish> RoomWish { get; set; } = default!;

public DbSet<TodoApi.Models.RoomCollect> RoomCollect { get; set; } = default!;

public DbSet<TodoApi.Models.Tag> Tag { get; set; } = default!;

public DbSet<TodoApi.Models.Order> Order { get; set; } = default!;

public DbSet<TodoApi.Models.OpLog> OpLog { get; set; } = default!;

public DbSet<TodoApi.Models.Notice> Notice { get; set; } = default!;

public DbSet<TodoApi.Models.ErrorLog> ErrorLog { get; set; } = default!;

public DbSet<TodoApi.Models.Comment> Comment { get; set; } = default!;

public DbSet<TodoApi.Models.Classification> Classification { get; set; } = default!;

public DbSet<TodoApi.Models.Banner> Banner { get; set; } = default!;

public DbSet<TodoApi.Models.Address> Address { get; set; } = default!;

public DbSet<TodoApi.Models.Ad> Ad { get; set; } = default!;

public DbSet<TodoApi.Models.Reservation> Reservation { get; set; } = default!;

public DbSet<TodoApi.Models.Payment> Payment { get; set; } = default!;

public DbSet<TodoApi.Models.Confirmation> Confirmation { get; set; } = default!;
}