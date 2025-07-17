using TropicFeel.Domain.Entities;

namespace TropicFeel.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }
    DbSet<TodoItem> TodoItems { get; }
    DbSet<Currency> Currencies { get; }
    DbSet<CustbodyPwkTipoSo> CustbodyPwkTipoSos { get; }
    DbSet<CustomForm> CustomForms { get; }
    DbSet<Item> Items { get; }
    DbSet<Domain.Entities.Order> Orders { get; }
    
    DbSet<SalesOrder> SalesOrder { get; }
    DbSet<SalesOrderLog> SalesOrderLog { get; }
    DbSet<ProductMapping> ProductMapping { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
}
