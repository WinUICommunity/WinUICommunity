using Windows.Foundation;

namespace WinUICommunity;

public class TitleBarHelper
{
    private ColumnDefinition _LeftPaddingColumn { get; set; }
    private ColumnDefinition _IconColumn { get; set; }
    private ColumnDefinition _TitleColumn { get; set; }
    private ColumnDefinition _LeftDragColumn { get; set; }
    private ColumnDefinition _SearchColumn { get; set; }
    private ColumnDefinition _RightDragColumn { get; set; }
    private ColumnDefinition _RightPaddingColumn { get; set; }
    private Grid _AppTitleBar { get; set; }
    private TextBlock _TitleTextBlock { get; set; }
    private Window _MainWindowObject { get; set; }
    private AppWindow m_AppWindow { get; set; }
    private Grid _CustomDragRegion { get; set; }

    private static TitleBarHelper instance;
    public static TitleBarHelper Instance => instance;

    public static TitleBarHelper GetCurrent()
    {
        if (Instance == null)
        {
            instance = new TitleBarHelper();
        }
        return Instance;
    }

    public int LeftPadding { get; set; } = 0;
    public int RightPadding { get; set; } = 0;
    public TitleBarHelper() { }

    #region TitleBar
    public TitleBarHelper(Window window, TextBlock titleTextBlock, Grid appTitleBar, ColumnDefinition leftPaddingColumn, ColumnDefinition iconColumn, ColumnDefinition titleColumn, ColumnDefinition leftDragColumn, ColumnDefinition searchColumn, ColumnDefinition rightDragColumn, ColumnDefinition rightPaddingColumn)
    {
        InternalInitialize(window, titleTextBlock, appTitleBar, leftPaddingColumn, iconColumn, titleColumn, leftDragColumn, searchColumn, rightDragColumn, rightPaddingColumn);
    }
    public static TitleBarHelper Initialize(Window window, TextBlock titleTextBlock, Grid appTitleBar, ColumnDefinition leftPaddingColumn, ColumnDefinition iconColumn, ColumnDefinition titleColumn, ColumnDefinition leftDragColumn, ColumnDefinition searchColumn, ColumnDefinition rightDragColumn, ColumnDefinition rightPaddingColumn)
    {
        if (Instance == null)
        {
            instance = new TitleBarHelper(window, titleTextBlock, appTitleBar, leftPaddingColumn, iconColumn, titleColumn, leftDragColumn, searchColumn, rightDragColumn, rightPaddingColumn);
        }
        else
        {
            instance.InternalInitialize(window, titleTextBlock, appTitleBar, leftPaddingColumn, iconColumn, titleColumn, leftDragColumn, searchColumn, rightDragColumn, rightPaddingColumn);
        }
        return Instance;
    }
    private void InternalInitialize(Window window, TextBlock titleTextBlock, Grid appTitleBar, ColumnDefinition leftPaddingColumn, ColumnDefinition iconColumn, ColumnDefinition titleColumn, ColumnDefinition leftDragColumn, ColumnDefinition searchColumn, ColumnDefinition rightDragColumn, ColumnDefinition rightPaddingColumn)
    {
        _LeftPaddingColumn = leftPaddingColumn;
        _IconColumn = iconColumn;
        _TitleColumn = titleColumn;
        _LeftDragColumn = leftDragColumn;
        _SearchColumn = searchColumn;
        _RightDragColumn = rightDragColumn;
        _RightPaddingColumn = rightPaddingColumn;
        _AppTitleBar = appTitleBar;
        _TitleTextBlock = titleTextBlock;
        _MainWindowObject = window;

        m_AppWindow = _MainWindowObject.AppWindow;

        // Check to see if customization is supported.
        // Currently only supported on Windows 11.
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            var titleBar = m_AppWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;
            appTitleBar.Loaded += AppTitleBar_Loaded;
            appTitleBar.SizeChanged += AppTitleBar_SizeChanged;
        }
        else
        {
            // Title bar customization using these APIs is currently
            // supported only on Windows 11. In other cases, hide
            // the custom title bar element.
            appTitleBar.Visibility = Visibility.Collapsed;

            // Show alternative UI for any functionality in
            // the title bar, such as search.
        }
    }

    private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            SetDragRegionForCustomTitleBar(m_AppWindow);
        }
    }

    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (AppWindowTitleBar.IsCustomizationSupported()
            && m_AppWindow.TitleBar.ExtendsContentIntoTitleBar)
        {
            // Update drag region if the size of the title bar changes.
            SetDragRegionForCustomTitleBar(m_AppWindow);
        }
    }

    private void SetDragRegionForCustomTitleBar(AppWindow appWindow)
    {
        if (AppWindowTitleBar.IsCustomizationSupported()
            && appWindow.TitleBar.ExtendsContentIntoTitleBar)
        {
            var scaleAdjustment = ApplicationHelper.GetScaleAdjustment(_MainWindowObject);

            _RightPaddingColumn.Width = new GridLength((appWindow.TitleBar.RightInset + RightPadding) / scaleAdjustment);
            _LeftPaddingColumn.Width = new GridLength((appWindow.TitleBar.LeftInset + LeftPadding) / scaleAdjustment);

            List<Windows.Graphics.RectInt32> dragRectsList = new();

            Windows.Graphics.RectInt32 dragRectL;
            dragRectL.X = (int)((_LeftPaddingColumn.ActualWidth) * scaleAdjustment);
            dragRectL.Y = 0;
            dragRectL.Height = (int)(_AppTitleBar.ActualHeight * scaleAdjustment);
            dragRectL.Width = (int)((_IconColumn.ActualWidth
                                    + _TitleColumn.ActualWidth
                                    + _LeftDragColumn.ActualWidth) * scaleAdjustment);
            dragRectsList.Add(dragRectL);

            Windows.Graphics.RectInt32 dragRectR;
            dragRectR.X = (int)((_LeftPaddingColumn.ActualWidth
                                + _IconColumn.ActualWidth
                                + _TitleTextBlock.ActualWidth
                                + _LeftDragColumn.ActualWidth
                                + _SearchColumn.ActualWidth) * scaleAdjustment);
            dragRectR.Y = 0;
            dragRectR.Height = (int)(_AppTitleBar.ActualHeight * scaleAdjustment);
            dragRectR.Width = (int)(_RightDragColumn.ActualWidth * scaleAdjustment);
            dragRectsList.Add(dragRectR);

            var dragRects = dragRectsList.ToArray();

            appWindow.TitleBar.SetDragRectangles(dragRects);
        }
    }

    #endregion

    #region TabView in TitleBar
    public TitleBarHelper(Window window, TabView tabView, Grid customDragRegion)
    {
        InternalInitializeWithTabs(window, tabView, customDragRegion);
    }
    public static TitleBarHelper Initialize(Window window, TabView tabView, Grid customDragRegion)
    {
        if (Instance == null)
        {
            instance = new TitleBarHelper(window, tabView, customDragRegion);
        }
        else
        {
            instance.InternalInitializeWithTabs(window, tabView, customDragRegion);
        }
        return Instance;
    }
    private void InternalInitializeWithTabs(Window window, TabView tabView, Grid customDragRegion)
    {
        _MainWindowObject = window;
        m_AppWindow = _MainWindowObject.AppWindow;
        _CustomDragRegion = customDragRegion;
        var titleBar = m_AppWindow.TitleBar;

        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            titleBar.ExtendsContentIntoTitleBar = true;
            var btnColor = Colors.Transparent;
            titleBar.BackgroundColor = btnColor;
            titleBar.ButtonBackgroundColor = btnColor;
            titleBar.InactiveBackgroundColor = btnColor;
            titleBar.ButtonInactiveBackgroundColor = btnColor;
            tabView.Loaded += TabView_Loaded;
        }
    }

    private void TabView_Loaded(object sender, RoutedEventArgs e)
    {
        _CustomDragRegion.SizeChanged += OnTitleBarSizeChanged;
    }

    private void OnTitleBarSizeChanged(object sender, SizeChangedEventArgs e)
    {
        double scaleAdjustment = ApplicationHelper.GetScaleAdjustment(_MainWindowObject);

        _CustomDragRegion.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var customDragRegionPosition = _CustomDragRegion.TransformToVisual(null).TransformPoint(new Point(0, 0));

        List<Windows.Graphics.RectInt32> dragRectsList = new();

        Windows.Graphics.RectInt32 dragRectL;
        dragRectL.X = (int)(customDragRegionPosition.X * scaleAdjustment);
        dragRectL.Y = (int)(customDragRegionPosition.Y * scaleAdjustment);
        dragRectL.Height = (int)((_CustomDragRegion.ActualHeight - customDragRegionPosition.Y) * scaleAdjustment);
        dragRectL.Width = (int)((_CustomDragRegion.ActualWidth / 2) * scaleAdjustment);
        dragRectsList.Add(dragRectL);

        Windows.Graphics.RectInt32 dragRectR;
        dragRectR.X = (int)((customDragRegionPosition.X + _CustomDragRegion.ActualWidth / 2) * scaleAdjustment);
        dragRectR.Y = (int)(customDragRegionPosition.Y * scaleAdjustment);
        dragRectR.Height = (int)((_CustomDragRegion.ActualHeight - customDragRegionPosition.Y) * scaleAdjustment);
        dragRectR.Width = (int)((_CustomDragRegion.ActualWidth / 2) * scaleAdjustment);
        dragRectsList.Add(dragRectR);

        Windows.Graphics.RectInt32[] dragRects = dragRectsList.ToArray();

        m_AppWindow.TitleBar.SetDragRectangles(dragRects);
    }

    #endregion
}
