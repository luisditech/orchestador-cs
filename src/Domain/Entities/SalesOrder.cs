using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Entities;
public class SalesOrder : BaseAuditableEntity
{
    
    public string? OrderReferenceJlp { get; set; }
    public DateTime? OrderDateJlp { get; set; }
    public  string? OrderNetSuite { get; set; }
    public string? OrderSprint { get; set;}
    public required string Status { get; set; }
    public int? FulfillmentId { get; set; }
    public virtual ICollection<SalesOrderLog>? Logs { get; set; } 
}
