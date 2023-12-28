namespace WinUICommunity;
public partial class NetworkHelper
{
    public static bool IsNetworkAvailable()
    {
        return NetworkInformation.GetInternetConnectionProfile()?.NetworkAdapter != null;
    }
}
