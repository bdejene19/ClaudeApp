using ClaudeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaudeApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}
