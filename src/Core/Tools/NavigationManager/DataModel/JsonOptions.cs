namespace WinUICommunity;

public class JsonOptions
{
    public Type SectionPage { get; set; }
    public Type ItemPage { get; set; }
    public string JsonFilePath { get; set; }
    public PathType PathType { get; set; } = PathType.Relative;
    public IncludedInBuildMode IncludedInBuildMode { get; set; } = IncludedInBuildMode.CheckBasedOnIncludedInBuildProperty;
    public MenuFlyout MenuFlyout { get; set; }
    public bool HasInfoBadge { get; set; }
}
