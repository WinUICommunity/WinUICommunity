namespace WucGalleryApp.Views;
internal class myPageService : PageServiceEx
{
    public myPageService()
    {
        _pageKeyToTypeMap = new Dictionary<string, Type>
        {
            { "GeneralPage", typeof(GeneralPage) },
            { "AwakePage", typeof(AwakePage) },
            { "FancyZonesPage", typeof(FancyZonesPage) },
        };
    }
}
