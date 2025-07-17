using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TropicFeel.Domain.Dtos.Netsuit;

public class OrderDto
{
    [Newtonsoft.Json.JsonIgnore]
    public int OrderId { get; set; }
    public string? Action { get; set; }
    public string? RecordType { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public int CustomFormId { get; set; }
    public virtual CustomFormDto? Customform { get; set; }
    public string? Entity { get; set; }
    public string? Memo { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public int CustbodyPwkTipoSoId { get; set; }
    [JsonProperty("custbody_pwk_tipo_so")]
    public virtual CustbodyPwkTipoSoDto? CustbodyPwkTipoSo { get; set; }
    public int Location { get; set; }
    public int Class { get; set; }
    public int Department { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public int CurrencyId { get; set; }
    public virtual CurrencyDto? Currency { get; set; }
    public string? Orderstatus { get; set; }
    [JsonProperty("custbody_pwk_shipment_option_evx")]
    public int CustbodyPwkShipmentOptionEvx { get; set; }
    public int Shipaddresslist { get; set; }
    public string? Shipaddress { get; set; }
    [JsonProperty("custbody_pwk_courier")]    
    public int CustbodyPwkCourier { get; set; }

    [JsonProperty("custbody_pwk_ordertype")]
    public string? CustbodyPwkOrderType { get; set; }
    public int Shipmethod { get; set; }
    public int Terms { get; set; }
    [JsonProperty("item")]    
    public virtual ICollection<ItemDto>? Items { get; set; } 
}
