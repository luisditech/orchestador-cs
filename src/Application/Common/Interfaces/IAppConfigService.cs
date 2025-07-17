namespace TropicFeel.Application.Common.Interfaces;

public interface IAppConfigService
{
    int CustbodyPwkCourier { get; }
    int Location { get; }
    int Class { get; }
    int Department { get; }
    int Currency { get; }
    int CustbodyPwkShipmentOptionEvx { get; }
    string Entity { get; }
    int Shipmethod { get; }
    int Terms { get; }
    string Taxcode { get; }
    string CustbodyPwkOrderType { get; }
}
