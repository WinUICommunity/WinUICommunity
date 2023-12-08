<p align="center">
    <img alt="dotnet" src="https://img.shields.io/badge/.net-%3E=6.0-brightgreen"/>
    <img alt="os-require" src="https://img.shields.io/badge/OS-%3E%3D%20Windows%2010%20Build%201809-orange"/>
    <img alt="IDE-version" src="https://img.shields.io/badge/IDE-vs2022-red"/>
    <img alt="csharp-require" src="https://img.shields.io/badge/CSharp-Latest-yellow"/>
    <a href="https://github.com/WindowUIOrg">
        <img alt="projects" src="https://img.shields.io/badge/WindowUIOrg-Projects-green"></img>
    </a> 
    <a href="https://www.nuget.org/profiles/WindowUIOrg">
        <img alt="WindowUIOrg Nugets" src="https://img.shields.io/badge/WindowUIOrg-Nugets-green"></img>
    </a> 
    <a href="https://www.nuget.org/packages/WindowUI.LandingPages">
        <img alt="nuget-version" src="https://img.shields.io/nuget/v/WindowUI.LandingPages.svg"></img>
    </a> 
    <a href="https://www.nuget.org/packages/WindowUI.LandingPages">
        <img alt="Installed" src="https://img.shields.io/nuget/dt/WindowUI.LandingPages?color=brightgreen&label=Installs"></img>
    </a> 
    <a href="https://ghost1372.github.io/WindowUIOrg/">
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
We moved all namespaces into a single namespace. No matter which (WindowUIOrg) library you use, the namespace is always as follows
 For use in the Xaml:
 ```xml 
 xmlns:wui="using:WindowUI"
 ```
 For use in the Csharp:
 ```csharp
 using WindowUI;
 ```

## Install
```
Install-Package WindowUI.LandingPages
```

After installing, add the following resources to app.xaml

```xml
xmlns:wui="using:WindowUI"

<wui:ItemTemplates/>
<ResourceDictionary Source="ms-appx:///WindowUI.LandingPages/Themes/Generic.xaml" />
```

now:

```xml
xmlns:wui="using:WindowUI"
```


See the [Demo](https://github.com/WindowUIOrg/WindowUI) app to see how to use it

## Documentation

See Here for Online [Documentation](https://ghost1372.github.io/WindowUIOrg/)

![LandingsPage](https://raw.githubusercontent.com/ghost1372/Resources/main/LandingsPage/0.png)
