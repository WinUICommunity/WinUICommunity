﻿using System.Diagnostics.CodeAnalysis;

using Microsoft.UI.Xaml.Media.Animation;

namespace WinUICommunity;

public partial class NavigationServiceEx : INavigationServiceEx
{
    private readonly IPageServiceEx _pageService;
    private object? _lastParameterUsed;
    private Frame? _frame;

    public event NavigatedEventHandler? Navigated;

    public Frame? Frame
    {
        get
        {
            if (_frame == null)
            {
                _frame = Window?.Content as Frame;
                RegisterFrameEvents();
            }

            return _frame;
        }

        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => Frame != null && Frame.CanGoBack;

    public Window Window { get; set; }

    public NavigationServiceEx(IPageServiceEx pageService)
    {
        _pageService = pageService;
    }

    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated += OnNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated -= OnNavigated;
        }
    }

    public bool GoBack()
    {
        if (CanGoBack)
        {
            var vmBeforeNavigation = _frame.GetPageViewModel();
            var frameContentBeforeNavigationAOTSafe = _frame?.Content;
            
            _frame.GoBack();

            if (frameContentBeforeNavigationAOTSafe is Page page && page?.DataContext is INavigationAwareEx viewModel)
            {
                viewModel.OnNavigatedFrom();
            }
            else if (vmBeforeNavigation is INavigationAwareEx navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }

            return true;
        }

        return false;
    }

    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false, NavigationTransitionInfo transitionInfo = null)
    {
        var pageType = _pageService.GetPageType(pageKey);
        return Navigate(pageType, parameter, clearNavigation, transitionInfo);
    }

    public bool NavigateTo(Type pageType, object? parameter = null, bool clearNavigation = false, NavigationTransitionInfo transitionInfo = null)
    {
        return Navigate(pageType, parameter, clearNavigation, transitionInfo);
    }

    public bool Navigate(Type pageType, object? parameter = null, bool clearNavigation = false, NavigationTransitionInfo transitionInfo = null)
    {
        if (pageType == null)
        {
            return false;
        }

        if (_frame != null && (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed))))
        {
            _frame.Tag = clearNavigation;
            var vmBeforeNavigation = _frame.GetPageViewModel();
            var frameContentBeforeNavigationAOTSafe = _frame?.Content;
            var navigated = _frame.Navigate(pageType, parameter, transitionInfo);
            if (navigated)
            {
                _lastParameterUsed = parameter;

                if (frameContentBeforeNavigationAOTSafe is Page page && page?.DataContext is INavigationAwareEx viewModel)
                {
                    viewModel.OnNavigatedFrom();
                }
                else if (vmBeforeNavigation is INavigationAwareEx navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }

            return navigated;
        }

        return false;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                frame.BackStack.Clear();
            }

            if (_frame?.Content is Page page && page?.DataContext is INavigationAwareEx viewModel)
            {
                viewModel.OnNavigatedTo(e.Parameter);
            }
            else if (frame.GetPageViewModel() is INavigationAwareEx navigationAware)
            {
                navigationAware.OnNavigatedTo(e.Parameter);
            }

            Navigated?.Invoke(sender, e);
        }
    }
}
