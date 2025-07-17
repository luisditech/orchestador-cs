using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Infrastructure.Options;

namespace Microsoft.Extensions.DependencyInjection.Service;

public class AppConfigService : IAppConfigService
{
    private readonly AppConfig _appConfig;

    public AppConfigService(AppConfig appConfig)
    {
        _appConfig = appConfig;
    }

    public int CustbodyPwkCourier => _appConfig.CustbodyPwkCourier;
    public int Location => _appConfig.Location;
    public int Class => _appConfig.Class;
    public int Department => _appConfig.Department;
    public int Currency => _appConfig.Currency;
    public int CustbodyPwkShipmentOptionEvx => _appConfig.CustbodyPwkShipmentOptionEvx;
    public string Entity => _appConfig.Entity;
    public int Shipmethod => _appConfig.Shipmethod;
    public int Terms => _appConfig.Terms; 
    public string Taxcode  => _appConfig.Taxcode;
    public string CustbodyPwkOrderType => _appConfig.CustbodyPwkOrderType;
}
