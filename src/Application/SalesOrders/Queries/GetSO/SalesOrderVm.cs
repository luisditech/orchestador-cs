using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using TropicFeel.Domain.Entities;

namespace TropicFeel.Application.SalesOrders.Queries.GetSO;
public class SalesOrderVm
{
    public string? OrderReference { get; set; }
    public DateTime? OrderDate { get; set; }
}

