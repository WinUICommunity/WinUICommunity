using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WindowUI;
using WindowUI.DemoApp.Pages;

namespace DemoApp.Pages;
internal class myPageService : PageService
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
