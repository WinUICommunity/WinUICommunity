<p align="center">
    <img alt="dotnet" src="https://img.shields.io/badge/.net-%3E=6.0-brightgreen"/>
    <img alt="os-require" src="https://img.shields.io/badge/OS-%3E%3D%20Windows%2010%20Build%201809-orange"/>
    <img alt="IDE-version" src="https://img.shields.io/badge/IDE-vs2022-red"/>
    <img alt="csharp-require" src="https://img.shields.io/badge/CSharp-Latest-yellow"/>
    <a href="https://github.com/WinUICommunity">
        <img alt="projects" src="https://img.shields.io/badge/WinUICommunity-Projects-green"></img>
    </a> 
    <a href="https://www.nuget.org/profiles/WinUICommunity">
        <img alt="WinuiCommunity Nugets" src="https://img.shields.io/badge/WinUICommunity-Nugets-green"></img>
    </a> 
    <a href="https://www.nuget.org/packages/WinUICommunity.LandingPages">
        <img alt="nuget-version" src="https://img.shields.io/nuget/v/WinUICommunity.LandingPages.svg"></img>
    </a> 
    <a href="https://www.nuget.org/packages/WinUICommunity.LandingPages">
        <img alt="Installed" src="https://img.shields.io/nuget/dt/WinUICommunity.LandingPages?color=brightgreen&label=Installs"></img>
    </a> 
    <a href="https://ghost1372.github.io/winUICommunity/">
        <img alt="Docs" src="https://img.shields.io/badge/Document-Here-critical"></img>
    </a> 
</p>

<br>
<p align="center">
	<b>🙌 Donate Bitcoin with <a href="https://link.trustwallet.com/send?coin=0&address=bc1qzs4kt4aeqym6gsde669g5rksv4swjhzjqqp23a">Trust</a>🙌</b><br>
	<b>🙌 Donate ETH with <a href="https://link.trustwallet.com/send?coin=60&address=0x40Db4476c1D498b167f76A2c7ED9D45b65eb5d0C">Trust</a>🙌</b><br><br>
	<b>🙌 Bitcoin: bc1qzs4kt4aeqym6gsde669g5rksv4swjhzjqqp23a<br></b>
	<b>🙌 ETH: 0x40Db4476c1D498b167f76A2c7ED9D45b65eb5d0C</b>
</p>
<br>

# LandingPages
 
### Create a landing page in the style of WinUI 3 and WinUI-Gallery very quickly and easily

## Dependencies

This package is based on the following packages

- CommunityToolkit.WinUI.UI
- CommunityToolkit.WinUI.UI.Animations
- Microsoft.Graphics.Win2D


## Namespace
We moved all namespaces into a single namespace. No matter which (WinUICommunity) library you use, the namespace is always as follows
 For use in the Xaml:
 ```xml 
 xmlns:wuc="using:WinUICommunity"
 ```
 For use in the Csharp:
 ```csharp
 using WinUICommunity;
 ```

## Install
```
Install-Package WinUICommunity.LandingPages
```

After installing, add the following resources to app.xaml

```xml
xmlns:wuc="using:WinUICommunity"

<wuc:ItemTemplates/>
<ResourceDictionary Source="ms-appx:///LandingPages/Themes/Generic.xaml"/>
```

now:

```xml
xmlns:wuc="using:WinUICommunity"
```

use MainLandingsPage:

```xml
<wuc:MainLandingsPage x:Name="mainLandingPage" Loaded="mainLandingPage_Loaded"
                    HeaderImage="ms-appx:///Assets/GalleryHeaderImage.png"
                    HeaderText="WinUI 3 Gallery"
                    HeaderSubtitleText="WinAppSDK 1.2"
                    OnItemClick="mainLandingsPage_OnItemClick">
    <wuc:MainLandingPage.HeaderContent>
        <StackPanel Orientation="Horizontal" Spacing="10">
            <wuc:HeaderTile
                OnItemClick="HeaderTile_OnHeaderItemClick"
                Title="Getting started"
                Description="An overview of app development options, tools, and samples.">
                <wuc:HeaderTile.Source>
                    <Image Source="/Assets/HomeHeaderTiles/Header-WinUIGallery.png" />
                </wuc:HeaderTile.Source>
            </wuc:HeaderTile>
            <wuc:HeaderTile
                Title="Windows design"
                Description="Design guidelines and toolkits for creating native app experiences."
                Link="https://docs.microsoft.com/windows/apps/design/">
                <wuc:HeaderTile.Source>
                    <Image Source="/Assets/HomeHeaderTiles/Header-WindowsDesign.png" />
                </wuc:HeaderTile.Source>
            </wuc:HeaderTile>
            <wuc:HeaderTile
                Title="Community Toolkit"
                Description="A collection of helper functions, custom controls, and app services."
                Link="https://apps.microsoft.com/store/detail/windows-community-toolkit-sample-app/9NBLGGH4TLCQ">
                <wuc:HeaderTile.Source>
                    <Image Source="/Assets/HomeHeaderTiles/Header-Toolkit.png" />
                </wuc:HeaderTile.Source>
            </wuc:HeaderTile>
            <wuc:HeaderTile
                Title="Code samples"
                Description="Find samples that demonstrate specific tasks, features, and APIs."
                Link="https://docs.microsoft.com/windows/apps/get-started/samples">
                <wuc:HeaderTile.Source>
                    <FontIcon
                        Margin="0,8,0,0"
                        FontSize="44"
                        Foreground="{ThemeResource TextFillColorPrimaryBrush}"
                        Glyph="&#xE943;" />
                </wuc:HeaderTile.Source>
            </wuc:HeaderTile>
        </StackPanel>
    </wuc:MainLandingsPage.HeaderContent>
</wuc:MainLandingsPage>
```

in code-behind:
```cs
private void mainLandingPage_Loaded(object sender, RoutedEventArgs e)
{
    mainLandingPage.GetDataAsync("DataModel/ControlInfoData.json");
}

private void mainLandingPage_OnItemClick(object sender, RoutedEventArgs e)
{
}

private void HeaderTile_OnHeaderItemClick(object sender, RoutedEventArgs e)
{
}
```

now create `ControlInfoData.json` in DataModel (Build Action = Content)

```json
{
  "Groups": [
    {
      "UniqueId": "Design_Guidance",
      "Title": "Design guidance",
      "ApiNamespace": "",
      "Subtitle": "",
      "ImagePath": "",
      "ImageIconPath": "",
      "Description": "",
      "IsSpecialSection": true,
      "Items": [
        {
          "UniqueId": "Typography",
          "Title": "Typography",
          "ApiNamespace": "",
          "Subtitle": "Typography",
          "ImagePath": "ms-appx:///Assets/ControlImages/AppBarSeparator.png",
          "ImageIconPath": "ms-appx:///Assets/ControlIcons/AppBarSeparatorIcon.png",
          "Description": "",
          "Content": "",
          "IsNew": false,
          "IsUpdated": true,
          "IncludedInBuild": true,
          "HideSourceCodeAndRelatedControls": true,
          "Docs": [
            {
              "Title": "Typography in Windows Apps",
              "Uri": "https://learn.microsoft.com/windows/apps/design/style/typography"
            },
            {
              "Title": "XAML theme resources",
              "Uri": "https://learn.microsoft.com/windows/apps/design/style/xaml-theme-resources#the-xaml-type-ramp"
            },
            {
              "Title": "Typography in Windows 11",
              "Uri": "https://learn.microsoft.com/windows/apps/design/signature-experiences/typography"
            }
          ],
          "RelatedControls": []
        },
        {
          "UniqueId": "AccessibilityScreenReader",
          "Title": "Screen reader support",
          "ApiNamespace": "",
          "Subtitle": "Screen reader support",
          "ImagePath": "ms-appx:///Assets/ControlImages/AppBarSeparator.png",
          "ImageIconPath": "ms-appx:///Assets/ControlIcons/AppBarSeparatorIcon.png",
          "Description": "",
          "Content": "",
          "IsNew": false,
          "IsUpdated": false,
          "IsPreview": true,
          "IncludedInBuild": true,
          "HideSourceCodeAndRelatedControls": true,
          "Docs": [
            {
              "Title": "Accessibility",
              "Uri": "https://learn.microsoft.com/windows/apps/design/accessibility/accessibility"
            },
            {
              "Title": "Accessible text requirements",
              "Uri": "https://learn.microsoft.com/windows/apps/design/accessibility/accessible-text-requirements"
            }
          ],
          "RelatedControls": []
        }
      ]
    }
  ]
}
```

See the [Demo](https://github.com/winUICommunity/DemoApp) app to see how to use it

## Documentation

See Here for Online [Documentation](https://ghost1372.github.io/WinUICommunity/)

![LandingsPage](https://raw.githubusercontent.com/ghost1372/Resources/main/LandingsPage/0.png)
