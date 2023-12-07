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



public DbSet<TodoApi.Models.Tag> Tag { get; set; } = default!;

public DbSet<TodoApi.Models.Order> Order { get; set; } = default!;

public DbSet<TodoApi.Models.Classification> Classification { get; set; } = default!;


public DbSet<TodoApi.Models.Payment> Payment { get; set; } = default!;

public DbSet<TodoApi.Models.Confirmation> Confirmation { get; set; } = default!;
}