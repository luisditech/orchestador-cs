using System.Reflection;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Entities;
using TropicFeel.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TropicFeel.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<CustbodyPwkTipoSo> CustbodyPwkTipoSos => Set<CustbodyPwkTipoSo>();
    public DbSet<CustomForm> CustomForms => Set<CustomForm>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<SalesOrder> SalesOrder => Set<SalesOrder>();
    public DbSet<SalesOrderLog> SalesOrderLog => Set<SalesOrderLog>();
    public DbSet<ProductMapping> ProductMapping => Set<ProductMapping>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
