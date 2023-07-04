namespace WinUICommunity;

public static class AutoSuggestBoxHelper
{
    private static string NoResult = "No result found";

    public static void LoadSuggestions(AutoSuggestBox autoSuggestBox, AutoSuggestBoxTextChangedEventArgs args, IList<string> suggestList)
    {
        var suggestions = new List<string>();
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var querySplit = autoSuggestBox.Text.Split(' ');
            var matchingItems = suggestList.Where(
                item =>
                {
                    var flag = true;
                    foreach (var queryToken in querySplit)
                    {
                        if (item.IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) < 0)
                        {
                            flag = false;
                        }

                    }
                    return flag;
                });
            foreach (var item in matchingItems)
            {
                suggestions.Add(item);
            }
            if (suggestions.Count > 0)
            {
                for (var i = 0; i < suggestions.Count; i++)
                {
                    autoSuggestBox.ItemsSource = suggestions;
                }
            }
            else
            {
                autoSuggestBox.ItemsSource = new string[] { NoResult };
            }
        }
    }
    public static void LoadSuggestions(AutoSuggestBox autoSuggestBox, AutoSuggestBoxTextChangedEventArgs args, IList<string> suggestList, string noResultString)
    {
        NoResult = noResultString;
        LoadSuggestions(autoSuggestBox, args, suggestList);
    }
}
