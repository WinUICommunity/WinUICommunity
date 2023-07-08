namespace WinUICommunity;

public class NavigationArgs
{
    public NavigationView NavigationView { get; set; }
    public object Parameter { get; set; }
    public string JsonFilePath { get; set; }
    public PathType PathType { get; set; }
    public IncludedInBuildMode IncludedInBuildMode { get; set; }
}
