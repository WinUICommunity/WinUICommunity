﻿using System.Collections.Generic;

using DemoApp;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Windows.Graphics.Printing;

namespace WindowUI.DemoApp.Pages;
public sealed partial class PrintPage : Page
{
#if WINDOWS10_0_18362_0_OR_GREATER
    private PrintHelper _printHelper;
#endif
    public PrintPage()
    {
        this.InitializeComponent();
        Loaded += PrintPage_Loaded;
    }

    private void PrintPage_Loaded(object sender, RoutedEventArgs e)
    {
        ShowOrientationSwitch.IsOn = true;

        DefaultOrientationComboBox.ItemsSource = new List<PrintOrientation>()
            {
                PrintOrientation.Default,
                PrintOrientation.Portrait,
                PrintOrientation.Landscape
            };
        DefaultOrientationComboBox.SelectedIndex = 0;
    }

    private async void PrintingIsNotSupported()
    {
        // Printing is not supported on this device
        ContentDialog noPrintingDialog = new ContentDialog()
        {
            XamlRoot = App.currentWindow.Content.XamlRoot,
            Title = "Printing is not supported",
            Content = "\nSorry, printing is not supported on this device.",
            PrimaryButtonText = "OK"
        };
        await noPrintingDialog.ShowAsyncQueue();
    }

    private async void btnDirectPrint_Click(object sender, RoutedEventArgs e)
    {
#if WINDOWS10_0_18362_0_OR_GREATER

        if (PrintManager.IsSupported())
        {
            _printHelper = new PrintHelper(DirectPrintContainer);

            _printHelper.OnPrintCanceled += PrintHelper_OnPrintCanceled;
            _printHelper.OnPrintFailed += PrintHelper_OnPrintFailed;
            _printHelper.OnPrintSucceeded += PrintHelper_OnPrintSucceeded;

            var printHelperOptions = new PrintHelperOptions(false);
            printHelperOptions.Orientation = (PrintOrientation)DefaultOrientationComboBox.SelectedItem;

            if (ShowOrientationSwitch.IsOn)
            {
                printHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);
            }

            await _printHelper.ShowPrintUIAsync(WinRT.Interop.WindowNative.GetWindowHandle(App.currentWindow), "Windows Community Toolkit Sample App", printHelperOptions, true);
        }
        else
        {
            PrintingIsNotSupported();
        }
#endif
    }

#if WINDOWS10_0_18362_0_OR_GREATER
    private void ReleasePrintHelper()
    {
        _printHelper.Dispose();

        if (!DirectPrintContainer.Children.Contains(PrintableContent))
        {
            DirectPrintContainer.Children.Add(PrintableContent);
        }
    }

    private async void PrintHelper_OnPrintSucceeded()
    {
        ReleasePrintHelper();
        ContentDialog noPrintingDialog = new ContentDialog()
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Printing Done",
            Content = "\nDone, element printed.",
            PrimaryButtonText = "OK"
        };
        await noPrintingDialog.ShowAsyncQueue();
    }

    private async void PrintHelper_OnPrintFailed()
    {
        ReleasePrintHelper();
        ContentDialog noPrintingDialog = new ContentDialog()
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Printing error",
            Content = "\nSorry, failed to print.",
            PrimaryButtonText = "OK"
        };
        await noPrintingDialog.ShowAsyncQueue();
    }
    private void PrintHelper_OnPrintCanceled()
    {
        ReleasePrintHelper();
    }
#endif
}
