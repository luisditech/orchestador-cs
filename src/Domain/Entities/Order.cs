namespace TropicFeel.Domain.Entities;

public class Order  : BaseAuditableEntity
{
    public int OrderId { get; set; }
    public string? Action { get; set; }
    public string? RecordType { get; set; }
    public int CustomFormId { get; set; }
    public virtual CustomForm? Customform { get; set; }
    public string? Entity { get; set; }
    public string? Memo { get; set; }
    public int CustbodyPwkTipoSoId { get; set; }
    public virtual CustbodyPwkTipoSo? CustbodyPwkTipoSo { get; set; }
    public int Location { get; set; }
    public int Class { get; set; }
    public int Department { get; set; }
    public int CurrencyId { get; set; }
    public virtual Currency? Currency { get; set; }
    public string? Orderstatus { get; set; }
    public int CustbodyPwkShipmentOptionEvx { get; set; }
    public int Shipaddresslist { get; set; }
    public string? Shipaddress { get; set; }
    public int CustbodyPwkCourier { get; set; }
    public int Shipmethod { get; set; }
    public int Terms { get; set; }
    public int? FulfillmentId { get; set; }
    public string? CustbodyPwkOrderType { get; set; }
    public virtual ICollection<Item>? Items { get; set; } 
}
