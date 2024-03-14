using System;
using System.Collections.Generic;
using WinUICommunity;

namespace WinUICommunityGallery.Pages;
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
