namespace WindowUI;

public static class AutoSuggestBoxHelper
{
    private static string NoResult = "No result found";

    public static void LoadSuggestions(AutoSuggestBox autoSuggestBox, AutoSuggestBoxTextChangedEventArgs args, IList<string> suggestList)
    {
        List<string> list = new List<string>();
        if (args.Reason != 0)
        {
            return;
        }

        string[] querySplit = autoSuggestBox.Text.Split(' ');

        foreach (string item in suggestList)
        {
            bool result = true;
            foreach (string value in querySplit)
            {
                if (!item.Contains(value, StringComparison.CurrentCultureIgnoreCase))
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                list.Add(item);
            }
        }

        if (list.Count > 0)
        {
            autoSuggestBox.ItemsSource = list;
        }
        else
        {
            autoSuggestBox.ItemsSource = new string[1] { NoResult };
        }
    }

    public static void LoadSuggestions(AutoSuggestBox autoSuggestBox, AutoSuggestBoxTextChangedEventArgs args, IList<string> suggestList, string noResultString)
    {
        NoResult = noResultString;
        LoadSuggestions(autoSuggestBox, args, suggestList);
    }
}
