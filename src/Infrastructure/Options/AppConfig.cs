using TropicFeel.Application.Common.Interfaces;

namespace TropicFeel.Infrastructure.Options;

public class AppConfig
{
    public int CustbodyPwkCourier { get; set; }
    public int Location { get; set; }
    public int Class { get; set; }
    public int Department { get; set; }
    public int Currency { get; set; }
    public int CustbodyPwkShipmentOptionEvx { get; set; }
    public required string Entity { get; set; }
    public int Shipmethod { get; set; }
    public int Terms { get; set; }
    public required string Taxcode { get; set; }
    public required string CustbodyPwkOrderType { get; set; }  
}
